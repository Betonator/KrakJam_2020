using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DogMovement : MonoBehaviour
{
    Rigidbody dogBody;
    [SerializeField]
    private GameObject stunStars;
    public int dogIndex = 0;
    public float dogSpeed = 10f;
    public float rotateSpeed = 5f;
    public float energy = 0f;
    private float maxEnergy = 100f;
    public int runningMultiplier = 1;
    public CinemachineVirtualCamera dogCamera;
    public int maxHP = 2;
    public int currentHP = 2;

    public float stunInterval = 5.0f;
    private float stunTimer = 0.0f;
    public bool isResting = false;
    private Animator anim;

    void Start()
    {
        dogBody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        energy = maxEnergy/2;
    }

    private void Update()
    {
        if(stunTimer > 0.0f){
            stunTimer -= Time.deltaTime;
        }
        if (Input.GetAxis("Sprint" + dogIndex) > 0.0f && energy > 0.0f && !isResting)
        {
            runningMultiplier = 2;
            energy -= Time.deltaTime*4;
            energy = Mathf.Clamp(energy,0.0f, maxEnergy);
            if(energy == 0.0f)
            {
                isResting = true;
            }
            anim.SetBool("sprinting", true);

        } else {
            runningMultiplier = 1;
            energy += Time.deltaTime;
            energy = Mathf.Clamp(energy, 0.0f, maxEnergy);
            if(energy > 20.0f)
            {
                isResting = false;
            }
            anim.SetBool("sprinting", false);
        }
    }

    void FixedUpdate()
    {
        if(stunTimer <= 0.0f){
            stunStars.SetActive(false);
            float horizontal = Input.GetAxis("Horizontal" + dogIndex);
            float vertical = -Input.GetAxis("Vertical" + dogIndex) * runningMultiplier;

            if(vertical == 0.0f) {
                anim.SetBool("runing", false);
            }
            else{
                anim.SetBool("runing", true);
            }
            if (vertical >= 1.5f)
            {
                dogCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.05f;
            }
            else
            {
                dogCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            }
            transform.Rotate(new Vector3(0.0f, horizontal, 0.0f) * rotateSpeed);
                
            dogBody.velocity = transform.forward * dogSpeed * vertical + new Vector3(0.0f, -5.0f, 0.0f);
        }
    }

    public void TakeDMG(int dmg) {
        currentHP -= dmg;
        if(currentHP <= 0) {
            stunTimer = stunInterval;
            currentHP = maxHP;
            stunStars.SetActive(true);
            //DROP STICK HERE ALSO
        }
    }
}
