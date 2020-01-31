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
            Debug.Log(pickablePatyk);
            Transform[] points = pickablePatyk.GetComponentsInChildren<Transform>();

            Transform point = FindNearestPoint(points);
            Debug.Log( point.name+point.position);
            // pickablePatyk.transform.position += Vector3.up * 1.0f;
            pickablePatyk.transform.position -= point.localPosition;

            FixedJoint fix = pickablePatyk.GetComponentInChildren<FixedJoint>();
            fix.connectedBody = this.GetComponent<Rigidbody>();
        }
    }

    private Transform FindNearestPoint(Transform[] points) 
    {
        float distance;
        Transform closestPoint;
        foreach(Transform point in points) {
            if(point.tag == "Patyk") {
                continue;
            }
            float thisDistance = Vector3.Distance(this.GetComponent<Rigidbody>().position, point.position);
            Debug.Log(point.name + thisDistance);
            if(distance > thisDistance) 
            {
                distance = thisDistance;
                closestPoint = point;
            }
        }
        return closestPoint;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Patyk") {
            isPatykPickable = true;
            pickablePatyk = collision.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.tag == "Patyk") {
            isPatykPickable = false;
            pickablePatyk = null;
        }
    }
}
