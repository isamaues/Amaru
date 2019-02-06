using UnityEngine;
using System.Collections;

public class MenuAudioSource : MonoBehaviour {

	private static MenuAudioSource instance = null;
	public static MenuAudioSource Instance {
		get { return instance; }
	}
	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			instance.gameObject.SetActive(true);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
