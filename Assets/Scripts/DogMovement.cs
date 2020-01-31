using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    Rigidbody dogBody;
    public int dogIndex = 0;
    public float dogSpeed = 10f;
    public float rotateSpeed = 5f;

    void Start()
    {
        dogBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal" + dogIndex);
        float vertical = Input.GetAxis("Vertical" + dogIndex);
        transform.Rotate(new Vector3(0.0f, horizontal, 0.0f) * rotateSpeed);
        dogBody.velocity = transform.forward*vertical*dogSpeed + new Vector3(0.0f, dogBody.velocity.y, 0.0f);
    }
}
