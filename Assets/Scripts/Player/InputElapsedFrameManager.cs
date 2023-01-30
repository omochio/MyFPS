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
            SpaceKey,
            CtrlKey
        }

        // Elapsed frame(EF) from keys or buttons are pressed
        private readonly Dictionary<InputList, int> m_iptElapsedFrameDict = new()
            {
                { InputList.WKey, 0 },
                { InputList.AKey, 0 },
                { InputList.SKey, 0 },
                { InputList.DKey, 0 },
                { InputList.ShiftKey, 0 },
                { InputList.SpaceKey, 0 }, 
                { InputList.CtrlKey, 0 },
            };

        public Dictionary<InputList, int> GetIptElapsedFrameDict()
        {
            // Keyboard inputs
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;
            var shiftKey = Keyboard.current.shiftKey;
            var spaceKey = Keyboard.current.spaceKey;
            var ctrlKey = Keyboard.current.ctrlKey;

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
            if (spaceKey.wasReleasedThisFrame)
            {
                m_iptElapsedFrameDict[InputList.SpaceKey] = 0;
            }
            if (ctrlKey.wasReleasedThisFrame)
            {
                m_iptElapsedFrameDict[InputList.CtrlKey] = 0;
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
            if (spaceKey.isPressed)
            {
                m_iptElapsedFrameDict[InputList.SpaceKey]++;
            }
            if (ctrlKey.isPressed)
            {
                m_iptElapsedFrameDict[(InputList.CtrlKey)]++;
            }

            return m_iptElapsedFrameDict;
        }
    }
}
