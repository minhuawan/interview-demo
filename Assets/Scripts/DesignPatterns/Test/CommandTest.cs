using DesignPatterns.Command;
using UnityEngine;

namespace DesignPatterns.Test
{
    public class CommandTest : ITestRunner
    {
        public void Run(object args)
        {
            CommandCenter.Register(new CommandTestClass());
            CommandCenter.Execute<CommandTestClass>("Hello, there is TestCommand");
        }
    }

    public class CommandTestClass : Command.Command
    {
        public override void Execute(object args)
        {
            string message = args as string;
            Debug.Log("CommandTestClass receive " + message);
        }
    }
}