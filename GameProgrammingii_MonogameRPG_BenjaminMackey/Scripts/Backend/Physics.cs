using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend
{
    //Dev note:
    /*
    obviously this is not done...uhhhh theres really no physics in here at all actually

    in the future, id like to make a physics material class that you can give to coliders, and do some stuff like unity... i
    think unity uses them on ridgid boddies but, ah well, you get the idea.

    also avert your eyes this script is awfull right now
    */
    public static class PhysicsSystem
    {
        private static List<(GameObject parent, Transform begin, Transform end)> moveQue = new List<(GameObject parent, Transform begin, Transform end)>();


        //this method is INREDIBLY unfinnished right now... just had to get it working... go spaghetti...
        public static void FufillPhysicsRequests()
        {

            List<Vector3> collidingWith;

                           //For the future for when i implement the correct math (need to switch rotation to matrix)
            float xUp;
            float xLow;

            float yUp;
            float yLow;

            float zUp;
            float zLow;
                              //---------------------------------------------------//


            for (int i = 0; i < moveQue.Count; i++)
            {
                //per OBJECT-------------------------------------------------------------------------
                Transform curEnd = moveQue[i].end;
                collidingWith = new List<Vector3>();//list of everything its coliding with or would be after bounce


                //temp
                collidingWith.Add(moveQue[i].end._position);
                //--
                if (!ObjectManager._gameObjects.Contains(moveQue[i].parent)) continue;//making sure the game object is still there
                
                for (int j = 0; j < moveQue.Count; j++)
                {
                    if (i == j) continue;
                    
                    Transform checkEnd = moveQue[j].end;
                    PlaneColider checkCol = moveQue[j].parent.GetComponent<PlaneColider>();

                    Collider col = moveQue[i].parent.GetComponent<PlaneColider>();
                    if (col != null && col._static)
                    {
                        //Debug.WriteLine("yello");
                        collidingWith.Add(moveQue[i].end._position);
                        continue;
                    }

                    Vector3 newPos = moveQue[i].end._position;
                    float dist = (float)(curEnd._position - moveQue[i].parent._transform._position).Magnitude();

                    if (checkCol != null)
                    {
                        Vector3 reflected = Transform.ReflectOff(checkCol._plane,curEnd,checkEnd);

                        // if reflection happened
                        if (reflected != new Vector3(99854, 99854, 99854))
                        {
                            Vector3 direction = Vector3.Normalize(reflected);
                            newPos = moveQue[i].begin._position + direction * dist;

                            collidingWith.Add(newPos);
                            //Debug.WriteLine(moveQue[i].begin._position.x + " " + moveQue[i].begin._position.y + " " + moveQue[i].begin._position.z + " -> " +newPos.x + " " + newPos.y + " " + newPos.z);
                        }
                    }

                    //collidingWith.Add(newPos);
                }

                /*
                foreach (Vector3 item in collidingWith)
                {
                    bool hit = false;
                    for (int j = 0; j < moveQue.Count; j++)
                    {
                        if(Transform.Overlap())
                    }
                }
                */

                //lock it in!
                
                moveQue[i].parent.FlushTransform(new Transform(collidingWith[collidingWith.Count - 1],
                    moveQue[i].end._rotation,
                    moveQue[i].end._scale
                    ));
                //-----------------------------------------------------------------------------------
            }
        }

        public static void QuePhysicsTransformAdjustment(GameObject obj, Transform current, Transform attmeped)
        {
            if (moveQue.Contains((obj, current, attmeped))) return;
            moveQue.Add((obj, current, attmeped));
        }
    }


}
