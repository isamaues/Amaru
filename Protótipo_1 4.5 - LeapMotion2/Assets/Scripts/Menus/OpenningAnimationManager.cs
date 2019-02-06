using UnityEngine;

public class OpenningAnimationManager : MonoBehaviour
{
	//private BoxCollider playerBoxCollider;
	private float shipAngle = 90f;

	private float shipSinAngle;
	private float shipWaveVel = 0.4f;
	public AnimantionScene currentScene;
	public GameObject cam;
	public GameObject terra;
	public GameObject backGround0;
	public GameObject backGround1;
	public GameObject backGround2;
	public GameObject backGround3;
	public GameObject backGround4;
	public GameObject ship;
	public GameObject head;
	public GameObject asteroid;
	public GameObject ship_falling;
	public Texture2D splashLogo; // the logo to splash;
	public float fadeSpeed = 0.3f;
	public float splashTime;
	public Texture2D fadeTexture;
	private float currentSplashTime = 0;
	private bool fadeWait = false;
	private bool fadeOut = false;
	private bool fadeIn = false;
	private float alpha = 1.0f;
	private Rect splashPos;
	private bool fadeMode = false;

	public enum AnimantionScene
	{
		None,
		Splash,
		First,
		Second,
		Third,
		Fourth,
		Fifth
	}

	private GameObject[] asteroids = new GameObject[4];

	private void Awake()
	{
        //Screen.SetResolution((int)GameSetup.GameScreenSize.x, (int)GameSetup.GameScreenSize.y, Screen.fullScreen);
	}

	private void Start()
	{
		AnimationInitialization();
		GetComponent<AudioSource>().Play();
		//this.splashPos = new Rect(0, 0, Screen.width, Screen.height);
		currentScene = AnimantionScene.First;
	}

	private void AnimationInitialization()
	{
		timer = Time.time;
		for (int i = 0; i < asteroids.Length; i++)
		{
			asteroids[i] = Instantiate(asteroid, asteroid.transform.position, asteroid.transform.rotation) as GameObject;
		}
		asteroids[0].transform.position = (backGround0.transform.GetComponent<Collider>().bounds.min + Vector3.up * 1.6f + Vector3.left * 2.5f + Vector3.back * 8f);// *Time.deltaTime;
		asteroids[1].transform.position = (backGround0.transform.GetComponent<Collider>().bounds.min + Vector3.up * 1.0f + Vector3.left * 2.0f + Vector3.back * 5f);// *Time.deltaTime;
		asteroids[2].transform.position = (backGround0.transform.GetComponent<Collider>().bounds.min + Vector3.up * 1.4f + Vector3.left * 1.0f + Vector3.back * 4f);// *Time.deltaTime;
		asteroids [3].transform.position = (backGround0.transform.GetComponent<Collider> ().bounds.min + Vector3.up * 0.8f + Vector3.left * 0.5f + Vector3.back * 2f);// *Time.deltaTime;

		asteroids[0].transform.localScale = new Vector3(0.75f, 0.77f, 0.01f);
		asteroids[1].transform.localScale = new Vector3(0.479f, 0.5f, 0.01f);
		asteroids[2].transform.localScale = new Vector3(0.27f, 0.28f, 0.01f);
		asteroids[3].transform.localScale = new Vector3(0.19f, 0.2f, 0.01f);
	}

	private float timer = 0f;

