using UnityEngine;

public class HeadScript : MonoBehaviour
{
	public bool enabled = false;
	public MiniGameType type;

	public float returnVelocity = 7f;

	private void OnTriggerEnter(Collider collider)
	{
		if (enabled)
		{
			VerifyTopCollision(collider);
			VerifyFrontCollision(collider, true);
		}
	}

	private void OnTriggerStay(Collider collider)
	{
		if (enabled)
		{
			VerifyTopCollision(collider);
			VerifyFrontCollision(collider, false);
		}
	}

	private void VerifyTopCollision(Collider collider)
	{
		if (collider.tag != "Player")
		{
			var parent = transform.parent;
			var currentTransform = collider.transform;

			var count = 0;
			while (parent != null && count < 30)
			{
				currentTransform = parent;
				parent = parent.parent;
				count++;
			}
			PlayerAnimation playerAnimation = currentTransform.GetComponent<PlayerAnimation>();

			if (playerAnimation != null)
			{
				CubeScript cs = collider.transform.GetComponent<CubeScript>();
				if (cs != null)
				{
					if (collider.transform.position.y > transform.position.y)
					{
						if (type == MiniGameType.Cube && (collider.transform.position != cs.StartPosition || playerAnimation.currentState != PlayerAnimation.State.Jump)) return;
						else if (type == MiniGameType.Fall && (playerAnimation.currentState == PlayerAnimation.State.Fall)) return;

						cs.Animating = true;
						cs.dir = CubeScript.Direction.UP;
						cs.sendInfo = true;
					}
				}

				PlayerMovement playerMovement = currentTransform.GetComponent<PlayerMovement>();
				if (playerMovement != null)
				{
					if (collider.tag == "Tile")
					{
						Tile tile = collider.transform.GetComponent<Tile>();
						if (tile != null)
						{
							if (tile.blockBottom)
							{
								if (collider.transform.position.y > transform.position.y &&
									playerAnimation.currentState == PlayerAnimation.State.Jump)
								{
									if (playerMovement.MoveDirection.y > 0)
									{
										playerMovement.MoveDirection.y = 0;
									}
								}
							}
						}
					}
					else if (collider.tag != "Player" && collider.tag != "Hidden" &&
					   collider.tag != "Tile" && collider.tag != "Texture" && collider.tag != "Body")
					{
						if (collider.transform.position.y > transform.position.y &&
							playerAnimation.currentState == PlayerAnimation.State.Jump)
						{
							if (playerMovement.MoveDirection.y > 0)
							{
								playerMovement.MoveDirection.y = 0;
							}
						}
					}
				}
			}
		}
	}

	private void VerifyFrontCollision(Collider collider, bool enter)
	{
		if (collider.tag != "Player" && collider.tag != "Hidden" && collider.tag != "Tile" && collider.tag != "Texture" && collider.tag != "Body")
		{
			var parent = transform.parent;
			var currentTransform = collider.transform;

			var count = 0;
			while (parent != null && count < 30)
			{
				currentTransform = parent;
				parent = parent.parent;
				count++;
			}

			PlayerMovement playerMovement = currentTransform.GetComponent<PlayerMovement>();

			if (playerMovement != null)
			{
				PlayerAnimation playerAnimation = currentTransform.GetComponent<PlayerAnimation>();
				if (playerAnimation != null)
				{
					if (playerAnimation.currentState == PlayerAnimation.State.Jump &&
						playerAnimation.currentState == PlayerAnimation.State.Fall)
					{
						var move = Vector3.zero;
						//var horAxis = Input.GetAxis ("Horizontal");

						if (transform.position.x < collider.transform.position.x)
						{
							move = (Vector3.left);
						}
						else
						{
							move = (Vector3.right);
						}

						Debug.Log (playerMovement.MoveDirection + ", " + move);

						var result = move * Time.deltaTime * (playerMovement.MoveSpeed);

						Debug.Log("Chegou aqui");
						playerMovement.transform.position = Vector3.Slerp(playerMovement.transform.position,
							(playerMovement.transform.position + result), returnVelocity * Time.deltaTime);
					}
				}
			}
		}
	}
}