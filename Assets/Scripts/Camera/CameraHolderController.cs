using UnityEngine;


namespace Player
{
    public class CameraHolderController : MonoBehaviour
    {
        [SerializeField] Transform playerTransform;

        // Update is called once per frame
        void Update()
        {
            transform.position = playerTransform.position;
        }
    }
}

