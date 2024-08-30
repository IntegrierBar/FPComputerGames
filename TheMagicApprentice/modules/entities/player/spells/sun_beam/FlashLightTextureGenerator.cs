using Godot;
using System;

[Tool]
public partial class FlashLightTextureGenerator : Sprite2D
{
	[Export] public int TextureSize = 256;
	[Export(PropertyHint.Range, "0,180")] public float BeamAngle = 38.0f;
	[Export] public Color BeamColor = new Color(1, 1, 1, 1);

	[Export] public bool GenerateTexture = false;
	public override void _Ready()
	{
		GD.Print("FlashlightTextureGenerator is ready");
	}

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

		// Set the generated texture to this Sprite2D node
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
