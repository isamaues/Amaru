using UnityEngine;
using System.Collections;

public class OpenCredits : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Invoke("ChangeLevel",5);
	}
	
	void ChangeLevel()
	{
		AutoFade.LoadLevel("SplashScreen",1f,1f,Color.white);
	}

	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown && this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Final"))
		{
			AutoFade.LoadLevel("SplashScreen", 1,1 ,Color.white);
		}
	}
}
