using Godot;

public partial class HealthBar : Node2D
{
    private ProgressBar bar;
    private Node parent;

    public override void _Ready()   
    {
        bar = GetNode<ProgressBar>("ProgressBar");
        parent = GetParent();
    }

    public override void _Process(double delta)
    {
        if (parent == null) return;

        // Check if parent has the required *properties* (C# generates get_ methods)
        if (parent.HasMethod("GetCurrentHealth") && parent.HasMethod("GetMaxHealth"))
        {
            float current = (float)parent.Get("CurrentHealth");
            float max = (float)parent.Get("MaxHealth");

            if (max > 0)
                bar.Value = (current / max) * 100f;
        }
    }


}

