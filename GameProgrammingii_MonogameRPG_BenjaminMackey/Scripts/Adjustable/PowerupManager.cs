using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Adjustable
{
    //general pickup class

    //extend into boost pickup
    //mario star type thing
    //heal ability
    public class Pickup: Component
    {
        public virtual void Effect(CarController that)
        {

        }
    }
    public class NitrosBottle : Pickup
    {
        public GameObject obj;
        public NitrosBottle(Vector3 position)
        {
            obj = new GameObject();
            obj._transform._position = position;
            obj._transform._scale = new Vector3(500, 500, 500);

            SpriteRenderer sprite = new SpriteRenderer(SpriteBin.GetSprite("nos"), new Vector2(1,1), SpriteRenderer.RenderFrom.Centre);
            obj.AddComponent(sprite);

            Collider col = new Collider(new Vector2(0,0), true);
            col._static = false;
            obj.AddComponent(col);
            obj.AddTag("pickup");
            obj.AddTag("nos");

            obj.AddComponent(this);
        }
        public override void Effect(CarController that)
        {
            that.boostInTank += 10;
        }
    }
    public class Shield : Pickup
    {
        public GameObject obj;
        public Shield(Vector3 position)
        {
            obj = new GameObject();
            obj._transform._position = position;
            obj._transform._scale = new Vector3(500, 500, 500);

            SpriteRenderer sprite = new SpriteRenderer(SpriteBin.GetSprite("shield"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            obj.AddComponent(sprite);

            Collider col = new Collider(new Vector2(0, 0), true);
            col._static = false;
            obj.AddComponent(col);
            obj.AddTag("pickup");
            obj.AddTag("shield");

            obj.AddComponent(this);

        }
        public override void Effect(CarController that)
        {
            that.invFrames = 500;
        }
    }
    public class Heal : Pickup
    {
        public GameObject obj;
        public Heal(Vector3 position)
        {
            obj = new GameObject();
            obj._transform._position = position;
            obj._transform._scale = new Vector3(500, 500, 500);

            SpriteRenderer sprite = new SpriteRenderer(SpriteBin.GetSprite("health"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            obj.AddComponent(sprite);

            Collider col = new Collider(new Vector2(0, 0), true);
            col._static = false;
            obj.AddComponent(col);
            obj.AddTag("pickup");
            obj.AddTag("health");

            obj.AddComponent(this);
        }
        public override void Effect(CarController that)
        {
            that.health += 50;
        }
    }

    //eventually add things to attack the enemies
    internal class PickupManager
    {
        private static PickupManager _instance;
        public static PickupManager Instance { get { 
            if(_instance == null) _instance = new PickupManager(); return _instance;
            } }

        public Pickup MakeRandomPickup(Vector3 pos)
        {
            Random random = new Random();
            int rnd = random.Next(3);
            switch(rnd)
            {
                case 0: return new NitrosBottle(pos);
                case 1: return new Heal(pos);
            }
            return new Shield(pos);
        }
    }
}
