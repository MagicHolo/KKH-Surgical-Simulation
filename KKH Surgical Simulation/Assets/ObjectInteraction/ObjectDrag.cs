using HoloToolkit.Unity;
using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ObjectDrag : MonoBehaviour, INavigationHandler, IManipulationHandler, ISpeechHandler
{


    bool isManipulating, isRotating, isScaling;
    public float rotateSensitivity = 2.5f, scaleSensitivity = 1f, moveSensitivity = 1.5f;
    Vector3 position, previousPosition;
    Vector3 navigationPosition;
    float factor, size;
    public static GameObject optionUI, infoBoard, guideLine, ecg, headUpDisplay, boundingBox, codeBlueButton, checkSequence, checkSkinPrep;
    public static bool isCodeBlue = false;

    public enum Tool { Move, Rotate, Scale }
    public Tool toolState = Tool.Scale;


    private void OnEnable()
    {
        ///////if (gameObject.name == "Patient_fuse flat" || gameObject.name == "Patient_fuse 45") toolState = GetComponent<PatientClick>().otherPatient.GetComponent<ObjectDrag>().toolState;
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        Rotating();
        Scaling();
    }

    #region Rotation

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        if (toolState == Tool.Move) return;

        if (toolState == Tool.Rotate)
        {
            isRotating = true;
        }
        else if (toolState == Tool.Scale)
        {
            isScaling = true;
        }

        navigationPosition = eventData.CumulativeDelta;

    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        if (toolState == Tool.Move) return;

        if (toolState == Tool.Rotate)
            isRotating = true;
        else if (toolState == Tool.Scale)
            isScaling = true;

        navigationPosition = eventData.CumulativeDelta;
    }

    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        isRotating = false;
        isScaling = false;
    }

    public void OnNavigationCanceled(NavigationEventData eventData)
    {
        isRotating = false;
        isScaling = false;
    }

    void Rotating()
    {
        if (isRotating)
        {
            factor = navigationPosition.x * rotateSensitivity;
            if (gameObject == infoBoard || gameObject == ecg || gameObject == checkSequence || gameObject == checkSkinPrep)
            {
                transform.Rotate(Vector3.up * factor);
            }
            else
            {
                transform.Rotate(Vector3.forward * factor);
            }
        }
    }

    void Scaling()
    {
        if (isScaling)
        {
            size = navigationPosition.x * scaleSensitivity;
            transform.localScale += size * Vector3.one;
        }
    }

    #endregion

    #region Manipulation
    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        isManipulating = false;
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        isManipulating = false;
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        if (toolState != Tool.Move) return;

        isManipulating = true;

        position = eventData.CumulativeDelta;
        previousPosition = position;
        
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (toolState != Tool.Move) return;

        isManipulating = true;
        position = eventData.CumulativeDelta;
        ManipulateUpdate(position);
    }

    void ManipulateUpdate(Vector3 _position)
    {
        if (isManipulating)
        {
            Vector3 move = Vector3.zero;
            move = _position - previousPosition;
            transform.position += move * moveSensitivity;
            previousPosition = _position;
        }
    }
    #endregion

    #region ISpeechHandler
    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        switch(eventData.RecognizedText.ToLower()){
            case "option":
                optionUI.transform.position = Camera.main.transform.position + Vector3.forward * 1.5f;
                optionUI.SetActive(true);
                optionUI.transform.GetChild(0).GetComponent<OptionClick>().openBy = gameObject;
                optionUI.transform.GetChild(2).GetComponent<OptionClick>().openBy = gameObject;
                optionUI.transform.GetChild(1).GetComponent<OptionClick>().openBy = gameObject;
                break;

            case "open info":
                infoBoard.SetActive(true);
                break;
            case "close info":
                infoBoard.SetActive(false);
                break;

            case "hide all":
                guideLine.SetActive(false);
                infoBoard.SetActive(false);
                ecg.SetActive(false);
                headUpDisplay.SetActive(false);
                boundingBox.GetComponent<MeshRenderer>().enabled = false;
                codeBlueButton.GetComponent<MeshRenderer>().enabled = false;
                checkSequence.SetActive(false);
                break;

            case "hide bounding box":
                boundingBox.GetComponent<MeshRenderer>().enabled = false;
                break;

            case "restart":
                Application.LoadLevel(Application.loadedLevel);
                isCodeBlue = false;
                break;

            case "guideline":
                guideLine.SetActive(true);
                ecg.SetActive(true);
                break;

            case "reset":
                isCodeBlue = false;
                break;

            case "no response":
                headUpDisplay.GetComponent<HeadsUpDirectionIndicator>().TargetObject = codeBlueButton;
                headUpDisplay.SetActive(true);
                checkSequence.SetActive(true);
                
                break;

            case "next":
                headUpDisplay.GetComponent<HeadsUpDirectionIndicator>().TargetObject = boundingBox;
                checkSequence.SetActive(false);
                headUpDisplay.SetActive(true);
                checkSkinPrep.SetActive(false);
                isCodeBlue = true;
                break;
        }
    }

    #endregion

}
