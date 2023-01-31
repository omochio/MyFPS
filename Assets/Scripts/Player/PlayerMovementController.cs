using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        // Player speed variables
        [SerializeField] float m_normalLimitSpeed;
        [SerializeField] float m_boostLimitSpeed;
        [SerializeField] float m_normalForce;
        [SerializeField] float m_boostForce;

        // Orientation
        [SerializeField] Transform orientation;

        Rigidbody m_rb;

        // Player input manager
        InputElapsedFrameManager m_iptElapsedFrameMgr;

        void Awake()
        {
            m_iptElapsedFrameMgr = gameObject.AddComponent<InputElapsedFrameManager>();
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
        }

        private void FixedUpdate()
        {
            Move(GetMoveDirection(), m_iptElapsedFrameMgr.GetIptElapsedFrameDict());
        }

        Vector3 GetMoveDirection()
        {
            Vector3 moveDirection = new();
            var iptElapsedFrameDict = m_iptElapsedFrameMgr.GetIptElapsedFrameDict(); 

            // Keyboard inputs
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;
            var spaceKey = Keyboard.current.spaceKey;
            var ctrlKey = Keyboard.current.ctrlKey;

            // If W key and S key are pressed when already pressed another one, prioritize latter one
            if (wKey.isPressed && sKey.isPressed)
            {
                if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.WKey] < iptElapsedFrameDict[InputElapsedFrameManager.InputList.SKey])
                {
                    moveDirection += Vector3.forward;
                }
                else
                {
                    moveDirection += Vector3.back;
                }
            }
            else
            {
                if (wKey.isPressed)
                {
                    moveDirection += Vector3.forward;
                }
                if (sKey.isPressed)
                {
                    moveDirection += Vector3.back;
                }
            }
            // If A key and D key are pressed when already pressed another one, prioritize latter one
            if (aKey.isPressed && dKey.isPressed)
            {
                if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.AKey] < iptElapsedFrameDict[InputElapsedFrameManager.InputList.DKey])
                {
                    moveDirection += Vector3.left;
                }
                else
                {
                    moveDirection += Vector3.right;
                }
            }
            else
            {
                if (aKey.isPressed)
                {
                    moveDirection += Vector3.left;                     
                }
                if (dKey.isPressed)
                {
                    moveDirection += Vector3.right;
                }
            }

            moveDirection = orientation.rotation * moveDirection;

            // If Space key and Ctrl key are pressed when already pressed another one, prioritize latter one
            if (spaceKey.isPressed && ctrlKey.isPressed)
            {
                if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.SpaceKey] < iptElapsedFrameDict[InputElapsedFrameManager.InputList.CtrlKey])
                {
                    moveDirection += Vector3.up * 5;
                }
                else
                {
                    moveDirection += Vector3.down * 5;
                }
            }
            else
            {
                if (spaceKey.isPressed)
                {
                    moveDirection += Vector3.up * 5;
                }
                if (ctrlKey.isPressed)
                {
                    moveDirection += Vector3.down * 5;
                }
            }

            return moveDirection.normalized;
        }

        void Move(Vector3 moveDirection, Dictionary<InputElapsedFrameManager.InputList, int> iptElapsedFrameDict)
        {
            float force, limitSpeed;
            // Turn boost mode on if shift key is pressed
            if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.ShiftKey] == 0)
            {
                force = m_normalForce;
                limitSpeed = m_normalLimitSpeed;
            }
            else
            {
                force = m_boostForce;
                limitSpeed = m_boostLimitSpeed;
            }

            // Limit player speed by adding opposite direction force
            // The closer the speed to limitSpeed, the greater the force from opposite direction
            // The father the moveDirection to velocity, the smaller the force from opposite direction
            var k = Mathf.Clamp01(m_rb.velocity.magnitude / limitSpeed) * (1 - Mathf.Acos(Mathf.Clamp01(Vector3.Dot(m_rb.velocity.normalized, moveDirection))) / Mathf.PI / 2);
            m_rb.AddForce(force * moveDirection, ForceMode.Force);
            m_rb.AddForce(force * k * -moveDirection, ForceMode.Force);
        }
    }
}

