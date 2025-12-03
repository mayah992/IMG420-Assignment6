using Godot;

public partial class BTChasePlayer : BTNode
{
    [Export]
    public float Speed = 50f;

    public override BTState Tick(Enemy enemy, double delta)
	{
		if (enemy.Player == null || !GodotObject.IsInstanceValid(enemy.Player))
			return BTState.Failure;

		Vector2 dir = (enemy.Player.GlobalPosition - enemy.GlobalPosition).Normalized();
		enemy.Velocity = dir * Speed;
		enemy.MoveAndSlide();

		enemy.SetStateLabel("CHASING");
		return BTState.Success;
	}
}

