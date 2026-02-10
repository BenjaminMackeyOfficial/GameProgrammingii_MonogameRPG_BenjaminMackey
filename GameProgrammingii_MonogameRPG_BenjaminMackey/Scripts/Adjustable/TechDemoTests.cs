using GameProgrammingii_MonogameRPG_BenjaminMackey.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
            Camera cam = new Camera(90f, 100f);
            mainCamera.AddComponent(cam);
    
            RenderController._camera = cam;
            RenderController._cameraTransform = mainCamera._transform;
            //------------------


            
            //making a square--
            GameObject square = new GameObject();
            SpriteRenderer sprite = new SpriteRenderer(Content.Load<Texture2D>("blackSquare"), new Vector2(1,1), SpriteRenderer.RenderFrom.Centre);

            square.AddComponent(sprite);
            //-----------------
            
        }
    }
}
