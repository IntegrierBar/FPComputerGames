using Godot;
using System;


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

    public AugmentEffect[] _augmentEffects = new AugmentEffect[3]; // Array of the AugmentEffects

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
}