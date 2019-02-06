using Leap;
using UnityEngine;
using UnityEngine.UI;

//REVISION 126
public class PlayerMovement : MonoBehaviour
{
    public Toggle isFollowMouse;

	public enum Direction
	{
		Left,
		Right,
		Up,
		Down
	}

    public float t = 0f;
    public bool jumpBool = false;
    public Vector3 MoveDirection;
	private PauseMenu pauseMenu;
	private PauseManager pauseManager;

	public float VerticalVelocity { get; set; }

	public float MoveSpeed = 8f;
	public float JumpSpeed = 8f;
	public float Gravity = 20f;
	public float variance = 30f;
	public Direction direction;
	public bool canMove = true;
	private CharacterController controller;
	private PlayerAnimation animator;

	public PlayerAnimation Animator
	{
		get
		{
			return animator;
		}
	}

	private bool _paused;
	private AudioClip jump;
	public float rotationConstant = 5f;
	private bool isUpDownMovement = false;

	public Slider jumpBar;
    public RectTransform canvasRect;

    //Leap Variables
    private Controller leapController = new Controller();

		private Frame currentFrame = Frame.Invalid;
	    private int palmLimit = 30;

	    private float lastYPositionHand = 0;
        private float lastYPositionTool = 0;
        private float lastZPositionTool = 0;
        private int lastFingerCount = 5;
        private int lastLastFingerCount = 5;
        private bool fired = false;


	public bool IsUpDownMovement
	{
		get
		{
			return this.isUpDownMovement;
		}
		set
		{
			isUpDownMovement = value;
		}
	}

	private void Start()
	{
		jump = Resources.Load("Sounds/jump") as AudioClip;
		pauseMenu = new PauseMenu();

		controller = transform.GetComponent<CharacterController>();
		animator = transform.GetComponent<PlayerAnimation>();

		pauseManager = GameObject.Find("GameManager").GetComponent<PauseManager>();

        isFollowMouse.isOn = false;

    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pauseMenu.Paused = pauseMenu.Paused ? false : true;
			Time.timeScale = pauseMenu.Paused ? 0f : 1f;
			pauseMenu.showConfirmation = false;
		}

		if (pauseMenu.Paused)
			pauseMenu.Update();
		else
			ProcessMotion();
		 

		if (!pauseManager.IsPaused)
		{
			Time.timeScale = 1f;
			//ProcessMotion();
		}
		else
		{
			Time.timeScale = 0f;
		}

