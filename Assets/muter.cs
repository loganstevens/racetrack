using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muter : MonoBehaviour {

AudioSource audioSource;
void Start() {
    audioSource = GetComponent<AudioSource>();
}
    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) || ((Input.GetKeyDown(KeyCode.U)) || (Input.GetKeyDown(KeyCode.F)))) { //INPUT - Mouse 
            Debug.Log("Mouse\a Clicked!");
            audioSource.mute = !audioSource.mute;
         }
    }
}
