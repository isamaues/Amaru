using UnityEngine;

public class CubeScript : MonoBehaviour
{
	public enum Direction
	{
		UP,
		DOWN,
		SORT,
		NONE
	}

	public Vector3 StartPosition { get; set; }

	public Vector3 FinalPosition { get; set; }

	public float velocity = 5f;

	public float Velocity
	{
		get
		{
			return velocity;
		}
		set
		{
			velocity = value;
		}
	}

	public bool Animating { get; set; }

	public Direction dir = Direction.NONE;

	public bool IsPingPong { get; set; }

	public float MinDistance { get; set; }

	public string CubeInfo { get; set; }

	public bool sendInfo = false;
	public bool smooth = true;
	private float cont;

	private void Update()
	{
		if (Animating)
		{
			if (dir == Direction.UP || dir == Direction.SORT)
			{
				if (Vector3.Distance(transform.position, FinalPosition) > MinDistance)
				{
					if (smooth)
					{
						transform.position = Vector3.Slerp(transform.position, FinalPosition, velocity * Time.deltaTime);
					}
					else
					{
						transform.position = Vector3.Lerp(transform.position, FinalPosition, velocity * Time.deltaTime);
					}
				}
				else if (IsPingPong)
				{
					transform.position = FinalPosition;
					if (dir != Direction.SORT)
					{
						dir = Direction.DOWN;
					}
					else
					{
						dir = Direction.NONE;
					}
				}
			}
			else if (dir == Direction.DOWN)
			{
				if (Vector3.Distance(transform.position, StartPosition) > MinDistance)
				{
					if (smooth)
					{
						transform.position = Vector3.Slerp(transform.position, StartPosition, velocity * Time.deltaTime);
					}
					else
					{
						transform.position = Vector3.Lerp(transform.position, StartPosition, velocity * Time.deltaTime);
					}
				}
				else
				{
					transform.position = StartPosition;
					dir = Direction.NONE;
				}
			}

			if (dir == Direction.NONE)
			{
				Animating = false;
			}
		}
	}
}