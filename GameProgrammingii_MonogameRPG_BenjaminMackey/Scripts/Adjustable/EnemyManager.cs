using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Adjustable
{
    public static class EnemyInfoBin
    {
        public static GameObject _player = new GameObject();
        
        public static List<Vector3> _PlayerPosHistory = new List<Vector3>();
    }

    //enemy class presets

    //the move if you're close to them and then blow up
    public class Bomber
    {
        public int _counter = 4000;
        public GameObject bomberObject;
        public Bomber(GameObject obj, int counter)
        {
            bomberObject = obj;
            _counter = counter;
        }
    }

    //the move horizontally straight across the track
    public class Blockade
    {
    
        public GameObject blockadeObject;
        public Blockade(GameObject obj)
        {
            blockadeObject = obj;
        }
    }

    //driving down the track normal, and just have one movment type for now but like make a few more later, maybe add key charachters? defo in future lol
    public class Shadower
    {
        public GameObject shadowObject;
        public EnemyAIController EnemyAIController;
        public Transform targTransform;
        public Shadower(GameObject obj)
        {
            targTransform = new Transform(EnemyInfoBin._PlayerPosHistory[0], Vector3.Zero(),new Vector3(1,1,1));
            EnemyAIController = (EnemyAIController)obj.GetComponent<TransformController>()._moveInputMap;
            EnemyAIController._target = targTransform;

        }
        public void UpdateTransformPos(Vector3 pos)
        {
            targTransform._position = pos;
        }
    }
    

    //driving styles
    //
    public class EnemyManager : Behavioral
    {

        private static EnemyManager _instance;
        public static EnemyManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                GameObject obj = new GameObject();
                _instance = new EnemyManager();
                obj.AddComponent(Instance);
                return _instance;
            }
 
        }




        public EnemyManager() { }

        private List<Shadower> shadowers = new List<Shadower>();
        private List<Blockade> blockades = new List<Blockade>();
        private List<Bomber> bombers = new List<Bomber>();

        private GameObject CreateGeneralEnemyTemplate(Vector3 pos, string spriteName)
        {
            GameObject enemy = new GameObject();
            enemy.AddTag("enemy");
            enemy._transform._position = pos;
            enemy._transform._scale = new Vector3(200, 200, 200); //random number

            Collider collider = new Collider(new Vector2(0, 0), true);
            collider._static = false;
            SpriteRenderer sprite = new SpriteRenderer(SpriteBin.GetSprite(spriteName), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            
            
            enemy.AddComponent(sprite);
            enemy.AddComponent(collider);


            return enemy;
        }


        public void CreateShadower(Vector3 pos)
        {
            GameObject enemy = CreateGeneralEnemyTemplate(pos, "shadower");
            enemy.AddTag("shadower");

            EnemyAIController enemyAIController = new EnemyAIController();
            enemyAIController._searchDistance = 1000f;
            enemyAIController._self = enemy._transform;
            //sets target every so often
            enemyAIController._movmentStrategy = new AgressiveMovmentStrategy();

            TransformController transformController = new TransformController(enemyAIController, new Vector2InputMap(true), 100f);

            enemy.AddComponent(transformController);

            shadowers.Add(new Shadower(enemy));
        }
        public void CreateBomber(Vector3 pos)
        {

            GameObject enemy = CreateGeneralEnemyTemplate(pos, "bomber");
            enemy.AddTag("bomber");

            EnemyAIController enemyAIController = new EnemyAIController();
            enemyAIController._searchDistance = 1000f;
            enemyAIController._self = enemy._transform;
            enemyAIController._target = EnemyInfoBin._player._transform; //not permanent, add general enemy info bin 
            enemyAIController._movmentStrategy = new AgressiveMovmentStrategy();

            TransformController transformController = new TransformController(enemyAIController, new Vector2InputMap(true), 100f);

            enemy.AddComponent(transformController);

            bombers.Add(new Bomber(enemy, 4000));
        }
        public void CreateBlockade(Vector3 pos,Vector3 scale)
        {
            GameObject enemy = CreateGeneralEnemyTemplate(pos, "blockade");
            enemy._transform._scale = scale;

            enemy.AddTag("blockade");

            bool result = enemy._components.Remove(enemy.GetComponent<Collider>());
            

            PlaneColider planeCoider = new PlaneColider(new Vector2(0, 0));
            enemy.AddComponent(planeCoider);
            
            EnemyAIController enemyAIController = new EnemyAIController();
            enemyAIController._searchDistance = 1000f;
            enemyAIController._self = enemy._transform;
            enemyAIController._target = EnemyInfoBin._player._transform; //not permanent, add general enemy info bin 
            enemyAIController._movmentStrategy = new SlideOnPlaneMovmentStrategy(planeCoider._plane);

            TransformController transformController = new TransformController(enemyAIController, new Vector2InputMap(true), 100f);
            enemy.AddComponent(transformController);

            blockades.Add(new Blockade(enemy));
        }
        public void CreateRandom(Vector3 pos,Vector3 scale)
        {
            Random random = new Random();
            int num = random.Next(2);
            switch(num)
            {
                case 0:
                    CreateBlockade(pos, scale); break;
                case 1:
                    CreateBomber(pos); break;
                case 2:
                    CreateShadower(pos); break;
            }
        }
    }
}
