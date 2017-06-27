using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
		private string _resFileName = "faces.txt";
		private AppHandleProvider _handle;
		private Hotkey _hotkey;

		private string[] _lennys = new[] {"( ͡° ͜ʖ ͡°)", "(⌐■_■)", "ಠ_ಠ", "¯\\_ツ_/¯"};

		public LennyApplicationContext()
		{
			StartupManageFaces();
			InitializeContext();
		}

		/// <summary>
		/// Loads faces.txt with lenny faces
		/// </summary>
		private void StartupManageFaces()
		{
			if (File.Exists(_resFileName) == false)
			{
				using (var writer = File.CreateText(_resFileName))
				{
					for (int i = 0; i < _lennys.Length -1; i++)
					{
						writer.WriteLine(_lennys[i]);
					}
					writer.Write(_lennys[_lennys.Length -1]);
				}
			}
			else
			{
				_lennys = File.ReadAllLines(_resFileName);
			}
		}

		private void CreateMenu()
		{
			MenuItem[] menus = new MenuItem[_lennys.Length + 2];
			for(int i = 1; i <= _lennys.Length; i++)
			{
				string f = _lennys[i -1];
				menus[i] = new MenuItem(f, (sender, args) => Clipboard.SetText(f));
			}
			menus[0]              = new MenuItem("Add face", OnFaceAdd);
			menus[menus.Length-1] = new MenuItem("I ain't need you", Exit);
			_trayIcon.ContextMenu = new ContextMenu(menus);
		}

		private void OnFaceAdd(object o, EventArgs eventArgs)
		{
			Flyout.Show(AddFace);
		}

		private void AddFace(string face)
		{
			if (_lennys.Contains(face) == false)
			{
				int l = _lennys.Length + 1;
				Array.Resize(ref _lennys, l);
				_lennys[l - 1] = face;
				CreateMenu();
				using (var writer = File.AppendText(_resFileName))
				{
					writer.WriteLine();
					writer.Write(face);
				}
			}
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

			CreateMenu();

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
