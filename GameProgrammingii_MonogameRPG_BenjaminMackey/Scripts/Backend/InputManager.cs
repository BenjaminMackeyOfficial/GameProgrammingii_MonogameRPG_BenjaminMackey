using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    
    public static class InputManager
    {
        

        private static List<Updatable> _inputActions = new List<Updatable>();

        
        public static void ActivateInputAction(Updatable action) 
        {
            _inputActions.Add(action);
        }
        public static void DeactivateInput(Updatable action)
        {
            _inputActions.Remove(action);
        }
        public static void updateAll()
        {
            foreach (Updatable item in _inputActions)
            {
                item.Update();
            }
        }


    }
    //overarching class==================
    public class InputAction : Updatable
    {
        
        public InputAction()
        {
            InputManager.ActivateInputAction(this);
        }
        public virtual void Update()
        {
        
        }
    }

    public class Vector2InputMap : Updatable
    {
        public double x { get; private set; }
        public double y { get; private set; }

        public ButtonAction _right;
        public ButtonAction _up;
        public ButtonAction _down;
        public ButtonAction _left;
        public JoystickAction _joyStick; //will come back to
        public Vector2InputMap(ButtonAction up, ButtonAction down, ButtonAction right, ButtonAction left)
        {
            InputManager.ActivateInputAction(this);
            if(up != null) _up = up;
            if(down != null) _down = down;
            if(right != null) _right = right;
            if(left != null) _left = left;
        }
        public virtual void Update()
        {
            Vector2 tmp = new Vector2(0,0);
            if (_right._isHeld) tmp.x += 1;
            if (_down._isHeld) tmp.y -= 1;
            if (_left._isHeld) tmp.x -= 1;
            if(_up._isHeld) tmp.y += 1;

            //for when i add joystick control
            tmp.y = tmp.y.Clamp(-1, 1);
            tmp.x = tmp.x.Clamp(-1, 1);
            //-----------------------------

            x = tmp.x;
            y = tmp.y;
            //.Debug.WriteLine(x);
        }
    }
    //=============================================

    //subclasses===============================
    //custom args
    public class InputArgs : EventArgs
    {
        public KeyState state;
        public InputArgs(KeyState downOrUp) { state = downOrUp; }
    }

    public class ButtonAction : InputAction
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);



        public event EventHandler<InputArgs> ButtonPressed;
        public ConsoleKey[] ConsoleKeys { get; private set; }
        private KeyState _state = KeyState.Up;
        public bool _isHeld { get; private set; }
        public bool _pressedThisFrame { get; private set; }
        public bool _releasedThisFrame { get; private set; }
        public void inputted()
        {
            ButtonPressed?.Invoke(this, new InputArgs(_state));
        }
        public ButtonAction(ConsoleKey[] keys)
        {
            if (keys != null) ConsoleKeys = keys;
        
        }
        public ButtonAction(ConsoleKey key)
        {
            if (key == null) return;

            ConsoleKey[] tmp = { key };
            ConsoleKeys = tmp;
        }
        public override void Update()
        {
            foreach (ConsoleKey key in ConsoleKeys)
            {
                _pressedThisFrame = false;
                _releasedThisFrame = false;
                if( GetAsyncKeyState((int)key) < 0 && _isHeld == false)
                {
                    _isHeld = true;
                    _pressedThisFrame = true;
                    _state = KeyState.Down;
                    inputted();
                }
                else if(GetAsyncKeyState((int)key) >= 0 && _isHeld == true)
                {
                    _isHeld = false;
                    _releasedThisFrame = true;
                    _state = KeyState.Up;
                }
                //Debug.WriteLine(key + " " + _isHeld);
            }
        }

    }


    public class JoystickAction : InputAction //i have no idea how to read joysicks
    {
        public JoystickAction() { }
    }
    //==========================================
}
