using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//This script will also handle al processing accrding to the peices
    //i.e., if (peice at 2x 5y == finishPeice) {terrainHere = rough} 

public class gridScriptOne : MonoBehaviour {
    // Start is called before the first frame update

    public int numberofRows = 10, numberofCols = 10;
    private GameObject[] rows;
    private GameObject[] cols;
    private List<Block> list;
    private List<KeyValuePair<int, int>> occupiedSpaces;
    private double maxLoc, minLoc; // coords: (0 - maxLoc, 0 - maxLoc)
    private bool canSet = false, checker = true;
    private StreamReader reader;
    private string path, totalLineInput, result;
    private string[] nums;
    // Change colors for special blocks
    // Open CV
    // NumPy

    void Start() {
        path = "Assets/outText.txt";
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

        reader = new StreamReader(path); //Find .txt Document

        while (checker) { //while ((totalLineInput = reader.ReadLine()) != null) {
            totalLineInput = reader.ReadLine(); //Line as string | Parse .txt Document
            if (totalLineInput == null) {
                checker = false;
                break;
            }
            nums = totalLineInput.Split(' ');

            //Debug.Log("buffer: " + totalLineInput + " nums: " + nums.Length);
            
            if (nums.Length > 2) {
                list.Add(new Block(int.Parse(nums[0]), (int.Parse(nums[1])), int.Parse(nums[2]), int.Parse(nums[3])));
                //set variables to pasrsing results
            }
            else {
                minLoc = int.Parse(nums[0]);
                maxLoc = int.Parse(nums[1]);
            }
            totalLineInput = ""; //Clear buffer
        }

        //Debug.Log("min & Max: " + minLoc + " " + maxLoc);

        placeBlocksOne(list); //Adds to occupiedList
        clearBlocks(occupiedSpaces); //Resets blank values to "lawn"
        //tileCheck();
        reader.Close(); //Will open again at next loop
    }

    void OnApplicationQuit() {
        reader.Close();
    }

