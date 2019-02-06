using System.Collections;
using UnityEngine;

//REVISION 126
public abstract class BaseMiniGame
{
	#region Atributes

	public float spaceBetweenItens = 1.5f;

	protected GameObject cancelCube = null;
	protected GameObject okCube = null;
	protected GameObject cubePrefab;
	protected GameObject textPrefab;

	protected float startTime = Time.time;
	protected float finalTime = 0f;
	protected bool canEraseAll = false;
	protected bool correctAnswer = false;
	protected bool minigameComplete = false;
	protected ArrayList comparisonList = new ArrayList();
	protected ArrayList choicesList = new ArrayList();
	protected ArrayList selectionList = new ArrayList();

	protected Vector3 itensMidPoint = Vector3.zero;

	protected int numeroTentativa = 1;
    public int GetNumeroTentativas
    {
        get { return numeroTentativa; }
    }
	protected static string nome;

	protected static ITaskManager taskManager = TaskManagerInstance.Instance;

	//Mod felipe.
	protected GameObject amaru = null;

	#endregion Atributes

	#region Proprieties

	public float MinigameTime
	{
		get
		{
			if (finalTime == 0)
				throw new System.Exception("Tempo final é nulo.");

			return finalTime - startTime;
		}
	}

	public static int CurrentTaskId
	{
		get { return taskManager.GetCurrentTaskId(); }
	}

	public static Task CurrentTask
	{
		get { return taskManager.GetCurrentTask(); }
	}

	public static string CurrentTaskType
	{
		get { return taskManager.GetCurrentTaskType(); }
	}

	public static short CurrentTaskModel
	{
		get { return taskManager.GetCurrentTask().Model; }//CurrentTask.Model;}
	}

	public static Word CurrentTaskModelWord
	{
		get { return null; }// UserManager.CurrentWordList.arrayList [CurrentTaskModel];}
	}

	public static string CurrentTaskModelName
	{
		get { return "1"; }// CurrentTaskModelWord.Name;}
	}

	#endregion Proprieties

	public BaseMiniGame()
	{
		Camera2DTracker.LockHorizontalCamera = true;
		LoadMiniGame();
		BuildMiniGame();
		UramaBehaviour.Instance.Notify(CurrentTaskType[0].ToString());
		startTime = Time.time;
	}

	public abstract void LoadMiniGame();

	public virtual void BuildMiniGame()
	{
		SetAmaru();
		CreateTask();
		ActivateCustomScripts();
		CreateCancelAcceptChoice();
		AdjustCamera();
	}

	public virtual void SetAmaru()
	{
		//		Debug.Log("Chegou aqui");
		amaru = GameObject.Find("Amaru");
		//		if (amaru != null) Debug.Log("Achou amaru");
	}

	public virtual void CreateTask()
	{
		//if (
		Debug.Log("Tipo da tentativa = " + CurrentTaskType);
		if (CurrentTaskType.Length > 1)
		{
			// CurrentTaskType [1] = Pega segunda letra do tipo de tarefa, referente às escolhas.
			switch (CurrentTaskType[1].ToString())
			{
			case "B":
				CreateBChoicesList();
				CreateBChoice(Vector3.zero);
				break;
			case "C":
				CreateCChoicesList();
				CreateCChoice();
				break;

			case "D":
				CreateEChoicesList();
				CreateEChoice();
				break;

			case "E":
				CreateEChoicesList();
				CreateEChoice();
				break;

			default:
				CreateBChoice(Vector3.zero);
				break;
			}
		}

		itensMidPoint = new Vector3(Camera.main.transform.position.x, itensMidPoint.y, itensMidPoint.z);

		OrganizeChoices();
	}

	protected virtual GameObject CreateCube(Texture2D texture, Vector3 pos, GameObject source)
	{

		source.GetComponent<Renderer>().enabled = true;
		
		GameObject inst = GameObject.Instantiate(source, pos, Quaternion.identity) as GameObject;
		inst.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
		inst.GetComponent<Renderer>().material.mainTexture = texture;
		//inst.renderer.material.mainTextureScale = this.cubeScale;
		//inst.renderer.material.mainTextureOffset = new Vector2 ((1f) / 2f, (1f) / 2f);
		
		//inst.transform.localScale *= cubeScale.x;//normalCubeScale.x;
		inst.transform.Rotate(new Vector3(0, 0, 180f));
		
		CubeScript cs = inst.gameObject.AddComponent<CubeScript>();
		if (cs != null)
		{
			cs.CubeInfo = texture.name;
			cs.MinDistance = 0.01f;
		}
//		Debug.Log("CUBO " + inst + " " + texture);
		return inst;
	}



