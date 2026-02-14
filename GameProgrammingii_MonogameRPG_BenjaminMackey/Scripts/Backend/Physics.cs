using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend
{
    public static class PhysicsSystem
    {
        private static List<(GameObject parent, Transform begin, Transform end)> moveQue = new List<(GameObject parent, Transform begin, Transform end)>();

        public static void FufillPhysicsRequests()
        {

            List<Transform> collidingWith;

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
                Transform curEnd = moveQue[0].end;
                collidingWith = new List<Transform>();//list of everything its coliding with or would be after bounce

                if (!ObjectManager._gameObjects.Contains(moveQue[i].parent)) continue;//making sure the game object is still there
                for (int j = 0; j < moveQue.Count; j++)
                {
                    Transform checkEnd = moveQue[i].end;
                    
                    //note: Hey so i actually DONT have time to implement seperated axis theorum right now, im considering all of these to be fixed plane

                    

                    
                }





                //-----------------------------------------------------------------------------------
            }
        }

        public static void QuePhysicsTransformAdjustment(ref GameObject obj, Transform current, Transform attmeped)
        {
            if (moveQue.Contains((obj, current, attmeped))) return;
            moveQue.Add((obj, current, attmeped));
        }
    }


}
