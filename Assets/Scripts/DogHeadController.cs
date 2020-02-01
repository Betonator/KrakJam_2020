using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHeadController : MonoBehaviour
{
    Rigidbody dogHeadBody;
    public int dogIndex = 1;
    public float speed = 2f;

    void Start()
    {
        dogHeadBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("HorizontalHead" + dogIndex);
        float vertical = Input.GetAxis("VerticalHead" + dogIndex);
        Vector3 inputRotationVector = new Vector3(vertical, 0.0f, horizontal).normalized * speed;
        dogHeadBody.AddRelativeTorque(inputRotationVector);
    }
}
