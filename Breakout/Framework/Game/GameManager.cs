#region --- Usings ---

using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Breakout.Framework.Base;
using Breakout.Framework.Engine.Interfaces;
using Breakout.Framework.Game.Delegates;
using Breakout.Framework.Game.Enums;
using Breakout.Framework.Game.Interfaces;

#endregion

namespace Breakout.Framework.Game
{
    /// <summary>
    ///     Gameplay/Logika hry
    /// </summary>
    public class GameManager : BaseNotifyPropertyChanged, IGameManager
    {
        // micek
        private IBall _ball;

        // hrack
        private IBlock _block;

        // horni UI menu s informacema
        private Grid _gameMenu;

        // cislo levelu
        private int _level;

        // pocet zbyvajicich zivotu
        private int _lives;

        // kontext na UI info zpravy
        private IMessengerContext _messengerContext;

        // hrac
        private IPlayer _player;

        // aktualni pocet bodu
        private int _score;

        // stav hry
        private GameState _state;

        // UI pro hru
        public Canvas GameCanvas { get; private set; }

        // velikost herni plochy - sirka
        public double PlayGroundBottom => GameCanvas.ActualHeight;

        // velikost herni plochy - vyska
        public double PlayGroundHeight => GameCanvas.ActualHeight - _gameMenu.ActualHeight;

        // velikost herni plochy - levy okraj
        public double PlayGroundLeft => 0;

        // velikost herni plochy - pravy okraj
        public double PlayGroundRight => GameCanvas.ActualWidth;

        // velikost herni plochy - horni okraj
        public double PlayGroundTop => _gameMenu.ActualHeight;

        // velikost herni plochy - sirka
        public double PlayGroundWidth => GameCanvas.ActualWidth;

