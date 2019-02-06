using System.Collections;
using UnityEngine;

//REVISION 126
public class FallingMiniGame : BaseMiniGame
{
	private Vector3 selectedPosition = Vector3.zero;

	// Public Variables

	public float upFactor = 4f;
	public float minDistance = 1f;

	private HeadScript hS;

	public FallingMiniGame()
		: base()
	{
		nome = "Falling";
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
		hS.type = MiniGameType.Fall;
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
			cs.Velocity = 1.4f;
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
		var prefabSize = cubePrefab.GetComponent<Renderer>().bounds.extents.x * 2;
		var xIntial = center.x - ((((objList.Count - 1) * space) + (objList.Count * prefabSize)) / 2) + (prefabSize / 2);
		var nextPos = prefabSize + space;

		//		Debug.Log("(objList.Count - 1) * space + objList.Count * prefabSize) = "+ ( (objList.Count - 1) * space + objList.Count * prefabSize).ToString());

		if (objList.Count > 0)
		{
			for (int i = 0; i < objList.Count; i++)
			{
				GameObject item = objList[i] as GameObject;

				//Posição Inicial
				var position = new Vector3(0, 0, 0);

				if (BaseMiniGame.CurrentTaskType[1].ToString().Equals("B") || BaseMiniGame.CurrentTaskType[1].ToString().Equals("C"))
					position = new Vector3(xIntial + (i * nextPos), (Camera2DTracker.Zoom + 2), center.z);
				else if ((BaseMiniGame.CurrentTaskType[0].ToString().Equals("C") || BaseMiniGame.CurrentTaskType[0].ToString().Equals("B")) && BaseMiniGame.CurrentTaskType[1].ToString().Equals("E"))
					position = new Vector3(xIntial + (i * nextPos), (Camera2DTracker.Zoom + 10), center.z);
				else
					position = new Vector3(xIntial + (i * nextPos), (Camera2DTracker.Zoom + 8), center.z);

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
						if (BaseMiniGame.CurrentTaskType[0].ToString().Equals("A") && BaseMiniGame.CurrentTaskType[1].ToString().Equals("B"))
							cs.FinalPosition = position - new Vector3(0, (Camera2DTracker.Zoom - 10), 0);

						if (BaseMiniGame.CurrentTaskType[0].ToString().Equals("C") && BaseMiniGame.CurrentTaskType[1].ToString().Equals("E"))
							cs.FinalPosition = position - new Vector3(0, (Camera2DTracker.Zoom - 10), 0);
						else
							cs.FinalPosition = position - new Vector3(0, (Camera2DTracker.Zoom - 18), 0);

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

		cancelCube = CreateCube(Resources.Load("Textures/Misc/borracha") as Texture2D, position, cubePrefab);
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

	protected override GameObject CreateCube(Texture2D texture, Vector3 pos, GameObject source)
	{
		GameObject inst = base.CreateCube(texture,pos,source);

		CubeFall cf = inst.gameObject.AddComponent<CubeFall>();

		return inst;
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

	public override GameObject CreateText(string name, Vector3 pos, GameObject source)
	{
		GameObject inst = base.CreateText(name,pos,source);
		CubeFall cf = inst.gameObject.AddComponent<CubeFall>();
		return inst;
	}

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
		float height = Camera.main.orthographicSize * 2f;

		//tamanho da tela = (num_comp(n) + model(1))*espaco_entre(space_between) + accept(1) + cancel(1);
		float width = height * Camera.main.aspect;

		//Accept + Cancel
		float buttons = cubePrefab.GetComponent<Renderer>().bounds.extents.x * 4f;

		float Total = buttons + ChoicesSize();
		//Debug.Log (":" + Total);

		var oSize = (Total * Camera.main.aspect) / 2f;

		if (oSize > Camera2DTracker.InicialOrthographicSize)
			Camera2DTracker.Zoom = (oSize - Camera.main.orthographicSize) * 0.5f;
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
			else if (CurrentTaskType[1].ToString().Equals("D"))
			{
				string wordName = taskManager.GetWordById((int)list[i]);
				escolhas += wordName.Length;
			}
			else if (CurrentTaskType[1].ToString().Equals("E"))
			{
				ArrayList syllables = taskManager.GetSyllabesByWordId((int)list[i]);
				escolhas += syllables.Count;
			}
		}

		escolhas++;

		//Debug.Log ("::" + escolhas + " - " + cubePrefab.renderer.bounds.extents.x * 2);
		//(Escolhas + modelo) * tamanho_do_cubo
		float escolhasTamanho = escolhas * (cubePrefab.GetComponent<Renderer>().bounds.extents.x);

		//  Count(E + M )*spaceBetween
		float espaco = escolhas * spaceBetweenItens;

		//Debug.Log ("::" + (espaco + escolhasTamanho));
		return espaco + escolhasTamanho;
	}
}