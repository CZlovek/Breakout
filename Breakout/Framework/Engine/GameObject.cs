#region --- Usings ---

using System.Diagnostics;
using System.Windows;
using Breakout.Framework.Engine.Interfaces;

#endregion

namespace Breakout.Framework.Engine
{
    /// <summary>
    ///     Zakladni herni objekt. Ma pozici, rozmery
    /// </summary>
    public class GameObject : IGameObject
    {
        public virtual Rect CollisionBounds { get; set; }
        public virtual double Height { get; set; }
        public virtual double Width { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }

        /// <summary>
        ///     Kontrola jestli doslo ke kolizi
        /// </summary>
        /// <param name="gameObject">
        ///     Objekt se kterym doslo ke kolizi
        /// </param>
        /// <returns></returns>
        public virtual bool Intersects(IGameObject gameObject)
        {
            return CollisionBounds.IntersectsWith(gameObject.CollisionBounds);
        }

        /// <summary>
        ///     Udalost k osetreni - kolize s objektem
        /// </summary>
        /// <param name="gameObject">
        ///     Objekt se kterym doslo ke kolizi
        /// </param>
        public virtual void OnCollide(IGameObject gameObject)
        {
            Debug.WriteLine("Collision: {0} -> {1}", ToString(), gameObject);
        }

        /// <summary>
        ///     Kolize v bodu
        /// </summary>
        /// <param name="point">
        ///     Bod se kterym se ma kontrolovat kolize
        /// </param>
        /// <returns></returns>
        public virtual bool PointIntersects(Point point)
        {
            return point.X > CollisionBounds.X &&
                   point.X < CollisionBounds.Width &&
                   point.Y > CollisionBounds.Y &&
                   point.Y < CollisionBounds.Height;
        }

        /// <summary>
        ///     Kolize s obdelnikem
        /// </summary>
        /// <param name="rectangle">
        ///     Bounding box objektu se kterym se kontroluje kolize
        /// </param>
        /// <returns></returns>
        public virtual bool RectIntersects(Rect rectangle)
        {
            return CollisionBounds.IntersectsWith(rectangle);
        }
    }
}