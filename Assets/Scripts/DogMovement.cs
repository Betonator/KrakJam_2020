using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DogMovement : MonoBehaviour
{
    Rigidbody dogBody;
    [SerializeField]
    private GameObject stunStars;
    public int dogIndex = 0;
    public float dogSpeed = 10f;
    public float rotateSpeed = 5f;
    public float energy = 0f;
    private float maxEnergy = 100f;
    public int runningMultiplier = 1;
    public CinemachineVirtualCamera dogCamera;

    public float extraGrav;

    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public float counterMovement = 0.175f;
    public float actualMaxSpeed = 20;

    float horizontal;
    float vertical;

    public int maxHP = 2;
    public int currentHP = 2;

    public float stunInterval = 5.0f;
    public float stunTimer = 0.0f;


    private float threshold = 0.01f;

    private Animator anim;

    void Start()
    {
        dogBody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        horizontal = Input.GetAxis("Horizontal" + dogIndex);
        vertical = -Input.GetAxis("Vertical" + dogIndex) * runningMultiplier;
    }

    private void Update()
    {
        if (stunTimer <= 0.0f)
        {
            stunStars.SetActive(false);
            horizontal = Input.GetAxis("Horizontal" + dogIndex);
            vertical = -Input.GetAxis("Vertical" + dogIndex) * runningMultiplier;

            Debug.Log("HOR: " + horizontal + " VERT: " + vertical);

            if (vertical == 0.0f)
            {
                anim.SetBool("runing", false);
            }
            else
            {
                anim.SetBool("runing", true);
            }
            if (vertical >= 1.5f)
            {
                dogCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.05f;
            }
            else
            {
                dogCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            }
            transform.Rotate(new Vector3(0.0f, horizontal, 0.0f) * rotateSpeed);

        }


        if (stunTimer > 0.0f){
            stunTimer -= Time.deltaTime;
        }
        if (Input.GetAxis("Sprint" + dogIndex) > 0.0f && energy > 0.0f)
        {
            runningMultiplier = 2;


            energy -= Time.deltaTime*4;
            energy = Mathf.Clamp(energy,0.0f, maxEnergy);
            anim.SetBool("sprinting", true);

        }
        else
        {
            runningMultiplier = 1;
            energy += Time.deltaTime;
            energy = Mathf.Clamp(energy, 0.0f, maxEnergy);
            anim.SetBool("sprinting", false);
        }
        actualMaxSpeed = runningMultiplier * maxSpeed;


    }

    void FixedUpdate()
    {
        Movement();

        
    }

    public void TakeDMG(int dmg) {
        currentHP -= dmg;
        if(currentHP <= 0) {
            stunTimer = stunInterval;
            currentHP = maxHP;
            stunStars.SetActive(true);
            //DROP STICK HERE ALSO
        }
    }
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(dogBody.velocity.x, dogBody.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = dogBody.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void CounterMovement( Vector2 mag)
    {


        //Counter movement
        
            dogBody.AddForce(moveSpeed * transform.right * Time.deltaTime * -mag.x * counterMovement);
        
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(vertical) < 0.05f || (mag.y < -threshold && vertical > 0) || (mag.y > threshold && vertical < 0))
        {
            dogBody.AddForce(moveSpeed * transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(dogBody.velocity.x, 2) + Mathf.Pow(dogBody.velocity.z, 2))) > actualMaxSpeed)
        {
            float fallspeed = dogBody.velocity.y;
            Vector3 n = dogBody.velocity.normalized * actualMaxSpeed;
            dogBody.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

       private void Movement()
        {
        //Extra gravity
        dogBody.AddForce(Vector3.down * Time.deltaTime * extraGrav);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(mag);

        //If holding jump && ready to jump, then jump
        

        //Set max speed

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        

        //If speed is larger than actualMaxSpeed, cancel out the input so you don't go over max speed
        
        if (vertical > 0 && yMag > actualMaxSpeed) vertical = 0;
        if (vertical < 0 && yMag < -actualMaxSpeed) vertical = 0;

        

       
        

        //Apply forces to move player
        
        dogBody.AddForce(transform.forward * vertical * moveSpeed * Time.deltaTime);
        Debug.Log("FORECE:" + transform.forward * vertical * moveSpeed * Time.deltaTime);
    }

}
