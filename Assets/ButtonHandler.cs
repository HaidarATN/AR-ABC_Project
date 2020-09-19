using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    public Image bg;
    public Button start, exit;
    public Animator animUI;
    public GameObject ARCam;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void StartButton()
    {
        animUI.Play("Hide");
        exit.interactable = false;
        start.interactable = false;
        ARCam.GetComponent<Vuforia.VuforiaBehaviour>().enabled = true;
    }

    public void BackButton()
    {
        animUI.Play("Show");
        exit.interactable = true;
        start.interactable = true;
        ARCam.GetComponent<Vuforia.VuforiaBehaviour>().enabled = false;
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
