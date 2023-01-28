using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAtackController : MonoBehaviour
    {
        Ray m_ray;
        bool m_attackFlag;

        void Update()
        {
            // If mouse left button is cricked while this frame, update ray and turn flag true
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                m_ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                m_attackFlag = true;
            }
        }

        void FixedUpdate()
        {
            // If flag is true, fire raycast and turn flag false
            if (m_attackFlag)
            {
                if (Physics.Raycast(m_ray, out RaycastHit hit))
                {
                    // If ray hit a enemy, destroy it
                    if (hit.collider.gameObject.CompareTag("Destroyable"))
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
                m_attackFlag = false;
            }
        }   
    }
}

