#region --- Usings ---

using System;

#endregion

namespace Breakout
{
    /// <summary>
    ///     Interaction logic for MenuScreen.xaml
    /// </summary>
    public partial class MenuScreen
    {
        /// <summary>
        /// </summary>
        public MenuScreen()
        {
            InitializeComponent();
        }


        /// <summary>
        ///     Ukonceni hry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     Spusteni nove hry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewGameClick(object sender, EventArgs e)
        {
            new GameScreen().Show();
            Close();
        }
    }
}