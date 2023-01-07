using UnityEngine;

namespace Gravity
{
    public class EnemyMovementController : MonoBehaviour
    {
        // Enemy speed
        [SerializeField] float limitSpeed;
        [SerializeField] float force;

        // Orientation
        [SerializeField] Transform orientation;

        Rigidbody rb;
        Vector3 moveDirection;
        Vector3 displacement;
        Vector3 rotation;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            MoveEnemy();
            SpeedControl();
        }

        void MoveEnemy()
        {
            displacement.Set(
                Random.Range(0, 2), 
                Random.Range(0, 2), 
                Random.Range(0, 2));
            moveDirection = orientation.rotation * displacement;

            rotation.Set(
            Random.Range(0.0f, 360.0f),
            Random.Range(0.0f, 360.0f),
            Random.Range(0.0f, 360.0f));            
            transform.rotation = Quaternion.Euler(rotation);

            rb.AddForce(moveDirection.normalized * force, ForceMode.Force);
        }

        void SpeedControl()
        {
            Vector3 flatVel = rb.velocity;

            if (flatVel.magnitude > limitSpeed)
            {
                rb.velocity = flatVel.normalized * limitSpeed;
            }
        }
    }
}

