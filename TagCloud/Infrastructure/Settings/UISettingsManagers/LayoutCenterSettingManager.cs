using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class LayoutCenterSettingManager : IMultiInputModifierManager
    {
        private readonly Regex regex;
        private readonly Func<ISpiralSettingsProvider> settingsProvider;
        private readonly Dictionary<string, Dictionary<string, Action>> modifiers;

        public LayoutCenterSettingManager(Func<ISpiralSettingsProvider> settingsProvider)
        {
            this.settingsProvider = settingsProvider;
            regex = new Regex(@"^(?<x>\d+)\s+(?<y>\d+)$");
            modifiers = new Dictionary<string, Dictionary<string, Action>>
            {
                {
                    "X", new Dictionary<string, Action>
                    {
                        {
                            "-10", () => settingsProvider().Center += new Size(-10, 0)
                        },
                        {
                            "-1", () => settingsProvider().Center += new Size(-1, 0)
                        },
                        {
                            "+1", () => settingsProvider().Center += new Size(1, 0)
                        },
                        {
                            "+10", () => settingsProvider().Center += new Size(10, 0)
                        }
                    }
                },
                {
                    "Y", new Dictionary<string, Action>
                    {
                        {
                            "-10", () => settingsProvider().Center += new Size(0, -10)
                        },
                        {
                            "-1", () => settingsProvider().Center += new Size(0, -1)
                        },
                        {
                            "+1", () => settingsProvider().Center += new Size(0, +1)
                        },
                        {
                            "+10", () => settingsProvider().Center += new Size(0, +10)
                        }
                    }
                }
            };
        }

        public string Title => "Layout Center";
        public string Help => "Choose where you want to see a layout. Point is counting from top left corner";

        public Result<string> TrySet(string input)
        {
            var match = regex.Match(input);
            if (!match.Success)
                return Result.Fail<string>("Incorrect input format ([x], [y])");
            settingsProvider().Center = new Point(
                int.Parse(match.Groups["x"].Value),
                int.Parse(match.Groups["y"].Value));
            return Get();
        }

        public string Get()
        {
            var settings = settingsProvider();
            return $"{settings.Center.X} {settings.Center.Y}";
        }

        public Dictionary<string, IEnumerable<string>> GetModifiers()
        {
            return modifiers.ToDictionary(
                kv => kv.Key,
                kv => kv.Value.Select(pair => pair.Key)
            );
        }

        public Result<string> ApplyModifier(string type, string modifier)
        {
            return Result.Of(() =>
            {
                modifiers[type][modifier]();
                return "";
            }).ReplaceError(s => "Cannot apply modifier");
        }
    }
}