using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public class RemoteCommand
    {
        private static readonly Regex CommandSplitRegex = new Regex(@"[:：]", RegexOptions.Compiled);
        private static readonly Regex ParamSplitRegex = new Regex(@"[-_|,， ]", RegexOptions.Compiled);

        public string fullCommand { get; }
        public string commandBody { get; }
        public IReadOnlyList<object> parameters { get; }

        public RemoteCommand(string command)
        {
            fullCommand = command ?? throw new ArgumentNullException(nameof(command));

            var splitResult = CommandSplitRegex.Split(fullCommand);
            commandBody = splitResult.Length > 0 ? splitResult[0] : string.Empty;

            var parameterList = new List<object>();
            if (splitResult.Length > 1)
            {
                var parametersStrings = ParamSplitRegex.Split(splitResult[1])
                    .Where(s => !string.IsNullOrEmpty(s));

                foreach (var param in parametersStrings)
                {
                    if (double.TryParse(param, out var num))
                        parameterList.Add(num);
                    else
                        parameterList.Add(param); // 保留非数字参数
                }
            }

            this.parameters = parameterList.AsReadOnly();
        }
    }
}