using UnityEngine;
using UnityEngine.UI;

public class Camera2DTracker : MonoBehaviour
{
	#region Atributtes

	public float horizontalCameraLimit = 40.0f;
	public float verticalCameraLimit = 4.0f;
	public float zPosition = -20f;
	public GameObject target;
	public bool blockLeft;
	public bool blockRight;
	public bool blockTop;
	public bool blockBottom;
	public float zoomSpeed = 16.5f;
	private float initHorLimit;
	private float finalHorLimit;
	public static float InicialOrthographicSize;
	public float arrowMinigameSpeed = 3f;
	public float outraVelocidade = 5f;
    public Text aspectText;

	//private float currentOrthographicSize;

	private static float zoom;
	private static int sign;

	private static bool isArrowMiniGame;
	private static bool isFinishingMiniGame;

	#endregion Atributtes

	#region Proprieties

	public static bool IsZoom { get; set; }

	public static bool LockVerticalCamera { get; set; }

	public static bool LockHorizontalCamera { get; set; }

	#endregion Proprieties

	private void Start()
	{
		IsZoom = false;
		InicialOrthographicSize = Camera.main.orthographicSize;
		zoom = InicialOrthographicSize;

		LockVerticalCamera = true;
		LockHorizontalCamera = false;

		initHorLimit = Camera.main.transform.position.x - (horizontalCameraLimit);
		finalHorLimit = Camera.main.transform.position.x;
	}

	private void Update()
	{

        float asd = Camera.main.transform.position.x;
		if (IsZoom)
		{
			Camera.main.orthographicSize += sign * zoomSpeed * Time.smoothDeltaTime;
			//if (isArrowMiniGame) Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(target.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), arrowMinigameSpeed * Time.deltaTime); //Modficao paulo.
			if (isFinishingMiniGame)
			{
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(target.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), arrowMinigameSpeed * Time.deltaTime); //Modficao paulo.
			}
			else
			{
			}

			if ((sign > 0 && Camera.main.orthographicSize >= zoom) || (sign < 0 && Camera.main.orthographicSize <= zoom))
			{
				sign = 0;
				IsZoom = false;
				Camera.main.orthographicSize = zoom;
				isFinishingMiniGame = false;

				asd = Camera.main.transform.position.x;
				initHorLimit = Camera.main.transform.position.x;
				finalHorLimit = Camera.main.transform.position.x + (horizontalCameraLimit);
				asd = ((LockHorizontalCamera) ? 0 : target.transform.position.x - 2f);
			}
		}

		float horCamPosition = asd;
		float verCamPosition = Camera.main.transform.position.y;

		if (target.transform.rotation.eulerAngles.y < 180)
		{
			if (target.transform.position.x > finalHorLimit - 2f)
			{
				initHorLimit = Camera.main.transform.position.x - (horizontalCameraLimit);
				finalHorLimit = Camera.main.transform.position.x;
				horCamPosition = ((LockHorizontalCamera) ? 0 : target.transform.position.x + 2f);
			}
		}
		else
		{
			if (target.transform.position.x < initHorLimit + 2f)
			{
				initHorLimit = Camera.main.transform.position.x;
				finalHorLimit = Camera.main.transform.position.x + (horizontalCameraLimit);
				horCamPosition = ((LockHorizontalCamera) ? 0 : target.transform.position.x - 2f);
			}
		}

		verCamPosition = ((LockVerticalCamera) ? 0 : target.transform.position.y) + (Camera.main.orthographicSize / 1.4f);

		if (blockLeft && Camera.main.transform.position.x - horCamPosition > 0)
			horCamPosition = Camera.main.transform.position.x;

		if (blockRight && Camera.main.transform.position.x - horCamPosition < 0)
			horCamPosition = Camera.main.transform.position.x;

		if (blockBottom && Camera.main.transform.position.y - verCamPosition > 0)
			verCamPosition = Camera.main.transform.position.y;

		if (blockTop && Camera.main.transform.position.y - verCamPosition < 0)
			verCamPosition = Camera.main.transform.position.y;

		Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, new Vector3(horCamPosition, Camera.main.transform.position.y, zPosition), arrowMinigameSpeed * Time.deltaTime);
		Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, new Vector3(Camera.main.transform.position.x, verCamPosition, zPosition), outraVelocidade * Time.deltaTime);
	}

	#region Camera Regions

	public static bool IsOnCamera(GameObject obj, bool onYAxys)
	{
		if (!onYAxys)
			return IsOnCamera(obj);

		return (
				IsOnCamera(obj) ||
				IsCameraUnderObject(obj) ||
				IsCameraOverObject(obj)
				);
	}

	public static bool IsOnCamera(GameObject obj)
	{
		if (obj == null)
			return false;

		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		return GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds);
	}

	public static bool IsCameraUnderObject(GameObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		var pos = BoundsToScreenRect(obj.GetComponent<Collider>().bounds);
		return (pos.x + pos.width >= 0) && (pos.x <= Screen.width) && (pos.y + pos.height < Camera.main.transform.position.y);
	}

	public static bool IsCameraOverObject(GameObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		var pos = BoundsToScreenRect(obj.GetComponent<Collider>().bounds);
		return (pos.x + pos.width >= 0) && (pos.x <= Screen.width) && (pos.y + pos.height > Camera.main.transform.position.y);
	}

	public static Rect BoundsToScreenRect(Bounds bounds)
	{
		Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
		Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));

		return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
	}

	public static float Zoom
	{
		get { return zoom; }
		set
		{
			if (value != 0 && !IsZoom)
			{
				sign = value > 0 ? 1 : -1;

				zoom = Camera.main.orthographicSize + value;
				IsZoom = true;
			}
		}
	}

	#endregion Camera Regions

	public static void ResetCameraZoom(bool finishingMiniGame = false)
	{
		sign = Camera.main.orthographicSize > InicialOrthographicSize ? -1 : 1;

		zoom = InicialOrthographicSize;
		IsZoom = true;

		isFinishingMiniGame = finishingMiniGame;
	}
}