using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GyorsulasEVA_AVA.Model
{
    public interface ITimer //Copied from the blueprint
    {
        public event EventHandler? Tick;
        public void Start();
        public void Stop();
    }
}
