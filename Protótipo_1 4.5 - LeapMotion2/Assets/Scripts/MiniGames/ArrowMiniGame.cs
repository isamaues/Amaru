using System.Collections;
using UnityEngine;

public class ArrowMiniGame : BaseMiniGame
{
	/*private GameObject cancelCube = null;
	private GameObject okCube = null;
	private GameObject cubePrefab;
	private GameObject textPrefab;*/

	private Vector3 selectedPosition = Vector3.zero;
	//private Vector2 cubeScale = new Vector2 (1f, 1f);

	// Public Variables

	public float upFactor = 4f;
	public float minDistance = 1f;
	private float oSize, distanceFromMid;
	private bool cameraUpdating;

    private ShootScript ss;
	private GameObject mira;
	private PlayerMovement pm;
	private JetPackScript jps;

	public ArrowMiniGame()
		: base()
	{
		nome = "Arrow";
		//		UserManager.IsInArrowMinigame = true;
		cameraUpdating = true;
	}

	public override void Update()
	{
		base.Update();

		CubeScript cs;
		foreach (GameObject item in comparisonList)
		{
			cs = item.transform.GetComponent<CubeScript>();
			if (cs)
			{
				if (cs.sendInfo)
				{
					cs.sendInfo = false;
					VerifyCube(cs);
					break;
				}
			}
		}

		cs = okCube.transform.GetComponent<CubeScript>();
		if (cs)
		{
			if (cs.sendInfo)
			{
				cs.sendInfo = false;
				VerifyCube(cs);
			}
		}

		cs = cancelCube.transform.GetComponent<CubeScript>();
		if (cs)
		{
			if (cs.sendInfo)
			{
				cs.sendInfo = false;
				VerifyCube(cs);
			}
		}
		//-----------------------------------------------------------------------
		if (cameraUpdating)
		{
		}
		//-----------------------------------------------------------------------
	}

