using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

/*
    
    So in the output txt:
    the first column is x (from left to right)
    the second column is y (from top to bottom)
    the third column consists of the marker ids.
    the fourth column is the orientation

    X | Y | ID | ANG

    e.g.,

    1104 807 20 90
    940 803 24 180
    200 134 21 90
    1149 682 10 0
    898 678 4 90
    1149 600 17 90
    817 596 6 180
    902 595 19 0
    1284 559 23 270
    1292 387 21 0
    1161 345 1 0
    1076 344 5 90
    989 344 14 90
    905 342 9 90
    820 340 18 0
    816 511 3 180
    818 430 2 270
    502 102 25 0

    */
public class Block {

    /*
    0: Lawn
    1: ID 14 Finish-Line/spawn
    2: ID 0-13 straight line (ID 0-2 acceleration, ID 3-5 deceleration);
    3: ID 15-19 small curve (ID 15 acceleration, ID 16 deceleration);
    4: ID 20-24 big curve (ID 20 acceleration, ID 21 deceleration);
    5: ID 25 baseboard
    */

    public int id;
    public int x;
    public int y;
    public int angle; // 0, 90, 180, 270

    /*
    0 = normal
    1 = acceleration
    2 = deceleration
    */
    public int attribute;
    public int type;
    public string data = "";

    public Block(double _x, double _y, int _id, int _angle) {
        int _type, _attribute;

        id = _id;
        if (id == 25) {
            x = -1;
            y = -1;
        }
        else {
            x = (int) _x;
            y = (int) _y;
        }
        angle = _angle;
        data = ("ID: " + id) + (" X: " + x) + (" Y: " + y) + (" ANG: " + angle);

        data += ("\n" + "TYPE: ");
        switch (_id) { //Type
            case int i when (i >= 0 && i <= 13):
                _type = 2; //Straight Road
                data += ("2: Straight Road" + "\n");
                break;
            case int i when (i >= 15 && i <= 19):
                _type = 3; //Small Curve
                data += ("3: Small Curve" + "\n");
                break;
            case int i when (i >= 20 && i <= 24):
                _type = 4; //Big Curve
                data += ("4: Big Curve" + "\n");
                break;
            case 14:
                _type = 1; //Finish
                data += ("1: Finish Line" + "\n");
                break;
            case 25:
                _type = 5; //Baseboard - Do not render
                data += ("5: BaseBoard" + "\n");
                break;
            default:
                _type = 0;
                data += ("0: NA" + "\n");
                break;
        }
        type = _type;

        data += ("ATTR: ");
        switch (_id) { //Attribute
            case int i when ((i >= 0 && i <= 2) || (i == 15 || i == 20)):
                _attribute = 1; //acc
                data += ("1: acc");
                break;
            case int i when ((i >= 3 && i <= 5) || (i == 16 || i == 21)):
                _attribute = 2; //dec
                data += ("2: dec");
                break;
            default:
                _attribute = 0; //norm
                data += ("0: norm");
                break;
        }
        attribute = _attribute;
    }

    public Block(int _x, int _y, int _id, int _angle) {
        int _type, _attribute;

        id = _id;
        if (id == 25) {
            x = -1;
            y = -1;
        }
        else {
            x = _x;
            y = _y;
        }
        data = ("ID: " + id) + (" X: " + x) + (" Y: " + y);

        data += ("\n" + "TYPE: ");
        switch (_id) { //Type
            case int i when (i >= 0 && i <= 13):
                _type = 2; //Straight Road
                data += ("2: Straight Road" + "\n");
                break;
            case int i when (i >= 15 && i <= 19):
                _type = 3; //Small Curve
                data += ("3: Small Curve" + "\n");
                break;
            case int i when (i >= 20 && i <= 24):
                _type = 4; //Big Curve
                data += ("4: Big Curve" + "\n");
                break;
            case 14:
                _type = 1; //Finish
                data += ("1: Finish Line" + "\n");
                break;
            case 25:
                _type = 5; //Baseboard - Do not render
                data += ("5: BaseBoard" + "\n");
                break;
            default:
                _type = 0;
                data += ("0: NA" + "\n");
                break;
        }
        type = _type;

        //For orientations,
        //0 for empty,
        //1 for 0 deg,
        //2 for 90 deg,
        //3 for 180 deg,
        //and 4 for 270 deg. (All clockwise)
        //Default:
        //Curves: N -> E
        //Stright: N -> S (Vertical)
        //Rotation: Clockwise
        switch (_angle) { //Pure
            case 2:
                angle = 90;
                break;
            case 3:
                angle = 180;
                break;
            case 4:
                angle = 270;
                break;
            default:
                angle = 0;
                break;
        }

        data += (" ANG: " + angle);

        data += (" ATTR: ");
        switch (_id) { //Attribute
            case int i when ((i >= 0 && i <= 2) || (i == 15 || i == 20)):
                _attribute = 1; //acc
                data += ("1: acc");
                break;
            case int i when ((i >= 3 && i <= 5) || (i == 16 || i == 21)):
                _attribute = 2; //dec
                data += ("2: dec");
                break;
            default:
                _attribute = 0; //norm
                data += ("0: norm");
                break;
        }
        attribute = _attribute;
    }

    public Block(int _x, int _y, int _id) {
        int _type, _attribute;

        id = _id;
        if (id == 25) {
            x = -1;
            y = -1;
        }
        else {
            x = _x;
            y = _y;
        }
        data = ("ID: " + id) + (" X: " + x) + (" Y: " + y);

        data += ("\n" + "TYPE: ");
        switch (_id) { //Type
            case int i when (i >= 0 && i <= 13):
                _type = 2; //Straight Road
                data += ("2: Straight Road" + "\n");
                break;
            case int i when (i >= 15 && i <= 19):
                _type = 3; //Small Curve
                data += ("3: Small Curve" + "\n");
                break;
            case int i when (i >= 20 && i <= 24):
                _type = 4; //Big Curve
                data += ("4: Big Curve" + "\n");
                break;
            case 14:
                _type = 1; //Finish
                data += ("1: Finish Line" + "\n");
                break;
            case 25:
                _type = 5; //Baseboard - Do not render
                data += ("5: BaseBoard" + "\n");
                break;
            default:
                _type = 0;
                data += ("0: NA" + "\n");
                break;
        }
        type = _type;

        data += ("ATTR: ");
        switch (_id) { //Attribute
            case int i when ((i >= 0 && i <= 2) || (i == 15 || i == 20)):
                _attribute = 1; //acc
                data += ("1: acc");
                break;
            case int i when ((i >= 3 && i <= 5) || (i == 16 || i == 21)):
                _attribute = 2; //dec
                data += ("2: dec");
                break;
            default:
                _attribute = 0; //norm
                data += ("0: norm");
                break;
        }
        attribute = _attribute;
    }

    public override string ToString() {
        return data;        
    }

    public static bool containsID(List<Block> c, int containsID) {
        foreach (Block o in c) {
            if (o != null && o.id == (containsID)) {
                return true;
            }
        }
        return false;
    }

    public override bool Equals(object obj) {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || this.GetType() != obj.GetType()) {
            return false;
        }

        Block other = (Block) obj;
        
        // TODO: write your implementation of Equals() here
        if (other.id == this.id) {
            return true;
        }
        return false;
    }
}