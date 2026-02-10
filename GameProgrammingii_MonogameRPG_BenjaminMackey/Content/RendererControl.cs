using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

        public static RenderObjectData[] renderObjects { get; private set; }
        public static void BuildNextRenderTable()
        {
            foreach (var item in ObjectManager._gameObjects)
            {
                if (item.GetComponent<SpriteRenderer>() != null) continue;
                SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();

            }
        }
    }
    public struct RenderObjectData
    {

    }

}
