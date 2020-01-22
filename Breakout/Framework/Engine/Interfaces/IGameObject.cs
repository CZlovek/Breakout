#region --- Usings ---

using System.Windows;

#endregion

namespace Breakout.Framework.Engine.Interfaces
{
    public interface IGameObject
    {
        Rect CollisionBounds { get; set; }
        double Height { get; set; }
        double Width { get; set; }
        double X { get; set; }
        double Y { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        bool Intersects(IGameObject gameObject);

        /// <summary>
        /// </summary>
        /// <param name="gameObject"></param>
        void OnCollide(IGameObject gameObject);

        /// <summary>
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        bool PointIntersects(Point point);

        /// <summary>
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        bool RectIntersects(Rect rectangle);
    }
}