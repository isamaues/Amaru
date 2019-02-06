using UnityEngine;

public class JetPackScript : MonoBehaviour
{
	public GameObject jetPackObjet;

	public void EnableJetPack()
	{
		jetPackObjet.SetActive(true);
	}

	public void DisableJetPack()
	{
		jetPackObjet.SetActive(false);
	}
}