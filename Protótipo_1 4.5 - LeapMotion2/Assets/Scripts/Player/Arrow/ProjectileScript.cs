using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	public float projectileSpeed;

	private void Update()
	{
		float speed = projectileSpeed * Time.deltaTime;

		transform.Translate(0, speed, 0);

		if (transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x) Destroy(this.gameObject);
	}

	private void OnTriggerEnter(Collider collider)
	{
		VerifyProjectileCollision(collider);
	}

	private void OnTriggerStay(Collider collider)
	{
		VerifyProjectileCollision(collider);
	}

	private void VerifyProjectileCollision(Collider collider)
	{
		if (collider.tag != "Projectile" && collider.tag != "Body")
		{
			Destroy(this.gameObject);
			var parent = transform.parent;
			var currentTransform = collider.transform;

			var count = 0;
			while (parent != null && count < 30)
			{
				currentTransform = parent;
				parent = parent.parent;
				count++;
			}

			CubeScript cs = collider.transform.GetComponent<CubeScript>();
			if (cs != null)
			{
				if (collider.transform.position.x > transform.position.x && collider.transform.position == cs.StartPosition)
				{
					cs.Animating = true;
					cs.dir = CubeScript.Direction.UP;
					cs.sendInfo = true;
				}
			}
		}
	}
}