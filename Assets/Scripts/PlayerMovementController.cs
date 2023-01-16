using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
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

        // Player input manager
        PlayerInputManager m_playerInputManager;

        // Start is called before the first frame update
        void Start()
        {
            m_playerInputManager = GetComponent<PlayerInputManager>();
            m_rb = GetComponent<Rigidbody>();
            m_rb.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
            CalcDisplacement();
            LimitVelocity();
        }

        private void FixedUpdate()
        {
            Move();
        }

        void CalcDisplacement()
        {
            m_displacement = Vector3.zero;

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

            // If W key and S key are pressed when already pressed another one, prioritize latter one
            if (wKey.isPressed && sKey.isPressed)
            {
                if (m_playerInputManager.iptElapsedframe[PlayerInputManager.InputList.WKey] < m_playerInputManager.iptElapsedframe[PlayerInputManager.InputList.SKey])
                {
                    m_displacement += Vector3.forward;
                }
                else
                {
                    m_displacement += Vector3.back;
                }
            }
            else
            {
                if (wKey.isPressed)
                {
                    m_displacement += Vector3.forward;
                }
                if (sKey.isPressed)
                {
                    m_displacement += Vector3.back;
                }
            }
            // If A key and D key are pressed when already pressed another one, prioritize latter one
            if (aKey.isPressed && dKey.isPressed)
            {
                if (m_playerInputManager.iptElapsedframe[PlayerInputManager.InputList.AKey] < m_playerInputManager.iptElapsedframe[PlayerInputManager.InputList.DKey])
                {
                    m_displacement += Vector3.left;
                }
                else
                {
                    m_displacement += Vector3.right;
                }
            }
            else
            {
                if (aKey.isPressed)
                {
                    m_displacement += Vector3.left;                     
                }
                if (dKey.isPressed)
                {
                    m_displacement += Vector3.right;
                }
            }

            m_moveDirection = orientation.rotation * m_displacement;
        }

        void Move()
        {
            if (m_playerInputManager.iptElapsedframe[PlayerInputManager.InputList.ShiftKey] == 0)
            {
                m_rb.AddForce(m_moveDirection.normalized * force, ForceMode.Force);
            }
            else
            {
                // While Shift key is pressed, turn on boost
                m_rb.AddForce(m_moveDirection.normalized * boostForce, ForceMode.Force);
            }

            // When Ctrl key is pressed, dodge
            if (m_playerInputManager.iptElapsedframe[PlayerInputManager.InputList.CtrlKey] == 1)
            {
                transform.position += m_moveDirection.normalized * dodgeLength;
            }
        }

        void LimitVelocity()
        {
            if (m_playerInputManager.iptElapsedframe[PlayerInputManager.InputList.ShiftKey] == 0)
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

