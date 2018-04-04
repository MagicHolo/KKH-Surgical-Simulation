using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public GameObject optionUI, infoBoard, guideLine, ecg, headUpDisplay, boundingBox, codeBlueButton, checkSequence, checkSkinPrep;

    // Use this for initialization
    void Start () {
        ObjectDrag.optionUI = optionUI;
        ObjectDrag.infoBoard = infoBoard;
        ObjectDrag.guideLine = guideLine;
        ObjectDrag.ecg = ecg;
        ObjectDrag.headUpDisplay = headUpDisplay;
        ObjectDrag.boundingBox = boundingBox;
        ObjectDrag.codeBlueButton = codeBlueButton;
        ObjectDrag.checkSequence = checkSequence;
        ObjectDrag.checkSkinPrep = checkSkinPrep;
	}
}
