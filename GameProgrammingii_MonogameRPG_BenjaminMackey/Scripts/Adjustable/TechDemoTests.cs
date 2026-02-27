
using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Adjustable;
using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    public class TechDemoTests : Game
    {
        public void testMethod()
        {

            GameObject mapObj = new GameObject();
            mapObj.AddTag("map");
            
            Map map = new Map("Map", 10000);
            mapObj.AddComponent(map);

            
            GameObject car = new GameObject();
            car._transform._scale = new Vector3(100, 100, 100);
            car._name = "Car";

            
            CarController carController = new CarController();
            car.AddComponent(carController);
            
        }
    }
}
