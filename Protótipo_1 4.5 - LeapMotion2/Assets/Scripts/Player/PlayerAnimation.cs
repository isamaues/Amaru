using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	private CharacterController controller;
	public bool inMovement;

	public const string IDLE = "Idle";
	public const string WALK = "Walk";
	public const string JUMP = "Jump";
	public const string FALL = "Fall";
	public const string LAND = "Land";
	public const string FLY = "Fall";

	public const string SHOOT = "Jump";

	public enum State
	{
		Walk,
		Idle,
		Jump,
		Fall,
		Land,
		Fly,
		Shoot
	}

	public State currentState;
	private bool _paused = false;

	public bool PauseMovement
	{
		get
		{
			return _paused;
		}

		set
		{
			_paused = value;
			foreach (AnimationState clip in GetComponent<Animation>())
			{
				clip.speed = (_paused) ? 0f : 1f;
			}
		}
	}

	// Use this for initialization
	private void Start()
	{
		controller = transform.GetComponent<CharacterController>();
		GetComponent<Animation>().Stop();
	}

	// Update is called once per frame
	private void Update()
	{
		if (!_paused)
		{
			DetermineCurrentState();

			PlayAnimation();
		}
	}

	private void DetermineCurrentState()
	{
		if (!controller.isGrounded &&
			currentState != State.Fall &&
			currentState != State.Jump &&
			currentState != State.Land)
		{
			Fall();
		}
	}

	private void PlayAnimation()
	{
		switch (currentState)
		{
			case State.Idle:
				Idle();
				break;

			case State.Walk:
				Walk();
				break;

			case State.Fall:
				Fall();
				break;

			case State.Jump:
				Jump();
				break;

			case State.Land:
				Land();
				break;

			case State.Fly:
				Fly();
				break;

			case State.Shoot:
				Shoot();
				break;
		}
	}

	public void Fly()
	{
		GetComponent<Animation>().CrossFade(FLY);
	}

	public void Shoot()
	{
		if (!GetComponent<Animation>().IsPlaying(SHOOT))
		{
			currentState = State.Fall;
			GetComponent<Animation>().CrossFade(FLY);
		}
	}

	public void Idle()
	{
		GetComponent<Animation>().CrossFade(IDLE);
	}

	public void Walk()
	{
		GetComponent<Animation>().CrossFade(WALK);
		GetComponent<Animation>()[WALK].speed = 1.5f;
	}

	public void Fall()
	{
		if (controller.isGrounded)
		{
			//if (inMovement)
			//{
			//	currentState = State.Walk;
			//	animation.CrossFade ("Walk");
			//}
			//else
			//{
			currentState = State.Idle;
			GetComponent<Animation>().CrossFade(IDLE);
			//}
		}
	}

	public void Jump()
	{
		if (controller.isGrounded)
		{
			/*if (inMovement)
			{
				animation.CrossFade ("Walk");
				currentState = State.Walk;
			}
			else
			{
				animation.CrossFade ("Idle");
				currentState = State.Idle;
			}*/

			currentState = State.Land;
			GetComponent<Animation>().CrossFade(LAND);
		}
		else if (!GetComponent<Animation>().IsPlaying(JUMP))
		{
			currentState = State.Fall;
			GetComponent<Animation>().CrossFade(FALL);
		}
	}

	public void Land()
	{
		if (inMovement)
		{
			currentState = State.Walk;
			GetComponent<Animation>().CrossFade(WALK);
		}
		if (!GetComponent<Animation>().IsPlaying(LAND))
		{
			GetComponent<Animation>().CrossFade(IDLE);
			currentState = State.Idle;
		}
	}
}