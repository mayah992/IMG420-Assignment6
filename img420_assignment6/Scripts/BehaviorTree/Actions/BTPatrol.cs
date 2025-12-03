using Godot;
using System.Collections.Generic;

public partial class BTPatrol : BTNode
{
    [Export] public NodePath PatrolPointsPath; // Assign per enemy instance
    [Export] public float Speed = 50f;
    [Export] public float ArriveDistance = 6f;

    private List<Node2D> _points = new();
    private int _index = 0;
    private bool initialized = false;

    private void Initialize(Enemy enemy)
    {
        if (initialized) return;
        initialized = true;

        if (enemy.PatrolPointsPath == null)
        {
            GD.PrintErr($"No patrol path assigned for {enemy.Name}");
            return;
        }

        var parent = enemy.GetNode<Node2D>(enemy.PatrolPointsPath);

        foreach (Node child in parent.GetChildren())
            if (child is Node2D n)
                _points.Add(n);
    }

    public override BTState Tick(Enemy enemy, double delta)
    {
        Initialize(enemy);
        if (_points.Count == 0) return BTState.Failure;

        var target = _points[_index].GlobalPosition;
        Vector2 dir = (target - enemy.GlobalPosition).Normalized();

        enemy.Velocity = dir * Speed;
        enemy.MoveAndSlide();
        enemy.SetStateLabel("PATROLLING");

        if (enemy.GlobalPosition.DistanceTo(target) < ArriveDistance)
            _index = (_index + 1) % _points.Count;

        return BTState.Running;
    }
}
