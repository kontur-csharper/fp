using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ResultOf;
using TagCloud.Settings;

namespace TagCloud.Commands
{
    public class CloudSettingsCommand : ICommand
    {
        private readonly CloudSettings cloudSettings;

        private readonly Dictionary<string, Func<CloudSettings, string, object>> setters
            = new Dictionary<string, Func<CloudSettings, string, object>>
            {
                {
                    nameof(CloudSettings.InnerColor),
                    (settings, color) => settings.InnerColor = ColorTranslator.FromHtml($"#{color}")
                },
                {
                    nameof(CloudSettings.OuterColor),
                    (settings, color) => settings.OuterColor = ColorTranslator.FromHtml($"#{color}")
                },
                {
                    nameof(CloudSettings.InnerColorRadius),
                    (settings, data) => Parse(data)
                        .Then(ShouldBeNonNegative)
                        .Then(radius => settings.InnerColorRadius = radius).GetValueOrThrow()
                },
                {
                    nameof(CloudSettings.OuterColorRadius),
                    (settings, data) => Parse(data)
                        .Then(ShouldBeNonNegative)
                        .Then(radius => settings.OuterColorRadius = radius).GetValueOrThrow()
                },
                {
                    nameof(CloudSettings.StartRadius),
                    (settings, data) => Parse(data)
                        .Then(ShouldBeNonNegative)
                        .Then(radius => settings.StartRadius = radius).GetValueOrThrow()
                },
                {
                    "FontName",
                    (settings, fontName) => settings.Font = new Font(new FontFamily(fontName), settings.Font.Size)
                }
            };

        public CloudSettingsCommand(CloudSettings cloudSettings)
        {
            this.cloudSettings = cloudSettings;
            Usage = $"{CommandId} <property> <data>, or without args";
        }

        public string CommandId { get; } = "cs";
        public string Description { get; } = "It is the cloud settings.\nAllows to set the specify cloud settings";
        public string Usage { get; }

        public ICommandResult Handle(string[] args)
        {
            if (args.Length == 0)
                return AllProperties();
            if (args.Length != 2)
                return CommandResult.WithNoArgs();
            var property = args[0];
            var value = args[1];

            var result = GetSetter(property)
                .Then(setter => setter(cloudSettings, value))
                .Then(obj => new CommandResult(true, $"{property} now is {obj}"));

            return result.IsSuccess
                ? result.GetValueOrThrow()
                : new CommandResult(false, result.Error);
        }

        private Result<Func<CloudSettings, string, object>> GetSetter(string property)
        {
            if (!setters.TryGetValue(property, out var setter))
                return Result.Fail<Func<CloudSettings, string, object>>(
                    "Property doesn't exists.\nYou can set the following properties:\n" +
                    string.Join("\n", setters.Select(x => x.Key))
                );
            return Result.Ok(setter);
        }

        private ICommandResult AllProperties()
        {
            var type = cloudSettings.GetType();
            var builder = new StringBuilder();
            builder.AppendLine("Cloud settings:");
            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(cloudSettings);
                builder.Append(property.Name);
                builder.Append($" [{property.PropertyType.Name}]");
                builder.Append(": ");
                builder.AppendLine(value != null ? value.ToString() : "null");
            }

            return new CommandResult(true, builder.ToString());
        }

        private static Result<T> Validate<T>(T value, Func<T, bool> predicate, string message)
        {
            return predicate(value) ? Result.Ok(value) : Result.Fail<T>(message);
        }

        private static Result<double> ShouldBeNonNegative(double value)
        {
            return Validate(value, v => v >= 0, "Should be non negative");
        }

        private static Result<double> Parse(string data)
        {
            return double.TryParse(data, out var value)
                ? Result.Ok(value)
                : Result.Fail<double>("Couldn't parse to double");
        }
    }
}
