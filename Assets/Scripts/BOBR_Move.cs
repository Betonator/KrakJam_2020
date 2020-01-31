using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOBR_Move : MonoBehaviour
{

    //Assingables
    public Transform POI;

    //adidtional gravity
    public float aditionalGravity = 20f;
    

    //Other
    private Rigidbody rb;


    //finding POI range
    public float desiredDistToPOI = 100f;
    public float nearPOI = 2f;

    //check if BOBR found his enemy
    int foundPOI;



    //Rotation and look
   
    public float maxRotarionSpeed = 800f;


    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 15;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    // Start is called before the first frame update
    void Start()
    {
        foundPOI = 0;
        
    }

    

    // Update is called once per frame
    void Update()
    {
        CheckDistanceToPOI();
        Look();
    }
    void FixedUpdate()
    {
        Movement();
    }


    private void CheckDistanceToPOI()
    {
        if(Vector3.Distance(transform.position,POI.position)<desiredDistToPOI && Vector3.Distance(transform.position, POI.position)>nearPOI)
        {
            foundPOI = 1;  
        }
        else
        {
            foundPOI = 0;
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }


    private void Look()
    {
        Vector3 posLook = POI.position - transform.position;
        posLook.y = 0;
       
        Quaternion targetRotation = Quaternion.LookRotation(posLook);
        transform.localRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotarionSpeed);
    }

    private void CounterMovement(Vector2 mag)
    {
       
        
        //Counter movement
       
            rb.AddForce(moveSpeed * transform.right * Time.deltaTime * -mag.x * counterMovement);
        
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(foundPOI) < 0.05f || (mag.y < -threshold && foundPOI > 0) || (mag.y > threshold && foundPOI == 0))
        {
            rb.AddForce(moveSpeed * transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    private void Movement()
    {
        //Extra Gravity
        rb.AddForce(Vector3.down * Time.deltaTime * aditionalGravity);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;
        CounterMovement(mag);



        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (foundPOI > 0 && yMag > maxSpeed) foundPOI = 0;
        if (foundPOI < 0 && yMag < -maxSpeed) foundPOI = 0;




        //apply force
        rb.AddForce(transform.forward * foundPOI * moveSpeed * Time.deltaTime);
       

    }
}
