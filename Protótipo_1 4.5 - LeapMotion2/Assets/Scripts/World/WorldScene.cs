using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class WorldScene : MonoBehaviour {

	Animator animator;
	ITaskManager taskManager;

	int currentPontuation;
	float percent;
	float totalPontuation;

	float totalWidth;

	public RectTransform redBar;
	public RectTransform yellowBar;
	public RectTransform greenBar;

	float redBarStartWidth;
	float redBarTargetPosition;
	float yellowBarStartWidth;
	float yellowBarTargetPosition;
	float greenBarStartWidth;
	float greenBarTargetPosition;

	bool yellow = false, green = false;

	void Start () {
		animator = GetComponent<Animator>();
		taskManager = TaskManagerInstance.Instance;
		animator.SetInteger("endWorld",taskManager.GetCurrentWorld());
		Invoke("ChangeLevel",8);

		currentPontuation = taskManager.CurrentPontuation;
		totalPontuation = taskManager.MaximumPontuation; 
		percent = currentPontuation/totalPontuation;

		Debug.Log(currentPontuation + " " + totalPontuation + " " + percent);

		//percent = 0.52f;

		redBarStartWidth = redBar.rect.width;
		yellowBarStartWidth = yellowBar.rect.width;
		greenBarStartWidth = greenBar.rect.width;

		totalWidth = redBarStartWidth + yellowBarStartWidth + greenBarStartWidth;

		redBarTargetPosition = percent * totalWidth;
		redBar.sizeDelta = new Vector2(1,redBar.rect.height);


		if (redBarTargetPosition > redBarStartWidth)
		{
			yellowBarTargetPosition = redBarTargetPosition - redBarStartWidth;
			redBarTargetPosition = redBarStartWidth;
			yellowBar.sizeDelta = new Vector2( 1,yellowBar.rect.height);
			yellow = true;
			if (yellowBarTargetPosition > yellowBarStartWidth)
			{
				greenBarTargetPosition = yellowBarTargetPosition - yellowBarStartWidth;
				yellowBarTargetPosition = yellowBarStartWidth;
				green = true;
				if ( greenBarTargetPosition > greenBarStartWidth)
				{
					greenBarTargetPosition = greenBarTargetPosition;
					greenBar.sizeDelta= new Vector2 ( 1,greenBar.rect.height);
				}
			}
		}

        Debug.Log(redBarTargetPosition + " " + yellowBarTargetPosition + " " + greenBarTargetPosition);
        StartCoroutine("FillRedBar");
	}

	void ChangeLevel()
	{
		AutoFade.LoadLevel("new_level_001",1,1,Color.white);
	}

	IEnumerator FillRedBar() {
		for (float f = 0; f <= redBarTargetPosition; f += 2f) {
			redBar.sizeDelta = new Vector2(f,redBar.rect.height);
			yield return null;
		}
		if ( yellow )
			StartCoroutine("FillYellowBar");

		Debug.Log("Acabou");
	}

	IEnumerator FillYellowBar() {
		yellowBar.gameObject.SetActive(true);
		for (float f = 0; f <= yellowBarTargetPosition; f += 2f) {
			yellowBar.sizeDelta = new Vector2(f,yellowBar.rect.height);
			yield return null;
		}
		if ( green )
			StartCoroutine("FillGreenBar");

		Debug.Log("Acabou");
	}

	IEnumerator FillGreenBar() {
		greenBar.gameObject.SetActive(true);

		for (float f = 0; f <= greenBarTargetPosition; f += 2f) {
			greenBar.sizeDelta = new Vector2(f,greenBar.rect.height);
			yield return null;
		}
		Debug.Log("Acabou: " + greenBarTargetPosition);
	}

}
