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
            Camera cam = new Camera(90f, 1000f);
            mainCamera.AddComponent(cam);
            cam._parent._transform._position = Vector3.Zero();
    
            RenderController._camera = cam;
            RenderController._cameraTransform = mainCamera._transform;
            //------------------


            
            //making a square--
            GameObject square = new GameObject();

            SpriteRenderer sprite = new SpriteRenderer(SpriteBin.GetSprite("blueSquare"), new Vector2(1,1), SpriteRenderer.RenderFrom.Centre);

            square.AddComponent(sprite);

            square._transform._position = new Vector3(0, 10, 10);
            /*-----------------

            GameObject squareTwo = new GameObject();

            SpriteRenderer spriteTwo = new SpriteRenderer(SpriteBin.GetSprite("solidBlackSquare"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);

            squareTwo.AddComponent(spriteTwo);

            squareTwo._transform._position = new Vector3(0, 0, 5);
            */
        }
    }
}
