﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    public static Transform field;
    public Transform touchZonesParent;
    public GameObject RevivePanel;

    public void Start()
    {
        field = this.transform;
    }
    public void CheckFieldForFullLines()
    {
        List<GameObject> listFullCells = new List<GameObject>();
        bool horizontal = true;
        bool vertical = true;

        for (int i = 0; i < 10; i++)
        {
            horizontal = true;
            vertical = true;

            for (int j = 0; j < 10; j++)
            {
                if (!field.GetChild(i * 10 + j).transform.GetComponent<Cell>().isSet)
                    horizontal = false;

                if (!field.GetChild(j * 10 + i).transform.GetComponent<Cell>().isSet)
                    vertical = false;
            }

            if (horizontal)
                foreach (GameObject item in ReturnListOfCellsByHorizontal(i))
                {
                    listFullCells.Add(item);
                }


            if (vertical)
                foreach (GameObject item in ReturnListOfCellsByVertical(i))
                {
                    listFullCells.Add(item);
                }
        }


        foreach (var item in listFullCells)
        {
            item.transform.GetComponent<Image>().color = ColorManager.GetDefaultColour();
            item.transform.GetComponent<Cell>().SetValue(false);
        }

    }

    List<GameObject> ReturnListOfCellsByHorizontal(int numHorizontal)
    {
        List<GameObject> listFullCells = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            listFullCells.Add(field.GetChild(numHorizontal * 10 + i).gameObject);
        }
        return listFullCells;
    }

    List<GameObject> ReturnListOfCellsByVertical(int numVertical)
    {
        List<GameObject> listFullCells = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            listFullCells.Add(field.GetChild(i * 10 + numVertical).gameObject);
        }
        return listFullCells;
    }




    public void ChekShapeForPlacement(Transform touchZone)
    {
        CleerFieldColor();
        int length = BlockInShape.matrixLength;
        int numBoxWithColl = -1;
        int targetIndex = -1;
        Color color = new Color();

        List<int> listOfIndexs = new List<int>();

        for (int i = 0; i < touchZone.childCount; i++)
        {
            if (touchZone.GetChild(i).GetComponent<Image>().enabled)
            {
                listOfIndexs.Add(touchZone.GetChild(i).GetSiblingIndex());
            }
        }

        for (int i = 0; i < touchZone.childCount; i++)
        {
            if (touchZone.GetChild(i).GetComponent<BoxCollider2D>().isActiveAndEnabled)
            {
                numBoxWithColl = i;
                targetIndex = touchZone.GetChild(i).GetComponent<BoxCollider2D>().GetComponent<BlockInShape>().TargetIndex;
                color = touchZone.GetComponent<TouchZone>().currentColor;
                break;
            }
        }

        if (targetIndex == -1 || numBoxWithColl == -1)
        {
            Debug.Log(targetIndex + " " + numBoxWithColl);
            return ;
        }

        int zeroPoint = targetIndex - numBoxWithColl - (BlockInShape.matrixLength * (int)(numBoxWithColl / BlockInShape.matrixLength));


        int x = zeroPoint + 10 * (int)(listOfIndexs[0] / BlockInShape.matrixLength) + listOfIndexs[0] % BlockInShape.matrixLength;


        int line = (x / 10 - listOfIndexs[0] / 5);


        for (int i = 0; i < listOfIndexs.Count; i++)
        {
            x = zeroPoint + 10 * (int)(listOfIndexs[i] / BlockInShape.matrixLength) + listOfIndexs[i] % BlockInShape.matrixLength;
            if (x > 99 || x < 0)
                return ;

            if (field.GetChild(x).GetComponent<Cell>().isSet)
            {
                return ;
            }


            if (line != (x / 10 - listOfIndexs[i] / 5))
            {
                Debug.Log("linr = " + line + " != " + (x / 10 - listOfIndexs[i] / 5));
                return ;
            }

        }
        for (int i = 0; i < listOfIndexs.Count; i++)
        {
            x = zeroPoint + 10 * (int)(listOfIndexs[i] / BlockInShape.matrixLength) + listOfIndexs[i] % BlockInShape.matrixLength;
            field.GetChild(x).GetComponent<Cell>().SetValue(true);
            field.GetChild(x).GetComponent<Image>().color = color;

        }

       
        return ;
    }


    public void CleerFieldColor()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).GetComponent<Cell>().isSet)
            {
                transform.GetChild(i).GetComponent<Image>().color = ColorManager.GetDefaultColour();
            }
        }
    }

    public bool CheckThePossibilityOfPlacement(Transform touchZone)
    {
        // CleerFieldColor();
        int counter = 0;
        int length = BlockInShape.matrixLength;
        int numBoxWithColl = -1;
        //int targetIndex = -1;

        List<int> listOfIndexs = new List<int>();

        for (int i = 0; i < touchZone.childCount; i++)
        {
            if (touchZone.GetChild(i).GetComponent<Image>().enabled)
            {
                listOfIndexs.Add(touchZone.GetChild(i).GetSiblingIndex());
            }
        }

        for (int i = 0; i < touchZone.childCount; i++)
        {
            if (touchZone.GetChild(i).GetComponent<BoxCollider2D>().isActiveAndEnabled)
            {
                numBoxWithColl = i;
                break;
            }
        }

        if (numBoxWithColl == -1)
        {
            Debug.Log("numBoxWithColl = " + numBoxWithColl);
            
            return false;
        }

        int zeroPoint = -1;
        bool flag = true;
        //Debug.Log("transform.childCount = " + transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            flag = true;
            if (transform.GetChild(i).GetComponent<Cell>().isSet)
                continue;
            zeroPoint = i - numBoxWithColl - (BlockInShape.matrixLength * (int)(numBoxWithColl / BlockInShape.matrixLength));

            int x = zeroPoint + 10 * (int)(listOfIndexs[0] / BlockInShape.matrixLength) + listOfIndexs[0] % BlockInShape.matrixLength;
           // Debug.Log(x);

            int line = (x / 10 - listOfIndexs[0] / 5);


            for (int j = 0; j < listOfIndexs.Count; j++)
            {
                x = zeroPoint + 10 * (int)(listOfIndexs[j] / BlockInShape.matrixLength) + listOfIndexs[j] % BlockInShape.matrixLength;
                if (x > 99 || x < 0)
                {
                   // Debug.Log("(x > 99 || x < 0)");
                    flag = false;
                    break;
                    //return false;
                }

                if (field.GetChild(x).GetComponent<Cell>().isSet)
                {
                    //Debug.Log("field.GetChild(x).GetComponent<Cell>().isSet");
                    flag = false;
                    break;
                    //return false;
                }


                if (line != (x / 10 - listOfIndexs[j] / 5))
                {
                    //Debug.Log("linr = " + line + " != " + (x / 10 - listOfIndexs[j] / 5));
                    flag = false;
                    break;
                    //return false;
                }
                

            }
            if (flag)
            {
                counter++;
            }
            
        }
        if (counter == 0)
            return false;
        //Debug.Log(counter);

        return true;
    }
    public void CheckForLoss()
    {
        int count = 0;
        for (int i = 0; i < touchZonesParent.childCount; i++)
        {
            if (CheckThePossibilityOfPlacement(touchZonesParent.GetChild(i)))
            {
                count++;
            }         
        }
        if (count == 0)
            RevivePanel.SetActive(true);
    }
}







