#region --- Usings ---

using System.Windows;
using Breakout.Framework.Engine.Interfaces;
using Breakout.Framework.Game.Delegates;

#endregion

namespace Breakout.Framework.Game.Interfaces
{
    public interface IBlock : IGameComponent
    {
        FrameworkElement View { get; }
        event OnBrickHitHandler OnBrickHit;
        event OnCompletedHandler OnCompleted;


        /// <summary>
        /// </summary>
        void Create(int level);
    }
}