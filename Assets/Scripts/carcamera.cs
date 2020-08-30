using System.Collections;
using UnityEngine;

public class carcamera : MonoBehaviour
{
    public Transform car;
    public float distanceFromCar;
    public float heightFromCar;
    public float rotationDamping;
    public float heightDamping;
    public float zoomRatio;
    public float defautFOV;

    public Transform cameraTarget;

    void LateUpdate()
    {
        //POSITION CAMERA
        float wantedAngle = car.eulerAngles.y;
        float wantedHeight = car.position.y + heightFromCar;
        float cameraAngle = transform.eulerAngles.y;
        float cameraHeight = transform.position.y;

        cameraAngle = Mathf.LerpAngle (cameraAngle, wantedAngle, rotationDamping * Time.deltaTime);
        cameraHeight = Mathf.Lerp(cameraHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, cameraAngle, 0);
        Vector3 cameraPos = car.position;
        cameraPos -= currentRotation * Vector3.forward * distanceFromCar;
        cameraPos.y = cameraHeight;
        transform.position = cameraPos;
        transform.LookAt (cameraTarget);

        //FOV
        float speed = car.GetComponent<Rigidbody> ().velocity.magnitude;
        GetComponent<Camera>().fieldOfView = defautFOV + (speed * zoomRatio * Time.deltaTime);






    }
}
