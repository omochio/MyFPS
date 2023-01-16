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
        [SerializeField] float dodgeLength;

        // Orientation
        [SerializeField] Transform orientation;

        Rigidbody m_rb;
        Vector3 m_moveDirection;
        Vector3 m_displacement = new();

        // Elapsed frame(EF) from keys are pressed
        Dictionary<string, int> pressedEF = new()
        {
            {"wKey", 0},
            {"aKey", 0},
            {"sKey", 0},
            {"dKey", 0},
            {"shiftKey", 0},
            {"ctrlKey", 0}
        };

        // Start is called before the first frame update
        void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_rb.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
            ManageInput();
            LimitVelocity();
        }

        private void FixedUpdate()
        {
            Move();
        }

        void ManageInput()
        {
            m_displacement = Vector3.zero;

            // Keyboard input
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;
            var shiftKey = Keyboard.current.shiftKey;
            var ctrlKey = Keyboard.current.ctrlKey;

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
            if (ctrlKey.wasReleasedThisFrame)
            {
                pressedEF["ctrlKey"] = 0;
            }

            if (ctrlKey.isPressed)
            {
                pressedEF["ctrlKey"]++;
            }

            if (shiftKey.isPressed)
            {
                pressedEF["shiftKey"]++;
            }

            // If W key and S key are pressed when already pressed another one, prioritize latter one
            if (wKey.isPressed && sKey.isPressed)
            {
                if (pressedEF["wKey"] < pressedEF["sKey"])
                {
                    m_displacement += Vector3.forward;
                }
                else
                {
                    m_displacement += Vector3.back;
                }
                pressedEF["wKey"]++;
                pressedEF["sKey"]++;
            }
            else
            {
                if (wKey.isPressed)
                {
                    m_displacement += Vector3.forward;
                    pressedEF["wKey"]++;
                }
                if (sKey.isPressed)
                {
                    m_displacement += Vector3.back;
                    pressedEF["sKey"]++;
                }
            }
            // If A key and D key are pressed when already pressed another one, prioritize latter one
            if (aKey.isPressed && dKey.isPressed)
            {
                if (pressedEF["aKey"] < pressedEF["dKey"])
                {
                    m_displacement += Vector3.left;
                }
                else
                {
                    m_displacement += Vector3.right;
                }
                pressedEF["aKey"]++;
                pressedEF["dKey"] = 0;
            }
            else
            {
                if (aKey.isPressed)
                {
                    m_displacement += Vector3.left;
                    pressedEF["aKey"]++;
                }
                if (dKey.isPressed)
                {
                    m_displacement += Vector3.right;
                    pressedEF["dKey"]++;
                }
            }

            m_moveDirection = orientation.rotation * m_displacement;
        }

        void Move()
        {
            if (pressedEF["shiftKey"] == 0)
            {
                m_rb.AddForce(m_moveDirection.normalized * force, ForceMode.Force);
            }
            else
            {
                // While Shift key is pressed, turn on boost
                m_rb.AddForce(m_moveDirection.normalized * boostForce, ForceMode.Force);
            }

            // When Ctrl key is pressed, dodge
            if (pressedEF["ctrlKey"] == 1)
            {
                transform.position += m_moveDirection.normalized * dodgeLength;
            }
        }

        void LimitVelocity()
        {
            if (pressedEF["shiftKey"] == 0)
            {
                if (m_rb.velocity.magnitude > limitSpeed)
                {
                    m_rb.velocity = m_moveDirection.normalized * limitSpeed;
                }
            }
            else
            {
                if (m_rb.velocity.magnitude > boostLimitSpeed)
                {
                    m_rb.velocity = m_moveDirection.normalized * boostLimitSpeed;
                }
            }
        }
    }
}

