using UnityEngine;

public enum GearType
{
	PorcaBronze,
	ParafusoOuro,
	EngrenagemOuro,
	Nada
}

public class GearScript : MonoBehaviour
{
	private GearsManager gC;
	private AudioClip collect;
	private GameObject particlesPref;

	public float smooth = 2.0F;
	public float tiltAngle = 30.0F;

	public GearType type;

	private void Start()
	{
		collect = Resources.Load("SoundFX/Misc/button_pressed") as AudioClip;
		particlesPref = Resources.Load("Prefabs/CollectParticles") as GameObject; ;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Head")
		{
			gC = GameObject.Find("GameManager").GetComponent<GearsManager>();
			gC.Add(type);
			AudioSource.PlayClipAtPoint(collect, Camera.main.transform.position);
			GameObject boom = (GameObject)Instantiate(particlesPref, new Vector3(transform.position.x, transform.position.y, -4), Quaternion.identity);
			Destroy(boom, 3f);
			DestroyObject(this.gameObject);
		}
	}

	private void Update()
	{
		transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * 90f);
	}
}