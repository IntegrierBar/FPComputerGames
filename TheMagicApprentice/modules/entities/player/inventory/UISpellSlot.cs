using Godot;
using System;
using System.Linq;


/**
\brief Class for the Spell Box that shows the currently active spell
*/
public partial class UISpellSlot : PanelContainer
{
	[Export]
	public Texture2D StartingTexture;
	// Reference to the progressbar where the icon is shown
	private TextureProgressBar _cooldownProgressBar;
	private Label _remainingCooldownLabel;

	/**
	Set the reference to the textureRect
	*/
    public override void _Ready()
    {
		_cooldownProgressBar = GetNode<TextureProgressBar>("CooldownProgressBar");
		_remainingCooldownLabel = GetNode<Label>("%Label");

		System.Diagnostics.Debug.Assert(_cooldownProgressBar is not null, "_cooldownProgressBar is null");
		System.Diagnostics.Debug.Assert(_remainingCooldownLabel is not null, "_remainingCooldownLabel is null");

		SetSpell(StartingTexture);
		SetPhysicsProcess(false);
    }

	/**
	Set the texture of the textureRect
	*/
    public void SetSpell(Texture2D icon)
	{
		_cooldownProgressBar.TextureProgress = icon;
		_cooldownProgressBar.TextureUnder = icon;
		_cooldownProgressBar.Value = _cooldownProgressBar.MaxValue;

		_remainingCooldownLabel.Visible = false;
	}


    public override void _PhysicsProcess(double delta)
    {
        _cooldownProgressBar.SetValueNoSignal(_cooldownProgressBar.Value + delta);

		_remainingCooldownLabel.Text = ConvertTimeToStringInSeconds(_cooldownProgressBar.MaxValue - _cooldownProgressBar.Value);

		if (_cooldownProgressBar.Value >= _cooldownProgressBar.MaxValue)
		{
			_remainingCooldownLabel.Visible = false;
			SetPhysicsProcess(false);
			// TODO this would be a good place to enable the skill again for casting
		}
    }

	public void Cast(double cooldown)
	{
		_cooldownProgressBar.MaxValue = cooldown;
		_cooldownProgressBar.Value = 0;

		_remainingCooldownLabel.Visible = true;
		_remainingCooldownLabel.Text = ConvertTimeToStringInSeconds(cooldown);

		SetPhysicsProcess(true);
	}

	/**
	Takes a double, rounds it up to the next integer and converts it to string and adds an s at the end.
	Uses System.Globalization.CultureInfo to make sure that we use . and not , to separate the ,
	*/
	public static string ConvertTimeToStringInSeconds(double time)
	{
		if (time < 0)
		{
			time = 0;
		}
		return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N1}", time) + "s";
	}

	/**
	returns true is physics process is enabled, false otherwise.
	This works because the physics process is enabled if and only if we are on cooldown.
	*/
	public bool IsOnCooldown()
	{
		return IsPhysicsProcessing();
	}
}
