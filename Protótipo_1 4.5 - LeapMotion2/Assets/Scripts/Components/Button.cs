using UnityEngine;

public class Button : TwoDComponent
{
	public string Name { get; set; }

	public Button(Vector2 centralPosition, string name, Vector2 size)
		: base(new Rect(centralPosition.x - size.x / 2, centralPosition.y - size.y / 2, size.x, size.y))
	{
		Name = name;
		AdjustRectangle();
	}

	public override void Draw()
	{
		throw new System.NotImplementedException();
	}

	public bool OnGUI()
	{
		return GUI.Button(this.realRect, Name);
	}

	public void Select()
	{
		this.realRect.x -= 15;
		this.realRect.width += 30;

		this.realRect.y -= 4;
		this.realRect.height += 8;
	}

	public void Diselect()
	{
		this.realRect.x += 15;
		this.realRect.width -= 30;

		this.realRect.y += 4;
		this.realRect.height -= 8;
	}
}