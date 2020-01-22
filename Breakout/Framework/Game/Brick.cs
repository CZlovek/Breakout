#region --- Usings ---

using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Breakout.Framework.Engine;
using Breakout.Framework.Game.Interfaces;

#endregion

namespace Breakout.Framework.Game
{
    public class Brick : GameComponent, IBrick
    {
        // barva cihly
        private SolidColorBrush _color;

        /// <summary>
        ///     Vytvoreni cihly
        /// </summary>
        /// <param name="view">
        ///     UI prototyp
        /// </param>
        public Brick(Rectangle view) : base(view)
        {
            view.Visibility = Visibility.Visible;
        }

        // pocet bodu za cislu... cim vys tim vic
        public int Value { get; set; }


        /// <summary>
        ///     Barva cihly
        /// </summary>
        public SolidColorBrush Color
        {
            get => _color;
            set
            {
                _color = value;
                ((Rectangle) View).Fill = _color;
            }
        }
    }
}