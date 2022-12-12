using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//New 
public class CameraMovementSimulator : MonoBehaviour {

    //Player Movement
    //Each piece is 15 coords wide in x & y (square)

    GameObject finish;

    [SerializeField]
    float rotationSpeed=1000.0f;
    [SerializeField]
    public float translationSpeed=4.0f; //Dfault Moving Speed
    int framesToWaitForMouseLock=30;
    public GameObject grid;
    private gridScriptTwo gridScript;
    private Block currentBlock;

    private int playerx, playery;
    void Awake() {
        //hit ESC to leave the window
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start() {
        //finish = GameObject.Find("small_finish_track");

        //transform.position = finish.transform.position;

        if (grid == null) {grid = GameObject.Find("grid10x10");}
        gridScript = (gridScriptTwo) grid.GetComponent(typeof(gridScriptTwo));

    }
    void Update() {
        //this is needed to prevent the huge delta from when your mouse is outside of the window to when it gets locked in the center from making the camera spin around when the lock happens (which can happen within a variable timeframe, so this gives it a few frames to complete the lock)
        //the question mark is a ternary operator, in case you haven't seen it before (it doesn't usually get taught in CS curriculums for some reason). it's basically shorthand for an if statement.
        if (framesToWaitForMouseLock <= 0) {
            if (Input.GetKey(KeyCode.Q)) {
                transform.position = finish.transform.position;
                Debug.Log("Player Returned!");
            }

            //position needs to be updated based on orientation. Time.deltaTime is used to normalize for framerate
            transform.position +=             
            ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? transform.forward*translationSpeed * Time.deltaTime:(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))?transform.forward*-translationSpeed * Time.deltaTime:Vector3.zero) +             
            ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? transform.right*-translationSpeed * Time.deltaTime:(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))?transform.right*translationSpeed * Time.deltaTime:Vector3.zero);
            //yaw rotates around world axis in order to not affect roll
            transform.RotateAround(transform.position,Vector3.up,Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed);
            //pitch rotates around local axis
            transform.RotateAround(transform.position,transform.right,-Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed);
            //ensures that roll is never affected (e.g. sometimes Unity gets confused between 0 and 180...causing the camera to turn upside down)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0);
        } else {
            framesToWaitForMouseLock--;
        }

        //Where is the player?

        playerx = (int) abs(transform.position.x) / 15;
        playery = (int) abs(transform.position.z) / 15;

        //Need id of current block

        currentBlock = Block.getBlockByCoords(gridScript.list, playerx, playery);

        /*
        0 = normal
        1 = acceleration
        2 = deceleration
        */

        if (currentBlock != null) {
            switch (currentBlock.attribute) {
                case 1: //faseter
                translationSpeed = 6.0f;
                break;
                case 2: //slower
                translationSpeed = 5.0f;
                break;
                default:
                translationSpeed = 4.0f;
                break;
            }
        }
        else { //Player is over nothing or a lawn block: Slower speed here
            translationSpeed = 2.0f;
        }

        if (currentBlock != null) {
            Debug.Log(currentBlock.GetType());
        }
        else {
            Debug.Log("--Empty Block-- | 2");
        }
    }
    void printArr(string[] input) {
        foreach (string num in input) {
            Debug.Log(num + " ");
        }
    }

    double min(double a, double b) {
        return a < b ? a : b;
    }

    int min(int a, int b) {
        return a < b ? a : b;
    }

    double abs(double a) {
        return a < 0 ? -a : a;
    }

    int abs(int a) {
        return a < 0 ? -a : a;
    }
}