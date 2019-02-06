using System.Collections;
using UnityEngine;

public class BackgroundSongManager : MonoBehaviour
{
	//public AudioSource audioSource;
	private WorldManager worldManager = WorldManager.GetInstance();

	private void Awake()
	{
		worldManager.CurrentWorld.LoadAudioCLIP();

		gameObject.GetComponent<AudioSource>().clip = worldManager.CurrentWorld.BackgroundSong;
		gameObject.GetComponent<AudioSource>().Play();
	}
}