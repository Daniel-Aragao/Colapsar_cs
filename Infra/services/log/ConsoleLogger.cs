using System;


namespace Infra.services.log
{
    public class ConsoleLogger : ILogger
    {
        public virtual void Write(string msg)
        {
            Console.Write(msg);
        }

        public virtual void WriteLine(string msg)
        {
            Console.WriteLine(msg);            
        }

        public virtual void WriteLine()
        {
            Console.WriteLine();
        }

        public virtual void WriteError(string msg)
        {
            Console.Error.WriteLine(msg);
        }
        
        public virtual void WriteError(Exception e)
        {
            Console.Error.Write(e);
        }
    }
}