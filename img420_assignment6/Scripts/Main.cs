using Godot;
using System;

public partial class Main : Node2D
{
	private Player player;
	private AudioStreamPlayer2D music;

	bool anyEnemyExist;
	bool anyAllyExist;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		music = GetNode<AudioStreamPlayer2D>("Music");
		music.Play();

		player.GameOver += OnGameOver;


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		anyEnemyExist = HasChildType<Enemy>();
		anyAllyExist = HasChildType<Ally>();

		
        if (!anyEnemyExist && !anyAllyExist)
        {
            GD.Print("All enemies defeated");
			OnGameWin();
        }
    }

	public bool HasChildType<T>() where T : Node
    {
        foreach (Node child in GetChildren())
        {
            if (child is T)
            {
                // Found a child of the specified type, return true immediately
                return true;
            }
        }
        // No children of the specified type were found
        return false;
	}

	private void OnGameOver()
	{
		music.Stop();
		GetTree().ChangeSceneToFile("res://Scenes/game_over.tscn");
	}

	private void OnGameWin()
    {
		music.Stop();
		GetTree().ChangeSceneToFile("res://Scenes/game_win.tscn");
    }
}
