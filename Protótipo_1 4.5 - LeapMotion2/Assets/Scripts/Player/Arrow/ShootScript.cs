using UnityEngine;

public class ShootScript : MonoBehaviour
{
	public GameObject ProjectilePrefab;
	private GameObject mira;
	private AudioClip shoot;
	private PlayerMovement pm;

	// Use this for initialization
	private void Start()
	{
		mira = GameObject.Find("Mira");
		shoot = Resources.Load("SoundFX/Misc/shoot") as AudioClip;
		pm = this.gameObject.GetComponent<PlayerMovement>();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Shoot();
		}
	}

	public void Shoot()
	{
		Instantiate(ProjectilePrefab, new Vector3(transform.position.x + 1.2f, transform.position.y + 1, transform.position.z), ProjectilePrefab.transform.rotation);
		AudioSource.PlayClipAtPoint(shoot, Camera.main.transform.position);

		pm.Animator.currentState = PlayerAnimation.State.Shoot;
		pm.Animator.GetComponent<Animation>().Play(PlayerAnimation.JUMP);
	}
}