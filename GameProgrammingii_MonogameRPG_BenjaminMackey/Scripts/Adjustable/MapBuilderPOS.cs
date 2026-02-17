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
        private GameObject MakeColider(Vector3 pos, Vector3 rotationVector)
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
            SpriteRenderer spriteRenderer = new SpriteRenderer(SpriteBin.GetSprite("seeThroughBlackSquare"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            obj.AddComponent(spriteRenderer);

            return obj;

        }


        //cordinates are the number of nums in the file x2000 world cordinates, starting from the middle of the tile
        
        private void BuildTile(Vector2 old, Vector2 current, Vector2 newT)
        {
            Vector2 dirOld = old - current;
            Vector2 dirNew = newT - current;

            //Debug.WriteLine(dirOld.x + " " + dirOld.y);
            Debug.WriteLine("Old: " + dirOld.x + " " + dirOld.y);
            Debug.WriteLine("New: " + dirNew.x + " " + dirNew.y);

            Vector2[] posDirections = {
                new Vector2(1,0)
                , new Vector2(-1,0)
                , new Vector2(0,1)
                , new Vector2(0,-1)
            };
            List<Vector2> drawDirs = new List<Vector2>();

            foreach (Vector2 dir in posDirections)
            {
                if (dir != dirOld && dir != dirNew) 
                {
                    drawDirs.Add(dir);
                    //Debug.WriteLine(dir.x + " " +dir.y);
                };

            }
            //Coliders
            Vector3 adjustedForWorld;
            for (int i = 0; i < 2; i++)
            {
                Vector3 toVec3 = new Vector3(drawDirs[i].x, 0, drawDirs[i].y);
                adjustedForWorld = (new Vector3(current.x, 0, current.y) * _scale +
                    toVec3 * ((float)_scale /2f) 
                    );
                MakeColider(adjustedForWorld, toVec3);



                Vector3 traceAlong = new Vector3();
                
                if (i == 0) traceAlong = new Vector3(drawDirs[1].x, 0, drawDirs[1].y);
                else traceAlong = new Vector3(drawDirs[0].x, 0, drawDirs[0].y);



                Vector3 wallDir = Vector3.Normalize(traceAlong);
                float spacing = (float)_scale / _pylonsPerWall;

                for (int j = 0; j < _pylonsPerWall; j++)
                {
                    Vector3 offset = wallDir * spacing * j;
                    Vector3 placeAt = adjustedForWorld + offset - new Vector3(0, 700, 0);

                    MakePylon(placeAt);
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
            List<Vector2> alreadyVisited = new List<Vector2>();
            Vector2 searchBounds = new Vector2(1, 1);
            Vector2 newFocus = new Vector2(0, 0);
            Vector2 curentFocus = new Vector2(0, 0); //will start off as the start position ()
            Vector2 lastFocus = new Vector2(0, 0);

            
            curentFocus = FindTile(cordinateData, 4);
            newFocus = FindTile(cordinateData, 5);
            lastFocus = FindTile(cordinateData, 3);

            BuildTile(lastFocus, curentFocus, newFocus);

            int counter = 0;
            while (true)
            {
                counter++;
                newFocus = cordinateData.DigThrouh2D(curentFocus, lastFocus, 1, searchBounds);

                
                alreadyVisited.Add(newFocus);

                if (newFocus.x == -1 && newFocus.y == -1)break;

                lastFocus = curentFocus;
                curentFocus = newFocus;

                BuildTile(lastFocus, curentFocus, newFocus);
                //Debug.WriteLine(curentFocus.x + " " + curentFocus.y);
            } 
            
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
