using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Adjustable;
using GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    public class ScreenUiForCar
    {
        public GameObject _boostMeter;
        public GameObject _healthMeter;
        public GameObject _killedEnemyAlert;

        float screenEndPosPercent = 0.4f;

        public ScreenUiForCar()
        {
            _boostMeter = new GameObject();
            SpriteRenderer bostMeeterUi = new SpriteRenderer(SpriteBin.GetSprite("solidBlackSquare"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            bostMeeterUi.zLayer = 0.5f;
            bostMeeterUi.UI = true;
            _boostMeter.AddComponent(bostMeeterUi);

            _healthMeter = new GameObject();
            SpriteRenderer healthMeterUi = new SpriteRenderer(SpriteBin.GetSprite("solidBlackSquare"), new Vector2(1, 1), SpriteRenderer.RenderFrom.Centre);
            healthMeterUi.zLayer = 0.4f;
            healthMeterUi.UI = true;
            _healthMeter.AddComponent(healthMeterUi);
        }
        public void AdjustUiMeters(float healthPercent, float boostPercent)
        {
            Debug.WriteLine("health perc = " + healthPercent + "boost perc = " + boostPercent);
            healthPercent = healthPercent.Clamp(0, 1);
            boostPercent = boostPercent.Clamp(0, 1);

            float barSize = screenEndPosPercent;

            float boostBarSize = (barSize * boostPercent);
            float healthBarSize = (barSize * healthPercent);
            

           
            _boostMeter._transform._position = new Vector3(10 + barSize * 2, 10, 0);
            _boostMeter._transform._scale = new Vector3(boostBarSize, 0.1f,0);

            _healthMeter._transform._position = new Vector3(10 + barSize * 2, 140, 0);
            _healthMeter._transform._scale = new Vector3(healthBarSize, 0.1f, 0);
        }
        public void PingEnemyKill()
        {

        }
    }
    public class CarController : Behavioral
    {
        private GameObject _cam;
        private Collider _collider;
        private SpriteRenderer _spriteRenderer;
        private Vector2InputMap _baseControls;
        private ButtonAction _boostKey;
        private ButtonAction _driftKey;

        private ScreenUiForCar _screenUiForCar;

        


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

        public bool queDamage=false;

        private void ColliderHit((Collider self, Collider other) data)
        {
            GameObject collision = data.other._gameObject;

            if (collision.GetComponent<PlaneColider>() != null)
            {
                
                speed = -speed * 0.8f;
            }
            if (collision.CheckTag("enemy"))
            {
                if (collision.CheckTag("bomber")) ForceTakeDamage(int.MaxValue);

                if (boosting == true && boostInTank > 0f) collision.Destroy();
                else queDamage = true;
            }
            else if(collision.CheckTag("pickup"))
            {
                Debug.WriteLine("col = " + collision.GetComponent<Pickup>());
                collision.GetComponentAmbig<Pickup>().Effect(this);
                collision.Destroy();
            }
            

        }
        
        public void ForceTakeDamage(float dmg)
        {
            if (invFrames > 0) return;
            health -= dmg;
        }

        private GameObject _map;
        public override void Initialize()
        {
             _map = ObjectManager.FindObjectByTag("map");
            if (_map != null) _gameObject._transform._position = _map.GetComponent<Map>()._playerSpawn;

            _cam = BuildFollowCam();
            _collider = CreateCarColider();
            _spriteRenderer = CreateCarRenderer();
            _baseControls = Vector2InputMap.DefaultWasdMap();

            _driftKey = new ButtonAction(ConsoleKey.P);//add more buttons in future
            _boostKey = new ButtonAction(ConsoleKey.O);

            _gameObject.AddComponent(_collider);
            _collider.OnTriggerEnter += ColliderHit;
            _collider._static = false;
            _gameObject.AddComponent(_spriteRenderer);
            _screenUiForCar = new ScreenUiForCar();
            //_map.GetComponent<Map>().TellEnemiesHeyImOverHere(_gameObject);
        }
        


        public bool boosting = false;

        public float _maxTurn = 2;
        public float _maxSpeed = 300;
        public float _acceleration = 100f;
        public float _boostMult = 0.4f;
        public float _aeroFactor = 1.001f;
        public float _rollingResistance = 1.01f;

        public float _bodyStifness = 100f; 

        private float turn= 0f;
        
        private float speed = 0f;
        public float boostInTank = 0f;

        public float health = 100f;
        //physics

        public int invFrames = 0;
        private void adjustCarInputValues() //ill make this better in the future
        {
            speed += (float)_baseControls.y * _acceleration * framegenTimeEqv;
            speed /= _aeroFactor;
            speed = speed.Clamp(-30f, _maxSpeed);


            if (_boostKey._isHeld && boostInTank > 0) speed += _boostMult * _acceleration;

            turn = (-(float)_baseControls.x * _bodyStifness ).Clamp(-_maxTurn, _maxTurn);
        }
        //
        private void moveCar() //go set up a propper move method or somehting in wherever
        {
            Vector3 newRotation = new Vector3(0, turn, 0);
            Vector3 newPosition = _gameObject._transform.Right() * speed;

            _gameObject._attemptedTransform._position += newPosition;
            _gameObject._attemptedTransform._rotation += new Vector3(0, turn, 0);

            //Debug.WriteLine(turn);
            //Debug.WriteLine(_gameObject._transform._position.x + " " + newPosition.x);

        }



        private void Die()
        {
            _acceleration = 0;
            _bodyStifness = 0;
            _maxSpeed = 0;
            _gameObject.GetComponent<SpriteRenderer>()._spriteSheet = SpriteBin.GetSprite("eplodedCar");
        }
        private float framegenTimeEqv = 0.01f; //ask simon about


        //camera vars
        Vector3 angleoffset = new Vector3(-30, 0, 0);
        private Vector3 _cameraOffset;
        //
        public override void Update()
        {
            adjustCarInputValues();
            moveCar();
            //Debug.WriteLine("speed = " + speed + ", turn = " + turn);

            float focalMod = 0.5f + (float)Math.Sin((speed / _maxSpeed) / 2f);
            //Debug.WriteLine(speed);
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
            boostInTank = boostInTank.Clamp(0f, 40f);

            if (queDamage == true && invFrames <= 0) health -= 1f;
            if (health < 0)
            {
                focalMod = 0.5f;
                Die();
            }
            //Debug.WriteLine(health);
            health = health.Clamp(0f, 100f);
            invFrames = (invFrames-1).Clamp(0,100);
            queDamage = false;


            _screenUiForCar.AdjustUiMeters(health / 100f, boostInTank / 40f);
            
            _cameraOffset = new Vector3(0, 400, -700 * focalMod );
            Vector3 angleSet = -_gameObject._attemptedTransform._rotation; //make it feel livley in the future
            Vector3 adjPos = _gameObject._attemptedTransform._position + _cameraOffset;
            _cam._transform._position = Vector3.RotatePositionAroundWorldPoint(adjPos,_gameObject._attemptedTransform._position, angleSet);
            _cam._transform._rotation = Vector3.LookAtRotation(_gameObject._attemptedTransform._position - _cam._transform._position) + angleoffset;
            
        }
    }
}
