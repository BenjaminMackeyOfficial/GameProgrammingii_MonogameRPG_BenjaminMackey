using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    public class CarController : Behavioral
    {
        private GameObject _cam;
        private Collider _collider;
        private SpriteRenderer _spriteRenderer;
        private Vector2InputMap _baseControls;
        private ButtonAction _boostKey;
        private ButtonAction _driftKey;


        
        private GameObject BuildFollowCam()
        {
            GameObject CameraObj = new GameObject();
            CameraObj._transform._position = _cameraOffset;
            CameraObj._transform._scale = new Vector3(100, 100, 100);
            CameraObj._name = "TestCam";

            Camera cam = new Camera(40f, 1000f);

            RenderController._camera = cam;
            RenderController._cameraTransform = CameraObj._transform; //somehow make this auto searching with gameobject updating
            
            return CameraObj;
        }

        private SpriteRenderer CreateCarRenderer()
        {
            SpriteRenderer carRenderer = new SpriteRenderer(SpriteBin.GetSprite("TempCar"), new Vector2(1,1), SpriteRenderer.RenderFrom.Centre);
            return carRenderer;
        }

        private Collider CreateCarColider()
        {
            Collider collider = new Collider(new Vector2(0,0), false);
            collider._static = false;
            return collider;

        }
        
 
        
        public override void Initialize()
        {
            _cam = BuildFollowCam();
            _collider = CreateCarColider();
            _spriteRenderer = CreateCarRenderer();
            _baseControls = Vector2InputMap.DefaultWasdMap();

            _driftKey = new ButtonAction(ConsoleKey.P);//add more buttons in future
            _driftKey = new ButtonAction(ConsoleKey.O);

            _gameObject.AddComponent(_collider);
            _gameObject.AddComponent(_spriteRenderer);
        }

        public bool boosting = false;

        public float _maxTurn = 2;
        public float _maxSpeed = 200;
        public float _acceleration = 100f;
        public float _boostMult = 2f;
        public float _aeroFactor = 1.001f;
        public float _rollingResistance = 1.01f;

        public float _bodyStifness = 100f; 

        private float turn= 0f;
        
        private float speed = 0f;
        private float boostInTank = 0f;
        //physics
        private void adjustCarInputValues() //ill make this better in the future
        {
            

            speed += (float)_baseControls.y * _acceleration * framegenTimeEqv;
            speed /= _aeroFactor;
            speed = speed.Clamp(-30f, _maxSpeed);


            if (_boostKey._isHeld && boostInTank > 0) speed += _boostMult;

            turn = (-(float)_baseControls.x * _bodyStifness ).Clamp(-_maxTurn, _maxTurn);
        }
        //
        private void moveCar() //go set up a propper move method or somehting in wherever
        {
            Vector3 newRotation = new Vector3(0, turn, 0);
            Vector3 newPosition = _gameObject._transform.Right() * speed;

            _gameObject._attemptedTransform._position += newPosition;
            _gameObject._attemptedTransform._rotation += new Vector3(0, turn, 0);

            Debug.WriteLine(turn);
            //Debug.WriteLine(_gameObject._transform._position.x + " " + newPosition.x);

        }

        private float framegenTimeEqv = 0.01f; //ask simon about


        //camera vars
        Vector3 angleoffset = new Vector3(-30, 0, 0);
        private Vector3 _cameraOffset = new Vector3(0, 400, -500);
        //
        public override void Update()
        {

            adjustCarInputValues();
            moveCar();

            Debug.WriteLine("speed = " + speed + ", turn = " + turn);


            if (_boostKey._isHeld)
            {
                boosting = true;
                boostInTank -= 0.5f;
            }
            else
            {
                boosting = false;
                boostInTank += 0.1f;
            }


                Vector3 angleSet = -_gameObject._attemptedTransform._rotation; //make it feel livley in the future
            Vector3 adjPos = _gameObject._attemptedTransform._position + _cameraOffset;
            _cam._transform._position = Vector3.RotatePositionAroundWorldPoint(adjPos,_gameObject._attemptedTransform._position, angleSet);
            _cam._transform._rotation = Vector3.LookAtRotation(_gameObject._attemptedTransform._position - _cam._transform._position) + angleoffset;
            
        }
    }
}
