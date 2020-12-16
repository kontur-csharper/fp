using Autofac;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TagsCloudContainer;

namespace TagsCloudCreatorGUI
{
    public partial class Form1 : Form
    {
        public TagsCloudCreator creator;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetCreator();
            var project_path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(
                Path.GetDirectoryName(Directory.GetCurrentDirectory()))), "TagsCloudContainer");
            inputFilePath.Text = Path.Combine(project_path, "input.txt");
            targetImagePath.Text = project_path;
            textBoxName.Text = "TagCloud";
            textBoxFontFamily.Text = "Arial";
            textBoxColor.Text = "Black";
            textBoxImageSize.Text = "300";
            textBoxImageFormat.Text = "png";
        }

        private void SetCreator()
        {
            creator = Configurator.GetContainer()
                .Then(cont => cont.BeginLifetimeScope())
                .Then(scope => scope.Resolve<TagsCloudCreator>())
                .RefineError("DI Container не смог инициализировать TagsCloudCreator")
                .OnFail(err =>
                {
                    ShowError(err);
                    Close();
                })
                .Value;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void completeFontFamily_Click(object sender, EventArgs e)
        {
            LogIfSuccess(creator.SetFontFamily(textBoxFontFamily.Text).OnFail(err => ShowError(err)),
                "Установлен шрифт: " + textBoxFontFamily.Text);
        }

        private void imageBox_Click(object sender, EventArgs e)
        {

        }

        private void createButton_Click(object sender, EventArgs e)
        {
            var fullImagePath = Path.Combine(targetImagePath.Text, textBoxName.Text + "." + creator.GetImageFormat());
            if (CheckPathFields().OnFail(err => ShowError(err)).IsSuccess)
            {
                if (File.Exists(fullImagePath) && !AskForRewriting(fullImagePath))
                        return;
                if (creator.Create(inputFilePath.Text, targetImagePath.Text, textBoxName.Text)
                    .OnFail(err => ShowError(err)).IsSuccess)
                {
                    imageBox.Image = Image.FromFile(fullImagePath);
                    log.Text = "Облако успешно сгенерировано";
                }
            }
        }

        private Result<None> CheckPathFields()
        {
            if (!Directory.Exists(targetImagePath.Text))
                return Result.Fail<None>("Неверный путь назначения");
            if (!File.Exists(inputFilePath.Text))
                return Result.Fail<None>("По данному пути не найден текстовый файл");
            if (textBoxName.Text == "")
                return Result.Fail<None>("Введите имя файла");
            return Result.Ok();
        }

        private bool AskForRewriting(string fullImagePath)
        {
            var result = MessageBox.Show("Изображение с таким именем уже существует, желаете его перезаписать?",
                "Предупреждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (imageBox.Image != null)
                    imageBox.Image.Dispose();
                File.Delete(fullImagePath);
                return true;
            }

            return false;
        }

        private void RemoveStopWord_Click(object sender, EventArgs e)
        {
            if (textBoxStopWord.Text != "")
                LogIfSuccess(creator.RemoveStopWord(textBoxStopWord.Text).OnFail(err => ShowError(err)),
                    "Стопслово " + textBoxStopWord.Text + " удалено");
        }

        private void addStopWord_Click(object sender, EventArgs e)
        {
            if (textBoxStopWord.Text != "")
                LogIfSuccess(creator.AddStopWord(textBoxStopWord.Text).OnFail(err => ShowError(err)),
                    "Стопслово " + textBoxStopWord.Text + " добавлено");
        }

        private void completeColor_Click_1(object sender, EventArgs e)
        {
            if (textBoxColor.Text != "")
                LogIfSuccess(creator.SetFontColor(textBoxColor.Text).OnFail(err => ShowError(err)),
                    "Установлен цвет: " + textBoxColor.Text);
        }

        private void completeImageSize_Click(object sender, EventArgs e)
        {
            if (textBoxImageSize.Text != "")
            {
                int size;
                if (int.TryParse(textBoxImageSize.Text, out size))
                    LogIfSuccess(creator.SetImageSize(size).OnFail(err => ShowError(err)),
                        "Установлен размер: " + textBoxImageSize.Text + " на " + textBoxImageSize.Text);
                else
                    ShowError("Размер должен быть целим числом");
            }
        }

        private void completeImageFormat_Click(object sender, EventArgs e)
        {
            if (textBoxImageFormat.Text != "")
                LogIfSuccess(creator.SetImageFormat(textBoxImageFormat.Text).OnFail(err => ShowError(err)),
                    "Установлен формат: " + textBoxImageFormat.Text);
        }

        private void checkBoxRandomColor_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox) sender).Checked)
            {
                textBoxColor.Enabled = false;
                completeColor.Enabled = false;
                LogIfSuccess(creator.SetFontRandomColor().OnFail(err => ShowError(err)),
                    "Установлен рандомный цвет");
            }
            else
            {
                textBoxColor.Enabled = true;
                completeColor.Enabled = true;
                LogIfSuccess(creator.SetFontColor("Black").OnFail(err => ShowError(err)),
                    "Установлен цвет: Black");
            }
        }

        private void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Ошибка");
        }

        private void LogIfSuccess<T>(Result<T> result, string message)
        {
            if (result.IsSuccess)
                log.Text = message;
        }
    }
}
