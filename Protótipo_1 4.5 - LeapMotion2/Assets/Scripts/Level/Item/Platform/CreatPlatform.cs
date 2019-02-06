using UnityEngine;

public class CreatPlatform
{
	public static int platformCont = 0;

	public GameObject _gearPrefab, _screwPrefab, _porcaPrefab;
	private GameObject _tilePrefab;

	private WorldManager _wM = WorldManager.GetInstance();

	//bool waitActive = false;
	//bool canSwitch = false;
	//public int tamX = 20;
	//public int tamY = 20;

	public void LoadResources()
	{
		_gearPrefab = (GameObject)Resources.Load("Prefabs/Engrenagem");
		_screwPrefab = (GameObject)Resources.Load("Prefabs/Parafuso");
		_porcaPrefab = (GameObject)Resources.Load("Prefabs/Porca");
		_tilePrefab = (GameObject)Resources.Load("Prefabs/LevelItem/Tile");

		_wM.CurrentWorld.LoadMaterials();
	}

	public GameObject CreatGear(GearType type, float posx, float posy)
	{
		switch (type)
		{
			case GearType.PorcaBronze:
				return (GameObject)GameObject.Instantiate(_porcaPrefab, new Vector3(posx, posy, 0), _screwPrefab.transform.rotation);

			case GearType.EngrenagemOuro:
				return (GameObject)GameObject.Instantiate(_gearPrefab, new Vector3(posx, posy, 0), _gearPrefab.transform.rotation);

			case GearType.ParafusoOuro:
				return (GameObject)GameObject.Instantiate(_screwPrefab, new Vector3(posx, posy, 0), _screwPrefab.transform.rotation);
		}
		return null;
	}

	public GameObject CreatModule(System.Collections.Generic.List<PLATINFO> platforms, float offset, int maxX, int maxY, bool isMinigame)
	{
		GameObject module = new GameObject("Module");

		module.transform.position = new Vector3(offset, 0, 0);

		PLATCREATINFO[,] matrix = new PLATCREATINFO[maxX, maxY];
		PLATCREATINFO[,] bigMatrix = new PLATCREATINFO[maxX + 2, maxY + 1];

		GameObject tiles;

		int i, j;

		for (i = 0; i < maxX; i++)
		{
			for (j = 0; j < maxY; j++)
			{
				matrix[i, j] = new PLATCREATINFO();
				matrix[i, j].originalPassavel = 0;
				matrix[i, j].passavel = 0;
			}
		}

		foreach (PLATINFO plat in platforms)
		{
			int c = 1;
			if (plat.rigid)
				c = 2;
			for (i = (int)plat.position.x; i < plat.position.x + plat.size; i++)
			{
				if (matrix[i, (int)plat.position.y].passavel != 2)
				{
					matrix[i, (int)plat.position.y].originalPassavel = c;
					matrix[i, (int)plat.position.y].passavel = c;
				}
			}
		}

		for (i = 0; i < maxX; i++)
		{
			bigMatrix[0, i] = new PLATCREATINFO();
			bigMatrix[0, i].originalPassavel = 0;
			bigMatrix[0, i].passavel = 0;
		}

		for (i = 1; i < maxY + 1; i++)
		{
			bigMatrix[i, 0] = new PLATCREATINFO();
			bigMatrix[i, 0].originalPassavel = 0;
			bigMatrix[i, 0].passavel = 0;

			bigMatrix[i, maxX] = new PLATCREATINFO();
			bigMatrix[i, maxX].originalPassavel = 0;
			bigMatrix[i, maxX].passavel = 0;
		}

		for (i = 1; i < maxX + 1; i++)
		{
			for (j = 0; j < maxY; j++)
			{
				bigMatrix[i, j] = matrix[i - 1, j];
			}
		}

		for (j = maxY - 1; j >= 0; j--)
		{
			for (i = 1; i < maxX + 1; i++)
			{
				PLATCREATINFO c = bigMatrix[i, j];
				PLATCREATINFO up = bigMatrix[i, j + 1];

				if (c.passavel != 0)
				{
					if (up.passavel == 2)
					{
						c.passavel = 2;
						c.topoCore = 2;
					}
					else
					{
						c.topoCore = 1;
					}
				}
				if (c.passavel == 0)
				{
					c.passavel = up.passavel;

					if (c.passavel != 0)
					{
						c.topoCore = 2;
					}
				}
			}
		}

		for (j = maxY - 1; j >= 0; j--)
		{
			for (i = 1; i < maxX + 1; i++)
			{
				PLATCREATINFO c = bigMatrix[i, j];
				PLATCREATINFO up = bigMatrix[i, j + 1];
				PLATCREATINFO left = bigMatrix[i - 1, j];
				PLATCREATINFO right = bigMatrix[i + 1, j];

				if (c.passavel != 0)
				{
					if (left.passavel != c.passavel && right.passavel != c.passavel)
						c.posicao = 4;
					if (left.passavel == c.passavel && right.passavel != c.passavel)
						c.posicao = 3;
					if (left.passavel == c.passavel && right.passavel == c.passavel)
						c.posicao = 2;
					if (left.passavel != c.passavel && right.passavel == c.passavel)
						c.posicao = 1;
					c.fundo = up.posicao;
				}

				bigMatrix[i, j] = c;
				if (bigMatrix[i, j].passavel != 0)
				{ // && bigMatrix[i,j].posicao!=0 && bigMatrix[i,j].topoCore!=0
					tiles = CreatTile(bigMatrix[i, j], new Vector2(i, j), offset, isMinigame);//+Camera.main.transform.position.x,j));
					tiles.transform.parent = module.transform;
				}
			}
		}

		return module;
	}

