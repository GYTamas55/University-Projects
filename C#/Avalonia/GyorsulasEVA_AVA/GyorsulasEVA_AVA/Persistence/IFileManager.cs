using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyorsulasEVA_AVA.Persistence
{
    public interface IFileManager
    {
        //Asynchoz kell Task<string> és Task
        Task<string> LoadAsync(); 
        Task SaveAsync(string savefile);

    }
}
