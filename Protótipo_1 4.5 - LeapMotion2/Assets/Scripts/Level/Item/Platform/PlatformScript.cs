using System.Collections;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
	private ArrayList objList = null;

	public bool colisionEnter = false;

	public void SetArrayList(ArrayList cubesList)
	{
		objList = cubesList;
	}

	private void OnTriggerEnter(Collider collision)
	{
		//		PlayerAnimation playerAnimation = null;

		if (collision.tag == "Foot")
		{
			FootScript fs = collision.GetComponent<FootScript>();

			if (fs != null)
			{
				if (GetComponent<Collider>().transform.position.y <= transform.position.y && fs.IsLastOrCurrentValid())
				{
					colisionEnter = true;
					if (objList != null)
					{
						foreach (GameObject cube in objList)
						{
							if (cube.transform.position.y == 3f && (cube.transform.position.x <= this.transform.position.x + 3f && cube.transform.position.x >= this.transform.position.x - 3))
							{
								CubeScript cs = cube.GetComponent<CubeScript>();
								if (cs)
								{
									if (cs.sendInfo == false)
									{
										//									Debug.Log("Chegou Aqui");
										cs.Animating = true;
										cs.dir = CubeScript.Direction.UP;
										cs.sendInfo = true;
									}
								}
							}
						}
					}
				}
			}

			/*Transform parent = collision.transform.parent;
			Transform currentTransform = collision.transform;

			int count = 0;
			while (parent != null && count < 30) {
				currentTransform = parent;
				parent = parent.parent;
				count++;
			}

			playerAnimation = currentTransform.GetComponent<PlayerAnimation> ();

			if (playerAnimation != null) {
				if(collider.transform.position.y <= transform.position.y && (playerAnimation.currentState == PlayerAnimation.State.Fall || playerAnimation.currentState == PlayerAnimation.State.Land)) {
					colisionEnter = true;
					if (objList != null) {
						foreach (GameObject cube in objList) {
							if (cube.transform.position.y == 3f && (cube.transform.position.x <= this.transform.position.x + 3f && cube.transform.position.x >= this.transform.position.x -3)) {
								CubeScript cs = cube.GetComponent<CubeScript>();
								if(cs) {
									if (cs.sendInfo == false) {
									Debug.Log("Chegou Aqui");
										cs.Animating = true;
										cs.dir = CubeScript.Direction.UP;
										cs.sendInfo = true;
									}
								}
							}
						}
					}
				}
			}*/
		}
	}

	private void OnTriggerExit(Collider collisionInfo)
	{
		//Debug.Log("Exit");
		colisionEnter = false;
	}
}