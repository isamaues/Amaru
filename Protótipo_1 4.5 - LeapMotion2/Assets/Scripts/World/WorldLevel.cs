using System.Collections.Generic;
using UnityEngine;

public class WorldLevel : MonoBehaviour
{
	public List<GameObject> worldList = new List<GameObject>();
	public GameObject amaruImage;

	private GameObject lastWorld;
	private GameObject nextWorld;

	private bool ableToStart = false;

	public float velocity = 2f;

	private int totalGear;

	private static ITaskManager taskManager;// = XMLTaskManager.Instance;

	// Use this for initialization
	private void Start()
	{
		taskManager = TaskManagerInstance.Instance;
		int cur = taskManager.GetCurrentWorld();
		if (cur == 0)
		{
			lastWorld = worldList[cur];
			amaruImage.transform.position = new Vector3(lastWorld.transform.position.x, lastWorld.transform.position.y, lastWorld.transform.position.z - 0.1f);
			nextWorld = worldList[cur];
			ableToStart = true;
		}
		else if (cur == worldList.Count)
		{
			ableToStart = true;
		}
		else
		{
			lastWorld = worldList[cur - 1];
			amaruImage.transform.position = new Vector3(lastWorld.transform.position.x, lastWorld.transform.position.y, lastWorld.transform.position.z - 0.1f);
			nextWorld = worldList[cur];
		}

		totalGear = taskManager.MaximumPontuation;
	}

	// Update is called once per frame
	private void Update()
	{
		amaruImage.transform.position = Vector3.MoveTowards(amaruImage.transform.position, new Vector3(nextWorld.transform.position.x, nextWorld.transform.position.y, amaruImage.transform.position.z), velocity * Time.deltaTime);

		/*if (Input.GetKeyDown (KeyCode.Space)) {
			AutoFade.LoadLevel("new_level_001",2f,2f,Color.white);
		}*/
	}

	public void ChangeLevel()
	{
		if (AutoFade.Fading)
			return;
		Debug.Log("!@#!@#");
		AutoFade.LoadLevel("new_level_001", 2f, 2f, Color.white);
	}

	private void OnGUI()
	{/*
		GUIStyle grey = new GUIStyle();
		GUIStyle red = new GUIStyle();

		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, Color.grey);
		texture.Apply();
		grey.normal.background = texture;
		GUI.Box(new Rect(10, 10, (Screen.width - 20), 40f), "", grey);

		texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, Color.red);
		texture.Apply();
		red.normal.background = texture;
		GUI.Box(new Rect(10, 10, ((Screen.width - 20) / totalGear) * UserManager.CurrentUserSetings.quantidade_Parafusos, 40f), "", red);
	*/
	}
}