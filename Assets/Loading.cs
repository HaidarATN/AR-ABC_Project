using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
        yield break;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
