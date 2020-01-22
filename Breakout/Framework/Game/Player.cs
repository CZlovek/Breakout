#region --- Usings ---

using System.Windows;
using System.Windows.Shapes;
using Breakout.Framework.Engine;
using Breakout.Framework.Engine.Interfaces;
using Breakout.Framework.Game.Interfaces;
using Breakout.Framework.Utils;

#endregion

namespace Breakout.Framework.Game
{
    public class Player : GameComponent, IPlayer
    {
        // maximalni uhel odrazu od palky
        private const float MAX_ANGLE = 45;

        // okraje palky
        private readonly double _paddleLeftBoundary;
        private readonly double _paddleRightBoundary;
        private readonly double _paddleYPosition;

        // predchozi pozice
        private readonly Point _previousMousePosition = new Point();

        /// <summary>
        ///     Vytvoreni hrace
        /// </summary>
        /// <param name="view">
        ///     UI prototyp
        /// </param>
        public Player(Rectangle view) : base(view)
        {
            var gameManager = IocKernel.Get<IGameManager>();

            _paddleYPosition = gameManager.PlayGroundHeight - 100;

            _paddleLeftBoundary = 10;
            _paddleRightBoundary = gameManager.PlayGroundWidth - 10;

            Center = Width / 2;
        }

        /// <summary>
        /// </summary>
        public double Center { get; }

        /// <summary>
        /// </summary>
        public sealed override double Width
        {
            get => base.Width;
            set => base.Width = value;
        }

        /// <summary>
        /// </summary>
        /// <param name="gameObject"></param>
        public override void OnCollide(IGameObject gameObject)
        {
            if (gameObject is Ball ball)
            {
                // vzdalenost od stredu palky
                var distance = ball.X + ball.Width / 2 - (X + Width / 2);

                // vektor rychlosti
                var vector = MathUtils.CalculateVelocityVector(MAX_ANGLE / Center * distance);

                ball.XVelocity = vector.X * Constants.BALL_SPEED;
                ball.YVelocity = vector.Y * Constants.BALL_SPEED;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(double deltaTime)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="mousePosition"></param>
        public void UpdateMouse(Point mousePosition)
        {
            var mouseX = mousePosition.X;

            if (!mousePosition.Equals(_previousMousePosition))
            {
                mouseX -= Center;

                if (mouseX < _paddleLeftBoundary)
                {
                    mouseX = _paddleLeftBoundary;
                }
                else if (mouseX > _paddleRightBoundary - Width)
                {
                    mouseX = _paddleRightBoundary - Width;
                }

                X = mouseX;
                Y = _paddleYPosition;
            }
        }
    }
}