using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        // Keyboard and Mouse input keys
        public enum InputList
        {
            WKey,
            AKey,
            SKey,
            DKey,
            ShiftKey,
            CtrlKey,
            MLeftButton,
            MRightButton
        }

        // Elapsed frame(EF) from keys or buttons are pressed
        private Dictionary<InputList, int> m_iptElapsedframe = new()
            {
                {InputList.WKey, 0},
                {InputList.AKey, 0},
                {InputList.SKey, 0},
                {InputList.DKey, 0},
                {InputList.ShiftKey, 0},
                {InputList.CtrlKey, 0},
                {InputList.MLeftButton, 0},
                {InputList.MRightButton, 0}
            };

        public Dictionary<InputList, int> iptElapsedframe
        {
            get { return m_iptElapsedframe; }
        }

        // Update is called once per frame
        void Update()
        {
            ManageInput();
        }

        void ManageInput()
        {
            // Keyboard inputs
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;
            var shiftKey = Keyboard.current.shiftKey;
            var ctrlKey = Keyboard.current.ctrlKey;

            // Mouse button inputs
            var mLeftButton = Mouse.current.leftButton;
            var mRightButton = Mouse.current.rightButton;

            // When release keys, reset elapsed time
            if (wKey.wasReleasedThisFrame)
            {
                iptElapsedframe[InputList.WKey] = 0;
            }
            if (aKey.wasReleasedThisFrame)
            {
                iptElapsedframe[InputList.AKey] = 0;
            }
            if (sKey.wasReleasedThisFrame)
            {
                iptElapsedframe[InputList.SKey] = 0;
            }
            if (dKey.wasReleasedThisFrame)
            {
                iptElapsedframe[InputList.DKey] = 0;
            }
            if (shiftKey.wasReleasedThisFrame)
            {
                iptElapsedframe[InputList.ShiftKey] = 0;
            }
            if (ctrlKey.wasReleasedThisFrame)
            {
                iptElapsedframe[InputList.CtrlKey] = 0;
            }

            if (shiftKey.isPressed)
            {
                iptElapsedframe[InputList.ShiftKey]++;
            }
            if (ctrlKey.isPressed)
            {
                iptElapsedframe[InputList.CtrlKey]++;
            }
            if (mLeftButton.isPressed)
            {
                iptElapsedframe[InputList.MLeftButton]++;
            }

            // If W key and S key are pressed when already pressed another one, prioritize latter one
            if (wKey.isPressed && sKey.isPressed)
            {
                iptElapsedframe[InputList.WKey]++;
                iptElapsedframe[InputList.SKey]++;
            }
            else
            {
                if (wKey.isPressed)
                {
                    iptElapsedframe[InputList.WKey]++;
                }
                if (sKey.isPressed)
                {
                    iptElapsedframe[InputList.SKey]++;
                }
            }
            // If A key and D key are pressed when already pressed another one, prioritize latter one
            if (aKey.isPressed && dKey.isPressed)
            {
                iptElapsedframe[InputList.AKey]++;
                iptElapsedframe[InputList.DKey] = 0;
            }
            else
            {
                if (aKey.isPressed)
                {
                    iptElapsedframe[InputList.AKey]++;
                }
                if (dKey.isPressed)
                {
                    iptElapsedframe[InputList.DKey]++;
                }
            }
        }
    }
}
