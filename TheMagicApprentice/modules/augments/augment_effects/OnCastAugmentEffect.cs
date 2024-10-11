using Godot;
using System;


/**
Base class for all augments whose effects are only activated when a spell is cast
*/
[GlobalClass]
public partial class OnCastAugmentEffect : AugmentEffect
{
    public virtual void OnCast(Spell spell, Vector2 playerPosition, Vector2 targetPosition) {}
}
