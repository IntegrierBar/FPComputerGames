using Godot;
using System;


/**
The base class for all augment effects
*/
[GlobalClass]
public partial class AugmentEffect : Resource
{
    /**
    This function gets called whenever the player equips an augment.
    Gets reference to the current SceneTree in order to access Groups
    */
    public virtual void Equip(SceneTree sceneTree) {}

    /**
    This function gets called if the player unequips the augment.
    It is responsible for the cleanup
    */
    public virtual void UnEquip(SceneTree sceneTree) {}
}
