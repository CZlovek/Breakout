#region --- Usings ---

using System.Windows;
using System.Windows.Controls;
using Breakout.Framework.Engine.Interfaces;

#endregion

namespace Breakout.Framework.Engine
{
    /// <summary>
    ///     Zakladni herni komponenta. Je to objekt ktery ma nejaky UI prototyp
    /// </summary>
    public class GameComponent : GameObject, IGameComponent
    {
        // bounding box objektu podle UI prototypu
        private Rect _boundingBox;

        /// <summary>
        ///     Vytvoreni objektu
        /// </summary>
        /// <param name="view">
        ///     UI prototyp
        /// </param>
        public GameComponent(FrameworkElement view)
        {
            View = view;

            X = 0;
            Y = 0;
        }

        /// <summary>
        ///     UI prototyp, pouze pro cteni
        /// </summary>
        public FrameworkElement View { get; }

        /// <summary>
        ///     Bounding box objektu
        /// </summary>
        public override Rect CollisionBounds
        {
            get
            {
                UpdateCollisionBounds();
                return _boundingBox;
            }
            set => _boundingBox = value;
        }

        /// <summary>
        ///     Vyska objektu
        /// </summary>
        public override double Height
        {
            get => View.Height;
            set
            {
                // pri zmene prepocti velikost bounding boxu
                View.Height = value;
                UpdateCollisionBounds();
            }
        }

        /// <summary>
        ///     Sirka objektu
        /// </summary>
        public override double Width
        {
            get => View.Width;
            set
            {
                // pri zmene prepocti velikost bounding boxu
                View.Width = value;
                UpdateCollisionBounds();
            }
        }

        /// <summary>
        ///     X pozice
        /// </summary>
        public sealed override double X
        {
            get => (double) View.GetValue(Canvas.LeftProperty);
            set
            {
                View.SetValue(Canvas.LeftProperty, value);

                // pri zmene prepocti velikost bounding boxu
                UpdateCollisionBounds();
            }
        }

        /// <summary>
        ///     Y pozice
        /// </summary>
        public sealed override double Y
        {
            get => (double) View.GetValue(Canvas.TopProperty);
            set
            {
                View.SetValue(Canvas.TopProperty, value);

                // pri zmene prepocti velikost bounding boxu
                UpdateCollisionBounds();
            }
        }

        /// <summary>
        ///     Prepocti rozmery bounding boxu
        /// </summary>
        protected void UpdateCollisionBounds()
        {
            _boundingBox = new Rect(X, Y, Width, Height);
        }
    }
}