//public void CheckShapeForPlacement(int targetIndex, int numBoxWithColl, List<int> listOfIndex)
//{
//    int length = BlockInShape.matrixLength;

//    if (targetIndex == -1 || numBoxWithColl == -1)
//    {
//        // Debug.Log(targetIndex + " " + numBoxWithColl);
//        return;
//    }

//    //int colPosX = numBoxWithColl / length;
//    //int colPosy = numBoxWithColl - colPosX * length;

//    ////Debug.Log(colPosX + " +++ "+ colPosy);

//    //int targetPosX = targetIndex / 10;
//    //int targetPosY = targetIndex - targetPosX * 10;
//    //// Debug.Log(targetPosX + " --- " + targetPosY);
//    //Debug.Log(listOfIndex.Count);

//    int zeroPoint = targetIndex - numBoxWithColl - 10;

//    int x = zeroPoint + 10 * (int)(listOfIndex[0] / BlockInShape.matrixLength) + listOfIndex[0] % BlockInShape.matrixLength;


//    int line = (x / 10 - listOfIndex[0] / 5);
//    for (int i = 0; i < listOfIndex.Count; i++)
//    {
//        x = zeroPoint + 10 * (int)(listOfIndex[i] / BlockInShape.matrixLength) + listOfIndex[i] % BlockInShape.matrixLength;
//        if (x > 99 || x < 0)
//            return;

//        if (field.GetChild(x).GetComponent<Cell>().isSet)
//        {
//            return;
//        }


//        if (line != (x / 10 - listOfIndex[i] / 5))
//        {
//            Debug.Log("linr = " + line + " != " + (x / 10 - listOfIndex[i] / 5));
//            return;
//        }

//    }
//    for (int i = 0; i < listOfIndex.Count; i++)
//    {
//        x = zeroPoint + 10 * (int)(listOfIndex[i] / BlockInShape.matrixLength) + listOfIndex[i] % BlockInShape.matrixLength;
//        field.GetChild(x).GetComponent<Cell>().SetValue(true);
//        field.GetChild(x).GetComponent<Image>().color = ColorManager.GetNextColor();

//    }
//    //ColorManager.IncrementColor();
//}