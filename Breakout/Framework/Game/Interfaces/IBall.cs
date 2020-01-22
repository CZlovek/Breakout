#region --- Usings ---

using System.Windows;
using Breakout.Framework.Engine.Interfaces;
using Breakout.Framework.Game.Delegates;

#endregion

namespace Breakout.Framework.Game.Interfaces
{
    public interface IBall : IGameComponent
    {
        FrameworkElement View { get; }

        /// <summary>
        /// </summary>
        double XVelocity { get; set; }

        /// <summary>
        /// </summary>
        double YVelocity { get; set; }


        event OnMissedHandler OnMissed;

        /// <summary>
        /// </summary>
        void Launch();


        /// <summary>
        /// </summary>
        void Reset();

        /// <summary>
        /// </summary>
        /// <param name="deltaTime"></param>
        void Update(double deltaTime);
    }
}