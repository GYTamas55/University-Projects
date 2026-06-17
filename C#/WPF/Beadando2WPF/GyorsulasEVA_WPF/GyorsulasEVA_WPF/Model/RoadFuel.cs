using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GyorsulasEVA_WPF.Model
{
    public class RoadFuel : IMove
    {
        //This is the fuel can that generates on the road randomly
        //It should raise the '_fuel' of the Motor if the Motor touches it
        //It generates randomly on the road
        //It moves faster as the game goes on

        #region Properties
        //Contains all the properties for RoadFuel
        public double _posX { get; set; }
        public double _posY { get; set; }
        public double _width { get; set; } = 0.1;
        public double _height { get; set; } = 0.1;
        private Random rnd = new Random();
        #endregion

        #region Custom Events
        public event EventHandler<CollectEventArgs>? FuelCollected; //When the fuel is collected this happens?


        #endregion
        public RoadFuel(double beY)
        {            
            double tmp = rnd.NextDouble() * 0.8;
            _posX = tmp;
            _posY = beY;
        }
        public RoadFuel(double beX, double beY)
        {
            _posX = beX;
            _posY = beY;
        }

        #region Methods


        public void Move(double initialSpeed)
        {
            double rndX = rnd.NextDouble() * 0.9;
     
            _posY += initialSpeed; //Move down the bg by speed
            if (_posY >= 1) //If it goes out of bounds, let it go back up
            {
                _posX = rndX;
                _posY = -1;
            }
        }

        public void ResetPos() 
        {
            _posX = rnd.NextDouble() * 0.9;
            _posY = -1;
        }
        public void OnFuelCollected()
        {
            FuelCollected?.Invoke(this, new CollectEventArgs(10));
            ResetPos();
        }
        #endregion


    }
}

