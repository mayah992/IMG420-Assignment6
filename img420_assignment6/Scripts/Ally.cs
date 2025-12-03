using Godot;
using System;

public partial class Ally : CharacterBody2D
{
	public Player Player;
	private Area2D detectionArea;


	[Export] public float MoveSpeed = 50f;

    // health variables
    [Export] public float MaxHealth = 50f;
    [Export] public float CurrentHealth;
	public float HealthRatio;

	private Area2D attackArea;
    private double _attackCooldown = 0;
    public bool CanAttack() => _attackCooldown <= 0;

	private AudioStreamPlayer2D damageTaken;
    private AudioStreamPlayer2D killed;

    private AnimatedSprite2D _anim;

    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _anim.Play("idle");

		// set beginning health to max
        CurrentHealth = MaxHealth;

		damageTaken = GetNode<AudioStreamPlayer2D>("DamageTaken");
        killed = GetNode<AudioStreamPlayer2D>("Killed");

		attackArea = GetNode<Area2D>("AttackArea");
            // when it is entered, call function
        attackArea.BodyEntered += OnAttackAreaBodyEntered;
            // when it is exited, stop function

		Player = GetNode<Player>("../Player");

    }


	public override void _PhysicsProcess(double delta)
	{
		// Reduce attack cooldown
        if (_attackCooldown > 0)
        _attackCooldown -= delta;

		// update health
        HealthRatio = CurrentHealth / MaxHealth;

		FollowPlayer();

        MoveAndSlide();

	}

	private void FollowPlayer()
	{
		Vector2 dir = (Player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = dir * MoveSpeed;
	}


	private void OnAttackAreaBodyEntered(Node2D body)
    {   
		if (!CanAttack())
			return;

        // if the body entering is a player
        if (body is Player p)
        {
            // damage player
			_anim.Play("attack");
			GD.Print("Ally attacks!");
            p.TakeDamage(5);
            // start attack cooldown
            _attackCooldown = 2.0;
			GetTree().CreateTimer(2f).Timeout += () => _anim.Play("idle");
        }
    }


	public void TakeDamage(float amount)
	{
		CurrentHealth -= amount;
        GD.Print($"Allys took {amount}. HP = {CurrentHealth}");

        if (CurrentHealth >= 20)
		{
			damageTaken.Play();
		}

		if (CurrentHealth <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
        _anim.Play("die");
        killed.Play();
        GD.Print("Ally died!");

        GetTree().CreateTimer(2f).Timeout += () => QueueFree();
	}

	// used to update health bar (USED IN HealthBar.cs)
    public float GetMaxHealth()
    {
        return MaxHealth;
    }
    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }
}
