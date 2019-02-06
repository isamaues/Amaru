using UnityEngine;

public class Sun : MonoBehaviour
{
	public GameObject mainCamera;

	private void Update()
	{
		transform.position = new Vector3(mainCamera.transform.position.x - 10f, 25, 21);
	}
}