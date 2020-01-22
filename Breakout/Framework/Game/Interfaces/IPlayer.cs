#region --- Usings ---

using System.Windows;
using Breakout.Framework.Engine.Interfaces;

#endregion

namespace Breakout.Framework.Game.Interfaces
{
    public interface IPlayer : IGameComponent
    {
        /// <summary>
        /// </summary>
        double Center { get; }

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