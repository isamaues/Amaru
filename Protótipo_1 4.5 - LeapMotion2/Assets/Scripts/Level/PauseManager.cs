using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
	private bool isPaused = false;
    public GameObject exitButton;
    public GameObject exitMenu;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            exitButton.SetActive(!exitButton.active);
            exitMenu.SetActive(!exitMenu.active);
        }
    }

	public bool IsPaused
	{
		get { return isPaused; }
		set { isPaused = value; }
	}

	public void TogglePause()
	{
		isPaused = !isPaused;
	}

    public void Exit()
    {
        AutoFade.LoadLevel("SplashScreen", 0.1f, 0.1f, Color.white);
    }
}