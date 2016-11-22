using UnityEngine;

public class KartCameraController : MonoBehaviour
{

    public Transform target;
    public float distanceAway = 6f;
    public float distanceUp = 2f;
    public Vector3 lookAtOffset;
    private float verticalSensitivity = 50.0f;
    private float horizontalSensitivity = 150.0f;

    public float minXAngle = 5.0f;
    public float maxXAngle = 45.0f;
    public float yAngleOpening = 60f;

    public float damping = 4.0f;

    private float yRotation;
    private float xRotation;
    
    public void Awake()
    {
        Vector3 angles = transform.localEulerAngles;
    }

    public void LateUpdate ()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yRotation += mouseX * horizontalSensitivity * Time.deltaTime;
        xRotation += mouseY * verticalSensitivity * Time.deltaTime;
        
        xRotation = Mathf.Clamp(xRotation, minXAngle, maxXAngle);
        yRotation = Mathf.Clamp(yRotation, yAngleOpening * -0.5f, yAngleOpening * 0.5f);

        Quaternion rotation = Quaternion.Euler(xRotation + target.rotation.eulerAngles.x, 
            yRotation + target.rotation.eulerAngles.y, 0);
        Quaternion smothRotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

        transform.position = rotation * new Vector3(0, 0, -distanceAway) + target.position;
        //transform.position = smothRotation * new Vector3(0, 0, -distanceAway) + target.position;
        transform.LookAt(target.position + lookAtOffset);
        transform.position += Vector3.up * distanceUp;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < 180.0f) { 
            angle += 360F;
        }

        if (angle > 180.0f) { 
            angle -= 360F;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
