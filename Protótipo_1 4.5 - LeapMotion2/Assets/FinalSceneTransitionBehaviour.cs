using UnityEngine;
using System.Collections;

public class FinalSceneTransitionBehaviour : StateMachineBehaviour {

	
	private float startTime;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		startTime = Time.time;
		GameObject.Find("Canvas").transform.FindChild("Transition").gameObject.SetActive(true);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (Time.time - startTime > 0.5f)
		{
			GameObject.Find("Canvas").transform.FindChild("Scene1").gameObject.SetActive(false);
			GameObject.Find("Canvas").transform.FindChild("Scene2").gameObject.SetActive(true);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		GameObject.Find("Canvas").transform.FindChild("Transition").gameObject.SetActive(false);
	}
}
