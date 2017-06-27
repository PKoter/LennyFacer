using System;
using System.Windows.Forms;

namespace BW
{
	internal class AppHandleProvider : NativeWindow
	{
		public AppHandleProvider()
		{
			CreateHandle(new CreateParams());
		}

	}
}