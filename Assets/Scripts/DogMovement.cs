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
    public int maxHP = 2;
    public int currentHP = 2;

    public float stunInterval = 5.0f;
    private float stunTimer = 0.0f;

    void Start()
    {
        dogBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(stunTimer > 0.0f){
            stunTimer -= Time.deltaTime;
        }
        if (Input.GetAxis("Sprint" + dogIndex) > 0.5f)
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
        if(stunTimer <= 0.0f){
            float horizontal = Input.GetAxis("Horizontal" + dogIndex);
            float vertical = -Input.GetAxis("Vertical" + dogIndex) * runningMultiplier;
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

    public void TakeDMG(int dmg) {
        currentHP -= dmg;

        if(currentHP <= 0) {
            stunTimer = stunInterval;
            currentHP = maxHP;
        }
    }
}
