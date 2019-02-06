using UnityEngine;

public class WorldObject : MonoBehaviour
{
	public World world;
	public string worldName;

	// Use this for initialization
	private void Start()
	{
		switch (worldName)
		{
			case "Fazenda":
				world = World.fazenda;
				break;

			case "Floresta":
				world = World.floresta;
				break;

			case "Cidade":
				world = World.cidade;
				break;

			case "Industria":
				world = World.industria;
				break;

			case "Praia":
				world = World.praia;
				break;

			default:
				world = World.fazenda;
				break;
		}
	}

	// Update is called once per frame
	private void Update()
	{
	}

	private void OnTriggerEnter(Collider ohter)
	{
//		Debug.Log("!@#!@#");
		GameObject.Find("Background").GetComponent<WorldLevel>().ChangeLevel();
	}
}