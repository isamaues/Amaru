using System.Collections;
using UnityEngine;

public class CreateFloor : MonoBehaviour
{
	private GameObject _model;
	private ArrayList _tiles = new ArrayList();

	//private static GameObject _floorHolder;
	private WorldManager _wM = WorldManager.GetInstance();

	private void Start()
	{
		LoadResources();
		//if (_floorHolder == null) {
		//	_floorHolder = new GameObject ();
		//	_floorHolder.name = "FloorHolder";
		//}
	}

	private void LoadResources()
	{
		_model = (GameObject)Resources.Load("Prefabs/LevelItem/Tile");
	}

	private void Update()
	{
		if (_tiles.Count > 0)
		{
			if (IsOnCamera((_tiles[_tiles.Count - 1] as GameObject)) || IsCameraUnderTile(_tiles[_tiles.Count - 1] as GameObject))
			{
				UpdateFloor();
			}
			else
			{
				if (_tiles.Count > 1)
				{
					var obj = (_tiles[_tiles.Count - 2] as GameObject);

					if (!IsOnCamera(obj))
					{
						var last = (_tiles[_tiles.Count - 1] as GameObject);
						_tiles.Remove(last);
						Destroy(last);
					}
				}
			}
		}
		else if (IsOnCamera(this.gameObject))
		{
			UpdateFloor();
		}
	}

	private void UpdateFloor()
	{
		if (_model != null)
		{
			var initPos = Vector3.zero;
			if (_tiles.Count > 0)
			{
				initPos = (_tiles[_tiles.Count - 1] as GameObject).transform.position;
			}
			else
			{
				initPos = this.transform.position;
			}

			var distance = Camera.main.WorldToScreenPoint(initPos);
			var rect = BoundsToScreenRect(this.GetComponent<Collider>().bounds);

			for (int i = 0; i < (distance.y / rect.height); i++)
			{
				var pos = new Vector3(initPos.x, initPos.y - (i + 1), 2.5f);
				var obj = Instantiate(_model, pos, this.transform.rotation) as GameObject;
				obj.transform.Rotate(0, 0, 180);
				//				obj.transform.GetComponent<Tile> ().enabled = false;
				_tiles.Add(obj);
				obj.transform.parent = this.transform;
				//obj.renderer.material = _wM.CurrentWorld.TileMaterials["PBM"];
				obj.GetComponent<Renderer>().material = GameObject.Find("GameManager").GetComponent<FloorManager>().pbm;
			}
		}
	}

	private bool IsOnCamera(GameObject obj)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		return GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds);
		//		var pos = BoundsToScreenRect (obj.collider.bounds);
		//		return (pos.x + pos.width >= 0) && (pos.x <= Screen.width) && (pos.y >= 0) && (pos.y + pos.height <= Screen.height);
	}

	private bool IsCameraUnderTile(GameObject tile)
	{
		var pos = BoundsToScreenRect(tile.GetComponent<Collider>().bounds);
		return (pos.x + pos.width >= 0) && (pos.x <= Screen.width) && (pos.y + pos.height < Camera.main.transform.position.y);
	}

	public Rect BoundsToScreenRect(Bounds bounds)
	{
		Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
		Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));

		return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
	}

	private void OnDestroy()
	{
		foreach (GameObject item in _tiles)
		{
			Destroy(item);
		}
	}
}