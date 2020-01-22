#region --- Usings ---

using System.Windows.Shapes;
using Breakout.Framework.Engine;
using Breakout.Framework.Engine.Interfaces;
using Breakout.Framework.Game.Delegates;
using Breakout.Framework.Game.Interfaces;
using Breakout.Framework.Utils;

#endregion

namespace Breakout.Framework.Game
{
    public class Ball : GameComponent, IBall
    {
        private readonly IGameManager _gameManager;
        private readonly IPlayer _player;

        // info, jestli je micek zamceny k palce
        private bool _lockedToPaddle;

        /// <summary>
        ///     Vytvoreni micku.
        /// </summary>
        /// <param name="view">
        ///     UI prototyp
        /// </param>
        /// <param name="player">
        ///     Objekt hrace. Pred zacatkem hry, musi micek kopirovat pohyb hrace
        /// </param>
        public Ball(Ellipse view, IPlayer player) : base(view)
        {
            _gameManager = IocKernel.Get<IGameManager>();
            _player = player;
        }

        /// <summary>
        ///     Rychlost micku v ose X
        /// </summary>
        public double XVelocity { get; set; }

        /// <summary>
        ///     Rychlost micku v ose Y
        /// </summary>
        public double YVelocity { get; set; }

        /// <summary>
        ///     Udalost propadnuti micku na "podlahu"
        /// </summary>
        public event OnMissedHandler OnMissed;


        /// <summary>
        ///     Vystreleni noveho micku do nahodneho smeru
        /// </summary>
        public void Launch()
        {
            var vector = MathUtils.CalculateVelocityVector(Toolkit.Random.Next(-45, 45));

            // nastavit rychlost
            XVelocity = vector.X * Constants.BALL_SPEED;
            YVelocity = vector.Y * Constants.BALL_SPEED;

            // zrusit zamceni... jinak se nepohne
            _lockedToPaddle = false;
        }

        /// <summary>
        ///     Obsluha kolize s nejakym objektem
        /// </summary>
        /// <param name="gameObject">
        ///     Objekt se kterym doslo ke srazce
        /// </param>
        public override void OnCollide(IGameObject gameObject)
        {
            if (gameObject is IBrick)
            {
                YVelocity = -YVelocity;
            }
        }

        /// <summary>
        ///     Vynulovani rychlosti
        /// </summary>
        public void Reset()
        {
            // vynulovat rychlost a zamknout na palku
            _lockedToPaddle = true;

            XVelocity = 0;
            YVelocity = 0;
        }

        /// <summary>
        ///     Logika chovani micku.
        /// </summary>
        /// <param name="deltaTime">
        ///     Cas v sec od posledniho volani Update
        /// </param>
        public void Update(double deltaTime)
        {
            if (!_lockedToPaddle)
            {
                // kolize s okrajem
                if (X < _gameManager.PlayGroundLeft || X > _gameManager.PlayGroundRight - Width)
                {
                    // odraz + posun do plochy
                    X = X < _gameManager.PlayGroundLeft
                        ? _gameManager.PlayGroundLeft + 1
                        : _gameManager.PlayGroundRight - Width - 1;
                    XVelocity = -XVelocity;
                }
                else

                    // kolize s okrajem
                if (Y < _gameManager.PlayGroundTop || Y > _gameManager.PlayGroundBottom - Height)
                {
                    if (Y > _gameManager.PlayGroundBottom - Height)
                    {
                        // konec hry
                        OnMissed?.Invoke();
                    }
                    else
                    {
                        // odraz + posun do plochy
                        Y = Y < _gameManager.PlayGroundTop
                            ? _gameManager.PlayGroundTop + 1
                            : _gameManager.PlayGroundBottom - Height - 1;
                        YVelocity = -YVelocity;
                    }
                }
                else
                {
                    // posunuti do smeru
                    X += XVelocity * deltaTime;
                    Y += YVelocity * deltaTime;
                }
            }
            else
            {
                // pokud je micek zamceny, kopiruj pohyb hrace
                X = _player.X + (_player.Width - Width) / 2;
                Y = _player.Y - _player.Height - 2;
            }
        }
    }
}