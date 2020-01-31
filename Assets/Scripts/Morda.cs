using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Morda : MonoBehaviour
{
    private bool isPatykPickable = false;
    private Rigidbody pickablePatyk = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("PickUp")) {
            PickUpPatyk();
        }
    }

    private void PickUpPatyk() {
        if(isPatykPickable) {
            Debug.Log("dupa");
            Debug.Log(pickablePatyk);
            pickablePatyk.transform.position += Vector3.up * 1.0f;
            FixedJoint[] fix = pickablePatyk.GetComponentsInChildren<FixedJoint>();
            fix[0].connectedBody = this.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Patyk") {
            Debug.Log(collision.collider.name);
            isPatykPickable = true;
            pickablePatyk = collision.rigidbody;
        }
    }
}
