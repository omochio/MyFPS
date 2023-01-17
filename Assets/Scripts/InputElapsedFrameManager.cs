using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputElapsedFrameManager : MonoBehaviour
    {
        // Keyboard and Mouse input keys
        public enum InputList
        {
            WKey,
            AKey,
            SKey,
            DKey,
            ShiftKey,
        }

        // Elapsed frame(EF) from keys or buttons are pressed
        private readonly Dictionary<InputList, int> m_iptElapsedFrameDict = new()
            {
                {InputList.WKey, 0},
                {InputList.AKey, 0},
                {InputList.SKey, 0},
                {InputList.DKey, 0},
                {InputList.ShiftKey, 0},
            };

        public Dictionary<InputList, int> GetInputElapsedFrame()
        {
            // Keyboard inputs
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;
            var shiftKey = Keyboard.current.shiftKey;

            // When release keys, reset elapsed time
            if (wKey.wasReleasedThisFrame)
            {
                m_iptElapsedFrameDict[InputList.WKey] = 0;
            }
            if (aKey.wasReleasedThisFrame)
            {
                m_iptElapsedFrameDict[InputList.AKey] = 0;
            }
            if (sKey.wasReleasedThisFrame)
            {
                m_iptElapsedFrameDict[InputList.SKey] = 0;
            }
            if (dKey.wasReleasedThisFrame)
            {
                m_iptElapsedFrameDict[InputList.DKey] = 0;
            }
            if (shiftKey.wasReleasedThisFrame)
            {
                m_iptElapsedFrameDict[InputList.ShiftKey] = 0;
            }

            // When press keys, add elapsed time
            if (wKey.isPressed)
            {
                m_iptElapsedFrameDict[InputList.WKey]++;
            }
            if (aKey.isPressed)
            {
                m_iptElapsedFrameDict[InputList.AKey]++;
            }
            if (sKey.isPressed)
            {
                m_iptElapsedFrameDict[InputList.SKey]++;
            }
            if (dKey.isPressed)
            {
                m_iptElapsedFrameDict[InputList.DKey]++;
            }
            if (shiftKey.isPressed)
            {
                m_iptElapsedFrameDict[InputList.ShiftKey]++;
            }

            return m_iptElapsedFrameDict;
        }
    }
}
