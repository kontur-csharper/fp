using System.Collections.Generic;
using TagsCloud.GUI;
using TagsCloud.WordsProcessing;

namespace TagsCloud.UiActions
{
    public class ParserSettingsAction : IUiAction
    {
        private readonly ExcludingWordsConfigurator configurator;
        public ParserSettingsAction(ExcludingWordsConfigurator configurator)
        {
            this.configurator = configurator;
        }

        public string Category => "Настройки";
        public string Name => "Настройка отбора слов";
        public string Description => "Настройка отбора слов";

        public void Perform()
        {
            new ParserSettingsForm(configurator).Show();
        }
    }
}
