using Godot;

public enum BTState
{
    Success,
    Failure,
    Running
}

public abstract partial class BTNode : Node
{
    public abstract BTState Tick(Enemy enemy, double delta);
}
