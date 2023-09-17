using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Command
{
    public abstract class Command
    {
        public abstract void Execute(object args);
    }

    public static class CommandCenter
    {
        private static Dictionary<Type, Command> commands = new Dictionary<Type, Command>();

        public static void Register<T>(T cmd) where T : Command
        {
            Debug.Assert(commands.ContainsKey(typeof(T)), "commands.ContainsKey(typeof(T))");
            commands[typeof(T)] = cmd;
        }

        public static void Unregister<T>() where T : Command
        {
            commands.Remove(typeof(T));
        }

        public static void Execute<T>(object args) where T : Command
        {
            if (commands.TryGetValue(typeof(T), out Command command))
            {
                command.Execute(args);
            }
        }
    }
}