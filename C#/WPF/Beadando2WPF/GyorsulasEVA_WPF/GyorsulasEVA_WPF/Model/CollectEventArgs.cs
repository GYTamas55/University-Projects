using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyorsulasEVA_WPF.Model
{
    //To know how much we should add to fuel (CollectEventArgs)
    public class CollectEventArgs : EventArgs
    {
        public double FuelAdd { get; } 

        public CollectEventArgs(double fuel)
        {
            FuelAdd = fuel;
        }
    }
}