        /// <summary>
        ///     Cislo levelu
        /// </summary>
        public int Level
        {
            get => _level;

            private set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Zbyvajici pocet zivotu
        /// </summary>
        public int Lives
        {
            get => _lives;

            private set
            {
                _lives = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Aktualni skore
        /// </summary>
        public int Score
        {
            get => _score;

            private set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Udalost pro celkovy konec hry
        /// </summary>
        public event OnGameOverHandler OnGameOver;

        /// <summary>
        ///     Inicializace herniho manazeru
        /// </summary>
        /// <param name="gameCanvas">
        ///     UI prototyp herni plochy
        /// </param>
        /// <param name="gameMenu">
        ///     UI horniho menu
        /// </param>
        /// <param name="playerView">
        ///     UI prototyp hrace
        /// </param>
        /// <param name="blockView">
        ///     UI prototyp bloku do ktereho se generuji cihly
        /// </param>
        /// <param name="brickView">
        ///     UI prototyp cihly, ze ktere vznikaji pak dalsi
        /// </param>
        /// <param name="ballView">
        ///     UI prototyp micku
        /// </param>
        public void Initialize(Canvas gameCanvas, Grid gameMenu, Rectangle playerView, Rectangle blockView,
            Rectangle brickView, Ellipse ballView)
        {
            GameCanvas = gameCanvas;

            _gameMenu = gameMenu;

            // pro jistotu skryj prototyp cihly, at tam nesviti
            brickView.Visibility = Visibility.Hidden;

            // vytvor hrace a micek
            CreatePlayer(playerView);
            CreateBall(ballView, _player);

            // vytvor blok
            CreateBlock(blockView, brickView);
        }

        /// <summary>
        ///     Vystreleni micku po zacatku levelu nebo po ztrate micku
        /// </summary>
        public void LaunchLevel()
        {
            // kontrolovat, ze hra uz nebezi, pak nestrilet
            if (_state != GameState.Running)
            {
                // nastav ze hra bezi
                _state = GameState.Running;

                // a vystrel micek
                _ball.Launch();
            }
        }

        /// <summary>
        ///     Zacatek a vytvoreni nove hry
        /// </summary>
        public void NewGame()
        {
            // nova hra jenom kdyz se nic nedeje
            if (_state == GameState.Idle)
            {
                // vynulovat body, pridat zivoty a nastavit prvni level
                Score = 0;
                Lives = 3;
                Level = 1;

                // vygenerovat nove cihly
                CreateNewLevel();
            }
        }

        /// <summary>
        ///     Start noveho levelo
        /// </summary>
        public void NextLevel()
        {
            // zmenit stav a cekat na vystreleni
            _state = GameState.Waiting;

            // zvysit level
            Level++;

            // vygenerovat novy cihly
            CreateNewLevel();
        }

        /// <summary>
        ///     Nastaveni UI kontextu pro kratke zpravy na UI
        /// </summary>
        /// <param name="context">
        ///     Kontext pro zobrazovani zprav
        /// </param>
        public void SetMessenger(IMessengerContext context)
        {
            _messengerContext = context;
        }

        /// <summary>
        ///     Pravidelny update celeho manageru a jednotlivych komponent
        /// </summary>
        /// <param name="deltaTime">
        ///     Cas od posledniho update
        /// </param>
        public void Update(double deltaTime)
        {
            // updatuj micek a hrace
            _ball.Update(deltaTime);
            _player.Update(deltaTime);

            // zkontrolu ze micek se ma odrazit od palky
            if (_player.Intersects(_ball))
            {
                _player.OnCollide(_ball);
            }

            // zkontrolu ze micek nenarazil do cihly
            if (_block.Intersects(_ball))
            {
                _block.OnCollide(_ball);
            }
        }

        /// <summary>
        ///     Pravidelny update pozice mysi
        /// </summary>
        /// <param name="mousePosition">
        ///     Aktualni pozice mysi
        /// </param>
        public void UpdateMouse(Point mousePosition)
        {
            _player.UpdateMouse(mousePosition);
        }


        /// <summary>
        ///     Hrac minul micek
        /// </summary>
        private void BallMissed()
        {
            // resetovat micek a cekat na dalsi vystreleni
            _state = GameState.Waiting;

            // odpocitat zivot
            Lives--;

            // resetovat micek
            _ball.Reset();

            // zkontrolovat jestli muzu jeste hrat nebo uz konce... dosly zivoty
            if (Lives > 0)
            {
                _messengerContext?.Message(
                    "Ups... vedle. Stiskem mezerníku vystřelte další míček. Pozor na zbývající míčky.");
            }
            else
            {
                GameOver();
            }
        }

        /// <summary>
        ///     Micek trefil cihlu
        /// </summary>
        /// <param name="brick">
        ///     Cihla kterou trefil
        /// </param>
        private void BrickHitted(IBrick brick)
        {
            // zvys skore
            Score += brick.Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="player"></param>
        private void CreateBall(Ellipse view, IPlayer player)
        {
            _ball = new Ball(view, player);

            // osetrit kdyz hrac micek netrefi
            _ball.OnMissed += BallMissed;
        }


        /// <summary>
        ///     Vytvoreni bloku
        /// </summary>
        /// <param name="blockView"></param>
        /// <param name="brickView"></param>
        private void CreateBlock(Rectangle blockView, Rectangle brickView)
        {
            _block = new Block(blockView, brickView);

            // osetrit udalost pri stretu s cihlou
            _block.OnBrickHit += BrickHitted;

            // osetrit dokonceni levelu
            _block.OnCompleted += NextLevel;
        }

        /// <summary>
        ///     Vytvoreni noveho levelu
        /// </summary>
        private void CreateNewLevel()
        {
            // info uzivateli jak na to
            _messengerContext?.Message("Pohyb myši ovláda pálku. Stiskem mezerníku vystřelíte míček.");

            // resetuj
            _ball.Reset();

            //  avytvor level
            _block.Create(Level);
        }


        /// <summary>
        /// </summary>
        /// <param name="playerView"></param>
        private void CreatePlayer(Rectangle playerView)
        {
            _player = new Player(playerView);
        }


        /// <summary>
        ///     Konec hry
        /// </summary>
        private void GameOver()
        {
            // posli info ze je konec
            OnGameOver?.Invoke();
        }
    }
}