using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DogMovement : MonoBehaviour
{
    Rigidbody dogBody;
    public int dogIndex = 0;
    public float dogSpeed = 10f;
    public float rotateSpeed = 5f;
    private float energy = 0f;
    private float maxEnergy = 100f;
    private int runningMultiplier = 1;
    public CinemachineVirtualCamera dogCamera;

    void Start()
    {
        dogBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetAxis("Sprint" + dogIndex) > 0f)
        {
            runningMultiplier = 2;
            energy -= Time.deltaTime;
            energy = Mathf.Clamp(energy,0.0f, maxEnergy);
        }
        else
        {
            runningMultiplier = 1;
            energy += Time.deltaTime;
            energy = Mathf.Clamp(energy, 0.0f, maxEnergy);
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal" + dogIndex);
        float vertical = Input.GetAxis("Vertical" + dogIndex) * runningMultiplier;
        if (vertical >= 1.5f)
        {
            dogCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.05f;
        }
        else
        {
            dogCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        }
        transform.Rotate(new Vector3(0.0f, horizontal, 0.0f) * rotateSpeed);
        dogBody.velocity = transform.forward*vertical*dogSpeed + new Vector3(0.0f, dogBody.velocity.y, 0.0f);
    }
}
