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
