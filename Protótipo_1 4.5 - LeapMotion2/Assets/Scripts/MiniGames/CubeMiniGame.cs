using System.Collections;
using UnityEngine;

//REVISION 126
public class CubeMiniGame : BaseMiniGame
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

	//public Vector2 normalCubeScale = new Vector2 (1f, 1.1f);
	//public Vector2 syllableCubeScale = new Vector2 (0.5f, 0.55f);

	private HeadScript hS;

	public CubeMiniGame()
		: base()
	{
		nome = "Cube";
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
		/*
		Debug.DrawLine (cubePrefab.renderer.bounds.max, cubePrefab.renderer.bounds.min);

		Debug.DrawLine (new Vector3 (itensMidPoint.x - ((ChoicesSize () + (cubePrefab.renderer.bounds.extents.x * 2)) / 2), itensMidPoint.y, itensMidPoint.z),
			new Vector3 (itensMidPoint.x + ((ChoicesSize () + (cubePrefab.renderer.bounds.extents.x * 2)) / 2), itensMidPoint.y, itensMidPoint.z));
		Debug.DrawLine (new Vector3 (itensMidPoint.x, itensMidPoint.y + 1, itensMidPoint.z),
			new Vector3 (itensMidPoint.x + (ChoicesSize () + (cubePrefab.renderer.bounds.extents.x * 2)), itensMidPoint.y + 1, itensMidPoint.z), Color.blue);
		Debug.DrawLine (new Vector3 (itensMidPoint.x, itensMidPoint.y + 2, itensMidPoint.z),
			new Vector3 (itensMidPoint.x + 40.5f, itensMidPoint.y + 2, itensMidPoint.z), Color.yellow);

		//Debug.Log (">> " + (okCube.transform.position.x - cancelCube.transform.position.x));
		*/


	}

	public override void LoadMiniGame()
	{
		textPrefab = Resources.Load("Prefabs/TextPrefab") as GameObject;
		cubePrefab = Resources.Load("Prefabs/CubePrefab") as GameObject;
		itensMidPoint = new Vector3(Camera.main.transform.position.x, upFactor * 1.7f, 0);
	}

	public override void ActivateCustomScripts()
	{
		hS = base.amaru.GetComponentInChildren<HeadScript>();
		if (hS != null) hS.enabled = true;
		hS.type = MiniGameType.Cube;
	}

	public override void DesactivateCustomScripts()
	{
		hS.enabled = false;
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

	public override void CancelChoice()
	{
		if (selectionList.Count > 0)
		{
			GameObject item = selectionList[selectionList.Count - 1] as GameObject;
			CubeScript cs = item.gameObject.GetComponent<CubeScript>();
			cs.Animating = true;
			cs.dir = CubeScript.Direction.DOWN;
			selectionList.Remove(item);
			OrganizeCubes(selectionList, itensMidPoint + Vector3.up * upFactor, 0, false);
		}
	}

	public override void OrganizeChoices()
	{
		OrganizeCubes(comparisonList, itensMidPoint, spaceBetweenItens, true);
	}

	public void OrganizeCubes(ArrayList objList, Vector3 center, float space, bool initial)
	{
		//		Debug.Log(objList.Count);
		var prefabSize = cubePrefab.GetComponent<Renderer>().bounds.extents.x * 2;
		var xIntial = center.x - ((((objList.Count - 1) * space) + (objList.Count * prefabSize)) / 2) + (prefabSize / 2);
		var nextPos = prefabSize + space;

		//Debug.Log("(objList.Count - 1) * space + objList.Count * prefabSize) = "+ ( (objList.Count - 1) * space + objList.Count * prefabSize).ToString());

		if (objList.Count > 0)
		{
			for (int i = 0; i < objList.Count; i++)
			{
				GameObject item = objList[i] as GameObject;
				var position = new Vector3(xIntial + (i * nextPos), center.y, center.z);

				CubeScript cs = item.gameObject.GetComponent<CubeScript>();
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
				selectionList.Add(cs.gameObject);
			OrganizeCubes(selectionList, itensMidPoint + Vector3.up * upFactor, 0, false);
		}
		var t = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0f)) - Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
	}

	#region Criação dos cubos

	public override void CreateCancelAcceptChoice()
	{
		Vector3 position;
		Transform border;
		CubeScript cs;
		var prefabSize = cubePrefab.GetComponent<Renderer>().bounds.extents.x * 2;

		//position = new Vector3 ( itensMidPoint.x - ((( ChoicesSize () )/ 2) + (cubePrefab.renderer.bounds.extents.x * 2) + spaceBetweenItens)  , itensMidPoint.y, itensMidPoint.z);
		position = new Vector3(itensMidPoint.x - ((((comparisonList.Count - 1) * spaceBetweenItens) + (comparisonList.Count * prefabSize)) / 2) - (prefabSize / 2) - spaceBetweenItens, itensMidPoint.y, itensMidPoint.z);

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
			cs.Velocity = 12f;
			cs.StartPosition = cancelCube.transform.position;
			cs.FinalPosition = cs.StartPosition + Vector3.up * upFactor;
			cs.IsPingPong = true;
			cs.smooth = false;
		}

		//position = new Vector3 (itensMidPoint.x + ((( ChoicesSize () )/ 2) + (cubePrefab.renderer.bounds.extents.x * 2) + spaceBetweenItens), itensMidPoint.y, itensMidPoint.z);
		position = new Vector3(itensMidPoint.x + ((((comparisonList.Count - 1) * spaceBetweenItens) + (comparisonList.Count * prefabSize)) / 2) + (prefabSize / 2) + spaceBetweenItens, itensMidPoint.y, itensMidPoint.z);

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
			cs.FinalPosition = cs.StartPosition + Vector3.up * upFactor;
			cs.IsPingPong = true;
			cs.smooth = false;
		}
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

	/*public override void CreateBChoice()
	{
		/*comparisonList.Add(CreateCube(CurrentTask.Model, Vector3.zero, cubePrefab));

		for (int i = 0; i < CurrentTask.Choices.Count; i++)
		{
			Texture2D texture = taskManager.GetTextureById((int)CurrentTask.Choices[i]);
			int randomIndex = Random.Range(0, comparisonList.Count + 1);
			comparisonList.Insert(randomIndex, CreateCube(texture, Vector3.zero, cubePrefab));
		}


		for (int i = 0; i < selectionList.Count; i++)
		{
			comparisonList.Add(CreateCube((Texture2D)selectionList[0], Vector3.zero, cubePrefab));
		}
	}*/

	/*public override void CreateCChoice()
	{
		comparisonList.Add(CreateText(taskManager.GetTextureById(CurrentTask.Model).name, Vector3.zero, cubePrefab));

		for (int i = 0; i < CurrentTask.Choices.Count; i++)
		{
			string str = taskManager.GetTextureById((int)CurrentTask.Choices[i]).name;
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
//		Debug.Log(name);
		gText.text = name;
		gText.GetComponent<Renderer>().material.color = Color.blue;
		gText.fontSize = 20;

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

	#endregion Criação dos cubos

	public override void EraseAll()
	{
		EraseCompareList();
		DestroyCancelAcceptChoice();
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

	public override void AdjustCamera()
	{
		float Total = ChoicesSize();

		//Debug.Log("TOtal = " + Total);

		//Debug.Log(Camera.main.aspect);

		var oSize = (Total / Camera.main.aspect) / 2f;

		//Debug.Log("ozise = " + oSize);

		if (oSize > Camera2DTracker.InicialOrthographicSize)
			Camera2DTracker.Zoom = (oSize - Camera.main.orthographicSize);
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
				Debug.Log("Palavra " + wordName + " tamanho " + wordName.Length);
				escolhas += wordName.Length;
			}*/
			else if (CurrentTaskType[1].ToString().Equals("E") || CurrentTaskType[1].ToString().Equals("D"))
			{
				ArrayList syllables = taskManager.GetSyllabesByWordId((int)list[i]);
				//	Debug.Log("Palavra " + list[1] + " silabas " + syllables.Count);
				escolhas += syllables.Count;
			}
		}

		escolhas += 2;
		//	Debug.Log("escolhas " + escolhas);

		//Debug.Log(escolhas * (cubePrefab.GetComponent<Renderer>().bounds.extents.x));
		//(Escolhas + modelo) * tamanho_do_cubo

		float escolhasTamanho = escolhas * 4;

		//  Count(E + M )*spaceBetween
		float espaco = (escolhas - 1) * spaceBetweenItens;

		//Debug.Log("espaco " + espaco + " escolhastamanho " + escolhasTamanho);
		return espaco + escolhasTamanho + 5f;
	}
}