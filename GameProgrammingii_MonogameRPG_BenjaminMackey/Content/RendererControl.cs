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
        public static Camera _camera { get; private set; } //camera inside the game object, renders everything according to this things transform
        public static Transform _cameraTransform { get; private set; }
        public static WorldRenderingData _renderingData { get; private set; }
        public static RenderObjectData[] _renderObjects { get; private set; }
        public static void UpdateRenderVariables()//this doesnt happen in BuildNextRenderTable for efficiency
        {

        }
        public static void BuildNextRenderTable()
        {
            //individual renderer vars
            int screenPosX = 0;
            int screenPosY = 0;
            int sizeScalar = 0;

            Vector3 adjustedWorldPos;
            //-------------------------


            //adjusting for new camera movments
            double camAngleToHorLine = ((90 / _cameraTransform._rotation.y) + 1)/2 ;
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
                if ((adjustedWorldPos - _cameraTransform._position).Magnitude() > _camera._renderDistance) continue; //i have NOT figured out field of view yet :sob:



                SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();
            }
        }
    }
    public struct RenderObjectData
    {
         
    }
    public struct WorldRenderingData
    {
        public double _horizonLine; //from zero to 1, 1 being looking straight up, and 0 being looking straight down (not reccomended, would look wonky, try keep between 0.3, and 0.7)
        public int _horizonLinePixelCount;
        public int _height { get; private set; }
        public int _width { get; private set; }
        public void ChangeHorizonLine(double num)
        {
            _horizonLine = num.Clamp(0, 1);
            //_horizonLinePixelCount = (int)(_height * num); something to do with field of view
        }

    }


}
