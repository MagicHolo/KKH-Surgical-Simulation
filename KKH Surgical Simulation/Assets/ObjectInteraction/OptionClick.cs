using System.Collections;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OptionClick : MonoBehaviour, IInputClickHandler {

    public GameObject openBy;
    public ObjectDrag.Tool tool;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (openBy != null) openBy.GetComponent<ObjectDrag>().toolState = tool;
        transform.parent.gameObject.SetActive(false);
    }

    private void Awake()
    {

    }

    private void OnEnable()
    {
        transform.parent.GetComponent<AudioSource>().Play();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
