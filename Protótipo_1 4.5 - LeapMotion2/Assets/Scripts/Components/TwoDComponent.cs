using UnityEngine;

public abstract class TwoDComponent
{
	private Rect posRect;
	protected Rect realRect;

	public Vector2 Size
	{
		get { return new Vector2(realRect.width, realRect.height); }
	}

	public Vector2 Position2
	{
		get { return new Vector2(realRect.x, realRect.y); }

		set
		{
			this.posRect.x = value.x;
			this.posRect.y = value.y;

			this.realRect.x = posRect.x;
			this.realRect.y = posRect.y;
		}
	}

	public Vector3 Position3
	{
		get { return new Vector3(posRect.x, posRect.y, 0f); }

		set
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(value);

			this.posRect.x = vector.x;
			this.posRect.y = Screen.height - vector.y;

			this.realRect.x = posRect.x;
			this.realRect.y = posRect.y;
		}
	}

	public TwoDComponent(Rect position)
	{
		this.posRect = position;
		realRect = posRect;
	}

	public Rect Rectangle
	{
		get { return this.realRect; }
		set { this.realRect = value; }
	}

	public abstract void Draw();

	public void AdjustRectangle()
	{
		float FilScreenWidth = posRect.width / 800;
		float rectWidth = FilScreenWidth * Screen.width;
		float FilScreenHeight = posRect.height / 600;
		float rectHeight = FilScreenHeight * Screen.height;

		realRect.x = (posRect.x / 800) * Screen.width;
		realRect.y = (posRect.y / 600) * Screen.height;

		this.realRect.width = rectWidth;
		this.realRect.height = rectHeight;
	}
}