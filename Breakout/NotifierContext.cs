#region --- Usings ---

using Breakout.Framework.Engine.Interfaces;
using ToastNotifications;
using ToastNotifications.Messages;

#endregion

namespace Breakout
{
    /// <summary>
    ///     Adapter pro Notifier a Messenger ve hre pro zobrazovani zprav
    /// </summary>
    public class NotifierContext : IMessengerContext
    {
        private readonly Notifier _notifier;

        public NotifierContext(Notifier notifier)
        {
            _notifier = notifier;
        }

        /// <summary>
        ///     Metoda pro zobrazeni textu uzivateli na UI
        /// </summary>
        /// <param name="text"></param>
        public void Message(string text)
        {
            _notifier.ShowInformation(text);
        }
    }
}