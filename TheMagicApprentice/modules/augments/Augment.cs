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
    public AugmentEffect[] _augmentEffects = new AugmentEffect[3];

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
