using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Muzzle : MonoBehaviour
{
    private bool isStickPickable = false;
    private Rigidbody pickableStick = null;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("PickUp")) {
            PickUpStick();
        }
    }

    private void PickUpStick() {
        if(isStickPickable) {
            Transform[] points = pickableStick.GetComponentsInChildren<Transform>();
            List<Transform> otherPoints = new List<Transform>();

            foreach(Transform point in points) {
                if(point.tag == "Grapple"){
                    otherPoints.Add(point);
                }
            }

            Transform closestPoint = FindNearestPoint(otherPoints.ToArray());
            pickableStick.transform.position += Vector3.up * 1.0f;
            pickableStick.transform.position -= closestPoint.localPosition;

            FixedJoint fix = pickableStick.GetComponentInChildren<FixedJoint>();
            fix.connectedBody = this.GetComponent<Rigidbody>();
            
            isStickPickable = false;
        }
    }

    private Transform FindNearestPoint(Transform[] points) 
    {
        float distance =  Vector3.Distance(this.GetComponent<Rigidbody>().position, points[0].position);
        Transform closestPoint = points[0];
        foreach(Transform point in points) {
            float thisDistance = Vector3.Distance(this.GetComponent<Rigidbody>().position, point.position);
            if(distance > thisDistance) 
            {
                distance = thisDistance;
                closestPoint = point;
            }
        }
        return closestPoint;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Stick") {
            isStickPickable = true;
            pickableStick = collider.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.tag == "Stick") {
            isStickPickable = false;
            pickableStick = null;
        }
    }
}
