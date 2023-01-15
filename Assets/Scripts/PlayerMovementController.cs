using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gravity
{
    public class PlayerMovementController : MonoBehaviour
    {
        // Player speed
        [SerializeField] float limitSpeed;
        [SerializeField] float boostLimitSpeed;
        [SerializeField] float force;
        [SerializeField] float boostForce;

        // Orientation
        [SerializeField] Transform orientation;

        Rigidbody rb;
        Vector3 moveDirection;
        Vector3 displacement = new();

        // Elapsed frame(EF) from keys are pressed
        Dictionary<string, int> pressedEF = new()
        {
            {"wKey", 0},
            {"aKey", 0},
            {"sKey", 0},
            {"dKey", 0},
            {"shiftKey", 0}
        };

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
            ManageInput();
            LimitPlayerVelocity();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        void ManageInput()
        {
            displacement = Vector3.zero;

            // Keyboard input
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;
            var shiftKey = Keyboard.current.shiftKey;

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
            if (shiftKey.wasReleasedThisFrame)
            {
                pressedEF["shiftKey"] = 0;
            }

            // While Shift key is pressed, turn on boost
            if (shiftKey.isPressed)
            {
                pressedEF["shiftKey"]++;
            }

            // If W key and S key are pressed when already pressed another one, prioritize latter one
            if (wKey.isPressed && sKey.isPressed)
            {
                if (pressedEF["wKey"] < pressedEF["sKey"])
                {
                    displacement += Vector3.forward;
                }
                else
                {
                    displacement += Vector3.back;
                }
                pressedEF["wKey"]++;
                pressedEF["sKey"]++;
            }
            else
            {
                if (wKey.isPressed)
                {
                    displacement += Vector3.forward;
                    pressedEF["wKey"]++;
                }
                if (sKey.isPressed)
                {
                    displacement += Vector3.back;
                    pressedEF["sKey"]++;
                }
            }
            // If A key and D key are pressed when already pressed another one, prioritize latter one
            if (aKey.isPressed && dKey.isPressed)
            {
                if (pressedEF["aKey"] < pressedEF["dKey"])
                {
                    displacement += Vector3.left;
                }
                else
                {
                    displacement += Vector3.right;
                }
                pressedEF["aKey"]++;
                pressedEF["dKey"] = 0;
            }
            else
            {
                if (aKey.isPressed)
                {
                    displacement += Vector3.left;
                    pressedEF["aKey"]++;
                }
                if (dKey.isPressed)
                {
                    displacement += Vector3.right;
                    pressedEF["dKey"]++;
                }
            }

            moveDirection = orientation.rotation * displacement;
        }

        void MovePlayer()
        {
            if (pressedEF["shiftKey"] == 0)
            {
                rb.AddForce(moveDirection.normalized * force, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * boostForce, ForceMode.Force);
            }
        }

        void LimitPlayerVelocity()
        {
            if (pressedEF["shiftKey"] == 0)
            {
                if (rb.velocity.magnitude > limitSpeed)
                {
                    rb.velocity = moveDirection.normalized * limitSpeed;
                }
            }
            else
            {
                if (rb.velocity.magnitude > boostLimitSpeed)
                {
                    rb.velocity = moveDirection.normalized * boostLimitSpeed;
                }
            }
        }
    }
}

