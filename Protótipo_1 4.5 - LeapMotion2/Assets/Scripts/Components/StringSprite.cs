using UnityEngine;

public class StringSprite : TwoDComponent
{
	public string Text { get; set; }

	public StringSprite(Rect position, string text)
		: base(position)
	{
		Text = text;
		AdjustRectangle();
	}

	public override void Draw()
	{
		GUI.Label(this.realRect, Text);
	}
}