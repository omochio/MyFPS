using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        // Sensitivity
        [SerializeField] float sensX = 0;
        [SerializeField] float sensY = 0;

        // Player orientation
        //[SerializeField] Transform orientation;

        float m_xRotation = 0;
        float m_yRotation = 0;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            float mouseX = Mouse.current.delta.ReadValue().x * Time.deltaTime * sensX;
            float mouseY = Mouse.current.delta.ReadValue().y * Time.deltaTime * sensY;

            m_yRotation += mouseX;
            m_xRotation -= mouseY;
            m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(m_xRotation, m_yRotation, 0);
        }
    }
}

