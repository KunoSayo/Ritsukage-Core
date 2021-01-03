﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ritsukage.Commands
{ 

    public class CommandArgs
    {
        string rawInput;
        List<string> singleArg;
        private int index = 0;
        /// <summary>
        /// 将参数部分拆分成args部分
        /// 注意不支持转义
        /// 字符串引用需要为'...'或者"..." 且不能有任意'或者"干扰
        /// 
        /// </summary>
        /// <param name="rawInput">args部分【不带空格情况】</param>
        public CommandArgs(string rawInput)
        {
            this.rawInput = rawInput;
            this.singleArg = new();
            int len = rawInput.Length;
            int i = 0;
            var sb = new StringBuilder();
            while (i < len)
            {
                char c = rawInput[i];
                switch (c) {
                    case ' ':
                        {
                            this.singleArg.Add(sb.ToString());
                            sb = new();
                            break;
                        }
                    case '\'':
                        {
                            if (sb.Length == 0) {
                                i += 1;
                                while (i < len && rawInput[i] != '\'')
                                {
                                    sb.Append(rawInput[i]);
                                    i += 1;
                                }
                            } else sb.Append(c);
                           
                            break;
                        }
                    case '"':
                        {
                            if (sb.Length == 0)
                            {
                                i += 1;
                                while (i < len && rawInput[i] != '"')
                                {
                                    sb.Append(rawInput[i]);
                                    i += 1;
                                }
                            }
                            else sb.Append(c);
                            break;
                        }
                    default:
                        {
                            sb.Append(c);
                            break;
                        }
                }
                i += 1;
            }
            if (sb.Length > 0) {
                singleArg.Add(sb.ToString());
            }
        }

        public string Next()
        {
            string s = this.singleArg[this.index];
            this.index += 1;
            return s;
        }
    }
    interface ICommandParser
    {
        object Parse(CommandArgs args);
    }

    public class Command
    {
        string startHeader = "/";
        string name;
        internal MethodInfo method;
        internal Type[] argTypes;
        public delegate void FailedCallback();
        internal FailedCallback failedCallback;

        internal Command(string s, string n, MethodInfo method, Type[] args, FailedCallback cb)
        {
            this.startHeader = s;
            this.name = n;
            this.method = method;
            this.argTypes = args;
            this.failedCallback = cb;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandInfo : System.Attribute
    {
        internal string startHeader = "/";
        internal string name;
        internal string[] alias;
        internal Command.FailedCallback failedCallback;

        public string StartHeader { get { return startHeader; } set { startHeader = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string[] Alias { get => alias; set { alias = value; } }
        public Command.FailedCallback FailedCallback { get => failedCallback; set { failedCallback = value; } }
    }
    public static class CommandManager
    {
        static Dictionary<Type, Ritsukage.Commands.ICommandParser> parsers;
        static Dictionary<string, Dictionary<string, Command>> commands = new();

        private static Dictionary<string, Command> getFromHeader(string header)
        {
            if (commands.TryGetValue(header, out var value))
            {
                return value;
            } else
            {
                var d = new Dictionary<string, Command>();
                commands.Add(header, d);
                return d;
            }
        }
        private static object ParseArgument(Type type, CommandArgs args)
        {
            Exception e = null;
            if(parsers.TryGetValue(type, out ICommandParser parser))
            {
                try
                {
                    return parser.Parse(args);
                } catch (Exception ee)
                {
                    e = ee;
                }
            }
            if (type == typeof(int))
            {
                return int.Parse(args.Next());
            } else if (type == typeof(bool))
            {
                string s = args.Next().ToLower();
                if (s == "真" || s == "true" || s == "t")
                {
                    return true;
                }
                else if (s == "假" || s == "false" || s == "f")
                {
                    return false;
                }
                else throw new ArgumentException($"{s} is not bool value.");
            } else if (type == typeof(string))
            {
                return args.Next();
            }

            throw new ArgumentException($"the type of {type} cannot be parsed from string", e);
        }

        public static void RegisterAllCommands(Type type)
        {
            foreach (var method in type.GetMethods())
            {
                var attrs = method.GetCustomAttribute<CommandInfo>();
                if (attrs != null)
                {
                    var ps = method.GetParameters();
                    var ts = new Type[ps.Length];
                    for (int i = 0; i < ps.Length; ++i)
                    {
                        ts[i] = ps[i].ParameterType;
                    }
                    var command = new Command(attrs.startHeader, attrs.name, method, ts, attrs.failedCallback);

                    var d = getFromHeader(attrs.startHeader);
                    if (attrs.alias != null)
                    {
                        foreach (var n in attrs.alias)
                        {
                            d.Add(n, command);
                        }
                    }
                    d.Add(attrs.name, command);
                }
            }
        }

        public static object? ReceiveMessage(string msg)
        {
            foreach (var node in commands)
            {
                if (msg.StartsWith(node.Key))
                {
                    var caa = msg.Substring(node.Key.Length).Split(" ", 2);
                    if(node.Value.TryGetValue(caa[0], out var command))
                    {
                        var args = new CommandArgs(caa.Length == 2 ? caa[1] : "");
                        object[] ps = new object[command.argTypes.Length];
                        try
                        {
                            for (int i = 0; i < ps.Length; ++i)
                            {
                                ps[i] = ParseArgument(command.argTypes[i], args);
                            }
                        } catch (Exception e)
                        {
                            if (command.failedCallback != null)
                            {
                                command.failedCallback();
                            }
                            return null;
                        }
                        return command.method.Invoke(null, ps);
                    }
                    return null;
                }
            }
            return null;
        }


        static CommandManager()
        {
            parsers = new();
        }
    }
}
