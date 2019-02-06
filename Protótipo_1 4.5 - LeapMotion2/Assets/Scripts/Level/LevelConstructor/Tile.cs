using UnityEngine;

public class Tile : MonoBehaviour
{
	public bool blockLeft = true;
	public bool blockRight = true;
	public bool blockTop = true;
	public bool blockBottom = true;
	public float debugSquareSize = 0.2f;
	public float arrowsSize = 0.2f;

	private void Start()
	{
	}

	private void Update()
	{
		#region Debug (pode deletar

		Color lineColor = Color.white;

		if (!transform.GetComponent<Collider>().isTrigger)
		{
			lineColor = Color.red;
		}

		/*if (blockBottom)
			DrawLines (transform.position + (Vector3.down * transform.collider.bounds.extents.y / 1.5f), Vector3.down, Vector3.up, Vector3.left, Vector3.right, lineColor, arrowsSize, Time.deltaTime);
		if (blockTop)
			DrawLines (transform.position + (Vector3.up * transform.collider.bounds.extents.y / 1.5f), Vector3.up, Vector3.down, Vector3.left, Vector3.right, lineColor, arrowsSize, Time.deltaTime);
		if (blockRight)
			DrawLines (transform.position + (Vector3.right * transform.collider.bounds.extents.y / 1.5f), Vector3.right, Vector3.left, Vector3.up, Vector3.down, lineColor, arrowsSize, Time.deltaTime);
		if (blockLeft)
			DrawLines (transform.position + (Vector3.left * transform.collider.bounds.extents.y / 1.5f), Vector3.left, Vector3.right, Vector3.up, Vector3.down, lineColor, arrowsSize, Time.deltaTime);
		 */

		#endregion Debug (pode deletar
	}

	private void OnTriggerEnter(Collider collider)
	{
		VerifyCollisions(collider);
	}

	private void OnTriggerStay(Collider collider)
	{
		VerifyCollisions(collider);
	}

	private void VerifyCollisions(Collider collider)
	{
		if (collider.tag == "Body")
		{
			transform.GetComponent<Collider>().isTrigger = !(blockTop || blockRight || blockLeft || blockBottom);

			if (!transform.GetComponent<Collider>().isTrigger)
			{
				transform.GetComponent<Collider>().isTrigger = true;

				if (blockBottom)
				{
					CharacterController cc = collider.transform.parent.GetComponent<CharacterController>();
					if (cc != null)
					{
						var passable = transform.GetComponent<Collider>().bounds.min.y > cc.GetComponent<Collider>().transform.position.y + cc.height;
						transform.GetComponent<Collider>().isTrigger = passable && transform.GetComponent<Collider>().isTrigger;
					}
				}

				if (blockTop)
				{
					CharacterController cc = collider.transform.parent.GetComponent<CharacterController>();
					if (cc != null)
					{
						var passable = transform.GetComponent<Collider>().bounds.max.y > cc.GetComponent<Collider>().transform.position.y;
						transform.GetComponent<Collider>().isTrigger = passable && transform.GetComponent<Collider>().isTrigger;
					}
				}

				if (blockLeft)
				{
					CharacterController cc = collider.transform.parent.GetComponent<CharacterController>();
					if (cc != null)
					{
						var passable = transform.GetComponent<Collider>().bounds.min.x < cc.GetComponent<Collider>().bounds.min.x;
						transform.GetComponent<Collider>().isTrigger = passable && transform.GetComponent<Collider>().isTrigger;
					}
				}

				if (blockRight)
				{
					CharacterController cc = collider.transform.parent.GetComponent<CharacterController>();
					if (cc != null)
					{
						var passable = transform.GetComponent<Collider>().bounds.max.x > cc.GetComponent<Collider>().bounds.max.x;
						transform.GetComponent<Collider>().isTrigger = passable && transform.GetComponent<Collider>().isTrigger;
					}
				}
			}
		}
	}

	#region Debug

	public void DrawSquare(Vector3 center, float size, Color color1, Color color2, float time)
	{
		Color color = (transform.GetComponent<Collider>().isTrigger) ? color1 : color2;

		var start = center + (Vector3.up * size / 2);

		var end = center + (Vector3.left * size / 2);
		Debug.DrawLine(start, end, color, time);

		end = center + (Vector3.right * size / 2);
		Debug.DrawLine(start, end, color, time);

		start = center + (Vector3.down * size / 2);

		end = center + (Vector3.left * size / 2);
		Debug.DrawLine(start, end, color, time);

		end = center + (Vector3.right * size / 2);
		Debug.DrawLine(start, end, color, time);
	}

	public void DrawLines(Vector3 center, Vector3 up, Vector3 down, Vector3 left, Vector3 right, Color color, float size, float time)
	{
		var start = Vector3.zero;
		var end = Vector3.zero;

		/*
		start = center + (down * size / 2);
		end = start + (left * size / 5);
		Debug.DrawLine (start, end, color, time);

		start = end;
		end = start + (up * size / 2);
		Debug.DrawLine (start, end, color, time);

		start = end;
		end = start + (left * size / 5);
		Debug.DrawLine (start, end, color, time);

		start = end;
		end = center + (up * size / 2);
		Debug.DrawLine (start, end, color, time);

		//

		start = center + (down * size / 2);
		end = start + (right * size / 5);
		Debug.DrawLine (start, end, color, time);

		start = end;
		end = start + (up * size / 2);
		Debug.DrawLine (start, end, color, time);

		start = end;
		end = start + (right * size / 5);
		Debug.DrawLine (start, end, color, time);

		start = end;
		end = center + (up * size / 2);
		Debug.DrawLine (start, end, color, time);*/

		//
		start = center + (up * size / 2);
		end = start + (right * (transform.GetComponent<Collider>().bounds.extents.x - size / 2));
		Debug.DrawLine(start, end, color, time);

		end = start + (left * (transform.GetComponent<Collider>().bounds.extents.x - size / 2));
		Debug.DrawLine(start, end, color, time);

		start = center + (up * size / 3);
		end = start + (right * (transform.GetComponent<Collider>().bounds.extents.x - size / 1.5f));
		Debug.DrawLine(start, end, color, time);

		end = start + (left * (transform.GetComponent<Collider>().bounds.extents.x - size / 1.5f));
		Debug.DrawLine(start, end, color, time);
		//
	}

	#endregion Debug
}