	private GameObject CreatTile(PLATCREATINFO tipe, Vector2 position, float offset, bool isMinigame)
	{
		string name = "Teste";
		float x, y;

		x = position.x + offset - 2;
		y = position.y;

		//GameObject prefab = null;
		GameObject collider = null;

		GameObject tile = (GameObject)GameObject.Instantiate(_tilePrefab, new Vector3(x, y, 2.55f), _tilePrefab.transform.rotation);

		if (tipe.passavel == 1)
		{
			if (tipe.topoCore == 1)
			{
				if (tipe.posicao == 1)
				{
					if (tipe.fundo == 0)
						tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PTL"];
					else if (tipe.fundo == 1)
						tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PTL2"];
					else if (tipe.fundo == 2)
						tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PTL1"];
					name = "CornerLeft";
				}
				else if (tipe.posicao == 2)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PTM"];
					name = "TopCenterTile";
				}
				else if (tipe.posicao == 3)
				{
					if (tipe.fundo == 0)
						tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PTR"];
					else if (tipe.fundo == 3)
						tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PTR2"];
					else if (tipe.fundo == 2)
						tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PTT1"];

					name = "CornerRight";
				}
				else if (tipe.posicao == 4)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PTS"];
					name = "SoloTop";
				}
				collider = GameObject.CreatePrimitive(PrimitiveType.Cube);
			}
			else if (tipe.topoCore == 2)
			{
				if (tipe.posicao == 1)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PBL"];
					name = "Left";
				}
				else if (tipe.posicao == 2)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PBM"];
					name = "Core";
				}
				else if (tipe.posicao == 3)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PBR"];
					name = "Right";
				}
				else if (tipe.posicao == 4)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["PBS"];
					name = "SoloCore";
				}
			}
		}
		else if (tipe.passavel == 2)
		{
			name = "Rigid";
			if (tipe.topoCore == 1)
			{
				if (tipe.posicao == 1)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["NTL"];
					name += "CornerLeft";
				}
				else if (tipe.posicao == 2)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["NTM"];
					name += "TopCenterTile";
				}
				else if (tipe.posicao == 3)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["NTR"];
					name += "CornerRight";
				}
				else if (tipe.posicao == 4)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["NTS"];
					name += "SoloTop";
				}
			}
			else if (tipe.topoCore == 2)
			{
				if (tipe.posicao == 1)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["NBL"];
					name += "Left";
				}
				else if (tipe.posicao == 2)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["NBM"];
					name += "Core";
				}
				else if (tipe.posicao == 3)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["NBR"];
					name += "Right";
				}
				else if (tipe.posicao == 4)
				{
					tile.GetComponent<Renderer>().material = _wM.CurrentWorld.TileMaterials["NBS"];
					name += "SoloCore";
				}
			}
			collider = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}

		tile.name = name;

		if (collider)
		{
			if (tipe.passavel == 1)
			{
				if (tipe.posicao == 1)
				{
					collider.transform.parent = tile.transform;
					collider.transform.localPosition = new Vector3(-0.1f, -0.45f, -20.5f);
					collider.transform.localScale = new Vector3(0.8f, 0.1f, 40);
				}
				else if (tipe.posicao == 2)
				{
					collider.transform.parent = tile.transform;
					collider.transform.localPosition = new Vector3(0, -0.45f, -20.5f);
					collider.transform.localScale = new Vector3(1f, 0.1f, 40);
				}
				else if (tipe.posicao == 3)
				{
					collider.transform.parent = tile.transform;
					collider.transform.localPosition = new Vector3(0.1f, -0.45f, -20.5f);
					collider.transform.localScale = new Vector3(0.8f, 0.1f, 40);
				}
				else if (tipe.posicao == 4)
				{
					collider.transform.parent = tile.transform;
					collider.transform.localPosition = new Vector3(0, -0.45f, -20.5f);
					collider.transform.localScale = new Vector3(0.7f, 0.1f, 40);
				}

				collider.name = "Collider";
				collider.tag = "Tile";
				collider.GetComponent<Renderer>().enabled = false;
				collider.GetComponent<Collider>().isTrigger = false;

				if (isMinigame)
				{
					//PlatformScript pS = collider.AddComponent<PlatformScript>();
					collider.AddComponent<PlatformScript>();
				}

				Tile tileScript = collider.AddComponent<Tile>();
				tileScript.blockBottom = false;
				tileScript.blockLeft = false;
				tileScript.blockRight = false;
				tileScript.blockTop = true;
			}
			if (tipe.passavel == 2)
			{
				collider.transform.parent = tile.transform;
				collider.transform.localPosition = new Vector3(0, 0, -20.5f);
				collider.transform.localScale = new Vector3(1, 1, 40);
				collider.name = "Collider";
				collider.tag = "Tile";
				collider.GetComponent<Renderer>().enabled = false;
				collider.GetComponent<Collider>().isTrigger = false;
			}
		}
		return tile;
	}
}

public class PLATINFO
{
	public bool rigid;
	public Vector2 position;
	public int size;

	public PLATINFO(bool isRigid, Vector2 platPosition, int platSize)
	{
		rigid = isRigid;
		position = platPosition;
		size = platSize;
	}
}

public class PLATCREATINFO
{
	public PLATINFO platInfo;
	public int passavel;
	public int originalPassavel;
	public int topoCore;
	public int posicao;
	public int fundo;
}