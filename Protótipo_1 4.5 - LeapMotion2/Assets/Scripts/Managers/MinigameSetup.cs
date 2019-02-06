using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameSelectionInfo
{
	public int fitness;
	public MiniGameType type;
}

public class MinigameSetup : MonoBehaviour
{
	public static bool CreatedInterTask = true;
	public static float lastTaskHorizontalPos = 0f;
	public static float minimumTaskTime = 10.5f;
	public GameObject amaru = null;
	public bool firstMiniGame = true;
	public float taskSpace;
	private static ArrayList audioList = new ArrayList();
	private static BaseMiniGame currentMiniGame;

	private static ArrayList textureList = new ArrayList();

	private InterMiniGames interMiniGameScript;
	private ResultadoObstaculo resultado;
	private float interStartTime = 0;
	private float interFinalTime = 0f;

	public static bool colectedItemInter { get; set; }

	private List<MiniGameSelectionInfo> miniGameSelection = new List<MiniGameSelectionInfo>();
	private List<MiniGameSelectionInfo> allMiniGames = new List<MiniGameSelectionInfo>();
	private int possibleMinigames;
	private int x = 0;

	public static bool WasLastAnswerCorret { get; set; }

	public static bool WasLastTaskTraining { get; set; }

	public static bool RunningTask { get; set; }

	public void ChangeToMinigame()
	{
		interFinalTime = Time.time;
		interMiniGameScript.DestroyInterTask();
		CreatedInterTask = false;

		resultado.Latencia = interFinalTime - interStartTime;
		resultado.Usuario = UserManager.CurrentUser.login;
		resultado.PegouItem = colectedItemInter;
		resultado.Mundo = WorldManager.GetInstance().CurrentWorldId;

		UserManager.CurrentResultadoObstaculoList.arrayList.Add(resultado);
		UserManager.GetInstance().UpdateResultadoObstaculoListXML();

		CreateMinigame();
		RunningTask = true;
		WasLastAnswerCorret = false;
		WasLastTaskTraining = false;
	}
	public MiniGameType minigameFitnesSelection()
	{
		if (possibleMinigames == 0)
		{
			possibleMinigames = miniGameSelection.Count;
			foreach (MiniGameSelectionInfo si in miniGameSelection)
			{
				si.fitness = 1;
			}
		}
		
		int a = UnityEngine.Random.Range(0, possibleMinigames);
		
		MiniGameSelectionInfo s = miniGameSelection[a];
		miniGameSelection.Remove(s);
		s.fitness = 0;
		miniGameSelection.Add(s);
		possibleMinigames -= 1;
		
		//return MiniGameType.Arrow;
		return s.type;
	}
	public MiniGameType selectMinigame()
	{
		int i = TaskManagerInstance.Instance.GetCurrentTask().MiniGame;
		//Debug.Log("Minigame Numero: " + i);
		if (i == 0)
		{
			MiniGameType type = minigameFitnesSelection();
			//Debug.Log("Minigame aleatorio " + type);
			return type;
		}
		else 
		{
			if (i <= 0 || i > allMiniGames.Count)
			{
				MiniGameType type = minigameFitnesSelection();
			//	Debug.Log("Minigame aleatorio " + type);
				return type;
			}
			else
			{
			//	Debug.Log("Minigame selecionado " + allMiniGames[i-1].type);
				return allMiniGames[i-1].type;
			}

		}
        //return MiniGameType.Arrow;
	}
	private void CreateMinigame()
	{
		switch (selectMinigame())
		{
			case MiniGameType.Cube:
				currentMiniGame = new CubeMiniGame();
				break;

			case MiniGameType.Platform:
				currentMiniGame = new platformMiniGame();
				break;

			case MiniGameType.Fall:
				currentMiniGame = new FallingMiniGame();
				break;

			case MiniGameType.Arrow:
				currentMiniGame = new ArrowMiniGame();
				break;
		}
	}

	private void LoadResources()
	{
		/*foreach (Word w in UserManager.CurrentWordList.arrayList)
		{
			textureList.Add(Resources.Load("Images/" + w.Name) as Texture2D);
			audioList.Add(Resources.Load("Sounds/" + w.Name) as AudioClip);
		}*/
	}

	private void Start()
	{
		lastTaskHorizontalPos = Camera.main.transform.position.x;
		RunningTask = false;

		LoadResources();
		Session.StartTime = Time.time;

		amaru = GameObject.Find("Amaru");

		interMiniGameScript = this.gameObject.AddComponent<InterMiniGames>();

		interMiniGameScript.Start();

		resultado = new ResultadoObstaculo();
		//int i = 0;
		//foreach (MiniGameType type in Enum.GetValues(typeof(MiniGameType)))
		foreach (MiniGameType type in Enum.GetValues(typeof(MiniGameType)))
		{
			//if (type == MiniGameType.Cube)
			//	continue;
			//if (type == MiniGameType.Arrow)
			//	continue;
			if (type == MiniGameType.Fall)
				continue;

			MiniGameSelectionInfo insert = new MiniGameSelectionInfo();
			insert.fitness = 1;
			insert.type = type;
			miniGameSelection.Add(insert);
			allMiniGames.Add(insert);
//			Debug.Log(allMiniGames[i++].type);
		}

		possibleMinigames = miniGameSelection.Count;

		WorldManager.GetInstance().checkCurrentWorld();
	}

	private void Update()
	{
		if (firstMiniGame && Camera.main.transform.position.x - lastTaskHorizontalPos >= taskSpace)
		{
			ChangeToMinigame();
			firstMiniGame = false;
		}
		if (RunningTask)
		{
			currentMiniGame.Update();
		}
		else if (!CreatedInterTask)
		{
			CreatInterTask();
		}
	}

	private void CreatInterTask()
	{
		CreatedInterTask = true;
		interStartTime = Time.time;
		int current = TaskManagerInstance.Instance.GetCurrentWorld();
		bool teste = (current != WorldManager.GetInstance().CurrentWorldId);

		float offset =  Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 5)).x + 15f;

		resultado = interMiniGameScript.CreatInterTask(NextGearType(), offset, teste);
		colectedItemInter = false;
	}

	private GearType NextGearType()
	{
        if (!WasLastTaskTraining)
        {
            if (WasLastAnswerCorret)
                return GearType.EngrenagemOuro;
            else
                return GearType.PorcaBronze;
        }
        else
        {
            if (WasLastAnswerCorret)
            {
                if (currentMiniGame.GetNumeroTentativas == 1)
                    return GearType.EngrenagemOuro;
                else if (currentMiniGame.GetNumeroTentativas == 2)
                    return GearType.ParafusoOuro;
                else
                    return GearType.PorcaBronze;
            }
            else
                return GearType.Nada;
        }
	}
}