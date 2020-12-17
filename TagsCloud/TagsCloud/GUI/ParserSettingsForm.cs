using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloud.WordsProcessing;

namespace TagsCloud.GUI
{
    class ParserSettingsForm : Form
    {
        public ParserSettingsForm(ExcludingWordsConfigurator configurator)
        {
            Text = "Настройка отбора слов";
            Size = new Size(300, 350);
            FormBorderStyle = FormBorderStyle.FixedDialog;

            var ignoredWords = new ListBox {Height = 250, Width = Size.Width};
            Shown += (sender, args) =>
            {
                ignoredWords.Items.Clear();
                ignoredWords.Items.AddRange(configurator.ExcludedWords?.ToArray());
            };
            Controls.Add(ignoredWords);

            var wordToExclude = new TextBox{Dock = DockStyle.Bottom};
            Controls.Add(wordToExclude);

            var addButton = new Button {Text = "Добавить слово для исключения",Dock = DockStyle.Bottom};
            addButton.Click += (sender, args) =>
            {
                if(string.IsNullOrEmpty(wordToExclude.Text))
                    return;

                if (!configurator.ExcludedWords.Contains(wordToExclude.Text))
                {
                    ignoredWords.Items.Add(wordToExclude.Text.ToLower());
                    configurator.ExcludedWords.Add(wordToExclude.Text.ToLower());
                }
                else
                    MessageBox.Show("Данное слово уже исключено");
                wordToExclude.Text = string.Empty;
            };
            Controls.Add(addButton);

            var deleteButton = new Button {Text = "Удалить выбранное слово", Dock = DockStyle.Bottom};
            deleteButton.Click += (sender, args) =>
            {
                configurator.ExcludedWords.Remove((string)ignoredWords.SelectedItem);
                ignoredWords.Items.Remove(ignoredWords.SelectedItem);
            };
            Controls.Add(deleteButton);



            
        }
    }
}
