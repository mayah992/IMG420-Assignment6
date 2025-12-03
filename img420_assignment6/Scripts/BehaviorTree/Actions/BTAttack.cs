using Godot;

public partial class BTAttack : BTNode
{
    [Export] public float AttackRange = 40f; // set in inspector

	public override BTState Tick(Enemy enemy, double delta)
	{
		if (!enemy.CanAttack() || enemy.Player == null || !GodotObject.IsInstanceValid(enemy.Player))
			return BTState.Failure;

		enemy.PerformAttack();

		// Enable enemy hitbox briefly
		enemy.GetNode<Area2D>("AttackArea").Monitoring = true;
		enemy.GetTree().CreateTimer(0.5f).Timeout += () =>
		{
			enemy.GetNode<Area2D>("AttackArea").Monitoring = false;
		};

		enemy.SetStateLabel("ATTACK");
		return BTState.Success;
	}




}

