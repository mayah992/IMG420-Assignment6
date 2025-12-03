using Godot;

public partial class BTCanAttack : BTNode
{
    public override BTState Tick(Enemy enemy, double delta)
	{
		if (enemy.Player == null || !GodotObject.IsInstanceValid(enemy.Player))
			return BTState.Failure;

		return enemy.CanAttack() ? BTState.Success : BTState.Failure;
	}

}
