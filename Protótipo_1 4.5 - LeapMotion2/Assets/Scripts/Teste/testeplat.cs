using UnityEngine;

public class testeplat : MonoBehaviour
{
	private CreatPlatform cP;

	public System.Collections.Generic.List<PLATINFO> a = new System.Collections.Generic.List<PLATINFO>();

	private void Start()
	{
		cP = new CreatPlatform();
		//
		//GameObject _tilePrefab = Resources.Load ("Prefabs/LevelItem/Tile") as GameObject;
		PLATINFO b = new PLATINFO(false, new Vector2(0, 5), 3);
		a.Add(b);

		PLATINFO c = new PLATINFO(false, new Vector2(5, 5), 3);
		a.Add(c);

		PLATINFO d = new PLATINFO(false, new Vector2(10, 5), 3);
		a.Add(d);

		PLATINFO e = new PLATINFO(false, new Vector2(15, 5), 3);
		a.Add(e);

		PLATINFO f = new PLATINFO(false, new Vector2(20, 5), 3);
		a.Add(f);

		cP.LoadResources();
		cP.CreatModule(a, -10, 40, 40, false);

		//tile = (GameObject)GameObject.Instantiate(_tilePrefab,new Vector3(3,3,2.55f),_tilePrefab.transform.rotation);
	}
}