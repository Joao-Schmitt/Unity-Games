using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("ThirdPerson")]
    public float thirdPersonSens = 300.0f;
    public float thirdPersonYOffset = 1.0f;
    public float thirdPersonFollowSpeed = 10.0f;
    public float thirdPersonVerticalLimit = 45.0f;
    public float thirdPersonRotationSpeed = 10.0f;
    public Transform thirdPersonCameraPosition;

    [Header("FirstPerson")]
    public float firstPersonSens = 150.0f;
    public float firstPersonRotationSpeed = 10.0f;
    public float firstPersonVerticalLimit = 45.0f;
    public float firstPersonFollowSpeed = 10.0f;
    public float firstPersonYOffset = 1.0f;
    public Transform firstPersonCameraPosition;

    [Header("Geral")]
    public CameraType cameraType = CameraType.ThirdPerson;
    public float transitionSpeed = 15.0f;

    private Transform target;
    private float rotX, rotY;

    public enum CameraType 
    {
        ThirdPerson = 0,
        FirstPerson = 1
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        CameraRotate();

        if (cameraType == CameraType.FirstPerson)
            FirstPersonCameraTargetRotate();

        if (Input.GetKeyDown(KeyCode.V))
            cameraType = cameraType == CameraType.FirstPerson ? CameraType.ThirdPerson : CameraType.FirstPerson;

        ChangeCameraMode();
    }

    private void LateUpdate()
    {
        Follow();
    }
    void CameraRotate()
    {
        var sens = cameraType == CameraType.FirstPerson ? firstPersonSens : thirdPersonSens;
        var limit = cameraType == CameraType.FirstPerson ? firstPersonVerticalLimit : thirdPersonVerticalLimit;

        rotX -= Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -limit, limit);
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    void ChangeCameraMode()
    {
        var positionCam = cameraType == CameraType.ThirdPerson ? thirdPersonCameraPosition : firstPersonCameraPosition;

        if (Vector3.Distance(Camera.main.transform.position, positionCam.position) > 0)
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, positionCam.position, transitionSpeed * Time.deltaTime);
    }
    void Follow()
    {
        var speed = cameraType == CameraType.FirstPerson ? firstPersonFollowSpeed : thirdPersonFollowSpeed;
        var yOffset = cameraType == CameraType.FirstPerson ? firstPersonYOffset : thirdPersonYOffset;

        transform.position = Vector3.Lerp(transform.position, target.position + target.up * yOffset, speed * Time.deltaTime);
    }
    void FirstPersonCameraTargetRotate()
    {
        target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(0, rotY, 0), firstPersonRotationSpeed * Time.deltaTime);
    }
}
