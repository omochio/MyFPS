using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        // Player speed variables
        [SerializeField] float m_normalForce;
        [SerializeField] float m_normalUpsAndDownsForce;
        [SerializeField] float m_normalLimitSpeed;
        [SerializeField] float m_boostForce;
        [SerializeField] float m_boostUpsAndDownsForce;
        [SerializeField] float m_boostLimitSpeed;

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
            Move(GetMoveDirection(), GetUpsAndDownsDirection(), m_iptElapsedFrameMgr.GetIptElapsedFrameDict());
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

            return orientation.rotation * moveDirection.normalized;
        }

        Vector3 GetUpsAndDownsDirection()
        {
            Vector3 upsAndDownsDirection = new();
            var iptElapsedFrameDict = m_iptElapsedFrameMgr.GetIptElapsedFrameDict();

            var spaceKey = Keyboard.current.spaceKey;
            var ctrlKey = Keyboard.current.ctrlKey;

            // If Space key and Ctrl key are pressed when already pressed another one, prioritize latter one
            if (spaceKey.isPressed && ctrlKey.isPressed)
            {
                if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.SpaceKey] < iptElapsedFrameDict[InputElapsedFrameManager.InputList.CtrlKey])
                {
                    upsAndDownsDirection += Vector3.up;
                }
                else
                {
                    upsAndDownsDirection += Vector3.down;
                }
            }
            else
            {
                if (spaceKey.isPressed)
                {
                    upsAndDownsDirection += Vector3.up;
                }
                if (ctrlKey.isPressed)
                {
                    upsAndDownsDirection += Vector3.down;
                }
            }

            return upsAndDownsDirection.normalized;
        }

        void Move(Vector3 moveDirection, Vector3 upsAndDownsDirection, Dictionary<InputElapsedFrameManager.InputList, int> iptElapsedFrameDict)
        {
            float force, limitSpeed, upsAndDownsForce;
            // Turn boost mode on if shift key is pressed
            if (iptElapsedFrameDict[InputElapsedFrameManager.InputList.ShiftKey] == 0)
            {
                force = m_normalForce;
                limitSpeed = m_normalLimitSpeed;
                upsAndDownsForce = m_normalUpsAndDownsForce;
            }
            else
            {
                force = m_boostForce;
                limitSpeed = m_boostLimitSpeed;
                upsAndDownsForce = m_boostUpsAndDownsForce;
            }

            // Limit player speed by adding opposite direction force
            var k = Mathf.Clamp01(m_rb.velocity.magnitude / limitSpeed);
            var l = Mathf.Clamp01(new Vector3(0.0f, m_rb.velocity.y, 0.0f).magnitude / limitSpeed);

            m_rb.AddForce(force * moveDirection, ForceMode.Force);
            m_rb.AddForce(upsAndDownsForce * upsAndDownsDirection, ForceMode.Force);
            m_rb.velocity = Vector3.ClampMagnitude(m_rb.velocity, limitSpeed);
        }
    }
}