	private void VerifyCube(CubeScript cs)
	{
		if (cs.CubeInfo.Equals("seta"))
		{
			AcceptChoice();
		}
		else if (cs.CubeInfo.Equals("borracha"))
		{
			CancelChoice();
        }
        else if (cs.CubeInfo.Equals("autofalante"))
        {
            UramaBehaviour.Instance.StartPlaySoundModel();
        }
		else
		{
			if (!CurrentTaskType.Contains("D") && !CurrentTaskType.Contains("E"))
			{
				CancelChoice();
			}
			if (!selectionList.Contains(cs.gameObject))
			{
				selectionList.Add(cs.gameObject);
				cs.gameObject.GetComponent<Collider>().enabled = false;
			}

			float leftFactor = oSize * Camera.main.aspect - cubePrefab.GetComponent<Renderer>().bounds.extents.x * 2;
			OrganizeCubes(selectionList, itensMidPoint + Vector3.left * leftFactor, 0.5f, false);
		}
		var t = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0f)) - Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
	}

	public override void LoadMiniGame()
	{
		textPrefab = Resources.Load("Prefabs/TextPrefab") as GameObject;
		cubePrefab = Resources.Load("Prefabs/CubePrefab") as GameObject;
		itensMidPoint = new Vector3(Camera.main.transform.position.x, upFactor * 1.7f, 0);
	}

	/*public override void CreateBChoice()
	{
		comparisonList.Add(CreateCube(taskManager.GetTextureById(CurrentTask.Model), Vector3.zero, cubePrefab));

		for (int i = 0; i < CurrentTask.Choices.Count; i++)
		{
			Texture2D texture = taskManager.GetTextureById((int)CurrentTask.Choices[i]);
			int randomIndex = Random.Range(0, comparisonList.Count + 1);
			comparisonList.Insert(randomIndex, CreateCube(texture, Vector3.zero, cubePrefab));
		}
	}*/

	/*
	public override void CreateCChoice()
	{
		comparisonList.Add(CreateText(taskManager.GetTextureById(CurrentTask.Model).name, Vector3.zero, cubePrefab));

		for (int i = 0; i < CurrentTask.Choices.Count; i++)
		{
			string str = taskManager.GetWordById((int)CurrentTask.Choices[i]);
			int randomIndex = Random.Range(0, comparisonList.Count + 1);
			comparisonList.Insert(randomIndex, CreateText(str, Vector3.zero, cubePrefab));
		}
	}*/

	public override void CreateDChoice()
	{
		string str = taskManager.GetTextureById(CurrentTask.Model).name;
		for (int j = 0; j < str.Length; j++)
		{
			comparisonList.Add(CreateText(str[j].ToString(), Vector3.zero, cubePrefab));
		}

		for (int i = 0; i < CurrentTask.Choices.Count; i++)
		{
			str = taskManager.GetTextureById((int)CurrentTask.Choices[i]).name;
			for (int j = 0; j < str.Length; j++)
			{
				int randomIndex = Random.Range(0, comparisonList.Count + 1);
				comparisonList.Insert(randomIndex, CreateText(str[j].ToString(), Vector3.zero, cubePrefab));
			}
		}
	}

	/*public override GameObject CreateText(string name, Vector3 pos, GameObject source)
	{
		source.GetComponent<Renderer>().enabled = false;

		GameObject inst = GameObject.Instantiate(source, pos, Quaternion.identity) as GameObject;
		//inst.transform.localScale *= cubeScale.x;
		inst.name = name;

		GameObject textObject = GameObject.Instantiate(textPrefab, pos, Quaternion.identity) as GameObject;
		TextMesh gText = textObject.GetComponent(typeof(TextMesh)) as TextMesh;
		gText.text = name;
		gText.GetComponent<Renderer>().material.color = Color.blue;

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
	}*/

	public override void OrganizeChoices()
	{
		//OrganizeCubes(comparisonList, itensMidPoint, spaceBetweenItens, true);
		//OrganizeCubes(comparisonList, new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0, 0)).x,  itensMidPoint.y, itensMidPoint.z), spaceBetweenItens, true);
		//float poxitionX =
		//-----------------------------------------------------------------------------------------------
		// ORGANIZA OS CUBOS A DIREITA DA TELA
		float height = oSize * 2;

		float width = height * Camera.main.aspect;

		distanceFromMid = (itensMidPoint.x + width / 2) - cubePrefab.GetComponent<Renderer>().bounds.extents.x * 3;

		Vector3 vetor = new Vector3(distanceFromMid, itensMidPoint.y, itensMidPoint.z);
		OrganizeCubes(comparisonList, vetor, spaceBetweenItens, true);
		//-----------------------------------------------------------------------------------------------
	}

	private void OrganizeCubes(ArrayList objList, Vector3 center, float space, bool initial)
	{
		var prefabXSize = cubePrefab.GetComponent<Renderer>().bounds.extents.x * 2;

		var prefabYSize = cubePrefab.GetComponent<Renderer>().bounds.extents.y * 2;

		var xInitial = center.x - ((((objList.Count - 1) * space) + (objList.Count * prefabXSize)) / 2) + (prefabXSize / 2);

		//Começa em baixo
		var yInitial = 1 + prefabYSize * 2;
		//Começa em Cima

		//var yInitial = 0 + ((((objList.Count - 1) * space) + (objList.Count * prefabYSize))) + (prefabYSize / 2);

		var nextXPos = prefabXSize + space;
		var nextYPos = prefabYSize + space;

		yInitial += (objList.Count - 1) * nextYPos;

		if (objList.Count > 0)
		{
			for (int i = 0; i < objList.Count; i++)
			{
				GameObject item = objList[i] as GameObject;
				//Adiciona o proximo em cima do anterior
				var position = new Vector3(center.x, yInitial - (i * nextYPos), center.z);
				CubeScript cs = item.gameObject.GetComponent<CubeScript>();
				//				Debug.Log (position.ToString());
				if (cs != null)
				{
					if (initial)
					{
						cs.StartPosition = position;
						item.transform.position = cs.StartPosition;
					}
					else
					{
						cs.FinalPosition = position;
						cs.Animating = true;
						if (cs.dir == CubeScript.Direction.NONE)
						{
							cs.dir = CubeScript.Direction.SORT;
						}
					}
				}
			}
		}
	}

	public override void AdjustCamera()
	{
        float buttons = cubePrefab.GetComponent<Renderer>().bounds.extents.y * 4f;

		float Total = 0;
		float alturaDoChao;

		//Debug.Log ("Aspect Ratio: " + Camera.main.aspect);
		if (Camera.main.aspect >= 1.25 && Camera.main.aspect <= 1.51) //Tela Padrao (5:4, 4:3, 3:2)
		{
			alturaDoChao = (ChoicesNumber() * 1.25f) < 7f ? 7f : (ChoicesNumber() * 1.5f);
			Total = ChoicesSize() + alturaDoChao + buttons;
			//Debug.Log ("Tela Padrao");
		}
		else if ((Camera.main.aspect >= 1.59 && Camera.main.aspect < 1.8)  || Camera.main.aspect >= 1.8)//Tela Panoramica (16:9, 16:10) ou Free Aspect
			 {
				 alturaDoChao = (ChoicesNumber()) < 7f ? 7f : (ChoicesNumber()/0.8f);
				 Total = (ChoicesSize() + alturaDoChao)/1.25f + buttons;
				 //Debug.Log ("Widescreen");
			 }

        oSize = (Total * Camera.main.aspect) / 2f;

		Vector3 a = Camera.main.transform.position;

		Camera.main.transform.position = new Vector3(a.x + oSize, a.y, a.z);

        if (oSize > Camera2DTracker.InicialOrthographicSize)
        {
            Camera2DTracker.Zoom = oSize - Camera.main.orthographicSize;
        }
        else
            Camera2DTracker.Zoom = 5;
        
	}

	private float ChoicesSize()
	{
		int escolhas = 0;

		ArrayList list = new ArrayList();
		list.AddRange(CurrentTask.Choices);
		int model = CurrentTask.Model;//necessário para conversão short to int
		list.Add(model);
		for (int i = 0; i < list.Count; i++)
		{
			if (CurrentTaskType[1].ToString().Equals("B") || CurrentTaskType[1].ToString().Equals("C"))
			{
				escolhas++;
			}
			/*else if (CurrentTaskTypeName[1].ToString().Equals("D"))
			{
				string wordName = taskManager.GetWordById((int)list[i]);
				escolhas += wordName.Length;
			}*/
			else if (CurrentTaskType[1].ToString().Equals("E") || CurrentTaskType[1].ToString().Equals("D"))
			{
				ArrayList syllables = taskManager.GetSyllabesByWordId((int)list[i]);
				escolhas += syllables.Count;
			}
		}

		//escolhas++;

		//Debug.Log ("::" + escolhas + " - " + cubePrefab.renderer.bounds.extents.x * 2);
		//(Escolhas + modelo) * tamanho_do_cubo
		float escolhasTamanho = escolhas * (cubePrefab.GetComponent<Renderer>().bounds.extents.y);

		//  Count(E + M )*spaceBetween
		float espaco = (escolhas - 1) * spaceBetweenItens;

		//Debug.Log ("::" + (espaco + escolhasTamanho));
        return escolhasTamanho + espaco;
	}

	// METODO NOVO
	private int ChoicesNumber()
	{
		int escolhas = 0;

		ArrayList list = new ArrayList();
		list.AddRange(CurrentTask.Choices);
		int model = CurrentTask.Model;
		list.Add(model);
		for (int i = 0; i < list.Count; i++)
		{
			if (CurrentTaskType[1].ToString().Equals("B") || CurrentTaskType[1].ToString().Equals("C"))
			{
				escolhas++;
			}
			/*else if (CurrentTaskTypeName[1].ToString().Equals("D"))
			{
				string wordName = taskManager.GetWordById((int)list[i]);
				escolhas += wordName.Length;
			}*/
			else if (CurrentTaskType[1].ToString().Equals("E") || CurrentTaskType[1].ToString().Equals("D"))
			{
				ArrayList syllables = taskManager.GetSyllabesByWordId((int)list[i]);
				escolhas += syllables.Count;
			}
		}
		return escolhas;
	}

	public override void CreateCancelAcceptChoice()
	{
		Vector3 position;
		Transform border;
		CubeScript cs;
		float positiondndn = oSize;
		float choicesSize = comparisonList.Count * ((cubePrefab.GetComponent<Renderer>().bounds.extents.y * 2) + spaceBetweenItens);
		position = new Vector3(distanceFromMid, itensMidPoint.y + choicesSize/* + cubePrefab.renderer.bounds.extents.y *2 + spaceBetweenItens*/, itensMidPoint.z);

		//position = new Vector3 (itensMidPoint.x - ((( ChoicesSize () )/ 2) + (cubePrefab.renderer.bounds.extents.x * 2) + spaceBetweenItens), itensMidPoint.y, itensMidPoint.z);

        if (CurrentTaskType[0].ToString().Equals("A"))
        {
            cancelCube = CreateCube(Resources.Load("Textures/Misc/autofalante") as Texture2D, position, cubePrefab);
        }
        else
        {
            cancelCube = CreateCube(Resources.Load("Textures/Misc/borracha") as Texture2D, position, cubePrefab);
        }

		border = cancelCube.transform.FindChild("Border") as Transform;
		if (border)
		{
			border.GetComponent<Renderer>().enabled = false;
		}
		cs = cancelCube.gameObject.GetComponent<CubeScript>();
		if (cs != null)
		{
			cs.StartPosition = cancelCube.transform.position;
			cs.Velocity = 12f;
			cs.FinalPosition = cs.StartPosition + Vector3.right;
			cs.IsPingPong = true;
			cs.smooth = false;
		}

		position = new Vector3(distanceFromMid, itensMidPoint.y - cubePrefab.GetComponent<Renderer>().bounds.extents.y * 2 - spaceBetweenItens, itensMidPoint.z);

		//position = new Vector3 (itensMidPoint.x + ((( ChoicesSize () )/ 2) + (cubePrefab.renderer.bounds.extents.x * 2) + spaceBetweenItens), itensMidPoint.y, itensMidPoint.z);
		okCube = CreateCube(Resources.Load("Textures/Misc/seta") as Texture2D, position, cubePrefab);
		border = okCube.transform.FindChild("Border") as Transform;
		if (border)
		{
			border.GetComponent<Renderer>().enabled = false;
		}
		cs = okCube.gameObject.GetComponent<CubeScript>();
		if (cs != null)
		{
			cs.StartPosition = okCube.transform.position;
			cs.Velocity = 12f;
			cs.FinalPosition = cs.StartPosition + Vector3.right;
			cs.IsPingPong = true;
			cs.smooth = false;
		}
	}

	public override void ActivateCustomScripts()
	{
		ss = base.amaru.GetComponent<ShootScript>();
		ss.enabled = true;
		mira = GameObject.Find("Mira");
		mira.GetComponent<Renderer>().enabled = true;
		pm = amaru.GetComponent<PlayerMovement>();
		pm.IsUpDownMovement = true;
		pm.Animator.currentState = PlayerAnimation.State.Fly;

		jps = base.amaru.GetComponent<JetPackScript>();
		jps.EnableJetPack();
	}

	public override void DesactivateCustomScripts()
	{
		ss.enabled = false;
		mira.GetComponent<Renderer>().enabled = false;
		pm.IsUpDownMovement = false;
		jps.DisableJetPack();
	}

	public override void CancelChoice()
	{
		if (selectionList.Count > 0)
		{
			GameObject item = selectionList[selectionList.Count - 1] as GameObject;
			CubeScript cs = item.gameObject.GetComponent<CubeScript>();
			cs.Animating = true;
			cs.dir = CubeScript.Direction.DOWN;
			selectionList.Remove(item);
			float leftFactor = oSize * Camera.main.aspect - cubePrefab.GetComponent<Renderer>().bounds.extents.x * 2;
			OrganizeCubes(selectionList, itensMidPoint + Vector3.left * leftFactor, 0.5f, false);
			item.GetComponent<Collider>().enabled = true;
		}
	}

	public override void EraseAll()
	{
		EraseCompareList();
		DestroyCancelAcceptChoice();
		//		UserManager.IsInArrowMinigame = false;
	}

	public void EraseCompareList()
	{
		foreach (GameObject item in comparisonList)
		{
			item.gameObject.GetComponent<CubeScript>().enabled = false;
			item.GetComponent<Collider>().enabled = false;
			GameObject.Destroy(item);
		}
		comparisonList = new ArrayList();
	}

	public void DestroyCancelAcceptChoice()
	{
		GameObject.Destroy(okCube);
		GameObject.Destroy(cancelCube);
	}

	public override string GetChoice(ArrayList list)
	{
		string s = "";
		foreach (GameObject item in list)
		{
			CubeScript cs = item.gameObject.GetComponent<CubeScript>();
			if (cs != null)
			{
				s += cs.CubeInfo;
			}
		}
		return s;
	}

	/*private GameObject CreateCube(Texture2D texture, Vector3 pos, GameObject source)
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

		return inst;
	}*/

	public override void BuildMiniGame()
	{
		AdjustCamera();
		CreateTask();
		CreateCancelAcceptChoice();
		SetAmaru();
		ActivateCustomScripts();
	}
}