using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHeadController : MonoBehaviour
{
    Rigidbody dogHeadBody;
    [SerializeField]
    private Transform defaultPosition;
    public int dogIndex = 1;
    public float stability = 0.3f;
    public float speed = 2f;

    void Start()
    {
        dogHeadBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("HorizontalHead" + dogIndex);
        float vertical = Input.GetAxis("VerticalHead" + dogIndex);
        Vector3 inputRotationVector = new Vector3(-vertical, horizontal, 0.0f).normalized * speed;
        //head stabilizer
        /*Vector3 predictedUp = Quaternion.AngleAxis(
        dogHeadBody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
        dogHeadBody.angularVelocity
        ) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);*/
        dogHeadBody.AddRelativeTorque(inputRotationVector);
        //dogHeadBody.AddTorque(torqueVector);
    }
}
