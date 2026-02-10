using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        public static void UpdateRenderVariables()//this doesnt happen in BuildNextRenderTable for efficiency
        {

        }
        public static void BuildNextRenderTable()
        {
            if (_camera == null) return;
            List<RenderObjectData> renderObjects = new List<RenderObjectData>();

            //individual renderer vars
            int screenPosX = 0;
            int screenPosY = 0;
            int sizeScalar = 0;

            Vector3 adjustedWorldPos;
            //-------------------------


            //adjusting for new camera movments
            double camAngleToHorLine = 90 / _cameraTransform._rotation.y;
            _renderingData.ChangeHorizonLine(camAngleToHorLine);
            //
            //vision cone variables----


            //-------------------------
            foreach (GameObject item in ObjectManager._gameObjects)
            {
                //checking if gameobject even has a renderer
                if (item.GetComponent<SpriteRenderer>() != null) continue;

                
                adjustedWorldPos = item._transform._position - _cameraTransform._position; //moving
                adjustedWorldPos = Vector3.RotatePositionAroundWorldPoint(adjustedWorldPos, Vector3.Zero(), _cameraTransform._rotation); // spinning adjusted for the camera

                //checking if the camera can see the thing
                if ((adjustedWorldPos - _cameraTransform._position).Magnitude() > _camera._renderDistance || adjustedWorldPos.z < 0) continue;


                SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
                //the actual placing of sprites
                RenderObjectData drawObj = new RenderObjectData();
                drawObj._texture = spriteRenderer._spriteSheet;
                drawObj._cutOut = new Microsoft.Xna.Framework.Rectangle();
                drawObj._scale = new Microsoft.Xna.Framework.Vector2((float)(
                    item._transform._scale.x *
                    (_camera._3dDepth / (adjustedWorldPos - _cameraTransform._position).Magnitude())),(float)( //world scaling
                    item._transform._scale.y *
                    (_camera._3dDepth / (adjustedWorldPos - _cameraTransform._position).Magnitude())
                    ));
                drawObj._position = new Microsoft.Xna.Framework.Vector2((float)(
                    adjustedWorldPos.x * _renderingData._width),(float)(
                    adjustedWorldPos.y + (_renderingData._height * _renderingData._horizonLine)
                    ));
                drawObj._rotation = 0f;//_cameraTransform._rotation.z; //this wont work, will re-visit when i meet the project requirments and have time :peace_sign:
                renderObjects.Add( drawObj );
            }
        }
    }
    public struct RenderObjectData
    {
        public Texture2D _texture;
        public Microsoft.Xna.Framework.Vector2 _scale;
        public Microsoft.Xna.Framework.Vector2 _position;
        public float _rotation;
        public Microsoft.Xna.Framework.Rectangle _cutOut;
    }
    public struct WorldRenderingData
    {
        public double _horizonLine; //from -1 to 1, 1 being looking straight up, and -1 being looking straight down (not reccomended, would look wonky, try keep between 0.3, and 0.7)
        public int _horizonLinePixelCount;
        public int _height { get; private set; }
        public int _width { get; private set; }
        public void ChangeHorizonLine(double num)
        {
            _horizonLine = num.Clamp(-1, 1);
            //_horizonLinePixelCount = (int)(_height * num); something to do with field of view
        }

    }


}
