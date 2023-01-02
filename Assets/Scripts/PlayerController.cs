using UnityEngine;
using UnityEngine.InputSystem;

namespace Gravity
{
    public class PlayerController : MonoBehaviour
    {
        // Player speed
        [SerializeField]
        float moveSpeed = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            transform.position = Vector3.up;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 displacement = new();

            // WASD keyboard input
            var wKey = Keyboard.current.wKey;
            var aKey = Keyboard.current.aKey;
            var sKey = Keyboard.current.sKey;
            var dKey = Keyboard.current.dKey;

            if (wKey.isPressed)
            {
                displacement += moveSpeed * Time.deltaTime * Vector3.forward;
            }
            if (aKey.isPressed)
            {
                displacement += moveSpeed * Time.deltaTime * Vector3.left;
            }
            if (sKey.isPressed)
            {
                displacement += moveSpeed * Time.deltaTime * Vector3.back;
            }
            if (dKey.isPressed)
            {
                displacement += moveSpeed * Time.deltaTime * Vector3.right;
            }

            transform.position += displacement;

        }
    }
}

