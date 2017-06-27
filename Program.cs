using System;
using System.Threading;
using System.Windows.Forms;

namespace BW
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool single;
			using (Mutex me = new Mutex(true, "LennyFacer", out single))
			{
				if (single)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					ApplicationContext context = new LennyApplicationContext();
					Application.Run(context);
				}
			}
		}
	}
}
