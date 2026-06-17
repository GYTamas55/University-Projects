using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GyorsulasEVA_WPF.Persistence
{
    public class FileManager : IFileManager
    {

        private readonly string _path;
        public FileManager(string path)
        {
            _path = path;
        }

        //Async kell mert async dolgok történnek és ezért await-et akarok használni majd
        //Async metódus -> Task
        //Async dolgok = háttérben futás
        public async Task<string> LoadAsync()
        {
            try
            {
                //using mert nem kell ilyenkoro manuálisan bezárni
                using (StreamReader reader = File.OpenText(_path))
                {
                    //await mert elkezdi a műveletet de addig nem foglalja le az egész programot
                    return await reader.ReadToEndAsync(); //ReadToEndAsync mert ez nem blokkolja
                }
                //automatikus bezárás ittennne a using miatt
            }
            catch (Exception e)
            {
                throw new FileManagerException("Error during file loading >~<", e);
            }
        }

        public async Task SaveAsync(string savefile)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_path))
                {
                    await writer.WriteAsync(savefile);
                }
            }
            catch (Exception e)
            {

                throw new FileManagerException("Error during file saving >~<", e);

            }
        }
    }
}
