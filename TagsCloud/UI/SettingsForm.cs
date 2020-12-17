using System;
using System.Drawing;
using System.Windows.Forms;
using TagsCloud.Infrastructure;

namespace TagsCloud.UI
{
    public class SettingsForm<TSettings> : Form
    {
        private readonly PropertyGrid propertyGrid;

        public SettingsForm(TSettings settings)
        {
            var okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom
            };
            propertyGrid = new PropertyGrid
            {
                SelectedObject = settings,
                Dock = DockStyle.Fill
            };
            propertyGrid.PropertyValueChanged += PropertyGridOnPropertyValueChanged;
            Controls.Add(okButton);
            Controls.Add(propertyGrid);

            AcceptButton = okButton;
        }

        private void PropertyGridOnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (propertyGrid.SelectedObject is ImageSize && (int) e.ChangedItem.Value < 0)
                e.ChangedItem.PropertyDescriptor.SetValue(propertyGrid.SelectedObject, e.ChangedItem.Value);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Настройки";
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // SettingsForm
            // 
            ClientSize = new Size(284, 261);
            Name = "SettingsForm";
            ResumeLayout(false);
        }
    }
}