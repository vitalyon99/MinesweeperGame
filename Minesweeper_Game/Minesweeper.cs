using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Game : Panel
    {

        #region Fields

        // Відступи компонент(manager та minefield) від країв.
        private const int DistanceToTop=5;
        private const int DistanseToBottom=5;
        private const int DistanseToLeft=5;
        private const int DistanseToRight=5;
        // Відступ між компонентами(manager та minefield).
        private const int DistanceBetweenComponents=5;
        private const int HeightForManager = 40;
        // Стан гри - один із трьох станів (Не розпочата, Розпочата,  Завершенна).
        private GameState gameState;
        // Компоненти.
        private MineField minefield;
        private Manager manager;
        private Timer timer;
        // Кількість відкритих плиток.
        private int CurrrentNumberOpenTiles;
        // Тривалість гри (Відображує manager.label_for_timing).
        public int Timing { get; private set; }
        public int NumberMines { get; private set; }
        // Кількість плиток у рядку.
        public int NumberTilesInRow { get; private set; }
        // Кількість плиток у стовбці.
        public int NumberTilesInColumn { get; private set; }
        private event EventHandler StartGame;
        public event EventHandler GameOver;
        public event EventHandler GameWin;

        #endregion

        #region Metods

        public Game(int NumberTilesInRow, int NumberTilesInColumn, int NumberMines)
        {
            this.NumberTilesInRow = NumberTilesInRow;
            this.NumberTilesInColumn = NumberTilesInColumn;
            this.NumberMines = NumberMines;
            minefield = new MineField(NumberTilesInRow, NumberTilesInColumn, NumberMines);
            manager = new Manager();
            timer = new Timer();
            Initialize();
        }

        public Game (DifficultyGame difficulty)
        {
            switch (difficulty)
            {
                case DifficultyGame.Basic:
                    NumberTilesInRow = 10;
                    NumberTilesInColumn = 10;
                    NumberMines = 12;
                    break;
                case DifficultyGame.Intermediate:
                    NumberTilesInRow = 16;
                    NumberTilesInColumn = 16;
                    NumberMines = 40;
                    break;
                case DifficultyGame.Advanced:
                    NumberTilesInRow = 30;
                    NumberTilesInColumn = 16;
                    NumberMines = 99;
                    break;
            }
            minefield = new MineField(NumberTilesInRow, NumberTilesInColumn, NumberMines);
            manager = new Manager();
            timer = new Timer();
            Initialize();
        }

        // Ініціалізації компонент (Розміщення, Розмір, Додавання обробників подій і тд. і тп.).
        void Initialize()    
        {
            //
            // minefield
            //
            minefield.BorderStyle = BorderStyle.Fixed3D;
            minefield.Width += 4;
            minefield.Height += 4;
            // +4 це поправка на BorderStyle.Fixed3D.
            minefield.Location = new System.Drawing.Point(DistanseToLeft, HeightForManager + DistanceBetweenComponents
               + DistanceToTop);
            this.Controls.Add(minefield);
            //
            // manager
            //
            manager.BorderStyle = BorderStyle.Fixed3D;
            manager.Width = minefield.Width;
            manager.Height = HeightForManager;
            manager.Location = new System.Drawing.Point(DistanseToLeft,DistanceToTop);
            manager.BackColor = System.Drawing.Color.Silver;
            manager.InitializeComponent();
            manager.label_for_flags.NumberFlags = minefield.NumberMines;
            manager.PictureBoxSmile.MouseClick += Smile_Click; 
            this.Controls.Add(manager);
            //
            // timer
            //
            this.timer.Enabled = false;
            this.timer.Interval = 1000;
            this.timer.Tick += timer_Tick;
            // 
            // Game
            //
            this.Width = DistanseToLeft + DistanseToRight + minefield.Width;
            this.Height = DistanceToTop + manager.Height + DistanceBetweenComponents + minefield.Height + DistanseToBottom;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.gameState = GameState.NotStarted;
            this.StartGame += Game_Start_Game;
            this.GameOver += Game_Over;
            this.GameWin += Game_Win;
            //
            // Підписка на подію MouseClick для кожної плитки.
            //
            for (int i = 0; i < minefield.NumberTilesInRow; i++)
            {
                for(int j = 0; j < minefield.NumberTilesInColumn; j++)
                {
                    minefield.Tiles[i, j].MouseClick += Tile_Click;
                }
            }
        }

        public void ResetField()
        {
            // Завершуємо поточну гру, готуємося до нової гри.
            gameState = GameState.NotStarted;
            manager.label_for_timing.Timing = 0;
            Timing = 0;
            manager.label_for_flags.NumberFlags = minefield.NumberMines;
            timer.Stop();
            CurrrentNumberOpenTiles = 0;
            // Ініціалізуємо новий стан поля(нове розміщення мін).
            minefield.ReSetField();
        }

        // Відкрити плитку.
        private void OpenTile(Tile tile)
        {
            // Якщо уже відкрита - то ігноруєм.
            if (tile.ExternalState == KindsOfExternalTile.Open) return;
            // Якщо позначена флагом - то його зняти.
            if (tile.ExternalState == KindsOfExternalTile.Flagged)
                manager.label_for_flags.NumberFlags++;
            tile.ExternalState = KindsOfExternalTile.Open;
            CurrrentNumberOpenTiles++;
            // Якщо відкрита плитка пуста - то потрібно відкрити усіх сусідніх до неї.
            // Пуста тобто не має міни чи числа.
            if(tile.InternalState == KindsOfInternalTile.Zero)
            {
                for (int i = tile.Index.X - 1; i <= tile.Index.X + 1; i++)
                {
                    // Перевірка чи не має виходу за межі поля.
                    if ((i < 0) || (i >= minefield.NumberTilesInRow)) continue;
                    for (int j = tile.Index.Y - 1; j <= tile.Index.Y + 1; j++)
                    {
                        // Перевірка чи не має виходу за межі поля.
                        if ((j < 0) || (j >= minefield.NumberTilesInColumn)) continue;
                        // Відкриваємо сусіда за допомогою рекурсії.
                        OpenTile(minefield.Tiles[i, j]);
                    }
                }
            }
        }

        public void StopGame()
        {
            timer.Stop();
        }

        #endregion

        #region Event Handlers

        // Обробник події натискання клавіші на плитку(єдиний для усіх плиток).
        // Тобто для усіх minefield.Tiles[i,j].MouseClick.
        private void Tile_Click(object sender, MouseEventArgs e)
        {
            // Якщо гра уже завершенна то натискання ігноруєм.
            if ( gameState == GameState.Finished ) return;
            // Якщо не розпочата то розпочинаєм.
            if( gameState == GameState.NotStarted)
            {
                // Генеруємо подію StartGame.
                StartGame(this, EventArgs.Empty);
                gameState = GameState.Started;

            }
            Tile tile = (Tile)sender;
            // Якщо натиснута ліва кнопка миші.
            if (e.Button == MouseButtons.Left)
            {
                // Якщо плитка відкрита чи зафлагована - то ігноруєм натискання. 
                if ((tile.ExternalState == KindsOfExternalTile.Open)||
                    (tile.ExternalState == KindsOfExternalTile.Flagged)) return;
                OpenTile(tile);
                // Якщо відкрилася міна то завершуємо гру(Генеруємо подію GameOver).
                if (tile.InternalState == KindsOfInternalTile.Mine)
                {
                    GameOver(tile, EventArgs.Empty);
                    return;
                }
                // Якщо залишилися закритими тільки плитки з мінами(Генеруємо подію GameWin).
                if (CurrrentNumberOpenTiles == (NumberTilesInRow * NumberTilesInColumn - NumberMines))
                    GameWin(this, EventArgs.Empty);
            }
            // Якщо натиснута права кнопка миші.
            if (e.Button == MouseButtons.Right)
            {
                // Якщо плитка відкрита - то ігноруємо натискання.
                if (tile.ExternalState == KindsOfExternalTile.Open) return;
                // Якщо закрита - то ставимо флажок.
                if (tile.ExternalState == KindsOfExternalTile.Closed)
                {
                    // Якщо ліміт флажків вичерпаний - то флажка не ставимо.
                    if (manager.label_for_flags.NumberFlags == 0) return;
                    tile.ExternalState = KindsOfExternalTile.Flagged;
                    // Контроль поставлених флажків.
                    manager.label_for_flags.NumberFlags--;
                }
                // Якщо зафлагована - то знімаєм флажок і робимо закритою.
                else
                {
                    tile.ExternalState = KindsOfExternalTile.Closed;
                    // Контроль поставлених флажків.
                    manager.label_for_flags.NumberFlags++;
                }
            }
        }

        // Обробник подіїї кліку таймера.
        private void timer_Tick(object sender, EventArgs e)
        {
            // Контроль за тривалістю гри.
            this.manager.label_for_timing.Timing++;
            Timing++;
        }

        // Обробник події GameStart.
        private void Game_Start_Game(object sender, EventArgs e)
        {
            this.minefield.SetUpMines();
            this.minefield.SetUpNumbers();
            // Починаємо відлік часу(тривалості гри).
            this.timer.Enabled = true;
            this.timer.Start();
        }

        // Обробник події GameOver.
        private void Game_Over(object sender, EventArgs e)
        {
            // Відкриваємо поле, зупиняємо відлік часу, завершуємо гру.
            minefield.OpenTilesAfterLose();
            Tile tile = (Tile)sender;
            tile.Image = Game.Mine_Red_Tile;
            timer.Stop();
            manager.PictureBoxSmile.State = SmileState.Sad;
            gameState = GameState.Finished;
        }

        // Обробник події GameWin.
        public void Game_Win ( object sender, EventArgs e)
        {
            // Зупиняємо відлік часу, зафлаговуємо не зафлаговані плитки.
            manager.PictureBoxSmile.Image = Game.Smile_Cool;
            timer.Stop();
            minefield.FlagTilesAfterWin();
            manager.label_for_flags.NumberFlags = 0;
        }

        // Обробник події manager.PictureBoxSmile.MouseClick(клік мишкою на смайлик).
        private void Smile_Click (object sender, MouseEventArgs e)
        {
            PictureSmile obj = (PictureSmile)sender;
            obj.State = SmileState.Normal;
            // Якщо натиснута ліва кнопка миші.
            if(e.Button == MouseButtons.Left)
            {
                // Завершуємо поточну гру, готуємося до нової гри.
                gameState = GameState.NotStarted;
                manager.label_for_timing.Timing = 0;
                Timing = 0;
                manager.label_for_flags.NumberFlags = minefield.NumberMines;
                timer.Stop();
                CurrrentNumberOpenTiles = 0;
                // Ініціалізуємо новий стан поля(нове розміщення мін).
                minefield.ReSetField();
            }
        }

        #endregion

        #region Type Declarations

        // Індекс - [X,Y].
        private struct Index
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Index(int x,int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        // Внутрішні стани плитки.
        private enum KindsOfInternalTile
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Mine
        }

        // Зовнішні стани плитки.
        private enum KindsOfExternalTile
        {
            Closed,
            Open,
            Flagged
        }

        // Можливих стани гри.
        private enum GameState
        {
            NotStarted,
            Started,
            Finished
        }

        // Можливі стани "Смайлика".
        private enum SmileState
        {
            Normal,
            Sad,
            Cool
        }

        // "Смайлик".
        private class PictureSmile : PictureBox
        {
            // Стан Смайлика.
            private SmileState state;
            public SmileState State
            {
                get
                {
                    return state;
                }
                set
                {
                    state = value;
                    // При зміні стану одночасно змінюємо і зображення Смайлика.
                    switch (value)
                    {
                        case SmileState.Normal :
                            this.Image = Game.Smile_Normal;
                            break;
                        case SmileState.Sad :
                            this.Image = Game.Smile_Sad;
                            break;
                        case SmileState.Cool :
                            this.Image = Game.Smile_Cool;
                            break;
                    }
                }
            }
        }

        // Плитка.
        private class Tile : PictureBox
        {
            public Index Index { get; set; }
            private KindsOfInternalTile Internal_State;
            private KindsOfExternalTile External_State;
            public KindsOfExternalTile ExternalState
            {
                get
                {
                    return External_State;
                }
                set
                {
                    // При зміні стану одночасно змінюємо і зображення плитки.
                    switch (value)
                    {
                        case KindsOfExternalTile.Closed:
                            this.Image = Game.Closed_Tile;
                            break;
                        case KindsOfExternalTile.Flagged:
                            this.Image = Game.Flag_Tile;
                            break;
                        // Якщо відкрити то змінити зображення плитки відповідно до внутрішнього стану.
                        case KindsOfExternalTile.Open:
                            switch (Internal_State)
                            {
                                case KindsOfInternalTile.Zero:
                                    this.Image = Game.Empty_Tile;
                                    break;
                                case KindsOfInternalTile.One:
                                    this.Image = Game.One_Tile;
                                    break;
                                case KindsOfInternalTile.Two:
                                    this.Image = Game.Two_Tile;
                                    break;
                                case KindsOfInternalTile.Three:
                                    this.Image = Game.Three_Tile;
                                    break;
                                case KindsOfInternalTile.Four:
                                    this.Image = Game.Four_Tile;
                                    break;
                                case KindsOfInternalTile.Five:
                                    this.Image = Game.Five_Tile;
                                    break;
                                case KindsOfInternalTile.Six:
                                    this.Image = Game.Six_Tile;
                                    break;
                                case KindsOfInternalTile.Seven:
                                    this.Image = Game.Seven_Tile;
                                    break;
                                case KindsOfInternalTile.Eight:
                                    this.Image = Game.Eight_Tile;
                                    break;
                                case KindsOfInternalTile.Mine:
                                    this.Image = Game.Mine_Tile;
                                    break;
                            }
                            break;
                    }
                    External_State = value;
                }
            }
            public KindsOfInternalTile InternalState
            {
                get
                {
                    return Internal_State;
                }
                set
                {
                    Internal_State = value;
                }
            }
            public Tile()
            {
                InternalState = KindsOfInternalTile.Zero;
                ExternalState = KindsOfExternalTile.Closed;
                // Розтягнути зображення під розмір плитки.
                this.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        // Мінне поле.
        private class MineField : Panel
        {
            // Масив силок на плитки.
            public Tile[,] Tiles;
            // Масив індексів на плитки з мінами.
            public Index[] IndexesOfMines;
            public int NumberMines;
            // Кількість плиток у рядку.
            public int NumberTilesInRow;
            // Кількість плиток у стовбці.
            public int NumberTilesInColumn;
            // Розмір плитки.
            System.Drawing.Size SizeOfTile = new System.Drawing.Size(16, 16);

            public MineField(int NumberTilesInRow, int NumberTilesInColumn, int NumberMines)
            {
                this.NumberTilesInRow = NumberTilesInRow;
                this.NumberTilesInColumn = NumberTilesInColumn;
                this.NumberMines = NumberMines;
                this.IndexesOfMines = new Index[NumberMines];
                this.Size = new System.Drawing.Size(SizeOfTile.Width * NumberTilesInRow, 
                    SizeOfTile.Height * NumberTilesInColumn);
                this.Tiles = new Tile[NumberTilesInRow, NumberTilesInColumn];
                SetField();
            }
            // Ініціалізувати початковий стан поля(Створити і Розмістити плитки, Поставити міни і числа).
            public void SetField()
            {
                for (int i = 0; i < NumberTilesInRow; i++)
                {
                    for (int j = 0; j < NumberTilesInColumn; j++)
                    {
                        Tile tile = new Tile();
                        tile.Name = $"Tile[{i},{j}]";
                        tile.Index = new Index(i, j);
                        tile.Location = new System.Drawing.Point(i * SizeOfTile.Width, j * SizeOfTile.Height);
                        tile.Size = SizeOfTile;
                        this.Controls.Add(tile);
                        Tiles[i, j] = tile;
                    }
                }
                //SetUpMines();
                //SetUpNumbers();
            }
            // Закрити плитки і одночасно забрати міни.
            private void CloseTiles()
            {
                for (int i = 0; i < NumberTilesInRow; i++)
                {
                    for (int j = 0; j < NumberTilesInColumn; j++)
                    {
                        Tiles[i, j].ExternalState = KindsOfExternalTile.Closed;
                        // Забрати міни.
                        Tiles[i, j].InternalState = KindsOfInternalTile.Zero;
                    }
                }
            }
            // Встановити інший стан поля.
            public void ReSetField()
            {
                CloseTiles();
                //SetUpMines();
                //SetUpNumbers();
            }
            // Розставити міни рандомним чином.
            public void SetUpMines()
            {
                Random random = new Random();
                // і - для контролю того щоб коректно завершити цикл розміщення мін.
                // Щоб не поставити замало.
                int i = NumberMines;
                Index index = new Index();
                do
                {
                    index.X = random.Next(0, NumberTilesInRow);
                    index.Y = random.Next(0, NumberTilesInColumn);
                    // Якщо данна плитка не містить міни.
                    if((Tiles[index.X,index.Y].InternalState)!= KindsOfInternalTile.Mine)
                    {
                        // Ставимо міну.
                        Tiles[index.X, index.Y].InternalState = KindsOfInternalTile.Mine;
                        // Запам'ятовуємо індекс плитки з міною.
                        IndexesOfMines[i-1] = index;
                        i--;
                    }
                } while (i != 0);
            }
            // Розоставити числа відповідно до уже розставлених мін.
            public void SetUpNumbers()
            {
                for(int k =0; k<NumberMines; k++)
                {
                    for(int i = IndexesOfMines[k].X - 1; i <= IndexesOfMines[k].X + 1; i++)
                    {
                        // Перевірка чи не має виходу за межі поля.
                        if ((i < 0) || (i >= NumberTilesInRow)) continue;
                        for(int j = IndexesOfMines[k].Y - 1; j <= IndexesOfMines[k].Y + 1; j++)
                        {
                            // Перевірка чи не має виходу за межі поля.
                            if ((j < 0) || (j >= NumberTilesInColumn)) continue;
                            if(Tiles[i,j].InternalState != KindsOfInternalTile.Mine)
                            {
                                Tiles[i, j].InternalState++;
                            }
                        }
                    }
                }
            }
            // Розставити флажки після перемоги.
            public void FlagTilesAfterWin()
            {
                for(int i = 0; i < NumberTilesInRow; i++)
                {
                    for(int j = 0; j < NumberTilesInColumn; j++)
                    {
                        if(Tiles[i,j].ExternalState == KindsOfExternalTile.Closed)
                        Tiles[i, j].ExternalState = KindsOfExternalTile.Flagged;
                    }
                }
            }
            // Відкрити поле після програшу(Показати розміщення мін).
            public void OpenTilesAfterLose()
            {
                for (int i = 0; i < NumberTilesInRow; i++)
                {
                    for (int j = 0; j < NumberTilesInColumn; j++)
                    {
                        switch (Tiles[i, j].ExternalState)
                        {
                            // Якщо плитка закрита і у ній є міна то відкрити.
                            case KindsOfExternalTile.Closed:
                                if (Tiles[i, j].InternalState == KindsOfInternalTile.Mine)
                                    Tiles[i, j].ExternalState = KindsOfExternalTile.Open;
                                break;
                            // Якщо плитка зафлагована і вона не має міни то показати помилку.
                            case KindsOfExternalTile.Flagged:
                                if (Tiles[i, j].InternalState != KindsOfInternalTile.Mine)
                                    Tiles[i, j].Image = Game.Mine_Mistake_Tile;
                                break;
                        }
                    }
                }
            }
            // Відкрити усі плитки з мінами.
            public void OpenTilesWithMines()
            {
                for(int i = 0; i < NumberMines; i++)
                {
                    Tiles[IndexesOfMines[i].X, IndexesOfMines[i].Y].ExternalState = KindsOfExternalTile.Open;
                }
            }
        }

        // Мітка для відображення тривалості гри.
        private class LabelForTiming : Label
        {
            private int Time = 0;
            public int Timing
            {
                get
                {
                    return Time;
                }
                // При зміні часу, контролюємо коректний його вивід.
                set
                {
                    if (value < 0) return;
                    if(value <= 9)
                    {
                        Time = value;
                        this.Text = "00" + Time.ToString();
                        return;
                    }
                    if(value <= 99)
                    {
                        Time = value;
                        this.Text = "0" + Time.ToString();
                        return;
                    }
                    if(value <= 999)
                    {
                        Time = value;
                        this.Text =  Time.ToString();
                        return;
                    }
                }
            }
        }

        // Мітка для відображення ще наявних флажків.
        private class LabelForFlags : Label
        {
            private int numberFlags ;
            public int NumberFlags
            {
                get
                {
                    return numberFlags;
                }
                // При зміні кількості флажків, контролюємо коректний її вивід.
                set
                {
                    if (value < 0)
                    {
                        return;
                    }
                    if (value <= 9)
                    {
                        numberFlags = value;
                        this.Text = "00" + numberFlags.ToString();
                        return;
                    }
                    if (value <= 99)
                    {
                        numberFlags = value;
                        this.Text = "0" + numberFlags.ToString();
                        return;
                    }
                    else
                    {
                        numberFlags = value;
                        this.Text =  numberFlags.ToString();
                        return;
                    }
                    
                }
            }
        }

        // Панель управління.
        private class Manager : Panel
        {
            // Відступи компонент до країв Manager.
            private const int DistanceToTop = 6;
            private const int DistanceToLeft = 9;
            private const int DictanceToRight = 11;
            // Розмір Смайлика.
            private System.Drawing.Size SizeForSmile = new System.Drawing.Size(26,26);
            public LabelForFlags label_for_flags;
            public LabelForTiming label_for_timing;
            public PictureSmile PictureBoxSmile;

            public Manager()
            {

            }
            public void InitializeComponent()
            {
                // Ініціалізація компонент(Створення, Розміщення і тд. і тп.).
                this.label_for_flags = new LabelForFlags();
                this.label_for_timing = new LabelForTiming();
                this.PictureBoxSmile = new PictureSmile();
                // 
                // PictureBoxSmile
                //
                this.PictureBoxSmile.SizeMode = PictureBoxSizeMode.StretchImage;
                this.PictureBoxSmile.Size = SizeForSmile;
                this.PictureBoxSmile.State = SmileState.Normal;
                this.PictureBoxSmile.Location = new System.Drawing.Point(this.Width / 2 - this.PictureBoxSmile.Width / 2,
                    DistanceToTop);
                this.Controls.Add(this.PictureBoxSmile);
                //
                // LabelForFlags
                //
                this.label_for_flags.Width = 50;
                this.label_for_flags.Location = new System.Drawing.Point(DistanceToLeft,DistanceToTop);
                this.label_for_flags.Font = new System.Drawing.Font("MS Reference Sans Serif",15.75f,
                    System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,204);
                this.label_for_flags.BackColor = System.Drawing.Color.DimGray;
                this.label_for_flags.ForeColor = System.Drawing.Color.Maroon;
                this.label_for_flags.AutoSize = true;
                this.label_for_flags.Text = "000";
                this.Controls.Add(this.label_for_flags);
                //
                // label_for_timing
                //
                this.label_for_timing.Width = 50;
                this.label_for_timing.Font = new System.Drawing.Font("MS Reference Sans Serif", 15.75f,
                    System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
                this.label_for_timing.BackColor = System.Drawing.Color.DimGray;
                this.label_for_timing.ForeColor = System.Drawing.Color.Maroon;
                this.label_for_timing.AutoSize = true;
                this.label_for_timing.Location = new System.Drawing.Point
                    (this.Width - Manager.DictanceToRight - label_for_timing.Width, DistanceToTop);
                this.label_for_timing.Text = "000";
                this.Controls.Add(this.label_for_timing);
            }
        }

        #endregion

        #region Images

        // Створення усіх об'єктів зображень.

        private static readonly Image Closed_Tile = Minesweeper_Game.Properties.Resources.Closed_Tile;
        private static readonly Image One_Tile = Minesweeper_Game.Properties.Resources.One_Tile;
        private static readonly Image Two_Tile = Minesweeper_Game.Properties.Resources.Two_Tile;
        private static readonly Image Three_Tile = Minesweeper_Game.Properties.Resources.Three_Tile;
        private static readonly Image Four_Tile = Minesweeper_Game.Properties.Resources.Four_Tile;
        private static readonly Image Five_Tile = Minesweeper_Game.Properties.Resources.Five_Tile;
        private static readonly Image Six_Tile = Minesweeper_Game.Properties.Resources.Six_Tile;
        private static readonly Image Seven_Tile = Minesweeper_Game.Properties.Resources.Seven_Tile;
        private static readonly Image Eight_Tile = Minesweeper_Game.Properties.Resources.Eight_Tile;
        private static readonly Image Mine_Tile = Minesweeper_Game.Properties.Resources.Mine_Tile;
        private static readonly Image Mine_Red_Tile = Minesweeper_Game.Properties.Resources.Mine_Red_Tile;
        private static readonly Image Mine_Mistake_Tile = Minesweeper_Game.Properties.Resources.Mine_Mistake_Tile;
        private static readonly Image Empty_Tile = Minesweeper_Game.Properties.Resources.Empty_Tile;
        private static readonly Image Flag_Tile = Minesweeper_Game.Properties.Resources.Flag_Tile;
        private static readonly Image Smile_Normal = Minesweeper_Game.Properties.Resources.Smile_Normal;
        private static readonly Image Smile_Sad = Minesweeper_Game.Properties.Resources.Smile_End;
        private static readonly Image Smile_Cool = Minesweeper_Game.Properties.Resources.Smile_Cool;

        #endregion

    }
    public enum DifficultyGame
    {
        Basic,
        Intermediate,
        Advanced
    }
}