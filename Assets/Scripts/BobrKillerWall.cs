using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobrKillerWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BOBR"))
        {
            other.GetComponent<BOBR_Move>().TakeDMG(1000000f);
        }
    }
}
