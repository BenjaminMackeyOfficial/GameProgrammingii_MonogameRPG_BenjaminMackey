
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
            /*making the camera 
            GameObject mainCamera = new GameObject();
            mainCamera._transform._position = new Vector3(0, 0, 0);
            mainCamera._transform._scale = new Vector3(100, 100, 100);
            mainCamera._name = "TestCam";
            Camera cam = new Camera(40f, 1000f);

            Collider collider = new Collider(new Vector2(0,0), false);
            collider._static = false;
            mainCamera.AddComponent(collider);
            mainCamera.AddComponent(cam);
            cam._gameObject._transform._position = Vector3.Zero();
    
            RenderController._camera = cam;
            RenderController._cameraTransform = mainCamera._transform;


            ButtonAction fwd = new ButtonAction(ConsoleKey.W);
            ButtonAction bck = new ButtonAction(ConsoleKey.S);
            ButtonAction lft = new ButtonAction(ConsoleKey.A);
            ButtonAction rght = new ButtonAction(ConsoleKey.D);

            Vector2InputMap inputMap = new Vector2InputMap(fwd, bck, rght, lft);

            ButtonAction fwdL = new ButtonAction(ConsoleKey.UpArrow);
            ButtonAction bckL = new ButtonAction(ConsoleKey.DownArrow);
            ButtonAction lftL = new ButtonAction(ConsoleKey.LeftArrow);
            ButtonAction rghtL = new ButtonAction(ConsoleKey.RightArrow);

            Vector2InputMap inputMapL = new Vector2InputMap(fwdL, bckL, rghtL, lftL);


            TransformController transformController = new TransformController(inputMap, inputMapL, 500f);

            mainCamera.AddComponent(transformController);

            */

            GameObject car = new GameObject();
            car._transform._scale = new Vector3(100, 100, 100);

            CarController carController = new CarController();

            car.AddComponent(carController);

            Map map = new Map("Map", 10000);
            
        }
    }
}
