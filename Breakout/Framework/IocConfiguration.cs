#region --- Usings ---

using Breakout.Framework.Game;
using Breakout.Framework.Game.Interfaces;
using Ninject.Modules;

#endregion

namespace Breakout.Framework
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<IGameManager>().To<GameManager>().InSingletonScope();
        }
    }
}