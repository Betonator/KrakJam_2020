using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Muzzle : MonoBehaviour
{
    private bool isStickPickable = false;
    private Rigidbody pickableStick = null;
    public Transform HeadEnd = null;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("PickUp")) {
            PickUpStick();
        }
        if(Input.GetButtonUp("PickUp")) {
            LoseStick();
        }
    }

    private void LoseStick() {
        if(pickableStick != null) {
            if(pickableStick.GetComponent<FixedJoint>().connectedBody == this.GetComponent<Rigidbody>()){
                FixedJoint fix = pickableStick.GetComponent<FixedJoint>();
                Destroy(fix);
                pickableStick = null;
            }
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
            pickableStick.transform.position = HeadEnd.position;
            pickableStick.transform.rotation = HeadEnd.rotation;

            pickableStick.transform.position = closestPoint.position;

            FixedJoint fix = pickableStick.gameObject.AddComponent<FixedJoint>() as FixedJoint;
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

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Stick" && pickableStick == null) {
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
