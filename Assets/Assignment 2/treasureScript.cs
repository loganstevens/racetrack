using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//New

public class treasureScript : MonoBehaviour {
    public GameObject theCollectible;
    [SerializeField]
    public GameObject ScoreText;
    public GameObject WinningText;
    public RaycastTest Ray;

    public int TotalNumberOfUniqueObjects; //Set Externally

    public bool[] uObjects;

    public int[] myTeper = new int[3];
    public bool[] keyLock = {false, false, false};
    public bool[] hit = {false, false, false};
    public bool mainHit = false;
    private bool textOn = false;
    public string viewResult = "";
    public string viewResultFull = "";
    public char typeString;
    public bool printVals = false;
    //Start is called before the first frame update
    public const int maxScore = 300; //9*5 + 9*10 + 9*15, 9 of each object type

    public string interactRaycast() { // V1
        Vector3 playerPosition = transform.position;
        Vector3 forwardDirection = transform.forward;

        Ray interactionRay = new Ray(playerPosition, forwardDirection);
        RaycastHit interactionRayHit;
        float interactionRayLength = 100.0f;

        Vector3 interactionRayEndpoint = forwardDirection* interactionRayLength;
        //Debug.DrawLine(playerPosition, interactionRayEndpoint);

        bool hitFound = Physics.Raycast(interactionRay, out interactionRayHit, interactionRayLength);
        if (hitFound) {
            GameObject hitGameObject = interactionRayHit.transform.gameObject;
            string hitFeedback = hitGameObject.name;
            
            return hitFeedback;
        }
        else {
            string nothingHitFeedback = "NothingHasBeenHit";
            //Debug.Log(nothingHitFeedback);
            return nothingHitFeedback;
        }
    }

    //--------------------------------------------------------------------------------------------
    public int currentFrame = 0, score = 0, objects = 0, uniObjects = 0;
    [SerializeField]

    GameObject origin;
    [SerializeField]
    TextMesh scoreText, objectText, uObjectText;
    public TextMesh debugKey;
    void Start() {
        //if (scoreText == null) {scoreText = GameObject.FindObjectOfType<TextMesh>();}
        if (scoreText == null) {
            scoreText = origin.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        }
        scoreText.text = "Score: " + score;

        if (objectText == null) {
            objectText = origin.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMesh>();
        }
        objectText.text = "Objects: " + objects;

        if (uObjectText == null) {
            uObjectText = origin.gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<TextMesh>();
        }
        uObjectText.text = "U-Objects: " + uniObjects;

        if (WinningText.activeSelf) {WinningText.SetActive(false);}

        if (TotalNumberOfUniqueObjects == 0) {TotalNumberOfUniqueObjects = 3;}
        uObjects = new bool[TotalNumberOfUniqueObjects + 1];

        if (Ray == null) { Ray = (RaycastTest) this.gameObject.transform.GetChild(0).GetComponent(typeof(treasureScript));}
    }

    // Update is called once per frame
    void Update() {
        /* { //On click on UniqueObject
        //scoreText.text = "Unique Objects: " + ++uniObjects;
        } */

        foreach(KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode)){
                debugKey.text = ("KeyCode pressed: " + keyCode);

            }
            if (Input.GetKeyUp(keyCode)){
                debugKey.text = ("KeyCode released: " + keyCode);
            }
        }


        if (printVals) {
            scoreText.text = "Score: " + score;
            objectText.text = "Objects: " + objects;
            uObjectText.text = "U-Objects: " + uniObjects;
            printVals = false;
        }

        viewResultFull = Ray.interactRaycastTwo();

        if (viewResultFull.Length > 12) {
            theCollectible = GameObject.Find(viewResultFull);        
            typeString = viewResultFull[12]; 
            viewResult = viewResultFull.Substring(0,11);
        }

        if (viewResultFull != "NothingHasBeenHit") {
            Debug.Log("We see: " + viewResultFull + " / " + viewResult + " / " + typeString);
        }      
        //.find(viewresultfull).getComponent<collectible>().deconstruct();
        //----------------------------
         if (Input.GetMouseButtonDown(0) || ((Input.GetKeyDown(KeyCode.U)) || (Input.GetKeyDown(KeyCode.F)))) { //INPUT - Mouse 
             Debug.Log("Mouse\a Clicked!");
         }
         if ((viewResult == "collectible") && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.U)) || (Input.GetKeyDown(KeyCode.F))) { //INPUT - Mouse -> Found GameObject           
            Debug.Log("Mouse Clicked on " + viewResultFull + " / " + viewResult);
            //theCollectible.SetActive(false);
            score += theCollectible.GetComponent<collectible>().mainValue;
            Destroy(theCollectible);

            scoreText.text = "Score: " + score;
            objectText.text = "Objects: " + ++objects;

            Debug.Log("this Index: " + (((int)(typeString - '0')) - 1));

            if (!uObjects[((int)(typeString - '0')) - 1]) {
                uObjectText.text = "U-Objects: " + ++uniObjects;
                uObjects[((int)(typeString - '0')) - 1] = true;
            }                     
         }

         //if (score == maxScore) { //Max Score reached -- WIN!!
         if (uniObjects >= 3) {
            if (!WinningText.activeSelf) {WinningText.SetActive(true);}
         }
    }
}
