using System;
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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			ApplicationContext context = new LennyApplicationContext();
			Application.Run(context);
		}
	}
}