	public virtual void ActivateCustomScripts()
	{
	}

	public virtual void DesactivateCustomScripts()
	{
	}

	public virtual void Update()
	{
        if (selectionList.Count == 0 && CurrentTaskType[0].ToString().Equals("A"))
        {
            cancelCube.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/Misc/autofalante") as Texture2D;
            cancelCube.GetComponent<CubeScript>().CubeInfo = "autofalante";
        }
        else
        {
            cancelCube.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/Misc/borracha") as Texture2D;
            cancelCube.GetComponent<CubeScript>().CubeInfo = "borracha";
            
        }

        string t = "";

        if (CurrentTaskType[1].ToString().Equals("E"))
        {
            if (selectionList.Count > 1)
            {
                foreach (GameObject g in selectionList)
                {
                    if (g.GetComponent<CubeScript>().dir == CubeScript.Direction.UP)
                    {
                        t += g.GetComponentInChildren<TextMesh>().text;
                    }
                }


            }
        }
	}

	public virtual void DestroyMiniGame()
	{
		MinigameSetup.RunningTask = false;

		Camera2DTracker.LockHorizontalCamera = false;
		Camera2DTracker.ResetCameraZoom(true);
		EraseAll();
		DesactivateCustomScripts();
		//UramaBehaviour.notificationType = TaskNotifycationType.None;
	}

	public int maxTentativas = 3;

	public virtual void AcceptChoice()
	{
		if (selectionList.Count > 0 && !minigameComplete)
		{
			finalTime = Time.time;

			correctAnswer = MiniGameResult();

			if (!correctAnswer && taskManager.ShouldRepeatTask() && numeroTentativa < maxTentativas)
			{
				UpdateRespostaList();

				while (selectionList.Count != 0)
				{
					CancelChoice();
				}

				if(CurrentTask.Reforco == "A" || CurrentTask.Reforco == "I" || CurrentTask.Reforco == "AI")
				{
					UramaBehaviour.Instance.Notify("N" + CurrentTask.Reforco);
				}
				numeroTentativa += 1;
				startTime = Time.time;
			}
			else
			{
				minigameComplete = true;
				foreach (GameObject item in comparisonList)
				{
					CubeScript cs = item.gameObject.GetComponent<CubeScript>();
					cs.enabled = false;
				}

				MinigameSetup.WasLastAnswerCorret = correctAnswer;
				MinigameSetup.WasLastTaskTraining = taskManager.GetCurrentTask().TaskRole == 0 ? false : true;

                Debug.Log("Reforco: " + CurrentTask.Reforco);

				if(CurrentTask.Reforco == "A" || CurrentTask.Reforco == "I" || CurrentTask.Reforco == "AI")
				{
					string t = correctAnswer? "Y" : "N";
					UramaBehaviour.Instance.Notify(t + CurrentTask.Reforco);
				}

				DestroyMiniGame();
				UpdateRespostaList();
				DoNextTask();
			}
		}
	}

	protected void UpdateRespostaList()
	{
		string escolhas = "";

		escolhas += taskManager.GetTextureById(CurrentTaskModel).name + " ";

		for (int i = 0; i < CurrentTask.Choices.Count; i++)
		{
			string str = taskManager.GetTextureById((int)CurrentTask.Choices[i]).name;
			escolhas += str + " ";
		}

		taskManager.RegisterTask(new TaskResponseModel(CurrentTaskType,
					 escolhas, taskManager.GetTextureById(CurrentTaskModel).name,
					 GetChoice(selectionList), numeroTentativa, finalTime - startTime, MiniGameResult(), nome, 0, System.DateTime.Now));
	}

	protected void DoNextTask()
	{
		taskManager.DoNextTask();
	}

	public abstract void AdjustCamera();

	public abstract void CancelChoice();

	public abstract void CreateCancelAcceptChoice();

	private void CreateBChoicesList()
	{
		int a = CurrentTask.Correct == 0 ? CurrentTask.Model : CurrentTask.Correct;
		choicesList.Add(taskManager.GetTextureById(a));
		
		for (int i = 0; i < CurrentTask.Choices.Count; i++)
		{
			int randomIndex = Random.Range(0, choicesList.Count + 1);
			choicesList.Insert(randomIndex, taskManager.GetTextureById((int)CurrentTask.Choices[i]));
		}
	}

