using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Muzzle : MonoBehaviour
{
    public int playerIndex = 1;
    private bool isStickPickable = false;
    private Rigidbody pickableStick = null;
    public Transform HeadEnd = null;

    private void Start()
    {
        playerIndex = GetComponent<DogHeadController>().dogIndex;
    }

    void Update()
    {
        if(Input.GetKeyDown("joystick " + playerIndex + " button 5")) {
            PickUpStick();
        }
        if(Input.GetKeyUp("joystick " + playerIndex + " button 5")) {
            LoseStick();
        }
    }

    private void LoseStick() {
        if(pickableStick != null) {
            if(pickableStick.GetComponent<FixedJoint>().connectedBody == this.GetComponent<Rigidbody>()){
                FixedJoint fix = pickableStick.GetComponent<FixedJoint>();
                Destroy(fix);
                pickableStick.GetComponent<Stick>().SetIsPickedUp(false);
                pickableStick = null;
            }
        }
    }

    private void PickUpStick() {
        if(isStickPickable && !pickableStick.GetComponent<Stick>().isPickedUp) {
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
            
            Vector3 closestPointVectorToMove = new Vector3(pickableStick.transform.position.x - closestPoint.position.x, pickableStick.transform.position.y - closestPoint.position.y, pickableStick.transform.position.z - closestPoint.position.z);
            
            pickableStick.transform.position += closestPointVectorToMove;
            FixedJoint fix = pickableStick.gameObject.AddComponent<FixedJoint>() as FixedJoint;
            fix.connectedBody = this.GetComponent<Rigidbody>();
            pickableStick.GetComponent<Stick>().SetIsPickedUp(true);
            isStickPickable = false;
        }
    }

    private Transform FindNearestPoint(Transform[] points) 
    {
        float distance = Mathf.Infinity; // Vector3.Distance(HeadEnd.position, points[0].position);
        Transform closestPoint = null;
        foreach(Transform point in points) {
            float thisDistance = Vector3.Distance(point.position, HeadEnd.position);
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
