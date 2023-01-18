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

        // Player input manager
        InputElapsedFrameManager m_inputElapsedFrame;

        void Awake()
        {
            m_inputElapsedFrame = gameObject.AddComponent<InputElapsedFrameManager>();
        }

        void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_rb.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
            LimitVelocity(CalcMoveDirection(), m_inputElapsedFrame.GetInputElapsedFrame());
        }

        private void FixedUpdate()
        {
            Move(CalcMoveDirection(), m_inputElapsedFrame.GetInputElapsedFrame());
        }

        Vector3 CalcMoveDirection()
        {
            Vector3 displacement = new();
            var iptElapsedFrameDict = m_inputElapsedFrame.GetInputElapsedFrame(); 

            // Keyboard inputs
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;

            // If W key and S key are pressed when already pressed another one, prioritize latter one
            if (wKey.isPressed && sKey.isPressed)
            {
                if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.WKey] < iptElapsedFrameDict[InputElapsedFrameManager.InputList.SKey])
                {
                    displacement += Vector3.forward;
                }
                else
                {
                    displacement += Vector3.back;
                }
            }
            else
            {
                if (wKey.isPressed)
                {
                    displacement += Vector3.forward;
                }
                if (sKey.isPressed)
                {
                    displacement += Vector3.back;
                }
            }
            // If A key and D key are pressed when already pressed another one, prioritize latter one
            if (aKey.isPressed && dKey.isPressed)
            {
                if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.AKey] < iptElapsedFrameDict[InputElapsedFrameManager.InputList.DKey])
                {
                    displacement += Vector3.left;
                }
                else
                {
                    displacement += Vector3.right;
                }
            }
            else
            {
                if (aKey.isPressed)
                {
                    displacement += Vector3.left;                     
                }
                if (dKey.isPressed)
                {
                    displacement += Vector3.right;
                }
            }

            Vector3 moveDirection = orientation.rotation * displacement;

            return moveDirection;
        }

        void Move(Vector3 moveDirection, Dictionary<InputElapsedFrameManager.InputList, int> iptElapsedFrameDict)
        {
            if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.ShiftKey] == 0)
            {
                m_rb.AddForce(moveDirection.normalized * force, ForceMode.Force);
            }
            else
            {
                // While Shift key is pressed, turn on boost
                m_rb.AddForce(moveDirection.normalized * boostForce, ForceMode.Force);
            }
        }

        void LimitVelocity(Vector3 moveDirection, Dictionary<InputElapsedFrameManager.InputList, int> iptElapsedFrameDict)
        {
            if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.ShiftKey] == 0)
            {
                if (m_rb.velocity.magnitude > limitSpeed)
                {
                    m_rb.velocity = moveDirection.normalized * limitSpeed;
                }
            }
            else
            {
                if (m_rb.velocity.magnitude > boostLimitSpeed)
                {
                    m_rb.velocity = moveDirection.normalized * boostLimitSpeed;
                }
            }
        }
    }
}

