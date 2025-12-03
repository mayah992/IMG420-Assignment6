using Godot;

public partial class BTSummonAlly : BTNode
{
    [Export] public PackedScene AllyScene;
    [Export] public float SpawnOffset = 50f;

    public override BTState Tick(Enemy enemy, double delta)
    {
        if (AllyScene == null)
        {
            GD.PrintErr("ERROR: AllyScene is not assigned!");
            return BTState.Failure;
        }

        // Instantiate the Ally (correct type!)
        Ally newAlly = AllyScene.Instantiate<Ally>();

        // Spawn near the enemy
        Vector2 offset = new Vector2(SpawnOffset, 0);
        newAlly.GlobalPosition = enemy.GlobalPosition + offset;

        // Add to main scene
        enemy.GetTree().CurrentScene.AddChild(newAlly);

        GD.Print("Ally spawned!");

        // Trigger 10s cooldown
        enemy.TriggerHelpCooldown();

        // Update debug label
        enemy.SetStateLabel("SUMMONING ALLY");

        return BTState.Success;
    }
}




