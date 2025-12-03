using Godot;

public partial class BTIsHealthLow : BTNode
{
    public override BTState Tick(Enemy enemy, double delta)
    {
        return enemy.HealthRatio <= 0.5f   // 50% or lower
            ? BTState.Success
            : BTState.Failure;
    }

}


