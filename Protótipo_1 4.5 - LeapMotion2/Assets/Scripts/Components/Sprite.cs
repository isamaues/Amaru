using UnityEngine;

public class Sprite : TwoDComponent
{
	private Texture2D texture;

	public Texture2D Textura
	{
		get { return texture; }
		set
		{
			this.texture = value;
			AdjustRectangle();
		}
	}

	public Sprite(Vector2 position, Texture2D texture)
		: base(new Rect(position.x, position.y, texture.width, texture.height))
	{
		Textura = texture;
	}

	public Sprite(Rect position, Texture2D texture)
		: base(position)
	{
		Textura = texture;
	}

	public override void Draw()
	{
		GUI.DrawTexture(this.realRect, this.texture);
	}
}