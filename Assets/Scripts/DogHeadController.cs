using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHeadController : MonoBehaviour
{
    Rigidbody dogHeadBody;
    [SerializeField]
    private Transform defaultPosition;
    public int dogIndex = 1;
    public float headRotationPower = 5f;

    void Start()
    {
        dogHeadBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("HorizontalHead" + dogIndex);
        float vertical = Input.GetAxis("VerticalHead" + dogIndex);
        //head stabilizer
        float rotX = defaultPosition.eulerAngles.x - transform.eulerAngles.x;
        float rotY = defaultPosition.eulerAngles.y - transform.eulerAngles.y;
        float rotZ = defaultPosition.eulerAngles.z - transform.eulerAngles.z;
        dogHeadBody.AddRelativeTorque(new Vector3(rotX, rotY, rotZ)*headRotationPower/2 +
            new Vector3(-vertical, horizontal, 0.0f).normalized * headRotationPower);
    }
}
