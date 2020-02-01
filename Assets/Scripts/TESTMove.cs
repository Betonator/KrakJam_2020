using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TESTMove : MonoBehaviour
{
    Vector3 pos;

    //adidtional gravity
    public float aditionalGravity = 20f;

    public float jumpForce = 10;

    public bool isGrounded = true;

    public float treshHold = 0.05f;
   
    public float animTrans = 5.0f;
    public float speed = 5.0f;
    public float rotationSpeed = 75.0f;

    

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

        



        rb = GetComponent<Rigidbody>();
       
        pos = rb.position;
        

    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("COL");
        if (col.gameObject.tag == ("Ground") && isGrounded == false)
        {
            isGrounded = true;
            
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Extra Gravity
        rb.AddForce(Vector3.down * Time.deltaTime * aditionalGravity);

        var input = new Vector3(Input.GetAxisRaw("Horizontal1"), 0, Input.GetAxisRaw("Vertical1"));

        float jump = Input.GetAxisRaw("Jump");

        if (jump > 0.5 && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            GameObject bobr = GameObject.FindGameObjectWithTag("BOBR");

            bobr.GetComponent<BOBR_Move>().TakeDMG(70);
            
        }




        float vel = Vector3.Distance(pos, rb.position);
        //Debug.Log(vel);
        float translation = Input.GetAxis("Vertical1") * speed;
        float rotation = Input.GetAxis("Horizontal1") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;



        Quaternion targetRotation = new Quaternion(0, 0, 0, 0);

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);

        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // transform.Rotate(0, rotation, 0);


        transform.Translate(0, 0, input.magnitude * speed * Time.fixedDeltaTime);

        
        pos = rb.position;
    }
}