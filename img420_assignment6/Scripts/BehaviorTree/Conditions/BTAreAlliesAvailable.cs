using Godot;

public partial class BTAreAlliesAvailable : BTNode
{
    public override BTState Tick(Enemy enemy, double delta)
    {
        return enemy.AlliesAvailable
            ? BTState.Success
            : BTState.Failure;
    }
}

