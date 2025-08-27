using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    internal class RemoteCommandExecutor
    {
        private readonly Dictionary<string, Delegate> executors = new Dictionary<string, Delegate>();

        public void AddExecutor(string command, Action action)
        {
            executors.Add(command, action);
        }

        public void AddExecutor(string command, Action<object> action)
        {
            executors.Add(command, action);
        }

        public void AddExecutor(string command, Action<object, object> action)
        {
            executors.Add(command, action);
        }

        public void AddExecutor(string command, Action<object, object, object> action)
        {
            executors.Add(command, action);
        }

        public void Execute(RemoteCommand command)
        {
            foreach (var executorsKey in executors.Keys)
            {
                if (!command.commandBody.EndsWith(executorsKey)) continue;
                if (!executors.TryGetValue(executorsKey, out var executor)) continue;
                try
                {
                    var expectedParams = executor.Method.GetParameters();
                    var actualParams = command.parameters.ToArray();

                    Debug.Log($"执行命令【{command.commandBody}】：当前参数数量：{actualParams.Length} " +
                              $"需要参数数量：{expectedParams.Length}");

                    if (expectedParams.Length != actualParams.Length)
                    {
                        Debug.LogError($"参数数量不匹配！需要 {expectedParams.Length} 个，但提供了 {actualParams.Length} 个");
                        return;
                    }

                    // 检查参数类型是否匹配（可选）
                    for (int i = 0; i < expectedParams.Length; i++)
                    {
                        if (actualParams[i] != null &&
                            !expectedParams[i].ParameterType.IsInstanceOfType(actualParams[i]))
                        {
                            Debug.LogError($"参数类型不匹配！参数 {i} 需要类型 {expectedParams[i].ParameterType}，" +
                                           $"但实际是 {actualParams[i].GetType()}");
                            return;
                        }
                    }

                    executor.DynamicInvoke(actualParams);
                    Debug.Log($"远程控制器 : 成功执行 {command.fullCommand} 命令！");
                    return;
                }
                catch (Exception e)
                {
                    // 如果是 InvocationException，获取内部异常
                    var actualException = e.InnerException ?? e;
                    Debug.LogError($"远程控制器 : {command.fullCommand} 执行失败！详细错误：{actualException.Message}" +
                                   $"\n调用栈：{actualException.StackTrace}");
                    return;
                }
            }

            Debug.LogWarning($"远程控制器 : {command.fullCommand} 执行失败！未找到对应执行方法。");
        }
    }
}