using Godot;
using System;

public partial class MainMenu : Node2D
{
	private AudioStreamPlayer2D music;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        Button start_button = GetNode<Button>("StartGameButton");
		Button exit_button = GetNode<Button>("ExitGameButton");

		music = GetNode<AudioStreamPlayer2D>("Music");
		music.Play();

		if (start_button != null)
		{
			start_button.Pressed += StartGame;
		}
		
		if (exit_button != null)
		{
            exit_button.Pressed += ExitGame;
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void StartGame()
    {
		music.Stop();
		GD.Print("Starting Game");
        GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
    }

	private void ExitGame()
	{
		music.Stop();
		GD.Print("Exiting Game");
		// Exit Game
		GetTree().Quit();
	}
}
