﻿using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Host.Core;

namespace Calculator
{
    /// <summary>
    /// 計算処理の実体です。
    /// </summary>
    /// <remarks>このクラスは計算処理のみ行います。ホストの入出力はICalculatorHostインターフェイスを通じて間接的に実行します。</remarks>
    public static class CalculatorImplementation
    {
        /// <summary>
        /// 指定されたホストインターフェースを使用して、計算を実行します。
        /// </summary>
        /// <returns>タスク</returns>
        public static async Task CalculateAsync()
        {
            // 入出力を司るInteractionクラスを生成する
            using (var interaction = new Interaction())
            {
                while (true)
                {
                    var line = await interaction.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    line = line.Trim();
                    if (line == "exit")
                    {
                        break;
                    }

                    var tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    try
                    {
                        var value0 = double.Parse(tokens[0]);
                        var oper = tokens[1];
                        var value1 = double.Parse(tokens[2]);

                        double result;
                        switch (oper)
                        {
                            case "+":
                                result = value0 + value1;
                                break;
                            case "-":
                                result = value0 - value1;
                                break;
                            case "*":
                                result = value0 * value1;
                                break;
                            case "/":
                                result = value0 / value1;
                                break;
                            case "%":
                                result = value0 % value1;
                                break;
                            default:
                                throw new ArgumentException("Unknown operator", oper);
                        }

                        await interaction.WriteLineAsync(
                            "Result={0}",
                            result);
                    }
                    catch (Exception ex)
                    {
                        await interaction.WriteLineAsync(
                            "Error: {0}: {1}",
                            Marshal.GetHRForException(ex),
                            ex.Message);
                    }
                }
            }
        }
    }
}
