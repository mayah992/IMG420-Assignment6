using Godot;

public partial class BTIsPlayerInRange : BTNode
{
    // Inspector-configurable range
    [Export] public float Range = 100f;

    public override BTState Tick(Enemy enemy, double delta)
    {
        if (enemy.Player == null || !GodotObject.IsInstanceValid(enemy.Player))
            return BTState.Failure;

        float distance = enemy.GlobalPosition.DistanceTo(enemy.Player.GlobalPosition);

        return distance <= Range ? BTState.Success : BTState.Failure;
    }
}

	
