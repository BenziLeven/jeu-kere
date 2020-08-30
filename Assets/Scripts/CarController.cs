using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{

    public Text TxtSpeed;
    public WheelCollider front_left;
    public WheelCollider front_right;
    public WheelCollider back_left;
    public WheelCollider back_right;
    public Transform FL;
    public Transform FR;
    public Transform BL;
    public Transform BR;

    public float Torque;
    public float Speed;
    public float MaxSpeed = 200f;
    public int Brake = 10000;
    public float CoefAcceleration = 10f;
    public float WheelAngleMax = 10f; 
    public float DAmax = 40f;
    public bool Freinage = false;
    
   
    public GameObject BackLight;

    void Start()
    {
        //reglage du CenterOfMass
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0.5f, 0.15f);
    }

    void Update()
    {

        //affichage de la vitesse
        Speed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        TxtSpeed.text = "Speed : " + (int)Speed;

        //Direction du vehicule
        float DA = (((WheelAngleMax - DAmax) / MaxSpeed) * Speed) + DAmax;  
       

        front_left.steerAngle = Input.GetAxis("Horizontal") * DA;
        front_right.steerAngle = Input.GetAxis("Horizontal") * DA;
        
        //Acceleration
        if(Input.GetKey(KeyCode.UpArrow) && Speed < MaxSpeed)
        {
            if(!Freinage)
            {
                front_left.brakeTorque = 0;
                front_right.brakeTorque = 0;
                back_left.brakeTorque = 0;
                back_right.brakeTorque = 0;
                back_left.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime;
                back_right.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime;
            }
        }

        //Deceleration
        if(!Input.GetKey(KeyCode.UpArrow) && !Freinage || Speed > MaxSpeed )
        {
            if(GetComponent<Rigidbody>().velocity.y>0)
            {
                back_left.motorTorque = -250;
                back_right.motorTorque = -250;
            }
            else
            {
                back_left.brakeTorque = 1;
                back_right.brakeTorque = 1;
            }
                       
        }

        //Freinage
        if (Input.GetKey(KeyCode.Space))
        {
            Freinage = true;
            BackLight.SetActive(true);
            back_left.brakeTorque = Mathf.Infinity;
            back_right.brakeTorque = Mathf.Infinity;
            front_left.brakeTorque = Mathf.Infinity;
            front_right.brakeTorque = Mathf.Infinity;
            back_left.motorTorque = 0;
            back_right.motorTorque = 0;
        }
        else
        {
            Freinage = false;
            BackLight.SetActive(false);
         }

        //Marche arriere
        if (Input.GetKey(KeyCode.DownArrow))
            {
                front_left.brakeTorque = 0;
                front_right.brakeTorque = 0;
                back_left.brakeTorque = 0;
                back_right.brakeTorque = 0;
                back_left.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime;
                back_right.motorTorque = Input.GetAxis("Vertical") * Torque * CoefAcceleration * Time.deltaTime;
            }

            // Rotation des roues
            FL.Rotate(front_left.rpm / 60 * 360 * Time.deltaTime, 0, 0);
            FR.Rotate(front_right.rpm / 60 * 360 * Time.deltaTime, 0, 0);
            BL.Rotate(back_left.rpm / 60 * 360 * -Time.deltaTime, 0, 0);
            BR.Rotate(back_right.rpm / 60 * 360 * Time.deltaTime, 0, 0);

            //SteerAngle Mesh roues
            FL.localEulerAngles = new Vector3(FL.localEulerAngles.x, front_left.steerAngle - FL.localEulerAngles.z, FL.localEulerAngles.z);
            FR.localEulerAngles = new Vector3(FR.localEulerAngles.x, front_right.steerAngle - FR.localEulerAngles.z, FR.localEulerAngles.z);
            
        


    }
}
