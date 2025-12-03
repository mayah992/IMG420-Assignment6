using Godot;

public partial class BTFlee : BTNode
{
    public override BTState Tick(Enemy enemy, double delta)
    {
        // Health check first
        if (enemy.HealthRatio > 0.3f)
            return BTState.Failure;

        // Player reference check
        if (enemy.Player == null || !GodotObject.IsInstanceValid(enemy.Player))
            return BTState.Failure; // Now safe

        // Flee movement
        Vector2 dir = (enemy.GlobalPosition - enemy.Player.GlobalPosition).Normalized();
        enemy.Velocity = dir * enemy.MoveSpeed;
        enemy.MoveAndSlide();

        enemy.SetStateLabel("FLEEING");

        return BTState.Running;
    }
}


