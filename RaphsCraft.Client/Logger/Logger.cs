using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaphsCraft.Client.Logger;

public enum LogType
{
    Info,
    Warning,
    Error,
    Crash
}

public class Logger
{
    public string Name { get; set; }

    public void Log(LogType lt, string message)
    {
        Console.Write(DateTime.Now.ToString());
        Console.Write("/");
        Console.Write(lt);
        Console.Write("/" + Thread.CurrentThread.Name + ":" + Name + "/ ");
        Console.WriteLine(message);
    }
}
