using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Gyorsulás.Model
{
    internal interface IMove //Haha get it? I Move.... haha
    {
        #region Properties
        //Contains all the properties for IMove
        public double _posX { get; set; }
        public double _posY { get; set; }
        public double _width { get; set; }
        public double _height { get; set; }
        #endregion

        public void Move(double speed);
   
    }
}
