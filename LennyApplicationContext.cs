using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using BW.Properties;

namespace BW
{
	public class LennyApplicationContext : ApplicationContext
	{
		private Container _components;
		private NotifyIcon _trayIcon;
		private string _defaultTooltip = "Feeds your conversations with Lenny vibe. Ctrl+L to activate";
		private Icon _icon;
		private string _iconPath = "LennyFacer.ico";
		private AppHandleProvider _handle;
		private Hotkey _hotkey;

		private string[] _lennys = new[] {"( ͡° ͜ʖ ͡°)", "(⌐■_■)", "ಠ_ಠ", "¯\\_ツ_/¯"};


		public LennyApplicationContext()
		{
			InitializeContext();
		}

		private void InitializeContext()
		{
			_components = new Container();
			_icon = Icon.ExtractAssociatedIcon(_iconPath);
			_trayIcon = new NotifyIcon(_components)
			{
				Text = _defaultTooltip,
				Icon = _icon,
				Visible = true
			};
			//need to have window handle, w/o actual window
			_handle = new AppHandleProvider();

			_hotkey = new Hotkey();
			_hotkey.Control = true;
			_hotkey.KeyCode = Keys.L;
			_hotkey.Register(_handle.Handle);
			_hotkey.Pressed += OnHotkey;

			MenuItem[] menus = new MenuItem[5];
			for (int i = 0; i < 4; i++)
			{
				string f = _lennys[i];
				menus[i] = new MenuItem(f, (sender, args) => Clipboard.SetText(f));
			}
			menus[4] = new MenuItem("I ain't need you", Exit);
			_trayIcon.ContextMenu = new ContextMenu(menus);

			base.ThreadExit += Exit;
		}

		private void OnHotkey(object sender, HandledEventArgs args)
		{
			Clipboard.SetText(_lennys.Aggregate((a, b) => a+" "+b));
			args.Handled = false;
		}

		private void Exit(object sender, EventArgs args)
		{
			if (_hotkey != null && _hotkey.Registered)
			{
				_hotkey.Unregister();
			}
			if (_handle != null)
			{
				_handle.DestroyHandle();
			}
			_trayIcon.Dispose();
			base.ThreadExit -= Exit;
			base.ExitThread();
		}
	}
}
