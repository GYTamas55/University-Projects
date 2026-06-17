using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//IDE MEHET AZ OSZTÁLYOK MŰKÖDÉSE
namespace Gyorsulás.Model
{
    public class Motor
    {
        #region Properties 
        //Contains all the properties for Motor
        public double _posX { get; set; }
        public double _posY { get; set; } = 0.8f;
        public float _width { get; set; } = 0.10f;
        public double _height { get; set; } = 0.10f;

        public double _fuel {  get; set; }
        public double _maxFuel {  get; set; } = 100;
        


        #endregion

        #region Public methods
        //Contains all the public methods for Motor
        
        //Constructor
        public Motor(double initialPositionX, double initialFuel)
        {
            
            _posX = initialPositionX;
            _fuel = initialFuel;

        }


        public void CalcTankAmount(double fuelAdd) //Always 10
        {
            _fuel = Math.Clamp(_fuel + fuelAdd, 0, _maxFuel); //Adds the 'inAmount' to '_fuel', but does not go over '_maxFuel'
        }
        
        public void ReduceTankAmount(double reduceAmount)
        {
            _fuel = Math.Clamp(_fuel - reduceAmount, 0, _maxFuel); //Reduces the 'fuelAmount' by 'outAmount', but does not go under '_maxFuel'
        }
        
        public void Move(WhereToMove inDirection)
        {
            //Using switch to maybe implement more directions in the future 
            switch (inDirection)
            {
                case WhereToMove.Left:
                    _posX -= 0.02;
                    break;
                case WhereToMove.Right:
                    _posX += 0.02; 
                    break;
            }
            _posX = Math.Clamp(_posX, 0.0, 1.0 - _width); //Stay in bounds
        }
        #endregion

    }
}
