using UnityEngine;
using System.Collections;

public class AudioLoader : MonoBehaviour {


	public ArrayList audioList;

	public IEnumerator LoadAudioFile(string file)
	{
		//Debug.Log("A");
		WWW audioLoader = new WWW(file);
		while( !audioLoader.isDone )
			yield return null;



		audioList.Add(audioLoader.GetAudioClip(false));
		Debug.Log(audioLoader.GetAudioClip(false).name);
	}

	public IEnumerator loadFile(string path) {
		WWW www = new WWW("file://"+path);
		//Debug.Log(www);
		
		AudioClip myAudioClip = www.audioClip;
		while (!myAudioClip.isReadyToPlay)
			yield return www;



		AudioClip clip = www.GetAudioClip(false);
		string[] parts = path.Split('\\');
		clip.name = parts[parts.Length - 1];
		clip.name = clip.name.Substring(0,clip.name.Length-4);
		//Debug.Log(clip.name);
        audioList.Add(clip);
	}


	public ArrayList LoadAudios(string path)
	{
	//	Debug.Log("@#!");
		audioList = new ArrayList();
		var audioFiles = System.IO.Directory.GetFiles(path + "Sons");
		foreach (string file in audioFiles)
		{
			if (file.Contains(".ogg"))
            {
			    StartCoroutine(loadFile(file));
			}
		}

		return audioList;
	}
}
