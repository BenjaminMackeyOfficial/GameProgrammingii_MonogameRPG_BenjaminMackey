using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey.Content
{
    //opperates like the main game world, figures out where to render everything accordingly
    public static class RenderController
    {
        public static Camera _camera { get; set; } //camera inside the game object, renders everything according to this things transform
        public static Transform _cameraTransform { get; set; }
        public static WorldRenderingData _renderingData { get; private set; }
        public static RenderObjectData[] _renderObjects { get; private set; }
        public static void UpdateRenderVariables(GraphicsDeviceManager graphicsDeviceManager)//this doesnt happen in BuildNextRenderTable for efficiency
        {
            _renderingData = new WorldRenderingData(graphicsDeviceManager, 1000, 1000);
            _renderingData._graphicsDeviceManager.PreferredBackBufferWidth = 1000;
            _renderingData._graphicsDeviceManager.PreferredBackBufferHeight = 1000;
            _renderingData._graphicsDeviceManager.ApplyChanges();
            //_renderingData.ChangeHorizonLine(0.5f);
        }
        public static void BuildNextRenderTable()
        {
            if (_camera == null) return;

            List<RenderObjectData> renderObjects = new List<RenderObjectData>();
           
            Vector3 adjustedWorldPos;
            //-------------------------


            //adjusting for new camera movments
           // double camAngleToHorLine = (_cameraTransform._rotation.x / 90) * 0.5;
            _renderingData.ChangeHorizonLine(0.5f);//change eventually for 3d
                                                                                               //
                                                                                               


            //-------------------------
            foreach (GameObject item in ObjectManager._gameObjects)
            {
                //checking if gameobject even has a renderer
                if (item.GetComponent<SpriteRenderer>() == null) continue;


                adjustedWorldPos = item._transform._position - _cameraTransform._position; //moving

                adjustedWorldPos = Vector3.RotatePositionAroundWorldPoint(adjustedWorldPos, Vector3.Zero(), -_cameraTransform._rotation); // spinning adjusted for the camera


                //checking if the camera can see the sprite
                //if (adjustedWorldPos.Magnitude() > _camera._renderDistance || adjustedWorldPos.z < 0) continue;


                SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
                //the actual placing of sprites
                RenderObjectData drawObj = new RenderObjectData();
                drawObj._texture = spriteRenderer._spriteSheet;
                drawObj._cutOut = spriteRenderer._spritePositions[spriteRenderer._serveImage];

                //drawing to screen (very important)

                float textureSizeBalance = (
                    (
                    (_renderingData._height * _renderingData._width) /
                    (drawObj._texture.Height * drawObj._texture.Width)
                    )/1000f
                    );

                float perspective = (float)(_camera._3dDepth / adjustedWorldPos.z); // * textureSizeBalance;
                

                drawObj._position = new Microsoft.Xna.Framework.Vector2(

                    _renderingData._width * 0.5f + (float)adjustedWorldPos.x * perspective,
                    (float)((float)_renderingData._height * _renderingData._horizonLine) - (float)adjustedWorldPos.y * perspective
                );
                /*in the attempt to fake combat edge of screen size
                float scaleDownSizeX = 1 / Math.Abs(drawObj._position.X - (_renderingData._width / 2f));
                float scaleDownSizeY = 1 / Math.Abs(drawObj._position.Y - (_renderingData._height / 2f));
                scaleDownSizeX = (float)Math.Sin((float)scaleDownSizeX);
                scaleDownSizeY = (float)Math.Sin((float)scaleDownSizeY);

               */
                drawObj._scale = new Microsoft.Xna.Framework.Vector2(
                    (((float)item._transform._scale.x + (float)item._transform._scale.z) / 2) * perspective * textureSizeBalance ,//* scaleDownSizeX,
                    ((float)item._transform._scale.y )* perspective * textureSizeBalance //* scaleDownSizeY
                );

                drawObj._dist = 1 / (float)adjustedWorldPos.z;
                //-----

                //Debug.WriteLine(adjustedWorldPos.Magnitude() + " " + adjustedWorldPos.z);
                //Debug.WriteLine(scaleDownSizeX + " " + scaleDownSizeY);
                drawObj._rotation = 0f;//_cameraTransform._rotation.z; //this wont work, will re-visit when i meet the project requirments and have time :peace_sign:
                renderObjects.Add( drawObj );
                

            }
            _renderObjects = renderObjects.ToArray();
            //Debug.WriteLine(_renderObjects[0]._cutOut + " " + _renderObjects[0]._scale + " " + screenPos);
        }
    }
    public struct RenderObjectData
    {
 
        public Texture2D _texture;
        public Microsoft.Xna.Framework.Vector2 _scale;
        public Microsoft.Xna.Framework.Vector2 _position;
        public float _rotation;
        public Microsoft.Xna.Framework.Rectangle _cutOut;
        public float _dist;
    }
    public class WorldRenderingData
    {
        public GraphicsDeviceManager _graphicsDeviceManager;
        public float _horizonLine { get; private set; } //from -1 to 1, 1 being looking straight up, and -1 being looking straight down (not reccomended, would look wonky, try keep between 0.3, and 0.7)
        public int _horizonLinePixelCount;
        public int _height { get; private set; }
        public int _width { get; private set; }
        public WorldRenderingData(GraphicsDeviceManager graphicsDeviceManager, int h, int w)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
            _height = h;
            _width = w;
        }
        public void ChangeHorizonLine(float num)
        {
            
            _horizonLine = num.Clamp(0f, 1f);
            //_horizonLinePixelCount = (int)(_height * num); something to do with field of view
        }

    }


}
