using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    // Sensitivity
    [SerializeField] float sensX = 0;
    [SerializeField] float sensY = 0;

    // Player orientation
    [SerializeField] Transform orientation;

    float xRotation = 0;
    float yRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Pointer.current.delta.ReadValue().x * Time.deltaTime * sensX;
        float mouseY = Pointer.current.delta.ReadValue().y * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
