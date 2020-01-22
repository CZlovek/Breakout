#region --- Usings ---

using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using Breakout.Framework.Engine;
using Breakout.Framework.Engine.Interfaces;
using Breakout.Framework.Game.Delegates;
using Breakout.Framework.Game.Interfaces;
using Breakout.Framework.Utils;

#endregion

namespace Breakout.Framework.Game
{
    public class Block : GameComponent, IBlock
    {
        // mezera mezi cihlama
        private const int MARGIN = 5;

        // UI prototyp cihly, ze ktereho se pak klonuji dalsi
        private readonly Rectangle _brickPrototypeView;

        // seznam cihel
        private readonly List<IBrick> _bricks = new List<IBrick>();

        // seznam moznych barev pro cihly
        private readonly Color[] _colors =
        {
            Colors.Yellow,
            Colors.Red,
            Colors.Blue,
            Colors.White,
            Colors.Purple,
            Colors.Brown,
            Colors.Green,
            Colors.Orange
        };

        private readonly IGameManager _gameManager;

        /// <summary>
        ///     Vytvoreni bloku obsahujiciho cihly
        /// </summary>
        /// <param name="view"></param>
        /// <param name="brickPrototypeView"></param>
        public Block(Rectangle view, Rectangle brickPrototypeView) : base(view)
        {
            _gameManager = IocKernel.Get<IGameManager>();
            _brickPrototypeView = brickPrototypeView;
        }

        public event OnBrickHitHandler OnBrickHit;
        public event OnCompletedHandler OnCompleted;

        /// <summary>
        ///     Vytvoreni cihel pro dany level. Cislo levelu je pocet radku cihel, kdy cihly zaplnuji celou sirku obrazovky.
        /// </summary>
        /// <param name="level">
        ///     Cislo levelu
        /// </param>
        public void Create(int level)
        {
            // smazat pripadne existujici cihly
            if (_bricks.Count > 0)
            {
                foreach (var brick in _bricks)
                {
                    // UI se musi vyhodit taky
                    _gameManager.GameCanvas.Children.Remove(brick.View);
                }

                _bricks.Clear();
            }

            // zjisti pocet sloupcu
            var cols = (int) (_gameManager.PlayGroundWidth / (_brickPrototypeView.ActualWidth + MARGIN));

            // radku je podle cisla levelu
            var rows = level;

            // offset od praveho okraje, tak aby byly cihly vycentrovane
            var leftOffset = _gameManager.PlayGroundLeft +
                             (_gameManager.PlayGroundWidth - cols * (_brickPrototypeView.ActualWidth + MARGIN)) / 2 +
                             MARGIN / 2;

            // odstup od horniho okraje
            var topOffset = _gameManager.PlayGroundTop + 10;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    // naklonuj cihlu
                    var brick = new Brick(Toolkit.Clone(_brickPrototypeView));

                    // nastav ji pozici
                    var left = leftOffset + col * (_brickPrototypeView.ActualWidth + MARGIN);
                    var top = topOffset + row * (_brickPrototypeView.ActualHeight + MARGIN);

                    // barvu
                    brick.Color = new SolidColorBrush(_colors[Toolkit.Random.Next(_colors.Length)]);

                    // pocet bodu k ziskani
                    brick.Value = (rows - row) * 5;
                    brick.X = left;
                    brick.Y = top;

                    // pridej na UI
                    _gameManager.GameCanvas.Children.Add(brick.View);

                    // a do seznamu
                    _bricks.Add(brick);
                }
            }

            // nastav hodnotu pro zjisteni bounding boxu celeho bloku
            X = _bricks[0].X;
            Y = _bricks[0].Y;
            Width = _bricks[_bricks.Count - 1].X + _bricks[_bricks.Count - 1].Width;
            Height = _bricks[_bricks.Count - 1].Y + _bricks[_bricks.Count - 1].Height;
        }

        /// <summary>
        ///     Obsluha kolize s nejakym objektem
        /// </summary>
        /// <param name="gameObject">
        ///     Objekt se kterym doslo ke srazce
        /// </param>
        public override void OnCollide(IGameObject gameObject)
        {
            // pokud jsem narazil na micek
            if (gameObject is Ball ball)
            {
                var collision = false;
                var index = 0;

                // zjisti jestli a do jake cihly jsem narazil
                while (!collision && index < _bricks.Count)
                {
                    var brick = _bricks[index];

                    // jestli narazim do cihly
                    if (brick.Intersects(ball))
                    {
                        // dej to vedet micku
                        ball.OnCollide(brick);

                        // dej to vedet vys
                        OnBrickHit?.Invoke(brick);

                        // smaz cihlu
                        _gameManager.GameCanvas.Children.Remove(brick.View);
                        _bricks.Remove(brick);

                        // jedna a dost
                        collision = true;
                    }

                    index++;
                }

                // pokud v seznamu nejsou cihly, pak by mel byt konec levelu
                if (_bricks.Count == 0)
                {
                    OnCompleted?.Invoke();
                }
            }
        }
    }
}