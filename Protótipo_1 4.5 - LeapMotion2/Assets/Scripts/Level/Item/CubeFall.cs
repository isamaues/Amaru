using UnityEngine;

public class CubeFall : MonoBehaviour
{
	private CubeScript cs;
	private int vel;

	// Use this for initialization
	private void Start()
	{
		cs = GetComponent<CubeScript>();
		vel = Random.Range(2, 7);
	}

	// Update is called once per frame
	private void Update()
	{
		Invoke("Caindo", 2);//Chama a função CAINDO depois de 2 segundos de delay.
	}

	private void Caindo()
	{
		if (!cs.IsPingPong)
		{
			if (cs.dir == CubeScript.Direction.NONE)// Faz com que a escolha só se movimente quando está em estado de NONE.
				transform.position = new Vector3(transform.position.x, transform.position.y - vel * Time.deltaTime, transform.position.z);

			if (transform.position.y < -10)
			{
				vel = Random.Range(2, 7);
				transform.position = cs.StartPosition;
			}

			if (cs.dir == CubeScript.Direction.DOWN)
				cs.dir = CubeScript.Direction.NONE;
		}
	}
}