    void placeBlocksOne(List<Block> _list) { //Starting/Default
        canSet = false;
        double minDist = double.MaxValue;
        //List<KeyValuePair<int, int>> minnerList;
        
        foreach (Block block in _list) { //Go through list
            int gridx = 0, gridy = 0;

            //Offset center marker value for Big Curves
            //Check if Block can be placed (Big Curve)
            if (block.type == 4) { //Move to nearest upper-left quadrant, regardless of orientation
                block.x -= (((maxLoc - minLoc)/10)/2);
                block.y -= (((maxLoc - minLoc)/10)/2);
                canSet = ((block.x + (((maxLoc - minLoc)/10)) <= maxLoc) && ((block.y) + (((maxLoc - minLoc)/10)) <= maxLoc));
            }

            // minLoc + ((maxLoc - minLoc)*(1...10))

            switch (block.x) {
                case double q when (q > minLoc && q <= (minLoc + (((maxLoc - minLoc)/10)*1))):
                    gridx = 0;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*1)) && q <= (minLoc + (((maxLoc - minLoc)/10)*2))):
                    gridx = 1;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*2)) && q <= (minLoc + (((maxLoc - minLoc)/10)*3))):
                    gridx = 2;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*3)) && q <= (minLoc + (((maxLoc - minLoc)/10)*4))):
                    gridx = 3;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*4)) && q <= (minLoc + (((maxLoc - minLoc)/10)*5))):
                    gridx = 4;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*5)) && q <= (minLoc + (((maxLoc - minLoc)/10)*6))):
                    gridx = 5;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*6)) && q <= (minLoc + (((maxLoc - minLoc)/10)*7))):
                    gridx = 6;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*7)) && q <= (minLoc + (((maxLoc - minLoc)/10)*8))):
                    gridx = 7;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*8)) && q <= (minLoc + (((maxLoc - minLoc)/10)*9))):
                    gridx = 8;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*9)) && q <= (minLoc + (((maxLoc - minLoc)/10)*10))):
                    gridx = 9;
                    break;
                default:
                    gridx = -1;
                    break;
            }
            
            switch (inverty(block.y)) {
                case double q when (q > minLoc && q <= (minLoc + (((maxLoc - minLoc)/10)*1))):
                    gridy = 0;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*1)) && q <= (minLoc + (((maxLoc - minLoc)/10)*2))):
                    gridy = 1;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*2)) && q <= (minLoc + (((maxLoc - minLoc)/10)*3))):
                    gridy = 2;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*3)) && q <= (minLoc + (((maxLoc - minLoc)/10)*4))):
                    gridy = 3;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*4)) && q <= (minLoc + (((maxLoc - minLoc)/10)*5))):
                    gridy = 4;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*5)) && q <= (minLoc + (((maxLoc - minLoc)/10)*6))):
                    gridy = 5;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*6)) && q <= (minLoc + (((maxLoc - minLoc)/10)*7))):
                    gridy = 6;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*7)) && q <= (minLoc + (((maxLoc - minLoc)/10)*8))):
                    gridy = 7;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*8)) && q <= (minLoc + (((maxLoc - minLoc)/10)*9))):
                    gridy = 8;
                    break;
                case double q when (q > (minLoc + (((maxLoc - minLoc)/10)*9)) && q <= (minLoc + (((maxLoc - minLoc)/10)*10))):
                    gridy = 9;
                    break;
                default:
                    gridy = -1;
                    break;
            }

            // BlockType 4 will override other block pieces anyway!

            if ((gridx > -1 && gridy > -1)) {
                //Confirm block placement - Collision?
                if (occupiedSpaces.Contains(new KeyValuePair<int, int>(gridx, gridy))) {
                    Block overlap = block;
                    foreach (Block _block in list) {
                        //If a block os distict, and closer than minDist away, get it in 'overlap'
                        if ((block != _block && _block.x - block.x != 0 && _block.y - block.y != 0) && (minDist > System.Math.Sqrt((System.Math.Pow(block.x - _block.x, 2) + System.Math.Pow(block.y - _block.y, 2))))) {
                            overlap = _block;
                            minDist = System.Math.Sqrt((System.Math.Pow(block.x - _block.x, 2) + System.Math.Pow(block.y - _block.y, 2)));
                        }
                    }
                    //dist farthest
                    if (abs(overlap.x - block.x) > abs(inverty(overlap.y) - inverty(block.y))) { //x
                        if (block.x - overlap.x > 0) {
                            if (!occupiedSpaces.Contains(new KeyValuePair<int, int>(gridx+1, gridy))) {
                                gridx += 1;
                            }
                        }
                        else {
                            if (!occupiedSpaces.Contains(new KeyValuePair<int, int>(gridx-1, gridy))) {
                                gridx -= 1;
                            }
                        }
                    }
                    else { //y
                        if (inverty(block.y) - inverty(overlap.y) > 0) {
                            if (!occupiedSpaces.Contains(new KeyValuePair<int, int>(gridx, gridy+1))) {
                                gridy -= 1; //Swapped - inverted
                            }                            
                        }
                        else {
                            if (!occupiedSpaces.Contains(new KeyValuePair<int, int>(gridx, gridy-1))) {
                                gridy += 1;
                            }   
                        }
                    }
                }

                result += ("X: " + gridx + " Y: " + gridy);

                //Activate block
                //Explicitly remove elements, and '-1' to gridy instead of '=1'
                    //y-1 will raise element
                for (int i = 0 ; i < 5 ; ++i) {
                    if (block.type != 4) {
                        if (i != block.type) {
                            // Deactivate any other overlapping block type
                            if (rows[gridy].transform.GetChild(gridx).transform.GetChild(i).gameObject.activeSelf) {
                                rows[gridy].transform.GetChild(gridx).transform.GetChild(i).gameObject.SetActive(false);
                                occupiedSpaces.Remove(new KeyValuePair<int, int>(gridx, gridy));
                            }                            
                        }
                        else {
                            // Activate blocktype
                            if (!rows[gridy].transform.GetChild(gridx).transform.GetChild(i).gameObject.activeSelf) {rows[gridy].transform.GetChild(gridx).transform.GetChild(i).gameObject.SetActive(true);}
                            // Block Rotation:
                            rows[gridy].transform.GetChild(gridx).transform.GetChild(i).transform.eulerAngles = new Vector3(0.0f, (float) block.angle, 0.0f);
                            occupiedSpaces.Add(new KeyValuePair<int, int>(gridx, gridy));
                        }
                    }
                    else if (block.type == 4 && canSet) { //Setting 4-spot grid piece
                        if (i != block.type) {
                            // Deactivate any other overlapping block type for all areas
                            if (rows[gridy].transform.GetChild(gridx).transform.GetChild(i).gameObject.activeSelf)     {
                                rows[gridy].transform.GetChild(gridx).transform.GetChild(i).gameObject.SetActive(false);
                                occupiedSpaces.Remove(new KeyValuePair<int, int>(gridx, gridy));
                            }
                            if (rows[gridy-1].transform.GetChild(gridx).transform.GetChild(i).gameObject.activeSelf)   {
                                rows[gridy-1].transform.GetChild(gridx).transform.GetChild(i).gameObject.SetActive(false);
                                occupiedSpaces.Remove(new KeyValuePair<int, int>(gridx, gridy-1));
                            }
                            if (rows[gridy].transform.GetChild(gridx+1).transform.GetChild(i).gameObject.activeSelf)   {
                                rows[gridy].transform.GetChild(gridx+1).transform.GetChild(i).gameObject.SetActive(false);
                                occupiedSpaces.Remove(new KeyValuePair<int, int>(gridx+1, gridy));
                            }
                            if (rows[gridy-1].transform.GetChild(gridx+1).transform.GetChild(i).gameObject.activeSelf) {
                                rows[gridy-1].transform.GetChild(gridx+1).transform.GetChild(i).gameObject.SetActive(false);
                                occupiedSpaces.Remove(new KeyValuePair<int, int>(gridx+1, gridy-1));
                            }

                        }
                        else {
                            // Activate blocktype
                            if (!rows[gridy].transform.GetChild(gridx).transform.GetChild(i).gameObject.activeSelf) {rows[gridy].transform.GetChild(gridx).transform.GetChild(i).gameObject.SetActive(true);}
                            
                            // Block Rotation:
                            rows[gridy].transform.GetChild(gridx).transform.GetChild(i).transform.eulerAngles = new Vector3(0.0f, (float) block.angle, 0.0f);
                            // Add occupied for all four grid spaces
                            occupiedSpaces.Add(new KeyValuePair<int, int>(gridx, gridy));
                            occupiedSpaces.Add(new KeyValuePair<int, int>(gridx+1, gridy));
                            occupiedSpaces.Add(new KeyValuePair<int, int>(gridx, gridy-1));
                            occupiedSpaces.Add(new KeyValuePair<int, int>(gridx+1, gridy-1));
                        }
                    }
                }
                //return true;
            }
            Debug.Log(block.id + ": " + result + "\n" + block.data);
            result = "";
        }
        //return false;
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
        return a < 0 ? -a : a;
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
