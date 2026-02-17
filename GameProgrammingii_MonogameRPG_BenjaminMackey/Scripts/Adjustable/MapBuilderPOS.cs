using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Adjustable
{
    //im aware this is an awful way to do this lol, this was just temporary until i set up the finished way... but this doest even work so oh well
    public class Map
    {
        private Random rand = new Random();
        private List<GameObject> _colliders = new List<GameObject>();
        private List<GameObject> _decoration = new List<GameObject>();
        private int _scale = 1000;
        private int _pylonsPerWall = 10; //maybe have palm trees eventually too?
        //some utilty stuff
        private GameObject MakePylon(Vector3 pos)
        {
            SpriteRenderer spriteRenderer = new SpriteRenderer(SpriteBin.GetSprite("pylon"), new Vector2(1,1), SpriteRenderer.RenderFrom.Centre);
            GameObject cone = new GameObject();
            cone._transform._position = pos;
            cone._transform._scale = new Vector3(400, 400, 400);
            cone.AddComponent(spriteRenderer);
            //Debug.WriteLine(pos.x + " " + pos.y + " " + pos.z);
            return cone;
        }
        private GameObject MakeColider(Vector3 pos, Vector2 rotationVector)
        {
            Vector3 rot = new Vector3(0,0,0);
            Vector3 scale = new Vector3(100,_scale, _scale);
            if (rotationVector.y != 0)
            {
                scale = new Vector3(_scale, _scale, 100);
            }


            PlaneColider colider = new PlaneColider(new Vector2(0, 0));

            GameObject obj = new GameObject();
            obj._transform._position = pos;

            obj._transform._scale = scale;
            obj.AddComponent(colider);
            //SpriteRenderer spriteRenderer = new SpriteRenderer(SpriteBin.GetSprite("seeThroughBlackSquare"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            //obj.AddComponent(spriteRenderer);

            return obj;

        }

        private Vector2[] GetOtherDirections(Vector2[] directions)
        {
            Vector2[] PossibleDirections = 
            {
                new Vector2(1,0),
                new Vector2(-1,0),
                new Vector2(0,1),
                new Vector2(0,-1),
            };
            List<Vector2> builtTable = new List<Vector2>();

            for (int i = 0; i < directions.Length; i++)
            {
                for (int j = 0; j < PossibleDirections.Length; j++)
                {
                    if (directions[i] == PossibleDirections[j]) PossibleDirections[j] = new Vector2(-5,-5);
                }
            }
            foreach (Vector2 item in PossibleDirections)
            {
                if (item != new Vector2(-5, -5)) builtTable.Add(item);
            }
            return builtTable.ToArray();
        }


        //cordinates are the number of nums in the file x2000 world cordinates, starting from the middle of the tile
        private void BuildTile(Vector2 currentTilePos, Vector2 lastWall, Vector2 nextWall)
        {

            Vector2 dir1 = Vector2.Normal(lastWall - currentTilePos);
            Vector2 dir2 = Vector2.Normal(nextWall - currentTilePos);



            Vector2[] temp = { dir1, dir2 };
            Vector2[] wallDirections = GetOtherDirections(temp);

            



            for (int i = 0; i <2; i++)
            {
                Debug.WriteLine(wallDirections[i].x + " " +wallDirections[i].y);

                Vector2 wallPosition = (currentTilePos * _scale) + (wallDirections[i] * ((float)_scale / 2f));
                Vector3 vec3WallPos = new Vector3(wallPosition.x, 0, wallPosition.y);

                _colliders.Add(MakeColider(vec3WallPos, wallDirections[i]));


                Vector3 tangent = new Vector3(-wallDirections[i].y, 0,wallDirections[i].x);

                for (int j = 0; j < _pylonsPerWall; j++)
                {
                    float offset = -0.5f + ((float)j / _pylonsPerWall);
                    Debug.WriteLine(offset);
                    MakePylon((vec3WallPos + (tangent * offset) * _scale) + new Vector3(0,-700,0));
                    //the pylon would be made in here
                }
            }
        }
       

        
        private Vector2 FindTile(List<int[]> map, int target)
        {
            for (int x = 0; x < map.Count; x++)
            {
                for (int y = 0; y < map[x].Length; y++)
                {
                    if (map[x][y] == target) return new Vector2(x, y);
                }
            }    
            return new Vector2(-1, -1);
        }
        private void SetUpMap(List<int[]> cordinateData)
        {
            Vector2 searchBounds = new Vector2(1, 1);
            Vector2 newFocus = new Vector2(0, 0);
            Vector2 curentFocus = new Vector2(0, 0); //will start off as the start position ()
            Vector2 lastFocus = new Vector2(0, 0);

            
            curentFocus = FindTile(cordinateData, 4); //temp
            newFocus = new Vector2(curentFocus.x + 1, curentFocus.y);
            lastFocus = new Vector2(curentFocus.x - 1, curentFocus.y);
            BuildTile(curentFocus, lastFocus, newFocus);
            


            int counter = 0;
            while (true)
            {
                counter++;
                newFocus = cordinateData.DigThrouh2D(curentFocus, lastFocus, 1, searchBounds);

                if (newFocus.x == -1 && newFocus.y == -1)break;

                BuildTile(curentFocus, lastFocus, newFocus);

                lastFocus = curentFocus;
                curentFocus = newFocus;
                //Debug.WriteLine(curentFocus.x + " " + curentFocus.y);
            }
            newFocus = cordinateData.DigThrouh2D(curentFocus, lastFocus, 4, searchBounds);
            BuildTile(curentFocus, lastFocus, newFocus);
        }
        public Map(string fileName, int mapScale) //one in the future will let you pick time of day and like background and whatnot
        {
            fileName += ".txt";
            if (File.ReadAllLines(fileName) == null) return;
            string[] text = File.ReadAllLines(fileName);
            List<int[]> cordinateData = new List<int[]>();
            for (int i = 0; i < text.Length; i++)
            {
                char[] temp = text[i].ToCharArray();
                int[] temp2 = new int[temp.Length];
                for (int j = 0; j < temp.Length; j++)
                {
                    temp2[j] = temp[j] - '0';
                }
                cordinateData.Add(temp2);
            }

            _scale = mapScale;
            SetUpMap(cordinateData);
        }
    }

}
