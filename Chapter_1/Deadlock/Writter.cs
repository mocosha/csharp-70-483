using System;
using System.IO;
using System.Text;

namespace Deadlock
{
    class Writter
    {
        protected string Filepath;
        protected bool Optimistic;
        protected object _locker = new object();

        public Writter(string filePath, bool optimistic = false)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }
            Filepath = filePath;
            Optimistic = optimistic;
        }

        public void Write(string text, Action<string> log = null)
        {
            if (Optimistic)
            {
                WriteToFile(text, log);
            }
            else
            {
                lock (_locker)
                {
                    WriteToFile(text, log);
                }
            }
        }

        protected void WriteToFile(string text, Action<string> log = null)
        {
            try
            {
                Log(log, "Started wtitting...");
                using (FileStream file = new FileStream(Filepath, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write(text);
                }
                Log(log, "Writting finished");
            }
            catch (Exception e)
            {
                Log(log, e.Message);
            }
        }

        protected void Log(Action<string> log, string text)
        {
            if (log != null)
            {
                log.Invoke(text);
            }
        }
    }
}
