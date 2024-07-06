using Godot;
using System;


/**
\brief Class for the Spell Box that shows the currently active spell
*/
public partial class UISpellBox : PanelContainer
{
	[Export]
	public Texture2D StartingTexture;
	// Reference to the textureRect where the icon is shown
	private TextureRect _textureRect;

	/**
	Set the reference to the textureRect
	*/
    public override void _Ready()
    {
        _textureRect = GetNode<TextureRect>("TextureRect");
		_textureRect.Texture = StartingTexture;
    }

	/**
	Set the texture of the textureRect
	*/
    public void SetSpell(Texture2D icon)
	{
		_textureRect.Texture = icon;
	}
}
