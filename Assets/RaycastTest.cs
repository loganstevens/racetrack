using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{

    public string viewResultFull = "";
    public string interactRaycast() { // V1
        Vector3 playerPosition = transform.position;
        Vector3 forwardDirection = transform.forward;

        Ray interactionRay = new Ray(playerPosition, forwardDirection);
        RaycastHit interactionRayHit;
        float interactionRayLength = 5.0f;

        Vector3 interactionRayEndpoint = forwardDirection* interactionRayLength;
        //Debug.DrawLine(playerPosition, interactionRayEndpoint);

        bool hitFound = Physics.Raycast(interactionRay, out interactionRayHit, interactionRayLength);
        if (hitFound) {
            GameObject hitGameObject = interactionRayHit.transform.gameObject;
            string hitFeedback = hitGameObject.name;
            /*
            if (Physics.Raycast(interactionRay, out interactionRayHit, 50f)) {
                colleType = interactionRayHit.collider.GetComponent<collectible>().itemType;
            }
            */
            //Debug.Log(hitFeedback);
            return hitFeedback;
        }
        else {
            string nothingHitFeedback = "NothingHasBeenHit";
            //Debug.Log(nothingHitFeedback);
            return nothingHitFeedback;
        }
    }

    public string interactRaycastTwo() { //Goes under Camera Like other Function
        //if (Input.GetKeyDown(KeyCode.R)) {
            //print("R");
            Debug.DrawLine(this.gameObject.transform.position,this.gameObject.transform.position+(100.0f*this.gameObject.transform.forward),Color.red,100.0f);
            RaycastHit result;
            if (Physics.Raycast(this.gameObject.transform.position,this.gameObject.transform.forward,out result, Mathf.Infinity)){
                print("My ray hit: " + result.collider.gameObject.name);
                viewResultFull = result.collider.gameObject.name;
            }
            return result.collider.gameObject.name.Length > 1 ? result.collider.gameObject.name : "NothingHasBeenHit";
        //}
    }
    
    void Update()
    {
        viewResultFull = interactRaycast();
        if (viewResultFull != "NothingHasBeenHit") {
            Debug.Log("RayCastTwo: " + viewResultFull + '\n');
        }
    }
}