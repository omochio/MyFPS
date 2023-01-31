using UnityEngine;

namespace Enemy
{
    public class EnemyMovementController : MonoBehaviour
    {
        // Enemy speed
        [SerializeField] float m_limitSpeed;
        [SerializeField] float m_force;

        // Orientation
        [SerializeField] Transform orientation;

        Rigidbody m_rb;
        Vector3 m_moveDirection;
        Vector3 m_displacement;
        Vector3 m_rotation;

        // Start is called before the first frame update
        void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            MoveEnemy();
            SpeedControl();
        }

        void MoveEnemy()
        {
            m_displacement.Set(
                Random.Range(0, 2), 
                Random.Range(0, 2), 
                Random.Range(0, 2));
            m_moveDirection = orientation.rotation * m_displacement;

            m_rotation.Set(
            Random.Range(0.0f, 360.0f),
            Random.Range(0.0f, 360.0f),
            Random.Range(0.0f, 360.0f));            
            transform.rotation = Quaternion.Euler(m_rotation);

            m_rb.AddForce(m_moveDirection.normalized * m_force, ForceMode.Force);
        }

        void SpeedControl()
        {
            Vector3 flatVel = m_rb.velocity;

            if (flatVel.magnitude > m_limitSpeed)
            {
                m_rb.velocity = flatVel.normalized * m_limitSpeed;
            }
        }
    }
}

