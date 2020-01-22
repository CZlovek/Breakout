#region --- Usings ---

using System.Windows;
using System.Windows.Media;
using Breakout.Framework.Engine.Interfaces;

#endregion

namespace Breakout.Framework.Game.Interfaces
{
    public interface IBrick : IGameComponent
    {
        FrameworkElement View { get; }
        SolidColorBrush Color { get; set; }
        int Value { get; set; }
    }
}