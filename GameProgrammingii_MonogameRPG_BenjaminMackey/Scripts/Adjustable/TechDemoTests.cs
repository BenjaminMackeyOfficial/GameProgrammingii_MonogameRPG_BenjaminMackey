using GameProgrammingii_MonogameRPG_BenjaminMackey.Content;
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
            //making the camera 
            GameObject mainCamera = new GameObject();
            mainCamera._transform._position = new Vector3(0, 0, 0);
            mainCamera._name = "TestCam";
            Camera cam = new Camera(40f, 1000f);
            
            mainCamera.AddComponent(cam);
            cam._parent._transform._position = Vector3.Zero();
    
            RenderController._camera = cam;
            RenderController._cameraTransform = mainCamera._transform;

            Debug.WriteLine(RenderController._camera._parent._name);



            //------------------

            //

            /*
        GameObject square = new GameObject();
        square._name = "Blue";
        SpriteRenderer sprite = new SpriteRenderer(SpriteBin.GetSprite("blueSquare"), new Vector2(1,1), SpriteRenderer.RenderFrom.Centre);

        square.AddComponent(sprite);

        square._transform._position = new Vector3(0, 0, 3);
        */

            GameObject squareTwo = new GameObject();
            SpriteRenderer spriteTwo = new SpriteRenderer(SpriteBin.GetSprite("solidBlackSquare"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            squareTwo.AddComponent(spriteTwo);
            squareTwo._transform._position = new Vector3(0, 0, 5);

            GameObject square3 = new GameObject();
            SpriteRenderer sprite3 = new SpriteRenderer(SpriteBin.GetSprite("solidBlackSquare"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            square3.AddComponent(sprite3);
            square3._transform._position = new Vector3(1000, 0, 3000);



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


            TransformController transformController = new TransformController(inputMap, inputMapL, 50f);

            mainCamera.AddComponent(transformController);
            //
        }
    }
}
