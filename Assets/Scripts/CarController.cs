using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    public GameObject grid;
    private gridScriptTwo gridScript;
    private Block currentBlock, finish;
    private int playerx, playery, numOfFrames = 0;
    private bool checker = true;

    void Awake() {
        //hit ESC to leave the window
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start() {
        if (grid == null) {grid = GameObject.Find("grid10x10");}
        gridScript = (gridScriptTwo) grid.GetComponent(typeof(gridScriptTwo));
    }

    private void FixedUpdate() {

        // Camera Rig Movement Control
    
        ControllerCheck();

    //--------------------------------------------
        
        finish = Block.getBlockByID(gridScript.list, 14);
        if (numOfFrames < 30) {
            transform.position = new Vector3(-((finish.x * 15) + 8), 0.25f , (finish.y * 15) + 8);
            transform.eulerAngles = new Vector3(0.0f, finish.angle, 0.0f);
            ++numOfFrames;
        }

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

        if (Input.GetKey(KeyCode.Q) || Input.GetButton("XboxX") || Input.GetButton("XboxRB")) {
            transform.position = new Vector3(-((finish.x * 15) + 8), 0.25f , (finish.y * 15) + 8);
            transform.eulerAngles = new Vector3(0.0f, finish.angle, 0.0f);
            ApplyBreaking();
            Debug.Log("Player Returned!");
        }

        playerx = (int) abs(transform.position.x) / 15; //Blocks are 15x15
        playery = (int) abs(transform.position.z) / 15;

        currentBlock = Block.getBlockByCoords(gridScript.list, playerx, playery);

        /*
        0 = normal
        1 = acceleration
        2 = deceleration
        */

        if (currentBlock != null) {
            switch (currentBlock.attribute) {
                case 1: //faseter
                motorForce = 1400;
                break;
                case 2: //slower
                motorForce = 600;
                break;
                default:
                motorForce = 1000;
                break;
            }
        }
        else { //Player is over nothing or a lawn block: Slower speed here
            motorForce = 600;
        }

        if (currentBlock != null) {
            Debug.Log(currentBlock.GetType());
        }
        else {
            Debug.Log("--Empty Block-- | 2");
        }
    }


    private void GetInput() {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space) || Input.GetButton("XboxA");
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();       
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    double abs(double a) {
        return a < 0 ? -a : a;
    }

    int abs(int a) {
        return a < 0 ? -a : a;
    }
    void ControllerCheck() {
        // float ltaxis = Input.GetAxis("XboxLeftTrigger");
        // float rtaxis = Input.GetAxis("XboxRightTrigger");
        // float dhaxis = Input.GetAxis("XboxDpadHorizontal");
        // float dvaxis = Input.GetAxis("XboxDpadVertical");
    
        bool xbox_a = Input.GetButton("XboxA");
        //bool xbox_b = Input.GetButton("XboxB");
        //bool xbox_x = Input.GetButton("XboxY");
        bool xbox_y = Input.GetButton("XboxX");
        // bool xbox_lb = Input.GetButton("XboxLB");
        // bool xbox_rb = Input.GetButton("XboxRB");
        // bool xbox_ls = Input.GetButton("XboxLS");
        // bool xbox_rs = Input.GetButton("XboxRS");
        // bool xbox_view = Input.GetButton("XboxView");
        // bool xbox_menu = Input.GetButton("XboxMenu");        
    
        string DebugText = (""+
            // "Horizontal: {14:0.000} Vertical: {15:0.000}\n" +
            // "HorizontalTurn: {16:0.000} VerticalTurn: {17:0.000}\n" +
            // "LTrigger: {0:0.000} RTrigger: {1:0.000}\n" +
            // "A: {2} B: {3} X: {4} Y:{5}\n" +
            // "LB: {6} RB: {7} LS: {8} RS:{9}\n" +
            // "View: {10} Menu: {11}\n" +
            // "Dpad-H: {12:0.000} Dpad-V: {13:0.000}\n" +
            //ltaxis + rtaxis +
            xbox_a + /* xbox_b + xbox_x +  */ xbox_y
            // +
            // xbox_lb+ xbox_rb+ xbox_ls+ xbox_rs+
            // xbox_view+ xbox_menu
            //dhaxis+dvaxis
            );
        
        Debug.Log(DebugText);
    }
}