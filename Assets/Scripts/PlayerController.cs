using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gravity
{
    public class PlayerController : MonoBehaviour
    {
        // Player speed
        [SerializeField]
        float moveSpeed = 1.0f;

        // Elapsed frame(EF) from keys are pressed
        Dictionary<string, int> pressedEF = new()
        {
            {"wKey", 0 },
            {"aKey", 0 },
            {"sKey", 0 },
            {"dKey", 0 }
        };

        // Start is called before the first frame update
        void Start()
        {
            transform.position = Vector3.up;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        void Move()
        {
            Vector3 displacement = new();

            // Keyboard input
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;

            // When release keys, reset elapsed time
            if (wKey.wasReleasedThisFrame)
            {
                pressedEF["wKey"] = 0;
            }
            if (aKey.wasReleasedThisFrame)
            {
                pressedEF["aKey"] = 0;
            }
            if (sKey.wasReleasedThisFrame)
            {
                pressedEF["sKey"] = 0;
            }
            if (dKey.wasReleasedThisFrame)
            {
                pressedEF["dKey"] = 0;
            }

            // If W key and S key are pressed when already pressed another one, prioritize latter one
            if (wKey.isPressed && sKey.isPressed)
            {
                if (pressedEF["wKey"] < pressedEF["sKey"])
                {
                    displacement += moveSpeed * Time.deltaTime * Vector3.forward;
                }
                else
                {
                    displacement += moveSpeed * Time.deltaTime * Vector3.back;
                }
                pressedEF["wKey"]++;
                pressedEF["sKey"]++;
            }
            else
            {
                if (wKey.isPressed)
                {
                    displacement += moveSpeed * Time.deltaTime * Vector3.forward;
                    pressedEF["wKey"]++;
                }
                if (sKey.isPressed)
                {
                    displacement += moveSpeed * Time.deltaTime * Vector3.back;
                    pressedEF["sKey"]++;
                }
            }
            // If A key and D key are pressed when already pressed another one, prioritize latter one
            if (aKey.isPressed && dKey.isPressed)
            {
                if (pressedEF["aKey"] < pressedEF["dKey"])
                {
                    displacement += moveSpeed * Time.deltaTime * Vector3.left;
                }
                else
                {
                    displacement += moveSpeed * Time.deltaTime * Vector3.right;
                }
                pressedEF["aKey"]++;
                pressedEF["dKey"] = 0;
            }
            else
            {
                if (aKey.isPressed)
                {
                    displacement += moveSpeed * Time.deltaTime * Vector3.left;
                    pressedEF["aKey"]++;
                }
                if (dKey.isPressed)
                {
                    displacement += moveSpeed * Time.deltaTime * Vector3.right;
                    pressedEF["dKey"]++;
                }
            }

            //foreach (var i in pressedEF)
            //{
            //    Debug.Log(i);
            //}

            transform.position += displacement;

        }
    }
}

