using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class gridScriptTwo : MonoBehaviour {

    public int numberofRows = 10, numberofCols = 10;
    private GameObject[] rows;
    private GameObject[] cols;
    public List<Block> list; //For player location: public
    private List<KeyValuePair<int, int>> occupiedSpaces;
    private int count = 0;
    private bool canSet = false, checker = true;
    private StreamReader readerOne, readerTwo;
    private string pathOne, pathTwo, totalLineInput, totalLineInputTwo, result;
    private string[] nums, numsTwo;
    private float offset = 0.0f;
    // Change colors for special blocks
    // Open CV
    // NumPy

    void Start() {
        pathTwo = "Assets/marker_orientations.txt"; // 2 Orientations
        pathOne = "Assets/marker_positions.txt"; // 3 Positions
        totalLineInput = "";
        
        rows = new GameObject[numberofRows]; //Arrays must be initialized here
        list = new List<Block>();
        occupiedSpaces = new List<KeyValuePair<int, int>>();

        for (int i = 0 ; i < rows.Length ; ++i) {
            if (rows[i] == null) {rows[i] = this.gameObject.transform.GetChild(i).gameObject;}
        }
        clearBlocks();
        // Run proc for finding maxLoc
    }

    // Update is called once per frame
    void FixedUpdate() {
        count = 0;
        checker = true;
        occupiedSpaces.Clear();
        list.Clear();

        readerOne = new StreamReader(pathOne); //
        readerTwo = new StreamReader(pathTwo); //Find .txt Document

        while (checker) { //while ((totalLineInput = readerOne.ReadLine()) != null) {
            totalLineInput = readerOne.ReadLine(); //Line as string | Parse .txt Document
            if (totalLineInput == null) {
                checker = false;
                break;
            }
            nums = totalLineInput.Split(' ');

            totalLineInputTwo = readerTwo.ReadLine(); //Line as string | Parse .txt Document
            if (totalLineInputTwo == null) {
                checker = false;
                break;
            }
            numsTwo = totalLineInputTwo.Split(' ');
            
            //Read form matrix test file
            
            if (nums.Length > 0) { //Parse nums, add blocks | Block(x y id angle) i = x, inverty(count) = y
                for (int i = 0 ; i < numberofCols ; ++i) {
                    if (int.Parse(nums[i]) != 0 && !Block.containsID(list, int.Parse(nums[i]))) {
                        list.Add(new Block(i, (count), int.Parse(nums[i]), int.Parse(numsTwo[i])));
                        //Debug.Log(new Block(i, (count), int.Parse(nums[i]), int.Parse(numsTwo[i])).ToString());
                    }
                }
            }
            totalLineInput = ""; //Clear buffer
            totalLineInputTwo = ""; //Clear buffer
            count = (count >= numberofRows) ? 0 : count+1;
        }

        //Debug.Log("min & Max: " + minLoc + " " + maxLoc);

        placeBlocksOne(list); //Adds to occupiedList
        clearBlocks(occupiedSpaces); //Resets blank values to "lawn"
        readerOne.Close(); //Will open again at next loop
        readerTwo.Close();
    }

    void OnApplicationQuit() {
        readerOne.Close();
        readerTwo.Close();
    }

    void placeBlocksOne(List<Block> _list) {
        canSet = false;

        /*
        1: Finish
        2: Straight
        3: Small Curve
        4: Big Curve
        */

        //straight & finish = -0.1
        //big Curve: -0.044
        //Small Curve: -0.058

        foreach(Block block in _list) {
            if (block.type == 4) { //Move to nearest upper-left quadrant, regardless of orientation
                canSet = (block.x + 1 < numberofCols && block.y + 1 < numberofRows) ? true : false;
            }
            for (int i = 0 ; i < 5 ; ++i) { //5 types of Block
                if (block.type != 4) {
                    if (i != block.type) {
                        // Deactivate any other overlapping block type
                        if (rows[block.y].transform.GetChild(block.x).transform.GetChild(i).gameObject.activeSelf) {
                            rows[block.y].transform.GetChild(block.x).transform.GetChild(i).gameObject.SetActive(false);
                            occupiedSpaces.Remove(new KeyValuePair<int, int>(block.x, block.y));
                        }                            
                    }
                    else {
                        // Activate blocktype
                        if (!rows[block.y].transform.GetChild(block.x).transform.GetChild(i).gameObject.activeSelf) {rows[block.y].transform.GetChild(block.x).transform.GetChild(i).gameObject.SetActive(true);}
                        // Block Rotation:
                        switch (block.type) {
                            case 1:
                            case 2:
                                offset = -0.1f;
                                break;
                            case 3:
                                offset = -0.058f;
                                break;
                            // case 4:
                            //     offset = -0.044f;
                            //     break;
                            default:
                                offset = 0.0f;
                                break;
                        }
                        rows[block.y].transform.GetChild(block.x).transform.GetChild(i).transform.eulerAngles = new Vector3(0.0f, (float) block.angle, 0.0f);
                        rows[block.y].transform.GetChild(block.x).transform.GetChild(i).transform.position = new Vector3(rows[block.y].transform.GetChild(block.x).transform.GetChild(i).transform.position.x, (offset), rows[block.y].transform.GetChild(block.x).transform.GetChild(i).transform.position.z);
                        occupiedSpaces.Add(new KeyValuePair<int, int>(block.x, block.y));
                    }
                }
                else if (block.type == 4 && canSet) { //must clear other adjacent values
                    if (i != block.type) {
                        // Deactivate any other overlapping block type for all areas
                        if (rows[block.y].transform.GetChild(block.x).transform.GetChild(i).gameObject.activeSelf)     {
                            rows[block.y].transform.GetChild(block.x).transform.GetChild(i).gameObject.SetActive(false);
                            occupiedSpaces.Remove(new KeyValuePair<int, int>(block.x, block.y));
                        }
                        if (rows[block.y+1].transform.GetChild(block.x).transform.GetChild(i).gameObject.activeSelf)   {
                            rows[block.y+1].transform.GetChild(block.x).transform.GetChild(i).gameObject.SetActive(false);
                            occupiedSpaces.Remove(new KeyValuePair<int, int>(block.x, block.y+1));
                        }
                        if (rows[block.y].transform.GetChild(block.x+1).transform.GetChild(i).gameObject.activeSelf)   {
                            rows[block.y].transform.GetChild(block.x+1).transform.GetChild(i).gameObject.SetActive(false);
                            occupiedSpaces.Remove(new KeyValuePair<int, int>(block.x+1, block.y));
                        }
                        if (rows[block.y+1].transform.GetChild(block.x+1).transform.GetChild(i).gameObject.activeSelf) {
                            rows[block.y+1].transform.GetChild(block.x+1).transform.GetChild(i).gameObject.SetActive(false);
                            occupiedSpaces.Remove(new KeyValuePair<int, int>(block.x+1, block.y+1));
                        }

                    }
                    else {
                        // Activate blocktype
                        if (!rows[block.y].transform.GetChild(block.x).transform.GetChild(i).gameObject.activeSelf) {rows[block.y].transform.GetChild(block.x).transform.GetChild(i).gameObject.SetActive(true);}
                        // Block Rotation:
                        rows[block.y].transform.GetChild(block.x).transform.GetChild(i).transform.eulerAngles = new Vector3(0.0f, (float) block.angle, 0.0f); //offset for type 4
                        rows[block.y].transform.GetChild(block.x).transform.GetChild(i).transform.position = new Vector3(rows[block.y].transform.GetChild(block.x).transform.GetChild(i).transform.position.x, (-0.044f), rows[block.y].transform.GetChild(block.x).transform.GetChild(i).transform.position.z); //offset for type 4
                        // Add occupied for all four grid spaces
                        occupiedSpaces.Add(new KeyValuePair<int, int>(block.x, block.y));
                        occupiedSpaces.Add(new KeyValuePair<int, int>(block.x+1, block.y));
                        occupiedSpaces.Add(new KeyValuePair<int, int>(block.x, block.y+1));
                        occupiedSpaces.Add(new KeyValuePair<int, int>(block.x+1, block.y+1));
                    }
                }
            }
        }
    }

    void clearBlocks() {
        for (int i = 0 ; i < numberofCols ; ++i) {
            for (int j = 0 ; j < numberofRows ; ++j) {
                    //row                   //col          //piece                                 | row                   col          piece
                    if (!rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);}
                    if (rows[j].transform.GetChild(i).transform.GetChild(1).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);}
                    if (rows[j].transform.GetChild(i).transform.GetChild(2).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);}
                    if (rows[j].transform.GetChild(i).transform.GetChild(3).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(false);}
                    if (rows[j].transform.GetChild(i).transform.GetChild(4).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(false);}
                    
            }
        }
    }

    void clearBlocks(List<KeyValuePair<int, int>> _occupiedSpaces) {
        for (int i = 0 ; i < numberofCols ; ++i) {
            for (int j = 0 ; j < numberofRows ; ++j) {
                if (!occupiedSpaces.Contains(new KeyValuePair<int, int>(i, j))) {
                    if (!rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);}
                    if (rows[j].transform.GetChild(i).transform.GetChild(1).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);}
                    if (rows[j].transform.GetChild(i).transform.GetChild(2).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);}
                    if (rows[j].transform.GetChild(i).transform.GetChild(3).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(false);}
                    if (rows[j].transform.GetChild(i).transform.GetChild(4).gameObject.activeSelf) {rows[j].transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(false);}
                }         
            }
        }
    }

    void tileCheck() {
        int count = 0;
        for (int i = 0 ; i < numberofCols ; ++i) {
            for (int j = 0 ; j < numberofRows ; ++j) {
                for (int z = 0 ; z < 5 ; ++z) {
                    count += (rows[j].transform.GetChild(i).transform.GetChild(z).gameObject.activeSelf) ? 1 : 0;
                }
                if (count == 2 && rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.activeSelf) {
                    rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                }
                else if (count == 0) { // && !occupiedSpaces.Contains(new KeyValuePair<int, int>(i, j))
                    rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (count > 1) {
                    if (rows[j].transform.GetChild(i).transform.GetChild(4).gameObject.activeSelf) {
                        rows[j].transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                        rows[j].transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                        rows[j].transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(false);
                        rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else {
                        rows[j].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                count = 0;
            }
        }
    }

    double inverty(double y) {
        return (numberofRows - y);
    }

    int inverty(int y) {
        return ((int) numberofRows - y);
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

    /*

    void testDataStructureStrings() {
        List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

        KeyValuePair<string, string> test = new KeyValuePair<string, string>("a","1");

        list.Add(test);
        list.Add(new KeyValuePair<string, string>("b","2"));

        foreach (KeyValuePair<string, string> pair in list){
            print("Key: " + pair.Key + " | Value " + pair.Value);
        }
    }

    void testDataStructureInts() {
        List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();

        KeyValuePair<int, int> test = new KeyValuePair<int, int>(1, 2);

        list.Add(test);
        list.Add(new KeyValuePair<int, int>(3, 4));

        foreach (KeyValuePair<int, int> pair in list){
            print("Key: " + pair.Key + " | Value " + pair.Value);
        }
    }
    
    */
}
