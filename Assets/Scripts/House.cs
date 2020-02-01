using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int houseIndex = 1;
    public int houseLevel;
    public int maxLevel = 4;

    //some points BRUH
    private int points;
    public int pointsToUpgrade = 2;

    //POS & ROT offfsets
    public Vector3 posOffset = new Vector3(0f,0f,0f);
    public Vector3 rotOffset = new Vector3(0f,0f,0f);

    public GameObject[] Houses;

    private GameObject currentHouse;

    void Start()
    {
        houseLevel = 0;
        points = 0;
        
        currentHouse = Instantiate(Houses[0], transform.position + posOffset, Quaternion.Euler(rotOffset.x,rotOffset.y,rotOffset.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stick"))
        {
            AddPoints(other.gameObject);
            CheckUpgrade();
        }
    }

    public void AddPoints(GameObject Stick)
    {
        //Wez punkty z patyka
    }

    private void CheckUpgrade()
    {
        if(Mathf.Floor(points/pointsToUpgrade)>= houseLevel+1)
        {
            if (houseLevel < maxLevel)
            {
                UpgradeHIM();
            }
        }
    }

    private void UpgradeHIM()
    {
        Destroy(currentHouse);
        houseLevel++;
       // Debug.Log("HOUSE LVL " + houseLevel + "     MAX LVL TO: " + maxLevel);
        currentHouse = Instantiate(Houses[houseLevel], transform.position + posOffset, Quaternion.Euler(rotOffset.x, rotOffset.y, rotOffset.z));
        CanvasManager.canvasManager.CheckForGameFinish();
    }


    // ======================================= DEBUS SECTION ============================================================================ HEIL HITLER
    public void DebugAddPoints(int pkt)
    {
        points += pkt;
        CheckUpgrade();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            DebugAddPoints(1);
        }
    }
}
