using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float speed = 100.0f;
	[Export] public float sprintSpeed = 150.0f; 
	[Export] public float sprintMultiplier = 1.2f;

    [Export] public float MaxHealth = 100f;
    [Export] public float CurrentHealth;

    public bool IsDead => CurrentHealth <= 0;

    private Vector2 currentVelocity;
    private AnimatedSprite2D _anim;
    private Area2D attackArea;
	private bool isAttacking = false;
	

    private Vector2 lastFacing = new Vector2(0, 1); // default facing down

	private GpuParticles2D attackParticles;

    // Combat
    [Export] public int AttackDamage = 20;
    [Export] public float AttackCooldown = 0.4f;

    private bool canAttack = true;

    [Signal] public delegate void GameOverEventHandler();

    private AudioStreamPlayer2D takeDamage;
    private AudioStreamPlayer2D attack;

    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        attackArea = GetNode<Area2D>("AttackArea");
		attackParticles = GetNode<GpuParticles2D>("AttackParticle");

        takeDamage = GetNode<AudioStreamPlayer2D>("TakeDamage");
        attack = GetNode<AudioStreamPlayer2D>("Attack");

        // Hitbox off until attack
        attackArea.Monitoring = false;

        // Connect signal
        attackArea.BodyEntered += OnAttackAreaBodyEntered;

        CurrentHealth = MaxHealth;
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleMovement();

        if (Input.IsActionJustPressed("attack") && canAttack)
            PerformAttack();
    }

    // -------------------------------
    // MOVEMENT + ANIMATION
    // -------------------------------
    private void HandleMovement()
    {
        Vector2 input = Input.GetVector("left", "right", "up", "down");

		bool sprintPressed = Input.IsActionPressed("sprint");
		float effectiveSpeed = sprintPressed ? sprintSpeed * sprintMultiplier : speed;

        currentVelocity = input * effectiveSpeed;

        Velocity = currentVelocity;
        MoveAndSlide();

        AnimateMovement(input);
    }

    private void AnimateMovement(Vector2 input)
    {
		// Do not change animation while attacking
    	if (isAttacking)
        	return;

        if (input.Length() > 0)
            lastFacing = input;

        string animName;

        if (Math.Abs(lastFacing.X) > Math.Abs(lastFacing.Y))
        {
            bool moving = currentVelocity.Length() > 0;
            animName = moving ? "side_walk" : "side_idle";
            _anim.FlipH = lastFacing.X < 0;
        }
        else if (lastFacing.Y < 0)
        {
            bool moving = currentVelocity.Length() > 0;
            animName = moving ? "back_walk" : "back_idle";
            _anim.FlipH = false;
        }
        else
        {
            bool moving = currentVelocity.Length() > 0;
            animName = moving ? "forward_walk" : "forward_idle";
            _anim.FlipH = false;
        }

        _anim.Play(animName);
    }

    // -------------------------------
    // ATTACK
    // -------------------------------
    private void PerformAttack()
	{
		if (!canAttack || isAttacking)
			return;

		canAttack = false;
		isAttacking = true;

        attack.Play();
		// Play attack animation fully
		_anim.Play("attack");

		// Emit particle burst
		if (attackParticles != null)
		{
			attackParticles.OneShot = true;   // Ensures it only bursts once per attack
			attackParticles.Restart();         // Start emission
		}

		// Enable hitbox briefly
		attackArea.Monitoring = true;
		GetTree().CreateTimer(0.15f).Timeout += () =>
		{
			attackArea.Monitoring = false;
		};

		// Wait for animation to finish
		_anim.AnimationFinished += OnAttackAnimationFinished;
	}

	private void OnAttackAnimationFinished()
	{
		if (_anim.Animation == "attack")
		{
			isAttacking = false;
			_anim.AnimationFinished -= OnAttackAnimationFinished;

			// 5Start cooldown
			GetTree().CreateTimer(AttackCooldown).Timeout += () =>
			{
				canAttack = true;
			};
		}
	}


    private void OnAttackAreaBodyEntered(Node2D body)
    {
        if (body is Enemy enemy)
        {
            GD.Print("Hitbox triggered on: ", body);
            enemy.TakeDamage(AttackDamage);
        }
        else if (body is Ally ally)
        {
            GD.Print("Hitbox triggered on: ", body);
            ally.TakeDamage(AttackDamage);
        }
    }

    // -------------------------------
    // DAMAGE + DEATH
    // -------------------------------
    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        GD.Print($"Player took {amount}. HP = {CurrentHealth}");

        if (CurrentHealth > 20)
        {
            takeDamage.Play();
        }

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GD.Print("Player died!");
        QueueFree();
        EmitSignal(SignalName.GameOver);
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



