using System;
using System.Drawing;
using System.Windows.Forms;
using TagsCloud.Infrastructure;

namespace TagsCloud.GUI
{
    public class MainForm : Form
    {
        public MainForm(IUiAction[] actions, PictureBoxImageHolder holder)
        {
            ClientSize = new Size(holder.Settings.Width, holder.Settings.Height);

            var mainMenu = new MenuStrip();
            mainMenu.Items.AddRange(actions.ToMenuItems());
            Controls.Add(mainMenu);

            var vScroller = new VScrollBar {Dock = DockStyle.Right, Visible = false};
            vScroller.Scroll += (sender, args) =>
                holder.Location = new Point(holder.Location.X, -vScroller.Value);
            Controls.Add(vScroller);

            var hScroller = new HScrollBar {Dock = DockStyle.Bottom, Visible = false};
            hScroller.Scroll += (sender, args) => 
                holder.Location = new Point(-hScroller.Value, holder.Location.Y);
            Controls.Add(hScroller);

            holder.Invalidated += (sender, args) => UpdateScrollers(holder, vScroller, hScroller);
            SizeChanged += (sender, args) => UpdateScrollers(holder, vScroller, hScroller);

            holder.RecreateCanvas(holder.Settings);
            Controls.Add(holder);
            holder.UpdateUi();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Text = "Tag Cloud Visualizer";
        }

        private void UpdateScrollers(PictureBoxImageHolder holder, VScrollBar vertical, HScrollBar horizontal)
        {
            if (holder.Image.Width > ClientSize.Width)
            {
                horizontal.Visible = true;
                horizontal.Maximum = holder.Image.Width - ClientSize.Width;
            }
            else
                horizontal.Visible = false;

            if (holder.Image.Height > ClientSize.Height)
            {
                vertical.Visible = true;
                vertical.Maximum = holder.Image.Height - ClientSize.Height;
            }
            else
                vertical.Visible = false;
        }
    }
}
