using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//This script will also handle al processing accrding to the peices
    //i.e., if (peice at 2x 5y == finishPeice) {terrainHere = rough} 

public class gridScriptTwo : MonoBehaviour {
    // Start is called before the first frame update

    public int numberofRows = 10, numberofCols = 10;
    private GameObject[] rows;
    private GameObject[] cols;
    private List<Block> list;
    private List<KeyValuePair<int, int>> occupiedSpaces;
    private bool checker = true;
    private StreamReader readerOne, readerTwo;
    private string pathOne, pathTwo, totalLineInput, result;
    private string[] nums;
    // Change colors for special blocks
    // Open CV
    // NumPy

    void Start() {
        pathOne = "Assets/marker_orientations.txt"; // 2 Orientations
        pathTwo = "Assets/marker_positions.txt"; // 3 Positions
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
            
            //Read form matrix test file
            
            if (nums.Length > 0) {
                list.Add(new Block(int.Parse(nums[0]), (int.Parse(nums[1])), int.Parse(nums[2]), int.Parse(nums[3])));
                //set variables to pasrsing results
            }
            totalLineInput = ""; //Clear buffer
        }

        //Debug.Log("min & Max: " + minLoc + " " + maxLoc);

        placeBlocksOne(list); //Adds to occupiedList
        clearBlocks(occupiedSpaces); //Resets blank values to "lawn"
        //tileCheck();
        readerOne.Close(); //Will open again at next loop
    }

    void OnApplicationQuit() {
        readerOne.Close();
    }

    void placeBlocksOne(List<Block> _list) {
        //Render and add to list of occipied spaces
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
        return (maxLoc - y) + minLoc;
    }

    int inverty(int y) {
        return ((int) maxLoc - y) + (int) minLoc;
    }

    int invertGridy(int y) {
        return (numberofRows - y);
    }

    void printArr(string[] input) {
        foreach (string num in input) {
            Debug.Log(num + " ");
        }
    }

    double min(double a, double b) {
        return a < b ? a : b;
    }

    double abs(double a) {
        return -a;
    }

    void demoOne() {
        //List<Block> list = new List<Block>();
        maxLoc = 1200;
        list.Add(new Block(817, 596, 6, 90));
        list.Add(new Block(902, 595, 19, 0)); // 902 595 19 0
        placeBlocksOne(list); //Adds to occupiedList
        clearBlocks(occupiedSpaces); //Resets blank values to "lawn"
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
