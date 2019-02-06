using UnityEngine;

public class FootScript : MonoBehaviour
{
	private PlayerAnimation playerAnimation = null;
	private PlayerAnimation.State lastState;
	private PlayerAnimation.State currentState;

	public Transform parent;

	private void Start()
	{
		playerAnimation = parent.GetComponent<PlayerAnimation>();
		lastState = playerAnimation.currentState;
		currentState = playerAnimation.currentState;
	}

	private void Update()
	{
		if (playerAnimation.currentState != currentState)
		{
			lastState = currentState;
			currentState = playerAnimation.currentState;
		}
	}

	public bool IsLastOrCurrentValid()
	{
		//	return (currentState == PlayerAnimation.State.Fall || currentState == PlayerAnimation.State.Land || lastState == PlayerAnimation.State.Fall || lastState == PlayerAnimation.State.Land);
		return (currentState == PlayerAnimation.State.Land || lastState == PlayerAnimation.State.Fall || lastState == PlayerAnimation.State.Land);
	}

	public PlayerAnimation.State GetLastState()
	{
		return lastState;
	}
}