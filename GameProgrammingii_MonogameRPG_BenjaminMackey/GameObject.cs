using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            _transform = new Transform(new Vector2(0,0), 0);
        }
        public void Add(Component component)
        {
            if(!ObjectManager.RequestAdd(this, component))
            {
                Debug.Write("Could not add component!");
                return;
            }
            component._parent = this;
            _components.Add(component);
        }

        public Component GetComponent<T>() where T : Component
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

            return component;
        }
        private void UpdateAndRead()
        {

        }
    }

    //=======================================COMPONENT CLASS STUFF==================================

    //-------------------------------litterally holds the object system together, UPDATE ONLY IF NESSECARY-----------------
    public static class ObjectManager
    {
        public static int _index = 0;
        public static GameObject[] _gameObjects;

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
        public Vector2 _position;
        public int _rotation;
        public Transform(Vector2 pos, int rotation) :base() 
        {
            _position = pos;
            _rotation = rotation;
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
        private Vector2 toMove;
        public ButtonAction _right;
        public ButtonAction _up;
        public ButtonAction _down;
        public ButtonAction _left;

        public TransformController() : base()
        {
            
        }

        private void up(EventArgs args)
        {
            
        }

        public void Update()
        {
            _parent._transform._position.x += toMove.x;
            _parent._transform._position.y += toMove.y;
            toMove = new Vector2(0, 0);
        }
        
    }

    public class Camera : Component
    {
        public float _fieldOfView;
        public float _renderDistance;

        public Camera(float fov, float renderDist) : base() 
        {
            _fieldOfView = fov;
            _renderDistance = renderDist; 
        }
    }
    //=================================================================================================

}
