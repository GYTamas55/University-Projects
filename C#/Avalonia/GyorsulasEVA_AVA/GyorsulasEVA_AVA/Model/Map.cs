using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyorsulasEVA_AVA.Model
{
    public class Map : IMove 
    {

        //IMPORTANT!!44!!!!4!!
        //We do not draw in the model part of the porgramme bcs it contains only the logic
        public double _posX { get; set; } = 0.5;
        public double _posY { get; set; }
        public double _width { get; set; } = 0.05;
        public double _height { get; set; } = 0.8;
        
        public Map(double beY) 
        {
            _posY = beY;
        }

        public void Move(double initialSpeed)
        {
            _posY += initialSpeed; //Move down the bg by speed
            if (_posY >= 1) //If it goes out of bounds, let it go back up
            {
                _posY = -1;
            }

        }
    }
    
}
