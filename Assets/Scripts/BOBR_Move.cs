using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOBR_Move : MonoBehaviour
{
    [SerializeField]
    private GameObject stunStars;
    [SerializeField]
    private Transform spawnPoint;

    bool alive;

    public AudioClip[] audio;

    bool isGrounded;

    public float stunInterval = 5.0f;
    private float stunTimer = 0.0f;
    public float attackInterval = 4.0f;
    private float attackTimer = 0.0f;

    //HP
    public float HP = 100f;

    //Assingables
    public Transform POI;

    //adidtional gravity
    public float aditionalGravity = 20f;
    bool isAttacking;
    

    //Other
    private Rigidbody rb;

    //Attacking
    public float jumpForce = 550f;
    public float attackForce = 330f;
    public float attackDelay = 5f;
    public float timeToAttack;

    //finding POI range
    public float desiredDistToPOI = 100f;
    public float nearPOI = 2f;
    AudioSource ASource;

    //check if BOBR found his enemy
    int foundPOI;

    //Rotation and look
   
    public float maxRotarionSpeed = 800f;


    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 15;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;


    public int beaverDMG = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        ASource = GetComponent<AudioSource>();

    }

    public void PlayAWOLOLO()
    {
        int clipAudio =(int) Mathf.Floor(Random.Range(0,audio.Length));
        ASource.PlayOneShot(audio[clipAudio]);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if(other.gameObject.tag == "Stick")
        {
            Stick patyk = other.gameObject.GetComponent<Stick>();
            if(patyk.isPickedUp) {
                TakeDMG(patyk.stickDMG);
            }
        }
        if(other.gameObject.tag == "Player")
        {
            if (attackTimer <= 0.0f)
            {
                DogMovement doggo = other.gameObject.GetComponent<DogMovement>();
                doggo.TakeDMG(beaverDMG);
                attackTimer = attackInterval;
            }
        }

    }


    private void ShearchForNearbyDOGGO()
    {
        
        GameObject[] doggos = GameObject.FindGameObjectsWithTag("Player");

        float distanceToDoggo = Mathf.Infinity;

        foreach (GameObject dog in doggos)
        { 
            float actDist = Vector3.Distance(transform.position, dog.transform.position);
            if (actDist < distanceToDoggo)
            { 
                distanceToDoggo = actDist;
                POI = dog.transform; 
            }
        }


    }

        // Start is called before the first frame update
        void Start()
    {
        foundPOI = 0;
        alive = true;
        isAttacking = false;
        timeToAttack = Time.time;
        ShearchForNearbyDOGGO();

        attackTimer = attackInterval;
    }

    public void TakeDMG(float dmg)
    {
        HP -= dmg;

        if(HP <= 0)
        {
            POI = spawnPoint;
            alive = false;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            ShearchForNearbyDOGGO();
        }
        Look();
        if(stunTimer > 0.0f){
            stunStars.SetActive(true);
            stunTimer -= Time.deltaTime;
        }
        if (attackTimer > 0.0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        CheckDistanceToPOI();
        Movement();
    }




    private void Attack()
    {
        isGrounded = false;
        rb.AddForce(transform.forward * attackForce * 1.5f, ForceMode.Impulse);
        rb.AddForce(transform.up * jumpForce * 1.5f,ForceMode.Impulse);
        PlayAWOLOLO();
        stunTimer = stunInterval;
    }



    //Checking distance to POI
    private void CheckDistanceToPOI()
    {
        if (alive)
        {


            if (Vector3.Distance(transform.position, POI.position) < desiredDistToPOI )
            {
                foundPOI = 1;
                if (Vector3.Distance(transform.position, POI.position) < nearPOI)
                {
                    if (Time.time > timeToAttack)
                    {
                        Attack();
                        timeToAttack = Time.time + attackDelay;
                        isAttacking = true;
                    }
                }
               
            }
            else
            {
                foundPOI = 0;
            }
        }else
        {
            foundPOI = 1;
            if (Vector3.Distance(transform.position, POI.position) < nearPOI)
            {
                Destroy(transform.parent.gameObject);
            }
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

        //Limit diagonal running. 
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

        //Find actual velocity relative to where BOBR is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        if (isGrounded)
        {
            CounterMovement(mag);



            //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
            if (foundPOI > 0 && yMag > maxSpeed) foundPOI = 0;
            if (foundPOI < 0 && yMag < -maxSpeed) foundPOI = 0;

        }


        if (stunTimer <= 0.0f)
        {
            stunStars.SetActive(false);
            //apply force
            rb.AddForce(transform.forward * foundPOI * moveSpeed * Time.deltaTime);
        }

    }
}
