using System;

namespace BazingaGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			//try
			//{
				using (var game = new BazingaGame())
					game.Run();
			//}
			//catch (Exception e)
			//{
			//	System.Windows.Forms.MessageBox.Show(e.ToString());
			//}
        }
    }
#endif
}
