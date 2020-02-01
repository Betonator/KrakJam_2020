using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public int score = 0;
    private bool isPickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.tag == "BOBR") {
            Debug.Log("Bobr");
        }
    }

    public void SetIsPickedUp(bool isPicked) {
        isPickedUp = isPicked;
    }
}
