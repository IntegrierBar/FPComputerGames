using Godot;
using System;

/**
A tool class for generating flashlight textures in the Godot editor.
This class extends PointLight2D and provides functionality to create custom flashlight beam textures.
*/
[Tool]
public partial class FlashLightTextureGenerator : PointLight2D
{
	[Export] public int TextureSize = 256; ///< The size of the generated texture in pixels
	[Export(PropertyHint.Range, "0,180")] public float BeamAngle = 38.0f; ///< The angle of the flashlight beam in degrees
	[Export] public Color BeamColor = new Color(1, 1, 1, 1); ///< The color of the flashlight beam

	[Export] public bool GenerateTexture = false; ///< Flag to trigger texture generation in the editor

	/**
	Called when the node enters the scene tree.
	Prints a ready message for debugging purposes.
	*/
	public override void _Ready()
	{
		GD.Print("FlashlightTextureGenerator is ready");
	}

	/**
	Called every frame to process the node.
	In the editor, this method checks if texture generation is requested and generates the texture if needed.
	*/
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			if (GenerateTexture)
			{
				GD.Print("Generating flashlight texture");
				GenerateFlashlightTexture();
				GenerateTexture = false;
				// Notify the editor that a property has changed
				NotifyPropertyListChanged();
			}
		}
	}

	/**
	Generates the flashlight texture based on the current settings.
	Creates an image, fills it with the beam pattern, and saves it as a texture resource.
	*/
	private void GenerateFlashlightTexture()
	{
		var image = Image.Create(TextureSize, TextureSize, false, Image.Format.Rgba8);
		var center = new Vector2(TextureSize / 2, TextureSize / 2);
		float radius = TextureSize / 2;

		for (int x = 0; x < TextureSize; x++)
		{
			for (int y = 0; y < TextureSize; y++)
			{
				Vector2 pixelPos = new Vector2(x, y) - center;
				float angle = Mathf.Abs(pixelPos.Angle()) * (180 / Mathf.Pi);
				float distance = pixelPos.Length();

				if (angle <= BeamAngle / 2 && distance <= radius)
				{
					float t = 1 - (distance / radius);
					float angleT = 1 - (angle / (BeamAngle / 2));
					float alpha = t * angleT * BeamColor.A;
					Color pixelColor = new Color(BeamColor.R, BeamColor.G, BeamColor.B, alpha);
					image.SetPixel(x, y, pixelColor);
				}
			}
		}

		var texture = ImageTexture.CreateFromImage(image);
		var resourceSaver = ResourceSaver.Singleton;
		var error = resourceSaver.Save(texture, "res://modules/entities/player/spells/sun_beam/flashlight_texture.tres");

		// Set the generated texture to this PointLight2D node
		Texture = texture;
		
		if (error != Error.Ok)
		{
			GD.PrintErr("An error occurred while saving the texture.");
		}
		else
		{
			GD.Print("Flashlight texture generated and saved successfully.");
		}
	}
}
