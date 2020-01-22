#region --- Usings ---

using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Breakout.Framework.Engine.Interfaces;
using Breakout.Framework.Game.Delegates;

#endregion

namespace Breakout.Framework.Game.Interfaces
{
    public interface IGameManager
    {
        Canvas GameCanvas { get; }

        int Level { get; }

        int Lives { get; }

        double PlayGroundBottom { get; }

        double PlayGroundHeight { get; }

        double PlayGroundLeft { get; }

        double PlayGroundRight { get; }

        double PlayGroundTop { get; }

        double PlayGroundWidth { get; }

        int Score { get; }

        event OnGameOverHandler OnGameOver;

        /// <summary>
        /// </summary>
        /// <param name="gameCanvas"></param>
        /// <param name="gameMenu"></param>
        /// <param name="playerView"></param>
        /// <param name="blockView"></param>
        /// <param name="brickView"></param>
        /// <param name="ballView"></param>
        void Initialize(Canvas gameCanvas, Grid gameMenu, Rectangle playerView, Rectangle blockView,
            Rectangle brickView, Ellipse ballView);

        /// <summary>
        /// </summary>
        void LaunchLevel();

        /// <summary>
        /// </summary>
        void NewGame();

        /// <summary>
        /// </summary>
        void NextLevel();

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        void SetMessenger(IMessengerContext context);

        /// <summary>
        /// </summary>
        /// <param name="deltaTime"></param>
        void Update(double deltaTime);

        /// <summary>
        /// </summary>
        /// <param name="mousePosition"></param>
        void UpdateMouse(Point mousePosition);
    }
}