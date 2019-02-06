using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TaskNotifycationType
{
	None,
	View,
	Spoken,
	Written,
	ViewSpoken,
	WrittenSpoken,
	ReforceA,
	ReforceI,
	ReforceAI
}

public class UramaBehaviour : MonoBehaviour
{
    public GameObject gameManager;

	private static UramaBehaviour instance = null;
	public static UramaBehaviour Instance {
		get { return instance; }
	}
	void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
	}

	public GameObject player;
	public Vector3 distanceFromPlayer;
	public float amplitude = 1f;
	public float velocity = 2f;
	public float smoothRange = 2f;
	public float smoothRangePingPong = 3f;
	public static bool OnMousePressing;
	public Font uramaFont;
	public int fontSize;
    public AudioClip[] audiosAntesTarefasA;
    public AudioClip[] audiosAntesTarefasB;
    public AudioClip[] audiosAntesTarefasC;
    public AudioClip[] audiosAcerto;
    public AudioClip[] audiosErro;
    public AudioClip[] audiosTenteDeNovo;

    private int tentativas = 0;
    private bool flagFalaAntesTarefa = false;
	private Vector3 _intDistFromPlayer;
	private GUIStyle labelStyle;
	/*pode ser retirado*/
	public Vector3 ballonDistanceOffset = new Vector3(2, 4.7f, 0);
	public Texture2D ballonTexture;
	/*pode ser retirado*/
	private Sprite ballonSprite;
	/*pode ser retirado*/
	private Sprite imageStimulusSprite;
	/*pode ser retirado*/
	private StringSprite textStimulusSprite;
	private bool lastRunning;
	/*pode ser retirado*/
	private int lastScreenWidth;
	public TaskNotifycationType notificationType = TaskNotifycationType.None;

	// Mod Paulo
	private GameObject ballonPrefab;

	private GameObject textPrefab;
	private GameObject ballon;
	private bool isBallonInstanceOn = false;
	public Vector3 newballonDistanceOffset = new Vector3(7, 1, 3);
	public Vector3 ballonSize = new Vector3(2, 1.5f, 0.1f);
	private Texture2D imageStimulusTexture;
	private string textStimulusString;
	private float tamanhoAntigo;

	private ITaskManager taskManager = TaskManagerInstance.Instance;

	private string lastType;
	private float timeLastReforce;

	public void Notify(string type)
	{
		Debug.Log("NOTIFY: " + type);
		switch (type)
		{
		case "A":
			notificationType = TaskNotifycationType.Spoken;
			break;
		case "B":
			notificationType = TaskNotifycationType.View;
			break;
		case "C":
			notificationType = TaskNotifycationType.Written;
			break;
		case "F":
			notificationType = TaskNotifycationType.ViewSpoken;
			break;
		case "G":
			notificationType = TaskNotifycationType.WrittenSpoken;
			break;
		case "YA":
		case "NA":
			notificationType = TaskNotifycationType.ReforceA;
			break;
		case "YI":
		case "NI":
			notificationType = TaskNotifycationType.ReforceI;
			break;
		case "YAI":
		case "NAI":
			notificationType = TaskNotifycationType.ReforceAI;
			break;
		default:
			notificationType = TaskNotifycationType.None;
			break;
		}

        if (type[0].ToString() ==  "N" || type[0].ToString() == "Y") {

            if (type[0].ToString() == "Y") {
                StartCoroutine(PlayCorrectOrWrong(true));
                flagFalaAntesTarefa = false;
                tentativas = 0;
            }
            else {
                StartCoroutine(PlayCorrectOrWrong(false));
                tentativas++;
                if (tentativas >= 3) {
                    flagFalaAntesTarefa = false;
                }
            }

            if (type.Contains("I"))
			{
				if (!isBallonInstanceOn){
					ballon = createImageBallon(imageStimulusTexture, transform.position + newballonDistanceOffset + new Vector3(10,0,0), ballonPrefab);
					distancia = new Vector3(10,0,0);
					isBallonInstanceOn = true;
				}
				if (ballon.GetComponent<MeshRenderer>().enabled == false)
				{
					ballon.GetComponent<MeshRenderer>().enabled = true;
				}
				ballon.GetComponent<Renderer>().material.mainTexture = reforcoI[type[0].ToString() + "I"];
			}

			if (type.Contains("A"))
			{
				AudioSource.PlayClipAtPoint(reforcoA[type[0].ToString() + "A"], Camera.main.transform.position);
			}

			timeLastReforce = Time.time;
		}
		else
		{
            Debug.Log("Mandou estimulo");
			lastType = type;
			imageStimulusTexture = taskManager.GetTextureById(taskManager.GetCurrentTask().Model);
			if (isBallonInstanceOn )
            {
				if (notificationType == TaskNotifycationType.Spoken || notificationType == TaskNotifycationType.ViewSpoken || notificationType == TaskNotifycationType.WrittenSpoken)
				{
                    GameObject.Destroy(ballon);
                    isBallonInstanceOn = false;
				}
				else
					ballon.GetComponent<Renderer>().material.mainTexture = imageStimulusTexture;
            }
			textStimulusString = taskManager.GetWordById(taskManager.GetCurrentTask().Model);
            
            if (notificationType == TaskNotifycationType.Spoken || notificationType == TaskNotifycationType.ViewSpoken || notificationType == TaskNotifycationType.WrittenSpoken)
            {
                Debug.Log("hu3hu3hu33hu: ");
                StartCoroutine(PlaySoundModel());
            }else {
                StartCoroutine(PlayBeforeModel());
            }
		}
	}

	Dictionary<string, Texture2D> reforcoI;
	Dictionary<string, AudioClip> reforcoA;
	Vector3 distancia;

	private void Start()
	{
		reforcoI = new Dictionary<string, Texture2D>();

		reforcoI.Add("NI",(Texture2D) Resources.Load("Reforco/ImageN"));
		reforcoI.Add("YI",(Texture2D) Resources.Load("Reforco/ImageY"));

		reforcoA = new Dictionary<string, AudioClip>();
		reforcoA.Add("NA",(AudioClip) Resources.Load("Reforco/SoundN"));
		reforcoA.Add("YA",(AudioClip) Resources.Load("Reforco/SoundY"));

		//Mod Paulo
		ballonPrefab = Resources.Load("Prefabs/BallonPrefab") as GameObject;
		//ballonPrefab = Resources.Load("Prefabs/CubePrefab") as GameObject;
		textPrefab = Resources.Load("Prefabs/TextPrefab") as GameObject;



		OnMousePressing = false;
		/*pode ser retirado*/
		//this.ballonSprite = new Sprite(new Rect(0, 0, 236, 175), ballonTexture);
		/*pode ser retirado*/
		//imageStimulusSprite = new Sprite(new Rect(0, 0, 170, 145), taskManager.GetTextureById(1));
		/*pode ser retirado*/
		//textStimulusSprite = new StringSprite(new Rect(0, 0, 400, 100), "Ola mundo");

		/*pode ser retirado*/
		this.lastScreenWidth = Screen.width;
		this.tamanhoAntigo = Camera.main.orthographicSize;
	}

	// Update is called once per frame
	private void Update()
	{
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 2.5f);

		if (MinigameSetup.RunningTask)
		{
			if (notificationType == TaskNotifycationType.View || notificationType == TaskNotifycationType.ViewSpoken 
			    || notificationType == TaskNotifycationType.ReforceAI || notificationType == TaskNotifycationType.ReforceI)
			{
				// Mod Paulo
				if (!isBallonInstanceOn)
				{
					ballon = createImageBallon(imageStimulusTexture, transform.position + newballonDistanceOffset, ballonPrefab);
					isBallonInstanceOn = true;
				}
				else
				{//
                   // ballon.GetComponent<Renderer>().material.mainTexture = imageStimulusTexture;
					ballon.transform.position = transform.position + newballonDistanceOffset + (Vector3.right * (float)((int)Camera.main.orthographicSize / 4 ^ 2));
					if (tamanhoAntigo != Camera.main.orthographicSize)
					{
						ballon.transform.localScale = ballonSize * Camera.main.orthographicSize / 4;
						//						Debug.Log(Camera.main.orthographicSize);
						tamanhoAntigo = Camera.main.orthographicSize;
						distancia = ballon.transform.position - transform.position;
					}
				}
			}
			else if (notificationType == TaskNotifycationType.Written || notificationType == TaskNotifycationType.WrittenSpoken)
			{
				//Mod Paulo
				if (!isBallonInstanceOn)
				{
					ballon = createTextBallon(textStimulusString, transform.position + newballonDistanceOffset, ballonPrefab);
					isBallonInstanceOn = true;
				}
				else
				{
					ballon.GetComponent<MeshRenderer>().enabled = false;
					ballon.transform.position = transform.position + newballonDistanceOffset + (Vector3.right * (float)((int)Camera.main.orthographicSize / 4 ^ 2));
					if (tamanhoAntigo != Camera.main.orthographicSize)
					{
						ballon.transform.localScale = ballonSize * Camera.main.orthographicSize / 4;
						//						Debug.Log(Camera.main.orthographicSize);
						tamanhoAntigo = Camera.main.orthographicSize;

						distancia = ballon.transform.position - transform.position;
					}
				}
			}

			if (notificationType == TaskNotifycationType.ReforceA || notificationType == TaskNotifycationType.ReforceAI ||
			    notificationType == TaskNotifycationType.ReforceI)
			{
				if (Time.time - timeLastReforce > 2f)
				{
                    Debug.Log("!@# + " + lastType);
					Notify(lastType);
				}
			}

			TopScreenMoviment();
		}
		else
		{
			if (notificationType == TaskNotifycationType.ReforceA)
			{
				notificationType = TaskNotifycationType.None;
			}
			bool flag = false;
			if (notificationType == TaskNotifycationType.ReforceAI ||
			    notificationType == TaskNotifycationType.ReforceI)
			{
				flag = true;
				if (Time.time - timeLastReforce > 2f)
				{
					notificationType = TaskNotifycationType.None;
				}

			}
			if (player.transform.rotation.eulerAngles.y > 180)
			{
				_intDistFromPlayer = new Vector3(-distanceFromPlayer.x, distanceFromPlayer.y, distanceFromPlayer.z);
				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
			else
			{
				_intDistFromPlayer = new Vector3(distanceFromPlayer.x, distanceFromPlayer.y, distanceFromPlayer.z);
				transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
			if (isBallonInstanceOn && !flag)
			{
				GameObject.Destroy(ballon);
				isBallonInstanceOn = false;
			}
			PingPongMoviment();
		}
	}

	private void PingPongMoviment()
	{
		Vector3 finalPosition = new Vector3(_intDistFromPlayer.x + player.transform.position.x, (_intDistFromPlayer.y + player.transform.position.y)
				+ Mathf.PingPong(Time.time * velocity, amplitude), _intDistFromPlayer.z + player.transform.position.z);

		transform.position = Vector3.Slerp(transform.position, finalPosition, smoothRangePingPong * Time.deltaTime);

		if(isBallonInstanceOn)
		{
			ballon.transform.position = transform.position + distancia;
			if (tamanhoAntigo != Camera.main.orthographicSize)
			{
				ballon.transform.localScale = ballonSize * Camera.main.orthographicSize / 4;
				//						Debug.Log(Camera.main.orthographicSize);
				tamanhoAntigo = Camera.main.orthographicSize;
				
			}
		}
	}

	private void TopScreenMoviment()
	{
		Vector3 topRightScreenToWorldPos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.9f, 0f));
		transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		_intDistFromPlayer = new Vector3(topRightScreenToWorldPos.x + transform.GetComponent<Renderer>().bounds.extents.x,
											topRightScreenToWorldPos.y - (transform.GetComponent<Renderer>().bounds.extents.y + 1),
											distanceFromPlayer.z);

		Vector3 finalPosition = new Vector3(_intDistFromPlayer.x, (_intDistFromPlayer.y), _intDistFromPlayer.z);

		transform.position = Vector3.Slerp(transform.position, finalPosition, smoothRange * Time.deltaTime);
	}

	private void OnMouseDown()
	{
		OnMousePressing = true;

        if (MinigameSetup.RunningTask && notificationType == TaskNotifycationType.Spoken)
        {
            AudioSource.PlayClipAtPoint(taskManager.GetAudioById(BaseMiniGame.CurrentTaskModel), Camera.main.transform.position);
        }
	}

	private void OnMouseUp()
	{
		OnMousePressing = false;
	}

	// Mod Paulo
	private GameObject createImageBallon(Texture2D texture, Vector3 pos, GameObject source)
	{
		source.GetComponent<Renderer>().enabled = true;

		GameObject inst = GameObject.Instantiate(source, pos, Quaternion.identity) as GameObject;
		inst.name = "ballon";

		inst.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
		inst.GetComponent<Renderer>().material.mainTexture = texture;

		inst.transform.Rotate(new Vector3(0, 0, 180f));

		return inst;
	}

	// Mod Paulo
	private GameObject createTextBallon(string name, Vector3 pos, GameObject source)
	{
		source.GetComponent<Renderer>().enabled = false;

		GameObject inst = GameObject.Instantiate(source, pos, Quaternion.identity) as GameObject;
		inst.name = "ballon";

		inst.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));

		inst.transform.Rotate(new Vector3(0, 0, 180f));

		GameObject textObject = GameObject.Instantiate(textPrefab, pos, Quaternion.identity) as GameObject;

		TextMesh gText = textObject.GetComponent(typeof(TextMesh)) as TextMesh;
		gText.text = name;
		gText.GetComponent<Renderer>().material.color = Color.black;
		gText.fontSize = 50;

		Bounds textBounds = gText.GetComponent<Renderer>().bounds;

		while (textBounds.extents.x * 2 + 0.5f > inst.GetComponent<Renderer>().bounds.extents.x * 2)
		{
			gText.transform.localScale *= 1f - (0.01f);
			textBounds = gText.GetComponent<Renderer>().bounds;
		}
		textObject.transform.parent = inst.transform;
		textObject.transform.position = inst.transform.position
						+ new Vector3(-textBounds.extents.x * 2 / 2f, textBounds.size.y / 2f, 0f);

		textObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/3D Text Shader");

		return inst;
	}

    /*Necessário para que classes que não herdam da classe MonoBehaviour "possam criar" uma Coroutine do aúdio*/
    public void StartPlaySoundModel()
    {
        StartCoroutine(PlaySoundModel());
    }

    private bool teste = false;
    private IEnumerator PlaySoundModel()
    {
        gameManager.GetComponent<AudioSource>().mute = true;

        if (!flagFalaAntesTarefa) {
            AudioClip audio1 = audiosAntesTarefasA[Random.Range(0, audiosAntesTarefasA.Length)];

            yield return new WaitForSeconds(0.5f);
            AudioSource.PlayClipAtPoint(audio1, Camera.main.transform.position);
            yield return new WaitForSeconds(audio1.length + 0.5f);
            flagFalaAntesTarefa = true;
        }
        
        while (teste) {
            yield return new WaitForSeconds(0.1f);
        }

        AudioClip audio2 = taskManager.GetAudioById(taskManager.GetCurrentTask().Model);

        yield return new WaitForSeconds(0.5f);
        AudioSource.PlayClipAtPoint(audio2, Camera.main.transform.position);
        yield return new WaitForSeconds(audio2.length + 0.5f);
        gameManager.GetComponent<AudioSource>().mute = false;
        
    }
    
    private IEnumerator PlayBeforeModel() {
        if (!flagFalaAntesTarefa) {
            AudioClip audio1 = null;

            switch (notificationType) {
                case TaskNotifycationType.View:
                    Debug.Log("View");
                    audio1 = audiosAntesTarefasB[Random.Range(0, audiosAntesTarefasB.Length)];
                    break;
                case TaskNotifycationType.Written:
                    Debug.Log("Written");
                    audio1 = audiosAntesTarefasC[Random.Range(0, audiosAntesTarefasC.Length)];
                    break;
                case TaskNotifycationType.ViewSpoken:
                    Debug.Log("ViewSpoken");
                    break;
                case TaskNotifycationType.WrittenSpoken:
                    Debug.Log("WrittenSpoken");
                    break;
                default:
                    Debug.Log("None");
                    break;
            }

            if (audio1 != null) {
                yield return new WaitForSeconds(0.5f);
                AudioSource.PlayClipAtPoint(audio1, Camera.main.transform.position);
                yield return new WaitForSeconds(audio1.length);
            }

            flagFalaAntesTarefa = true;
        }
    }

    private IEnumerator PlayCorrectOrWrong(bool correct) {
        AudioClip audio3;
        if (correct) {
            audio3 = audiosAcerto[Random.Range(0, audiosAcerto.Length)];
            yield return new WaitForSeconds(0.5f);
            AudioSource.PlayClipAtPoint(audio3, Camera.main.transform.position);
            yield return new WaitForSeconds(audio3.length + 0.5f);
        }
        else {
            teste = true;
            audio3 = audiosErro[Random.Range(0, audiosErro.Length)];
            yield return new WaitForSeconds(0.5f);
            AudioSource.PlayClipAtPoint(audio3, Camera.main.transform.position);
            yield return new WaitForSeconds(audio3.length + 0.5f);
            if (tentativas < 3) {
                Debug.Log(tentativas);
                audio3 = audiosTenteDeNovo[Random.Range(0, audiosTenteDeNovo.Length)];
                AudioSource.PlayClipAtPoint(audio3, Camera.main.transform.position);
                yield return new WaitForSeconds(audio3.length);
            }else {
                tentativas = 0;
            }
            teste = false;
        }
    }
}