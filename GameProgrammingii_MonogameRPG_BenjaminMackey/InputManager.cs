using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey
{
    public static class InputManager
    {
        private static List<InputAction> _inputActions = new List<InputAction>();

        
        public static void ActivateInputAction(InputAction action) 
        {
            _inputActions.Add(action);
        }
        public static void DeactivateInput(InputAction action)
        {
            _inputActions.Remove(action);
        }
        public static void updateAll()
        {
            foreach (InputAction item in _inputActions)
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

        }
        public virtual void Update()
        {
        
        }
    }

    public class Vector2InputMap : Updatable
    {
        public float x { get; private set; }
        public float y { get; private set; }

        public ButtonAction _right;
        public ButtonAction _up;
        public ButtonAction _down;
        public ButtonAction _left;
        public JoystickAction _joyStick; //will come back to
        public Vector2InputMap(ButtonAction up, ButtonAction down, ButtonAction right, ButtonAction left)
        {
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
        }
    }
    //=============================================

    //subclasses===============================
    public class ButtonAction : InputAction
    {
        public event EventHandler ButtonPressed;
        public ConsoleKey[] ConsoleKeys { get; private set; }
        public bool _isHeld { get; private set; }
        public void inputted()
        {
            ButtonPressed.Invoke(this, EventArgs.Empty);
        }
        public ButtonAction(ConsoleKey[] keys)
        {
            if (keys != null) ConsoleKeys = keys;
        }
    }

    public class JoystickAction : InputAction //i have no idea how to read joysicks
    {
        public JoystickAction() { }
    }
    //==========================================
}
