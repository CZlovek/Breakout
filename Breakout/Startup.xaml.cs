#region --- Usings ---

using System.Globalization;
using System.Threading;
using System.Windows;
using Breakout.Framework;

#endregion

namespace Breakout
{
    /// <summary>
    ///     Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            // inicializace DI kontejneru
            IocKernel.Initialize(new IocConfiguration());
        }
    }
}