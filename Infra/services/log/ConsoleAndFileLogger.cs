using System;


using System.IO;


namespace Infra.services.log
{
    public class ConsoleAndFileLogger : ConsoleLogger
    {
        private StreamWriter Writer;

        public ConsoleAndFileLogger(StreamWriter writer)
        {
            this.Writer = writer;
            this.Writer.AutoFlush = true;
        }

        public override void Write(string msg)
        {
            base.Write(msg);
            this.Writer.Write(msg);
        }
        public override void WriteLine(string msg)
        {
            base.WriteLine(msg);
            this.Writer.WriteLine(msg);
        }
        public override void WriteLine()
        {
            base.WriteLine();
            this.Writer.WriteLine("");
        }
        public override void WriteError(string msg)
        {
            base.WriteError(msg);
            this.Writer.WriteLine(msg);
        }
        public override void WriteError(Exception e)
        {
            base.WriteError(e);
            this.Writer.WriteLine(e.Message);
            this.Writer.WriteLine(e.StackTrace);
        }
    }
}