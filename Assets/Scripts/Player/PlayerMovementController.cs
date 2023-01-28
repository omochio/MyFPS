using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        // Player speed variables
        [SerializeField] float normalLimitSpeed;
        [SerializeField] float boostLimitSpeed;
        [SerializeField] float normalForce;
        [SerializeField] float boostForce;

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
            Vector3 displacement = new();
            var iptElapsedFrameDict = m_iptElapsedFrameMgr.GetIptElapsedFrameDict(); 

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

            return moveDirection.normalized;
        }

        void Move(Vector3 moveDirection, Dictionary<InputElapsedFrameManager.InputList, int> iptElapsedFrameDict)
        {
            float force, limitSpeed;
            // Turn boost mode on if shift key is pressed
            if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.ShiftKey] == 0)
            {
                force = normalForce;
                limitSpeed = normalLimitSpeed;
            }
            else
            {
                force = boostForce;
                limitSpeed = boostLimitSpeed;
            }

            // Limit player speed by adding opposite direction force
            // The closer the speed to limitSpeed, the greater the force from opposite direction
            // The father the moveDirection to velocity, the smaller the force from opposite direction
            var k = Mathf.Clamp(m_rb.velocity.magnitude / limitSpeed, 0.0f, 1.0f) * (1 - Mathf.Acos(Vector3.Dot(m_rb.velocity.normalized, moveDirection)) / Mathf.PI / 2);
            m_rb.AddForce(force * moveDirection, ForceMode.Force);
            m_rb.AddForce(force * k * -moveDirection, ForceMode.Force);
        }
    }
}

