using UnityEngine;

public class Cloud : MonoBehaviour
{
	public float maxSpeed = 4f;
	public float minSpeed = 1f;

	public Vector3 cloudDirection;
	public int cloudScreenOffset = 100;

	private float speed;

	public void SetZ(float z)
	{
		transform.position = Vector3.forward * z;
	}

	private void Start()
	{
		this.speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
	}

	private void Update()
	{
		transform.position += cloudDirection * (speed * Time.deltaTime);
		OnBecameInvisible();
	}

	private void OnBecameInvisible()
	{
		if (Camera.main == null)
			return;

		if (cloudDirection.x < 0)
		{
			float leftX = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
			if (transform.position.x + transform.GetComponent<Renderer>().bounds.size.x < leftX)
				WarpToRight();
		}
		else if (cloudDirection.x > 0)
		{
			float rightX = Camera.main.ViewportToWorldPoint(Vector3.right).x;
			if (transform.position.x > rightX + transform.GetComponent<Renderer>().bounds.size.x)
				WarpToLeft();
		}
		/*

		if(cloudDirection.y != 0)
		{
			Vector3 viewPos = Camera.main.WorldToScreenPoint (transform.position);
			if (viewPos.y < Camera.main.rect.y - cloudScreenOffset||
				viewPos.y > Camera.main.pixelHeight + cloudScreenOffset)
				WarpToRight ();
		}*/
	}

	private void WarpToRight()
	{
		float newZ = 20.1f;
		this.speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
		float rightX = Camera.main.ViewportToWorldPoint(Vector3.right).x + transform.GetComponent<Renderer>().bounds.size.x;
		transform.position = new Vector3(rightX, transform.position.y, newZ);
	}

	private void WarpToLeft()
	{
		float newZ = transform.position.z + UnityEngine.Random.Range(-1f, 1f);
		this.speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
		float leftX = Camera.main.ViewportToWorldPoint(Vector3.zero).x - transform.GetComponent<Renderer>().bounds.size.x;
		transform.position = new Vector3(leftX, transform.position.y, newZ);
	}
}