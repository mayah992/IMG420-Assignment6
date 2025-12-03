using Godot;
using System;

public partial class GameOver : Node2D
{
	private AudioStreamPlayer2D music;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        Button menu_button = GetNode<Button>("Return");
		music = GetNode<AudioStreamPlayer2D>("Music");
		music.Play();

		if (menu_button != null)
		{
			GD.Print("Return Button found");
			menu_button.Pressed += ReturnToMenu;
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ReturnToMenu()
    {
		music.Stop();
		GD.Print("Returning to Menu");
        GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }
}
