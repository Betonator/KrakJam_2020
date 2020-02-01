using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBOBR : MonoBehaviour
{
    public float timeToSpawn = 10;
    private float spawnTime;
    public GameObject BOBR;
    public Vector3 spawnLoc = new Vector3(0, 0, 0);
    public Vector3 spawnRot = new Vector3(0, 0, 0);
    // Start is called before the first frame update

    void Start()
    {
        spawnTime = Time.time + timeToSpawn;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= spawnTime)
            MakeBOBRLive();
    }

    private void MakeBOBRLive()
    {
        Instantiate(BOBR, transform.position + spawnLoc, Quaternion.Euler(spawnRot));
        spawnTime = Time.time + timeToSpawn;
    }
}