        float offsetPosY = transform.position.y + 5f;
        Vector3 offsetPos = new Vector3(transform.position.x, offsetPosY, transform.position.z);
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
        jumpBar.transform.localPosition = canvasPos;
    }

	public float decreaseGravity = 3f;

	private void ProcessMotion() //Mod paulo
	{
		// Leap Motion Edit
		currentFrame = leapController.Frame();
		Hand primeHand = frontMostHand(currentFrame);
        Tool frontTool = currentFrame.Tools.Frontmost;

		if (isUpDownMovement == true)
		{
			animator.inMovement = false;
			//Retirar Comentário apos finalizar a animação.
			//animator.currentState = PlayerAnimation.State.Fly;
			MoveDirection = new Vector3(0, Input.GetAxis("Vertical") * MoveSpeed, 0);

			//Movimentaçao com mouse
			if (Input.GetMouseButton(0) && !UramaBehaviour.OnMousePressing)
			{
				Vector3 position2D = Camera.main.WorldToScreenPoint(transform.position);
				if (position2D.y > Input.mousePosition.y)
				{
					MoveDirection = new Vector3(0, -1 * MoveSpeed, 0);
				}
				else if (position2D.y + variance < Input.mousePosition.y)
				{
					MoveDirection = new Vector3(0, 1 * MoveSpeed, 0);
				}
            }
			
            #region Movimentaçao com Leap Motion usando uma mão
            if (currentFrame.Hands.Count != 0)
				{
					if (primeHand.PalmPosition.y < 20)
						MoveDirection = Vector3.zero;
					if (primeHand.PalmPosition.y < 200 - 60)
						MoveDirection = new Vector3(0, -MoveSpeed, 0);
					else if (primeHand.PalmPosition.y < 200 - 30)
						MoveDirection = new Vector3(0, -(MoveSpeed / 2.5f), 0);
					else if (primeHand.PalmPosition.y > 200 + 60)
						MoveDirection = new Vector3(0, MoveSpeed, 0);
					else if (primeHand.PalmPosition.y > 200 + 30)
						MoveDirection = new Vector3(0, MoveSpeed / 2.5f, 0);
					else MoveDirection = Vector3.zero;

                    Debug.Log(currentFrame.Pointables.Count);
					//if (primeHand.Fingers.Count == 0 && lastFingerCount == 1 && lastFingerCount != 0 && lastLastFingerCount != 0) 
                    if (primeHand.PalmPosition.x < -palmLimit && !fired)
                    {
                        transform.GetComponent<ShootScript>().Shoot();
                        fired = true;
                    }
                    else if (primeHand.PalmPosition.x > -palmLimit)
                    {
                        fired = false;
                    }
                }
            #endregion

            #region Movimentação com Leap Motion utilizando ferramenta
            else if (currentFrame.Tools.Count != 0)
            {
                //Movimentaçao com Leap Motion
                if (frontTool.TipPosition.y < 20)
                    MoveDirection = Vector3.zero;
                if (frontTool.TipPosition.y < 200 - 60)
                    MoveDirection = new Vector3(0, -MoveSpeed, 0);
                else if (frontTool.TipPosition.y < 200 - 30)
                    MoveDirection = new Vector3(0, -(MoveSpeed / 2.5f), 0);
                else if (frontTool.TipPosition.y > 200 + 60)
                    MoveDirection = new Vector3(0, MoveSpeed, 0);
                else if (frontTool.TipPosition.y > 200 + 30)
                    MoveDirection = new Vector3(0, MoveSpeed / 2.5f, 0);
                else MoveDirection = Vector3.zero;

                //if (primeHand.Fingers.Count == 0 && lastFingerCount == 1 && lastFingerCount != 0 && lastLastFingerCount != 0) 
                if (frontTool.TipPosition.x < -palmLimit && !fired)
                {
                    transform.GetComponent<ShootScript>().Shoot();
                    fired = true;
                }
                else if (frontTool.TipPosition.x > -palmLimit)
                {
                    fired = false;
                }
            }
            #endregion
				
            if (MoveDirection.y > 0)
			{
				direction = Direction.Up;
			}
			else if (MoveDirection.y < 0)
			{
				direction = Direction.Down;
			}
			else
			{
				// Ta parado!!!!
			}
			if (transform.position.y > Camera.main.ViewportToWorldPoint(Vector3.up).y - 3 && direction == Direction.Up)
			{
				MoveDirection = new Vector3(MoveDirection.x * MoveSpeed, 0, MoveDirection.z);
			}
		}
		else
		{
			if (controller.isGrounded)
			{//Move o amaru no chao
				MoveDirection = new Vector3(Input.GetAxis("Horizontal") * MoveSpeed, 0, 0);
				#region Movimentaçao com Leap Motion usando uma mão
                if (currentFrame.Hands.Count != 0)
				{
									///Move o amaru usando o LeapMotion
					if (primeHand.PalmPosition.x > (palmLimit * 2))
					{
						MoveDirection = new Vector3(MoveSpeed, 0, 0);
					}
					else if (primeHand.PalmPosition.x > palmLimit)
					{
						MoveDirection = new Vector3(MoveSpeed / 2.5f, 0, 0);
					}
					else if (primeHand.PalmPosition.x < -(palmLimit * 2))
					{
                        MoveDirection = new Vector3(-MoveSpeed, 0, 0);
					}
					else if (primeHand.PalmPosition.x < -palmLimit)
					{
						MoveDirection = new Vector3(-(MoveSpeed / 2.5f), 0, 0);
					}
				}
                else
                #endregion

                #region Movimentaçao com Leap Motion usando uma ferramenta
                if (currentFrame.Tools.Count != 0)
				{
					if (frontTool.TipPosition.x > (palmLimit * 2))
					{
						MoveDirection = new Vector3(MoveSpeed/2, 0, 0);
					}
                    else if (frontTool.TipPosition.x > palmLimit)
					{
						MoveDirection = new Vector3(MoveSpeed/2, 0, 0);
					}
                    else if (frontTool.TipPosition.x < -(palmLimit * 2))
					{
                        MoveDirection = new Vector3(-MoveSpeed/2, 0, 0);
					}
                    else if (frontTool.TipPosition.x < -palmLimit)
					{
						MoveDirection = new Vector3(-MoveSpeed/2, 0, 0);
					}
                }

                #endregion

                float delayTime = 2.0f;
                if (frontTool.TipPosition.z < -40f)
                {
                    jumpBar.gameObject.SetActive(true);
                    float jumpTime = delayTime - (t - Time.time);
                    jumpBar.value = (jumpTime / delayTime);

                    print(frontTool.TipPosition.z + " | " + jumpBool + " | " + jumpTime + " | " + (jumpTime/delayTime));

                    if (Time.time > t)
                    {
                        jumpBool = true;
                        t = Time.time + delayTime + 0.8f;
                    }
                    else
                    {
                        jumpBool = false;
                    }
                }
                else
                {
                    print(frontTool.TipPosition.z);
                    jumpBool = false;
                    t = Time.time + delayTime;
                    jumpBar.gameObject.SetActive(false);
                }

                if ((Input.GetButtonDown("Jump")) || (Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetMouseButtonDown(0) && isFollowMouse.isOn)
                    || /*Faz amaru pular com o LM usando a mão*/ (lastYPositionHand != 0 && primeHand.PalmPosition.y - lastYPositionHand > 12)
                    || /*Faz amaru pular com o LM usando uma ferramenta (lastYPositionTool != 0 && frontTool.TipPosition.z < -80f)*/ jumpBool)
				{
					animator.currentState = PlayerAnimation.State.Jump;

					if (GetComponent<Animation>().IsPlaying("Jump"))
						GetComponent<Animation>().Stop("Jump");

					GetComponent<Animation>().Play("Jump");

					AudioSource.PlayClipAtPoint(jump, Camera.main.transform.position);
					MoveDirection.y = JumpSpeed;
				}
			}
			else
			{//Move o amaru no ar
				MoveDirection = new Vector3(Input.GetAxis("Horizontal") * MoveSpeed, MoveDirection.y, 0);

					if (currentFrame.Hands.Count != 0)
					{
						//Move o amaru usando o LM com a mão
						if (primeHand.PalmPosition.x > (palmLimit * 1.5f))
						{
							MoveDirection = new Vector3(MoveSpeed, MoveDirection.y, 0);
						}
						else if (primeHand.PalmPosition.x > palmLimit)
						{
							MoveDirection = new Vector3(MoveSpeed / 1.5f, MoveDirection.y, 0);
						}
						else if (primeHand.PalmPosition.x < -(palmLimit * 1.5f))
						{
							MoveDirection = new Vector3(-MoveSpeed, MoveDirection.y, 0);
						}
						else if (primeHand.PalmPosition.x < -palmLimit)
						{
							MoveDirection = new Vector3(-(MoveSpeed / 1.5f), MoveDirection.y, 0);
						}
                    }else if (currentFrame.Tools.Count != 0)
                    {
                        ///Move o amaru usando o LeapMotion com uma ferramenta
                        if (frontTool.TipPosition.x > (palmLimit * 1.5f))
                        {
                            MoveDirection = new Vector3(MoveSpeed, MoveDirection.y, 0);
                        }
                        else if (frontTool.TipPosition.x > palmLimit)
                        {
                            MoveDirection = new Vector3(MoveSpeed / 1.5f, MoveDirection.y, 0);
                        }
                        else if (frontTool.TipPosition.x < -(palmLimit * 1.5f))
                        {
                            MoveDirection = new Vector3(-MoveSpeed, MoveDirection.y, 0);
                        }
                        else if (frontTool.TipPosition.x < -palmLimit)
                        {
                            MoveDirection = new Vector3(-(MoveSpeed / 1.5f), MoveDirection.y, 0);
                        }
                    }

				if (animator.currentState != PlayerAnimation.State.Fall && Input.GetButtonUp("Jump") && MoveDirection.y >= 0)
				{
					MoveDirection.y -= decreaseGravity * Gravity * Time.deltaTime;
				}

				if (animator.currentState != PlayerAnimation.State.Fall &&
									animator.currentState != PlayerAnimation.State.Jump && animator.currentState != PlayerAnimation.State.Land)
				{
					animator.currentState = PlayerAnimation.State.Fall;
					GetComponent<Animation>().CrossFade("Fall");
				}
			}

            if (/*Input.GetMouseButton(0)*/ isFollowMouse.isOn && !UramaBehaviour.OnMousePressing)
			{
				Vector3 position2D = Camera.main.WorldToScreenPoint(transform.position);

				if (position2D.x + 10 < Input.mousePosition.x)
				{
					MoveDirection = new Vector3(0.7f * MoveSpeed, MoveDirection.y, 0);
				}
				else if (position2D.x - 50 > Input.mousePosition.x)
				{
					MoveDirection = new Vector3(-0.7f * MoveSpeed, MoveDirection.y, 0);
				}
			}

			if (Input.touches.Length != 0)
			{
				Vector3 position2D = Camera.main.WorldToScreenPoint(transform.position);

				if (position2D.x + variance < Input.touches[0].position.x)
				{
					MoveDirection = new Vector3(1 * MoveSpeed, MoveDirection.y, 0);
				}
				else if (position2D.x - variance > Input.touches[0].position.x)
				{
					MoveDirection = new Vector3(-1 * MoveSpeed, MoveDirection.y, 0);
				}
			}

			MoveDirection.y -= Gravity * Time.deltaTime;

			if (MoveDirection.x > 0)
			{
				direction = Direction.Right;
				animator.inMovement = true;
			}
			else if (MoveDirection.x < 0)
			{
				direction = Direction.Left;
				animator.inMovement = true;
			}
			else
			{
				animator.inMovement = false;
			}
		}

		if (direction == Direction.Right)
		{
			transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,
								new Vector3(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z), rotationConstant * Time.deltaTime));
		}
		else if (direction == Direction.Left)
		{
			transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,
								new Vector3(transform.rotation.eulerAngles.x, 270, transform.rotation.eulerAngles.z), rotationConstant * Time.deltaTime));
		}

		if (animator.currentState != PlayerAnimation.State.Fall && animator.currentState != PlayerAnimation.State.Jump &&
						animator.currentState != PlayerAnimation.State.Land)
		{
			if (controller.isGrounded)
			{
				if (animator.inMovement)
				{
					animator.currentState = PlayerAnimation.State.Walk;
				}
				else
				{
					animator.currentState = PlayerAnimation.State.Idle;
				}
			}
		}

		if (MoveDirection.y < 0 && animator.currentState == PlayerAnimation.State.Jump)
		{
			animator.currentState = PlayerAnimation.State.Fall;
		}

		if (!canMove)
		{
			MoveDirection = new Vector3(0, MoveDirection.y, MoveDirection.z);
		}

		if ((transform.position.x - Camera.main.ScreenToWorldPoint(Vector3.zero).x < 0.5f && direction == Direction.Left) ||
			(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0f)).x - transform.position.x < 0.5f && direction == Direction.Right))
		{
			MoveDirection = new Vector3(0, MoveDirection.y, MoveDirection.z);
		}

		controller.Move(MoveDirection * Time.deltaTime);
		lastYPositionHand = primeHand.PalmPosition.y;
        lastYPositionTool = frontTool.TipPosition.y;
        lastZPositionTool = frontTool.TipPosition.z;
        lastLastFingerCount = lastFingerCount;
		lastFingerCount = primeHand.Fingers.Count;
	}

	private void OnGUI()
	{
		pauseMenu.OnGUI();
	}

		private Hand frontMostHand(Frame curFrame)
		{
			Hand bestHand = Hand.Invalid;
			float minZ = float.MaxValue;

			foreach (Hand hand in curFrame.Hands)
			{
				if (hand.PalmPosition.z < minZ)
				{
					minZ = hand.PalmPosition.z;
					bestHand = hand;
				}
			}

			return bestHand;
		}
}