using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gyorsulás.Persistence
{
    public class FileManager : IFileManager
    {

        private readonly string _path;
        public FileManager(string path)
        {
            _path = path;
        }
        public string Load()
        {
            try
            {
                return File.ReadAllText(_path);
            }
            catch (Exception e)
            {
                throw new FileManagerException("Error during file loading >~<", e);
            }
        }

        public void Save(string savefile)
        {
            try
            {
                File.WriteAllText(_path, savefile);
            }
            catch (Exception e)
            {

                throw new FileManagerException("Error during file saving >~<", e);

            }
        }
    }
}
