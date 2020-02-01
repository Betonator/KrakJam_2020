using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public int score = 0;
    public bool isPickedUp = false;
    public float stickDMG = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsPickedUp(bool isPicked) {
        isPickedUp = isPicked;
    }
}