	public virtual void CreateBChoice(Vector3 pos) 
	{
		for (int i = 0; i < choicesList.Count; i++)
		{
//			Debug.Log(choicesList[i]);
			comparisonList.Add(CreateCube((Texture2D)choicesList[i], pos, cubePrefab));
		}
	}

	private void CreateCChoicesList()
	{
		int a = CurrentTask.Correct == 0 ? CurrentTask.Model : CurrentTask.Correct;
		choicesList.Add(taskManager.GetTextureById(a).name.ToUpper());

		for (int i = 0; i < CurrentTask.Choices.Count; i++)
		{
			int randomIndex = Random.Range(0, choicesList.Count + 1);
			choicesList.Insert(randomIndex, taskManager.GetTextureById((int)CurrentTask.Choices[i]).name.ToUpper());
		}
	}
	
	public virtual void CreateCChoice()
	{
		for (int i = 0; i < choicesList.Count; i++)
		{
			comparisonList.Add(CreateText((string)choicesList[i], Vector3.zero, cubePrefab));
		}

	}

	#region D Choices
		public virtual void CreateDChoicesList ()
		{
				//int a = CurrentTask.Correct == 0 ? CurrentTask.Model : CurrentTask.Correct;
		}
		public virtual void CreateDChoice ()
		{
	}
	#endregion

	public virtual void CreateEChoicesList()
	{
		int a = CurrentTask.Correct == 0 ? CurrentTask.Model : CurrentTask.Correct;

		ArrayList wordSyllables = taskManager.GetSyllabesByWordId(a);

		for (int j = 0; j < wordSyllables.Count; j++)
		{
			int randomIndex = Random.Range(0, choicesList.Count + 1);
			choicesList.Insert(randomIndex, wordSyllables[j].ToString());
		}

		for (int i = 0; i < taskManager.GetCurrentTask().Choices.Count; i++)
		{
			wordSyllables = taskManager.GetSyllabesByWordId((int)CurrentTask.Choices[i]);
			
			for (int j = 0; j < wordSyllables.Count; j++)
			{
				int randomIndex = Random.Range(0, choicesList.Count + 1);
				choicesList.Insert(randomIndex, wordSyllables[j].ToString());
			}
		}
	}

	public virtual void CreateEChoice()
	{
		for (int i = 0; i < choicesList.Count; i++)
		{
			comparisonList.Add(CreateText((string)choicesList[i], Vector3.zero, cubePrefab));
		}
	}

	public virtual GameObject CreateText(string name, Vector3 pos, GameObject source)
	{
		source.GetComponent<Renderer>().enabled = false;
		
		GameObject inst = GameObject.Instantiate(source, pos, Quaternion.identity) as GameObject;
		//inst.transform.localScale *= cubeScale.x;
		inst.name = name;
		
		GameObject textObject = GameObject.Instantiate(textPrefab, pos, Quaternion.identity) as GameObject;
		TextMesh gText = textObject.GetComponent(typeof(TextMesh)) as TextMesh;
		gText.text = name;
		gText.GetComponent<Renderer>().material.color = Color.black;
		gText.fontSize = 100;
		
		Bounds textBounds = gText.GetComponent<Renderer>().bounds;
		
		while (textBounds.extents.x * 2 + 0.5f > inst.GetComponent<Renderer>().bounds.extents.x * 2)
		{
			gText.transform.localScale *= 1f - (0.01f);
            textBounds = gText.GetComponent<Renderer>().bounds;
		}
		textObject.transform.parent = inst.transform;
		textObject.transform.position = inst.transform.position
			+ new Vector3(-textBounds.extents.x * 2 / 2f, textBounds.size.y / 2f, 0f);
		
		CubeScript cs = inst.gameObject.AddComponent<CubeScript>();
		if (cs != null)
		{
			cs.CubeInfo = name;
			cs.MinDistance = 0.01f;
		}
		
		textObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/3D Text Shader");
		return inst;
	}


	public abstract void OrganizeChoices();

	public abstract string GetChoice(ArrayList list);

	public abstract void EraseAll();

	public bool MiniGameResult()
	{
		return taskManager.GetCurrentTask().Correct == 0? 
			taskManager.GetTextureById(CurrentTaskModel).name == GetChoice(selectionList).ToLower() : 
				taskManager.GetTextureById(CurrentTask.Correct).name == GetChoice(selectionList).ToLower() ;
	}

}

public enum MiniGameType
{
	Arrow,
	Cube,
	Fall,
	Platform
}