	// Update is called once per frame
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			if (currentScene == AnimantionScene.Splash)
			{
				if (!GetComponent<AudioSource>().isPlaying)
					GetComponent<AudioSource>().Play();

				this.fadeOut = false;
				this.fadeWait = false;
				this.fadeIn = true;
			}
			else
				GoToMenu();
		}

		switch (currentScene)
		{
			case AnimantionScene.First:

				for (int i = 0; i < asteroids.Length; i++)
				{
					asteroids[i].transform.Translate(new Vector3(-.1f * (10f / (i + 1)) * Time.deltaTime, .04f * (10f / (i + 1)) * Time.deltaTime, 0));
				}

				cam.transform.position = backGround0.transform.position + Vector3.back * 10;
				if ((Time.time - timer) > 7f)
				{
					for (int i = 0; i < asteroids.Length; i++)
					{
						Destroy(asteroids[i]);
					}
					NextScene();
				}
				shipAngle = (float)((shipAngle + shipWaveVel) % 180);
				shipSinAngle = (float)(Mathf.Sin(shipAngle) * 0.1);
				backGround0.transform.Translate(new Vector3(0, shipSinAngle * Time.deltaTime, 0));
				ship.transform.Translate(new Vector3(-.04f * Time.deltaTime, 0, 0));
				break;

			case AnimantionScene.Second:
				cam.transform.position = backGround1.transform.position + Vector3.back * 10;
				if ((Time.time - timer) > 5f)
				{
					NextScene();
				}
				shipAngle = (float)((shipAngle + shipWaveVel) % 180);
				shipSinAngle = (float)(Mathf.Sin(shipAngle) * 20);
				backGround1.transform.Rotate(new Vector3(0, 0, shipSinAngle * Time.deltaTime));
				asteroid.transform.Rotate(new Vector3(0, 0, -20f * Time.deltaTime));
				break;

			case AnimantionScene.Third:
				cam.transform.position = backGround2.transform.position + Vector3.back * 10;
				if ((Time.time - timer) > 5f)
				{
					NextScene();
				}
				shipAngle = (float)((shipAngle + shipWaveVel) % 180);
				shipSinAngle = (float)(Mathf.Sin(shipAngle) * 20);
				backGround2.transform.Rotate(new Vector3(0, 0, shipSinAngle * Time.deltaTime));
				terra.transform.Rotate(new Vector3(0, -20f * Time.deltaTime, 0));
				//terra.transform.Translate (new Vector3 (-.0008f, -.0005f, 0));
				break;

			case AnimantionScene.Fourth:
				cam.transform.position = backGround3.transform.position + Vector3.back * 10;
				if ((Time.time - timer) > 6f)
				{
					NextScene();
				}
				ship_falling.transform.Translate(new Vector3(-Time.deltaTime * 0.1f, -Time.deltaTime * 0.06f, 0));

				break;

			case AnimantionScene.Fifth:
				cam.transform.position = backGround4.transform.position + Vector3.back * 10;
				if ((Time.time - timer) > 8f)
				{
					GoToMenu();
				}
				shipAngle = (float)((shipAngle + shipWaveVel / 4) % 180);
				shipSinAngle = (float)(Mathf.Sin(shipAngle) * 50f);
				head.transform.Rotate(new Vector3(0, 0, shipSinAngle * Time.deltaTime));
				break;

			default:
				break;
		}
	}

	private void GoToMenu()
	{
		currentScene = AnimantionScene.None;
		if (!this.fadeMode)
		{
			this.alpha = 0f;
			this.fadeOut = false;
			this.fadeWait = false;
			this.fadeIn = true;
			this.fadeMode = true;
			//MovieTexture asd = new MovieTexture();
			//var a123 = asd.duration;
		}
	}

	private void NextScene()
	{
		timer = Time.time;
		this.currentScene++;
	}

	private void OnGUI()
	{
		if (this.fadeMode)
		{
			if (currentScene == AnimantionScene.Splash)
			{
				GUI.DrawTexture(this.splashPos, this.splashLogo);
			}

			Color c = GUI.color;
			c.a = this.alpha;
			GUI.color = c;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.fadeTexture);

			if (fadeOut)
			{
				this.alpha -= this.fadeSpeed * Time.deltaTime;

				if (this.alpha <= 0f)
				{
					this.alpha = 0f;
					this.fadeOut = false;
					this.fadeWait = true;
				}
			}
			else if (fadeWait)
			{
				this.currentSplashTime += Time.deltaTime;

				if (this.currentSplashTime > splashTime)
				{
					this.fadeWait = false;
					this.fadeIn = true;
					GetComponent<AudioSource>().Play();
				}
			}
			else if (fadeIn)
			{
				this.alpha += this.fadeSpeed * Time.deltaTime;

				if (currentScene == AnimantionScene.None)
				{
					GetComponent<AudioSource>().volume -= Time.deltaTime * 0.1f;
				}

				if (this.alpha >= 1.0f)
				{
					this.fadeIn = false;
					AnimationInitialization();
					this.alpha = 1.0f;
					this.fadeMode = false;

					if (currentScene == AnimantionScene.None)
					{
						GetComponent<AudioSource>().Stop();
						AutoFade.LoadLevel("TutorialScene", 0, 0, Color.white);
					}
					else
					{
						currentScene = AnimantionScene.First;
					}
				}
			}
		}
	}
}