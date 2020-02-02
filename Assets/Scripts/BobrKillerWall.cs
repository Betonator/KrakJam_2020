using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobrKillerWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BOBR"))
        {
            collision.collider.GetComponent<BOBR_Move>().TakeDMG(1000000f);
        }
    }
}
