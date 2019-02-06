using System.Collections;
using UnityEngine;

//REVISION 126
public class platformMiniGame : BaseMiniGame
{
	/*private GameObject cancelCube = null;
	private GameObject okCube = null;
	private GameObject cubePrefab;
	private GameObject textPrefab;*/

	private Shader textShader;
	private Vector3 selectedPosition = Vector3.zero;

	private GameObject platforms;
	private FootScript fs;

	//private Vector2 cubeScale = new Vector2 (1f, 1f);

	// Public Variables

	public float upFactor = 4f;
	public float minDistance = 1f;

	private int platCount;

	//public Vector2 normalCubeScale = new Vector2 (1f, 1.1f);
	//public Vector2 syllableCubeScale = new Vector2 (0.5f, 0.55f);

	private CreatPlatform cP;

	public platformMiniGame()
		: base()
	{
		nome = "Platform";
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
					VerifyPlat(cs);
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
				VerifyPlat(cs);
			}
		}

		cs = cancelCube.transform.GetComponent<CubeScript>();
		if (cs)
		{
			if (cs.sendInfo)
			{

				cs.sendInfo = false;
				VerifyPlat(cs);
			}
		}
	}

	public override void LoadMiniGame()
	{
		textPrefab = Resources.Load("Prefabs/TextPrefab") as GameObject;
		cubePrefab = Resources.Load("Prefabs/CubePrefab") as GameObject;
		itensMidPoint = new Vector3(Camera.main.transform.position.x, -6f, 0);
		cP = new CreatPlatform();
		cP.LoadResources();
	}

	public override void ActivateCustomScripts()
	{
		fs = amaru.GetComponentInChildren<FootScript>();
		fs.enabled = true;
	}

	public override void DesactivateCustomScripts()
	{
		fs.enabled = false;
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
		OrganizePlatforms(comparisonList, 3f);
	}

	public void OrganizePlatforms(ArrayList objList, float space)
	{
		float prefabSize = 3f;
		Vector3 inital = new Vector3(-(((platCount + 2) * 6) / 2) + 9f + Camera.main.transform.position.x + 0.506177f + 0.01810479f, 3, 1);
		float xInitial = inital.x;
		float nextPos = prefabSize + space;

		for (int i = 0; i < objList.Count; i++)
		{
			GameObject item = objList[i] as GameObject;
			var position = new Vector3(xInitial + (i * nextPos), inital.y, inital.z);

			CubeScript cs = item.gameObject.GetComponent<CubeScript>();
			if (cs != null)
			{
				cs.StartPosition = position;
				item.transform.position = cs.StartPosition;
//				Debug.Log(position.z);
			}
		}
	}

	public void OrganizeCubes(ArrayList objList, Vector3 center, float space, bool initial)
	{
		var prefabSize = cubePrefab.GetComponent<Renderer>().bounds.extents.x * 2;
		//Debug.Log(prefabSize);
		var xIntial = center.x - ((((objList.Count - 1) * space) + (objList.Count * prefabSize)) / 2) + (prefabSize / 2);
		var nextPos = prefabSize + space;

		//Debug.Log("(objList.Count - 1) * space + objList.Count * prefabSize) = "+ ( (objList.Count - 1) * space + objList.Count * prefabSize).ToString());

		if (objList.Count > 0)
		{
			for (int i = 0; i < objList.Count; i++)
			{
				GameObject item = objList[i] as GameObject;
				var position = new Vector3(xIntial + (i * nextPos), center.y, 0);

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
            Debug.LogWarning("Deveria ter falado alguma coisa");
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
		//		var t = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f)) - Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
	}

	private void VerifyPlat(CubeScript cs)
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
        for (int i = 0; i < selectionList.Count; i++ )
            Debug.Log("Resposta: " + selectionList[i] );
        Debug.Log(" Quant.: " + selectionList.Count);
		//		var t = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0f)) - Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f));
	}

	#region Criação dos cubos

	public override void CreateCancelAcceptChoice()
	{
		Vector3 position;
		Transform border;
		CubeScript cs;

		position = new Vector3(-(((platCount + 2) * 6) / 2) + 3f + Camera.main.transform.position.x + 0.506177f + 0.01810479f, 3, 2);

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
			cs.FinalPosition = cs.StartPosition - Vector3.up * upFactor;
			cs.IsPingPong = true;
			cs.smooth = false;
		}

		position.x += 6 * (platCount + 1);
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
			cs.FinalPosition = cs.StartPosition - Vector3.up * upFactor;
			cs.IsPingPong = true;
			cs.smooth = false;
		}

		ArrayList newArrayList = comparisonList;
		newArrayList.Add(okCube);
		newArrayList.Add(cancelCube);

		foreach (PlatformScript ps in platforms.GetComponentsInChildren<PlatformScript>())
		{
			ps.SetArrayList(newArrayList);
		}
	}

	/*private GameObject CreateCube(Texture2D texture, Vector3 pos, GameObject source)
	{
		source.GetComponent<Renderer>().enabled = true;

		GameObject inst = GameObject.Instantiate(source, pos, Quaternion.identity) as GameObject;
		inst.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
		inst.GetComponent<Renderer>().material.mainTexture = texture;
		inst.transform.localScale = new Vector3(4, 3, 0.2f);

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

		inst.GetComponent<Collider>().enabled = false;

		return inst;
	}/*/

	private GameObject CreatePlatform(int nPlats, float offset, int maxX, int maxY)
	{
		System.Collections.Generic.List<PLATINFO> a = new System.Collections.Generic.List<PLATINFO>();

		for (int i = 0; i < nPlats; i++)
		{
			a.Add(new PLATINFO(false, new Vector2(i * 6, 5), 4));
		}

		GameObject returnObject = cP.CreatModule(a, Camera.main.transform.position.x + offset, maxX, maxY, true);

		return returnObject;
	}

	protected override GameObject CreateCube (Texture2D texture, Vector3 pos, GameObject source)
	{
		GameObject inst = base.CreateCube (texture, pos, source);
		inst.transform.localScale = new Vector3(inst.transform.localScale.x, inst.transform.localScale.y, 0.01f);
		return inst;
	}

	public override void CreateBChoice(Vector3 pos)
	{
		int count = CurrentTask.Choices.Count + 3;
		float xInit = -(((count) * 6) / 2) + 3f;

		platforms = CreatePlatform(count, xInit, 100, 100);

		base.CreateBChoice(new Vector3(Camera.main.transform.position.x + xInit + 6.52428f, 3, 0));

		//new Vector3(Camera.main.transform.position.x + xInit + 6.52428f, 3, 0)

		platCount = count - 2;
	}

	public override void CreateCChoice()
	{
		int count = CurrentTask.Choices.Count + 3;
		float xInit = -(((count) * 6) / 2) + 3f;

		platforms = CreatePlatform(count, xInit, 100, 100);

		base.CreateCChoice();

		platCount = count - 2;
	}

	public override void CreateDChoice()
	{
		platforms = CreatePlatform(CurrentTask.Choices.Count + 3, 0, 40, 40);

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

	public override void CreateEChoice()
	{
		int count = choicesList.Count;

		float xInit = -(((count + 2) * 6) / 2) + 3f;

		platforms = CreatePlatform(count + 2, xInit, 100, 100);

		base.CreateEChoice();

		platCount = count;
	}

	public override GameObject CreateText(string name, Vector3 pos, GameObject source)
	{
		source.GetComponent<Renderer>().enabled = false;

		GameObject inst = GameObject.Instantiate(source, pos, Quaternion.identity) as GameObject;
		//inst.transform.localScale *= cubeScale.x;
		inst.name = name;

		inst.transform.localScale = new Vector3(4, 3, 0.2f);

		inst.GetComponent<Collider>().enabled = false;
		//inst.collider.transform.localPosition = new Vector3(0, 0.8f, 0);

		GameObject textObject = GameObject.Instantiate(textPrefab, pos, Quaternion.identity) as GameObject;
		TextMesh gText = textObject.GetComponent(typeof(TextMesh)) as TextMesh;

		gText.text = name;
        gText.GetComponent<Renderer>().material.color = Color.black;
        gText.fontSize = 100;
        //gText.renderer.material.set

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

	#endregion Criação dos cubos

	public override void EraseAll()
	{
		EraseCompareList();
		DestroyCancelAcceptChoice();
		GameObject.Destroy(platforms);
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
		//Accept + Cancel
		//float buttons = cubePrefab.GetComponent<Renderer>().bounds.extents.x * 2f;

		float Total = ChoicesSize();

		var oSize = (Total / Camera.main.aspect) / 2f;

		//Debug.Log("oSise = " + oSize);

		if (oSize > Camera2DTracker.InicialOrthographicSize)
			Camera2DTracker.Zoom = oSize - Camera.main.orthographicSize;
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

		string modelname = taskManager.GetWordById(model);
//		Debug.Log(modelname);
		Texture tTexture = taskManager.GetTextureById(model);
//		Debug.Log(tTexture.name);

		for (int i = 0; i < list.Count; i++)
		{
			string w = taskManager.GetWordById((int)list[i]);

			if (CurrentTaskType[1].ToString().Equals("B") || CurrentTaskType[1].ToString().Equals("C"))
			{
				escolhas++;
			}
			else if (CurrentTaskType[1].ToString().Equals("E") || CurrentTaskType[1].ToString().Equals("D"))
			{
				string wordName = taskManager.GetWordById((int)list[i]);
				ArrayList syllables = taskManager.GetSyllabesByWordId((int)list[i]);
//				Debug.Log("Palavra " + wordName + " silabas " + syllables.Count);
				escolhas += syllables.Count;
			}
		}

		escolhas += 2;
//		Debug.Log("escolhas " + escolhas);

		float escolhasTamanho = escolhas * 4;

		float espaco = (escolhas - 1) * 2;

		return espaco + escolhasTamanho + 10f;
	}
}