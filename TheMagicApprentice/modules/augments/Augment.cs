using Godot;
using System;
using System.Linq;


/**
The Augment class
Every augment is a resource using this script
Each Augment can have up to 3 Augment effects
*/
[GlobalClass]
public partial class Augment : Resource
{
    [Export]
    public string Description = ""; ///< the discription of the augment that is displayed when hovering over it

    public AugmentEffect[] _augmentEffects = new AugmentEffect[3]; // Array of the AugmentEffects. It is public but should only be accessed with Get/SetAugmentEffect

    /**
    Equips the Augment by calling Equip for each AugmentEffect inside _augmentEffects
    */
    public void Equip(SceneTree sceneTree)
    {
        foreach (var augmentEffect in _augmentEffects)
        {
            if (augmentEffect is not null)
            {
                augmentEffect.Equip(sceneTree);
            }
        }
    }

    /**
    UnEquips the Augment by calling UnEquip for each AugmentEffect inside _augmentEffects
    */
    public void UnEquip(SceneTree sceneTree)
    {
        foreach (var augmentEffect in _augmentEffects)
        {
            if (augmentEffect is not null)
            {
                augmentEffect.UnEquip(sceneTree);
            }
        }
    }

    /**
    Build the description of the Augment from the AugmentEffects
    */
    public void BuildDescription()
    {
        Description = "";
        foreach (AugmentEffect effect in _augmentEffects)
        {
            if (effect is not null)
            {
                Description += "\n" + effect.Description();
            }
        }
    }

    /**
    Get the AugmentEffect at index. Returns null if outside of range and prints a debug message
    */
    public AugmentEffect GetAugmentEffect(int index)
    {
        if (0 <= index && index < _augmentEffects.Length)
        {
            return _augmentEffects[index];
        }
        System.Diagnostics.Debug.Print("Trying to access augment effect outside of range");
        return null;
    }

    /**
    Set the AugmentEffect at index. Also updates the Description.
    Prints a debug message if outside of range
    */
    public void SetAugmentEffect(int index, AugmentEffect augmentEffect)
    {
        if (0 <= index && index < _augmentEffects.Length)
        {
            _augmentEffects[index] = augmentEffect;
            BuildDescription(); // rebuild the description
        }
        System.Diagnostics.Debug.Print("Trying to set augmenteffect outside of range");
    }
}
