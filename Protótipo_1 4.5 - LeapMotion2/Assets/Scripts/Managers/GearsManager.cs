using UnityEngine;

public class GearsManager : MonoBehaviour
{
	public static int PontuacaoPorcaBronze
	{
		get { return 1; }
	}

	public static int PontuacaoParafusoOuro
	{
		get { return 5; }
	}

	public static int PontuacaoEngrenagemOuro
	{
		get { return 10; }
	}

	private int totalWorld;
	private int totalSession;
	private int collectedWorld;
	private int collectedSession;

	private float barFilled = 2;

	private bool isAnimating = false;

	public static int maxColectable;

	private ITaskManager taskManager = TaskManagerInstance.Instance;

	public void Start()
	{
		maxColectable = taskManager.MaximumPontuation;
		totalWorld = (taskManager.MaximumPontuation / WorldManager.GetInstance().WorldList.Count);
		totalSession = taskManager.MaximumPontuation;
		//		Debug.Log(totalSession);
		collectedWorld = 0;
		collectedSession = UserManager.CurrentProgress.quantidade_Parafusos;
	}

	public int TotalWorld
	{
		get
		{
			return totalWorld;
		}
		set
		{
			totalWorld = value;
		}
	}

	public void Add(GearType type)
	{
		var soma = 0;
		switch (type)
		{
			case GearType.EngrenagemOuro:
				soma = PontuacaoEngrenagemOuro;
				break;

			case GearType.ParafusoOuro:
				soma = PontuacaoParafusoOuro;
				break;

			case GearType.PorcaBronze:
				soma = PontuacaoPorcaBronze;
				break;
		}

		//		Debug.Log(soma);

		UserManager.CurrentProgress.quantidade_Parafusos += soma;
		UserManager.GetInstance().UpdateCurrentProgressXML();

		MinigameSetup.colectedItemInter = true;
	}
}