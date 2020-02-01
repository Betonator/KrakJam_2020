using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningStars : MonoBehaviour
{
    public float parentRotationSpeed;
    public float childSelfRotationSpeed;
    private float randomRoty;

    private void Start()
    {
        randomRoty = Random.Range(0.0f, parentRotationSpeed);
    }

    void FixedUpdate()
    {
        transform.Rotate(0.0f, randomRoty, 0.0f);
    }
}
