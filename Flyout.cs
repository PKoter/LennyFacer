using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BW
{
	public class Flyout : Form
	{
		private Button         _add;
		private TextBox        _faceInput;
		private Action<string> _callback;

		public static void Show(Action<string> inputCallback)
		{
			Flyout flyout = new Flyout()
			{
				FormBorderStyle = FormBorderStyle.FixedDialog,
				StartPosition = FormStartPosition.Manual,
			};
			flyout.InitializeComponent();
			Rectangle r = Screen.PrimaryScreen.Bounds;
			flyout.Location = new Point(r.Width - 190, r.Height - 140);
			flyout.Visible = true;

			flyout._callback = inputCallback;
			flyout.LostFocus += (sender, args) => { flyout.Close(); };
		}

		private void InitializeComponent()
		{
			var resources = new System.ComponentModel.ComponentResourceManager(typeof(Flyout));
			_faceInput = new TextBox();
			_add = new Button();
			SuspendLayout();
			// 
			// faceInput
			// 
			_faceInput.Dock = DockStyle.Fill;
			_faceInput.Font = new Font("Microsoft Sans Serif", 10F);
			_faceInput.Location = new Point(0, 0);
			_faceInput.Margin = new Padding(5, 5, 5, 4);
			_faceInput.MaxLength = 20;
			_faceInput.Name = "_faceInput";
			_faceInput.Size = new Size(180, 26);
			_faceInput.TabIndex = 0;
			_faceInput.WordWrap = false;
			// 
			// Add
			// 
			_add.Location = new Point(0, 33);
			_add.Name = "_add";
			_add.Size = new Size(182, 35);
			_add.TabIndex = 1;
			_add.Text = "Add";
			_add.UseVisualStyleBackColor = true;
			_add.Click += new EventHandler(OnAdded);
			// 
			// Flyout
			// 
			ClientSize = new Size(180, 70);
			Controls.Add(_add);
			Controls.Add(_faceInput);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "Flyout";
			Opacity = 0.75D;
			Text = "Add face";
			ResumeLayout(false);
			PerformLayout();

		}

		private void OnAdded(object sender, EventArgs e)
		{
			var s = _faceInput.Text.Trim();
			if (s.Length >= 3)
			{
				_callback.Invoke(s);
				Close();
			}
		}
	}
}
