using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyorsulás.Persistence
{
    public interface IFileManager
    {
        string Load();
        void Save(string savefile);

    }
}
