using System.Collections;
using UnityEngine;

public class CreateBG : MonoBehaviour
{
	private Texture2D _mainTexture;
	public ArrayList _skys = new ArrayList();
	private WorldManager _wM = WorldManager.GetInstance();
	private float _scaleFactor = 25.6f;

	private void Start()
	{
		LoadResources();
	}

	private void LoadResources()
	{
		_mainTexture = _wM.CurrentWorld.SkyTop;
	}

	private void Update()
	{
		if (_skys.Count > 0)
		{
			if (IsOnCamera((_skys[_skys.Count - 1] as GameObject)) || IsCameraOverSky(_skys[_skys.Count - 1] as GameObject))
			{
				UpdateSky();
			}
			else
			{
				if (_skys.Count > 1)
				{
					var obj = (_skys[_skys.Count - 2] as GameObject);

					if (!IsOnCamera(obj))
					{
						var last = (_skys[_skys.Count - 1] as GameObject);
						_skys.Remove(last);
						Destroy(last);
					}
				}
			}
		}
		else if (IsOnCamera(this.gameObject))
		{
			UpdateSky();
		}
	}

	private void UpdateSky()
	{
		var initPos = Vector3.zero;
		if (_skys.Count > 0)
		{
			initPos = (_skys[_skys.Count - 1] as GameObject).transform.position;
		}
		else
		{
			initPos = this.transform.position;
		}

		var distance = Camera.main.WorldToScreenPoint(initPos);
		var rect = BoundsToScreenRect(this.GetComponent<Collider>().bounds);

		for (int i = 0; i < (distance.y / rect.height); i++)
		{
			var scale = new Vector2(_mainTexture.width / _scaleFactor, _mainTexture.height / _scaleFactor);
			var pos = new Vector3(initPos.x, initPos.y + (i + 1) * scale.y - 0.5f, 0);
			var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.transform.localScale = new Vector3(scale.x, scale.y, 0.01f);
			obj.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
			obj.GetComponent<Renderer>().material.mainTexture = _mainTexture;
			obj.transform.Rotate(0, 0, 180);
			obj.transform.position = pos;

			_skys.Add(obj);
			obj.transform.parent = this.transform;
			obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);
		}
	}

	private bool IsOnCamera(GameObject obj)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		return GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds);
		//		var pos = BoundsToScreenRect (obj.collider.bounds);
		//		return (pos.x + pos.width >= 0) && (pos.x <= Screen.width) && (pos.y >= 0) && (pos.y + pos.height <= Screen.height);
	}

	private bool IsCameraOverSky(GameObject tile)
	{
		var pos = BoundsToScreenRect(tile.GetComponent<Collider>().bounds);
		return (pos.x + pos.width >= 0) && (pos.x <= Screen.width) && (pos.y + pos.height > Camera.main.transform.position.y);
	}

	public Rect BoundsToScreenRect(Bounds bounds)
	{
		Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
		Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));

		return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
	}

	private void OnDestroy()
	{
		foreach (GameObject item in _skys)
		{
			Destroy(item);
		}
	}
}