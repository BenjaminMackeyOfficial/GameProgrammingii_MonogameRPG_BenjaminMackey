using GameProgrammingii_MonogameRPG_BenjaminMackey.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    public class GameObject
    {
        public string _name;
        public List<Component> _components { get; private set; }

        public Transform _transform { get; private set; } //threw this in cause most game objects are used for world things

        public GameObject()
        {
            _components = new List<Component>();
            _transform = new Transform(new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(10,10,10));
            ObjectManager.AddToWorld(this);
        }
        public void AddComponent(Component component)
        {
            if(!ObjectManager.RequestAdd(this, component))
            {
                Debug.Write("Could not add component!");
                return;
            }
            component._parent = this;
            _components.Add(component);
        }

        public T GetComponent<T>() where T : Component
        {
            Component component = null;
            foreach(Component comp in _components)
            {
                if(comp.GetType() == typeof(T))
                {
                    component = comp;
                    break;
                }
            }
            return (T)component;
        }
        public void UpdateAndRead()
        {
            
            foreach (Component item in _components)
            {
                if(item is Updatable)//thank you stack overflow user Robert C. Barth! what a shorthand
                {
                    (item as Updatable).Update(); // this makes things so easy bruh what, thanks robert 
                }
            }
        }
    }

    //=======================================COMPONENT CLASS STUFF==================================

    //-------------------------------litterally holds the object system together, UPDATE ONLY IF NESSECARY-----------------
    public static class ObjectManager
    {
        public static int _index = 0;
        public static List<GameObject> _gameObjects = new List<GameObject>();
        public static void AddToWorld(GameObject obj)
        {

            if(!_gameObjects.Contains(obj)) _gameObjects.Add(obj);      
        }
        public static bool RequestAdd(GameObject gameObj, Component component)
        {
            if(component.GetType() == typeof(Transform))

            foreach (Component item in gameObj._components)
            {
                if (item.GetType() == component.GetType()) return false;
            }
            return true;
        }
        public static int[] RequestBuildInfo()
        {
            int[] data = { _index };
            _index++;

            return data;
        }
        public static void UpdateAllGameObjects()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].UpdateAndRead();
            }
        }
        
        public static void UpdateSingleGameObject(GameObject obj)
        {
            obj.UpdateAndRead();
        }
    }
    //----------------------------------------------------------

    public interface Updatable
    {
        void Update();
    }
    public class Component
    {
        public int _id { get; private set; }
        public GameObject _parent;
       
        public Component()
        {
            int[] data = ObjectManager.RequestBuildInfo();
            _id = data[0];
        }
    }

    public class Transform : Component
    {
        public Vector3 _position;
        public Vector3 _rotation;
        public Vector3 _scale;
        public float _linearVelocity = 0;
        public Transform(Vector3 pos, Vector3 rotation, Vector3 scale) :base() 
        {
            _position = pos;
            _rotation = rotation;
            _scale = scale;
        }
        public Vector3 Forward() 
        {
            double radX = _rotation.x * Math.PI / 180.0;
            double radY = _rotation.y * Math.PI / 180.0;
            return new Vector3(
                Math.Cos(radX) * Math.Cos(radY),
                Math.Sin(radX),
                Math.Cos(radX) * Math.Sin(radY)
                );
        }
        public Vector3 Right() //is flat
        {
            double radY = _rotation.y * Math.PI / 180.0;
            return new Vector3(
                -Math.Sin(radY),
                0,
                Math.Cos(radY)
            );
        }
        public Vector3 Up()
        {
            double radX = _rotation.x * Math.PI / 180.0;
            double radY = _rotation.y * Math.PI / 180.0;
            return new Vector3(
                Math.Sin(radX) * Math.Cos(radY),
                Math.Cos(radX),
                Math.Sin(radX) * Math.Sin(radY)
                );
        }
    }

    public class Collider : Component
    {
        public Vector2 _size;
        public Vector2 _offSet;

        public bool _isTrigger;

        public event Action<Collider> OnTriggerEnter;
        public Collider(Vector2 size, Vector2 offSet, bool isTrigger) :base()
        {
            _size = size;
            _offSet = offSet;
            _isTrigger = isTrigger;
        }
        public Collider(Vector2 size, Vector2 offSet) : base()
        {
            _size = size;
            _offSet = offSet;
            _isTrigger = false;
        }
    }

    
    public class TransformController : Component, Updatable
    {
        private Vector2InputMap _moveInputMap;
        private Vector2InputMap _rotationInputMap;
        public float _speed = 1.0f;

        public TransformController(Vector2InputMap inputMap, Vector2InputMap vector2InputMap, float speed) : base()
        {
            _moveInputMap = inputMap;
            _rotationInputMap = vector2InputMap;
            this._speed = speed;
        }
        public void Update()
        {
            Vector3 forward = _parent._transform.Forward();
            Vector3 right = _parent._transform.Right();
            
            _parent._transform._position.x += (_moveInputMap.y * forward.z) * _speed;
            _parent._transform._position.z += (_moveInputMap.y * forward.x) * _speed;

            _parent._transform._position.x += (_moveInputMap.x * right.z) * _speed;
            _parent._transform._position.z += (_moveInputMap.x * right.x) * _speed;

            _parent._transform._rotation.x += -_rotationInputMap.y;
            _parent._transform._rotation.y += (_rotationInputMap.x);

            //Debug.WriteLine(_parent._transform.Forward().x + " " + _parent._transform.Forward().y + " " + _parent._transform.Forward().z);
        }
    }

    public class Camera : Component
    {
        public float _fieldOfView;
        public float _renderDistance;
        public float _3dDepth;

        public Camera(float fov, float renderDist) : base() 
        {
            _fieldOfView = fov;
            _renderDistance = renderDist;

            _3dDepth = (float)((RenderController._renderingData._width / 2f));
        }
    }
    
    //renderers
    public class SpriteRenderer : Component
    {
        public int _serveImage { get; protected set;  }
        public Texture2D _spriteSheet { get; private set; }
        public RenderFrom _renderFrom;
        public bool _enabled = true;

        public bool _faceTheCameraX = true; // most likley will never do anything?
        public bool _faceTheCameraY = true; // same here

        public Microsoft.Xna.Framework.Rectangle[] _spritePositions;

        public enum RenderFrom
        {
            Centre, Top, Bottom, Left, Right
        }
        public SpriteRenderer(Texture2D spriteSheet, Vector2 numOfSpritesWidthAndHeight, RenderFrom rendFrom)
        {
            
            _spriteSheet = spriteSheet;
            _renderFrom = rendFrom;
           

            //BUILDING the sprite sheet into an array of cordinates
            int baseSizeX = _spriteSheet.Width / (int)numOfSpritesWidthAndHeight.x;
            int baseSizeY = _spriteSheet.Height / (int)numOfSpritesWidthAndHeight.y;

            List<Microsoft.Xna.Framework.Rectangle> tempCords = new List<Microsoft.Xna.Framework.Rectangle>();
            for (int i = 0; i < numOfSpritesWidthAndHeight.x; i++)
            {
                for (int j = 0; j < numOfSpritesWidthAndHeight.y; j++)
                {
                    tempCords.Add(new Microsoft.Xna.Framework.Rectangle(baseSizeX * i, baseSizeY * j, baseSizeX, baseSizeY));
                }
            }
            if (tempCords.Count != 0) _serveImage = 0;
            _spritePositions = tempCords.ToArray();
        }
        
    }

    public class AnimatedTrackSpriteRenderer : SpriteRenderer, Updatable
    {
        public int _FPS;
        public int _framesSinceLastSpriteChange { get; set; }


        public AnimatedTrackSpriteRenderer(Texture2D spriteSheet, Vector2 spriteSizeOnSheet, RenderFrom rendFrom) : base (spriteSheet, spriteSizeOnSheet, rendFrom)
        {
            _FPS = 0;
            _framesSinceLastSpriteChange = 0;
        }
        public void Update()
        {
            _framesSinceLastSpriteChange++;
            if(_framesSinceLastSpriteChange >= _FPS)
            {
                if (_serveImage >= _spritePositions.Length - 1) _serveImage = 0;
                else _serveImage++;
            }
        }
    }
    public class VariableSpriteRenderer : SpriteRenderer, Updatable
    {

        public VariableSpriteRenderer(Texture2D spriteSheet, Vector2 spriteSizeOnSheet, RenderFrom rendFrom) : base(spriteSheet, spriteSizeOnSheet, rendFrom)
        {
            
        }
        public void ChangeSprite(int num)
        {
            if (num > _spritePositions.Length - 1) return;
            _serveImage = num;
        }
        public void Update()
        {

        }
    }
    //---------
    //=================================================================================================

}
