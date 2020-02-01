using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float rotationSpeed;
    private float randomRotx;
    private float randomRoty;
    private float randomRotz;

    private void Awake()
    {
        rotationSpeed = GetComponentInParent<SpinningStars>().childSelfRotationSpeed;
    }

    void Start()
    {
        randomRotx = Random.Range(0.0f, rotationSpeed);
        randomRoty = Random.Range(0.0f, rotationSpeed);
        randomRotz = Random.Range(0.0f, rotationSpeed);
    }

    void FixedUpdate()
    {
        transform.Rotate(randomRotx, randomRoty, randomRotz);
    }
}
