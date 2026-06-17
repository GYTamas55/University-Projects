using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GyorsulasEVA_WPF.Persistence
{
    public class FileManagerException : IOException
    {
        public FileManagerException()
        {
        }
        public FileManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
