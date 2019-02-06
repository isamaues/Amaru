using UnityEngine;

public class Kids : MonoBehaviour
{
	public bool destroyable = false;

	private void OnEnable()
	{
		if (destroyable)
		{
			foreach (Renderer r in GetComponentsInChildren<Renderer>())
				r.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
		}
	}

	private void Update()
	{
		if (destroyable && !IsOnCamera(this.gameObject))
		{
			Destroy(this.gameObject);
		}
	}

	private bool IsOnCamera(GameObject obj)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		return GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds);
	}
}