using System;
using System.Collections;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
	public int floorBlocks = 0;
	private ArrayList floorList;
	public Vector3 initialPosition = Vector3.zero;

	private GameObject _floorModel;

	private GameObject _tilePrefab;
	private WorldManager _wM;

	public Material pbm, ptm;

	private void Awake()
	{
		_wM = WorldManager.GetInstance();
		LoadResources();

		if (_floorModel == null)
			throw new Exception("Modelo do piso nulo");

		floorList = new ArrayList();
	}

	private void LoadResources()
	{
		_tilePrefab = (GameObject)Resources.Load("Prefabs/LevelItem/Tile");
		_floorModel = (GameObject)Resources.Load("Prefabs/LevelItem/bloco");

		_wM.CurrentWorld.LoadMaterials();

		pbm = _wM.CurrentWorld.TileMaterials["PBM"];
		ptm = _wM.CurrentWorld.TileMaterials["PTM"];
	}

	private void Update()
	{
		UpdateFloor();
	}

	private GameObject InstantiateFloor(Vector3 position)
	{
		GameObject block = new GameObject("block");
		block = (GameObject)Instantiate(_floorModel, new Vector3(0, position.y, position.z), _floorModel.transform.rotation);

		foreach (Transform a in block.transform)
		{
			foreach (Transform c in a)
			{
				if (c.tag == "Texture")
				{
					c.GetComponent<Renderer>().material = ptm;
				}
			}
		}
		return block;
	}

	private void UpdateFloor()
	{
		if (floorList.Count < 2)
		{
			floorList = new ArrayList();

			GameObject temp = InstantiateFloor(new Vector3(0, initialPosition.y, initialPosition.z));

			Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
			Vector3 extent = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));

			floorBlocks = (int)((extent.x - origin.x) / temp.GetComponent<Collider>().bounds.size.x) + 2;

			var initialBlockPosition = new Vector3(Camera.main.transform.position.x - (floorBlocks / 2f) * temp.GetComponent<Collider>().bounds.size.x, initialPosition.y, initialPosition.z);

			temp.transform.position = initialBlockPosition;

			floorList.Add(temp);

			for (int i = 0; i < floorBlocks; i++)
			{
				Vector3 pos = (floorList[i] as GameObject).transform.position + new Vector3((floorList[i] as GameObject).GetComponent<Collider>().bounds.size.x, initialPosition.y, initialPosition.z);

				CreateFloor(pos, i + 1);
			}
		}
		else
		{
			if (Camera2DTracker.IsOnCamera(floorList[0] as GameObject, true))
			{
				Vector3 pos = (floorList[0] as GameObject).transform.position - new Vector3((floorList[0] as GameObject).GetComponent<Collider>().bounds.size.x, initialPosition.y, initialPosition.z);
				CreateFloor(pos, 0);
			}
			else if (!Camera2DTracker.IsOnCamera(floorList[1] as GameObject, true))
			{
				DestroyFloor(floorList[0] as GameObject);
			}

			if (Camera2DTracker.IsOnCamera(floorList[floorList.Count - 1] as GameObject, true))
			{
				Vector3 pos = (floorList[floorList.Count - 1] as GameObject).transform.position + new Vector3((floorList[0] as GameObject).GetComponent<Collider>().bounds.size.x, initialPosition.y, initialPosition.z);
				CreateFloor(pos, floorList.Count);
			}
			else if (!Camera2DTracker.IsOnCamera(floorList[floorList.Count - 2] as GameObject, true))
			{
				DestroyFloor(floorList[floorList.Count - 1] as GameObject);
			}
		}
	}

	private void DestroyFloor(GameObject toDestroy)
	{
		floorList.Remove(toDestroy);
		Destroy(toDestroy);
	}

	private void CreateFloor(Vector3 position, int index)
	{
		GameObject temp = (GameObject)Instantiate(_floorModel, position, _floorModel.transform.rotation);
		foreach (Transform a in temp.transform)
		{
			foreach (Transform c in a)
			{
				if (c.tag == "Texture")
				{
					c.GetComponent<Renderer>().material = ptm;
				}
			}
		}

		floorList.Insert(index, temp);
	}
}