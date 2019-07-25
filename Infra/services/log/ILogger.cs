using System;


namespace Infra.services.log
{
    public interface ILogger
    {
        void Write(string msg);
        void WriteLine(string msg);
        void WriteLine();
        void WriteError(string msg);
        void WriteError(Exception e);
    }
}