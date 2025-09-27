using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    private float yRotation = 0f;
    private float xRotation = 0f;
    public GameObject player;

    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        
    }

}