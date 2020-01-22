#region --- Usings ---

using Ninject;
using Ninject.Modules;

#endregion

namespace Breakout.Framework
{
    /// <summary>
    ///     Konfigurace DI kontejneru
    /// </summary>
    public static class IocKernel
    {
        private static StandardKernel _kernel;

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return _kernel.Get<T>();
        }

        /// <summary>
        /// </summary>
        /// <param name="modules"></param>
        public static void Initialize(params INinjectModule[] modules)
        {
            if (_kernel == null)
            {
                _kernel = new StandardKernel(modules);
            }
        }
    }
}