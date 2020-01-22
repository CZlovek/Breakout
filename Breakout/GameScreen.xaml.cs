#region --- Usings ---

using System;
using System.Windows.Input;
using System.Windows.Threading;
using Breakout.Framework;
using Breakout.Framework.Game.Interfaces;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

#endregion

namespace Breakout
{
    /// <summary>
    ///     Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen
    {
        private readonly IGameManager _gameManager;

        /// <summary>
        ///     Timer pro updatovani hry
        /// </summary>
        private readonly DispatcherTimer _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(20)
        };

        // cas mezi updaty
        private double _deltaTime;
        private DateTime _lastUpdate = DateTime.Now;

        /// <summary>
        /// </summary>
        public GameScreen()
        {
            InitializeComponent();

            // hadler timeru
            _timer.Tick += TimerTickHandler;

            // ziskej game manager
            _gameManager = IocKernel.Get<IGameManager>();

            // nastav kontext pro UI at se zobrazuje zivot, level a body
            DataContext = _gameManager;

            // a cekej az se UI nahraje a pak poresime hru
            Loaded += (sender, args) => ScreenLoaded();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void KeyDownHandler(object sender, KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Space:
                    _gameManager.LaunchLevel();
                    break;

                // CHEAT: skoci do dalsiho levelu
                case Key.F12:
                    _gameManager.NextLevel();
                    break;
            }
        }

        /// <summary>
        ///     UI inicializovani a nahrano
        /// </summary>
        private void ScreenLoaded()
        {
            // vytvor notifier adapter a nastav notifier samotny
            var notifierContext = new NotifierContext(new Notifier(configuration =>
            {
                configuration.PositionProvider = new WindowPositionProvider(
                    parentWindow: this,
                    Corner.BottomCenter,
                    offsetX: 10,
                    offsetY: 10);

                configuration.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(1));

                configuration.Dispatcher = Dispatcher;
            }));

            // inicializuj hru... posli UI hrace, palky, micku
            _gameManager.Initialize(GameCanvas, GameInfo, Player, Block, Brick, Ball);

            // nastav messenger at se zobrazuje info
            _gameManager.SetMessenger(notifierContext);

            // osetri ukonceni hry tzn. konec na tehle obrazovce
            _gameManager.OnGameOver += () =>
            {
                new MenuScreen().Show();
                Close();
            };

            // protoze je vsechno ok, spust novou hru
            _gameManager.NewGame();

            // spust timer
            _timer.IsEnabled = true;
        }

        /// <summary>
        ///     Pravidelna udalost timeru, mel by se volat update hry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TimerTickHandler(object sender, EventArgs args)
        {
            // vypocti ubehly cas
            _deltaTime = DateTime.Now.Subtract(_lastUpdate).TotalSeconds;

            // updatuj mys
            _gameManager.UpdateMouse(Mouse.GetPosition(null));

            // updatuj hru
            _gameManager.Update(_deltaTime);

            _lastUpdate = DateTime.Now;
        }
    }
}