using UnityEngine;
using System.Collections;

public class FinalScene1Behaviour : StateMachineBehaviour {

	private float startTime;
	

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		startTime = GameObject.Find("Canvas").GetComponent<Final>().startTime;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(Input.anyKeyDown || Time.time - startTime > 20f)
		{
			GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("EnterTransition");
		}
	}
}
