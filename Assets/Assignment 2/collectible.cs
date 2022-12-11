using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour {

    public GameObject OverSeer; //AROrigin
    public treasureScript tr_script;
    public int mainValue; //Declared Externally in prefab configuration.
    public bool hitPlayer = false;
    
    void OnCollisionEnter(Collision other){
        if (other.collider.gameObject.GetComponent<collectible>()) {
            print("I, "+this.gameObject.name+" hit another collectible called "+other.collider.gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other) { //Worked for two GOs
        if (other.gameObject.GetComponent<collectible>()) {
            print("I, "+this.gameObject.name+" overlapped another collectible called "+other.gameObject.name);
        }
        else if (other.gameObject.name == OverSeer.gameObject.name) {
            print("I, "+this.gameObject.name+" overlapped aPLAYER "+other.gameObject.name);
            //theCollectible.SetActive(false);
            tr_script.score += this.mainValue;
            ++tr_script.objects;

            Debug.Log("this Index: " + (((int)(this.gameObject.name[12] - '0')) - 1));

            if (!tr_script.uObjects[((int)((this.gameObject.name[12] - '0')) - 1)]) {
                ++tr_script.uniObjects;
                tr_script.uObjects[((int)((this.gameObject.name[12] - '0')) - 1)] = true;
            }
            tr_script.printVals = true;
            Destroy(this.gameObject);
        }
    }
    void Start() {
        tr_script = (treasureScript) OverSeer.GetComponent(typeof(treasureScript));
        if (OverSeer == null) {
            OverSeer = GameObject.Find("AROrigin");
        }
    }

    // Update is called once per frame
    /* void Update() {
    } */
}
