using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallMove : MonoBehaviour
{

    float startTime, setTime = 0;
    public float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        setTime += Time.deltaTime;
        if (setTime - startTime < 5) {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            //Debug.Log("10: " + (Time.deltaTime - startTime));
            //Debug.Log("10-1: " + (Time.deltaTime));
        }
        else {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            Debug.Log("20");
        }

        if (setTime > 10) {
            //startTime = Time.time;
            setTime = 0;
        }
        /* while (startTime < 4.0f) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            startTime += Time.deltaTime;
        }
        startTime = Time.time;
        while (startTime < 4.0f) {
            transform.position = new Vector3(0,0,-2) * Time.deltaTime;
            startTime += Time.deltaTime;
        } */
    }
}
