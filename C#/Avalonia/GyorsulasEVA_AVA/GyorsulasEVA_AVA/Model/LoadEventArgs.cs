using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GyorsulasEVA_AVA.Model
{
    public class LoadEventArgs : EventArgs
    {
        private Game gameState;
        public LoadEventArgs(Game currentGameState) { 
            
               gameState = currentGameState;

        }

    }
}
