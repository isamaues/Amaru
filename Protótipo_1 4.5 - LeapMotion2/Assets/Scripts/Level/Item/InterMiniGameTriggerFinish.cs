using UnityEngine;
using UnityEngine.UI;

public class InterMiniGameTriggerFinish : MonoBehaviour
{
	private MinigameSetup ms;
    public Image returnToMenuPanel;
    public GameObject auxPanel;
    public Canvas canvas;
    private PauseManager pauseMan;

    private void Awake()
    {
        auxPanel = Resources.Load("Prefabs/ReturnToMenu") as GameObject;
        canvas = GameObject.Find("ExitHUD").GetComponent<Canvas>();
        pauseMan = GameObject.Find("GameManager").GetComponent<PauseManager>();

        returnToMenuPanel = Instantiate(auxPanel.GetComponent<Image>());
        returnToMenuPanel.transform.SetParent(canvas.transform);
        returnToMenuPanel.transform.localPosition = new Vector3(0, 0, 0);
        returnToMenuPanel.rectTransform.sizeDelta = new Vector2(500, 300);
        returnToMenuPanel.gameObject.SetActive(false);
    }

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (WorldManager.GetInstance().checkCurrentWorld())
			{
				this.gameObject.SetActive(false);
				return;
			}
                else
                {
                    ms = GameObject.Find("Urama").GetComponent<MinigameSetup>();
                    ms.ChangeToMinigame();
                }
		}
	}
}