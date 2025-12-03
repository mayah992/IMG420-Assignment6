using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    // target variables
	public Player Player;

    // behavior tree variable
    private BTNode _behaviorTreeRoot;
    private Label stateLabel;
    public bool AlliesAvailable = false;

    // movement variables
    [Export] public float MoveSpeed = 50f;

    // health variables
    [Export] public float MaxHealth = 100f;
    public float CurrentHealth;
    public float HealthRatio;
	public bool IsDead => CurrentHealth <= 0;

    // attack variables
    private Area2D attackArea;
    private double _attackCooldown = 0;
    public bool CanAttack() => _attackCooldown <= 0;

    // detection variables
    private Area2D detectionArea;

    private AudioStreamPlayer2D damageTaken;
    private AudioStreamPlayer2D killed;

    private AnimatedSprite2D _anim;

    [Export] public NodePath PatrolPointsPath;


    public override void _Ready()
    {   
        // set beginning health to max
        CurrentHealth = MaxHealth;

        TriggerHelpCooldown();

        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _anim.Play("idle");

        // Get Nodes
            // behavior tree
        _behaviorTreeRoot = GetNode<BTNode>("BehaviorTree");
        GD.Print("Enemy Behavior Tree Initialized");
            // state label to display current behavior
        stateLabel = GetNode<Label>("StateLabel");

            //audio
        damageTaken = GetNode<AudioStreamPlayer2D>("DamageTaken");
        killed = GetNode<AudioStreamPlayer2D>("Killed");

            // detection area for chasing
        detectionArea = GetNode<Area2D>("DetectionArea");
        detectionArea.Monitorable = true;
            // when it is entered, call function
        detectionArea.BodyEntered += OnDetectionAreaEntered;
            // when it is exited, stop function
        detectionArea.BodyExited += OnDetectionAreaExited;
            // detection area for attacking
        attackArea = GetNode<Area2D>("AttackArea");
            // when it is entered, call function
        attackArea.BodyEntered += OnAttackAreaBodyEntered;
            // when it is exited, stop function
        attackArea.Monitoring = false;

    }

    // function that happens every second
    public override void _PhysicsProcess(double delta)
    {
        // if behavior tree exists, run it
        if (_behaviorTreeRoot != null)
            _behaviorTreeRoot.Tick(this, delta);

        // Reduce attack cooldown
        if (_attackCooldown > 0)
        _attackCooldown -= delta;

        // update health
        HealthRatio = CurrentHealth / MaxHealth;

        MoveAndSlide();
    }

    // -------------------------------
    // DETECTION
    // -------------------------------
    // when the detection area is entered, set player 
    private void OnDetectionAreaEntered(Node2D body)
    {
        GD.Print("DETECTED: ", body);
        if (body is Player p && GodotObject.IsInstanceValid(p))
        {
            Player = p;
        }
    }

    private void OnDetectionAreaExited(Node2D body)
    {
        if (body == Player)
        {
            Player = null;
        }
    }

    // -------------------------------
    // ATTACK
    // -------------------------------
    // when attack area is entered,
    private void OnAttackAreaBodyEntered(Node2D body)
    {   
        // if the body entering is a player
        if (body is Player p)
        {
            // damage player
            GD.Print("Hitbox triggered on: ", body);
            p.TakeDamage(20);
            // start attack cooldown
            _attackCooldown = 1.2;
        }
    }
    
    public void PerformAttack()
    {
        _anim.Play("attack");
        GD.Print("Enemy attacks!");
        GetTree().CreateTimer(2f).Timeout += () => _anim.Play("idle");
    }

    public void TriggerHelpCooldown()
    {
        AlliesAvailable = false;
        GetTree().CreateTimer(10f).Timeout += () => AlliesAvailable = true;
    }



    // -------------------------------
    // DAMAGE
    // -------------------------------
	public void TakeDamage(float amount)
	{
		CurrentHealth -= amount;
        GD.Print($"Enemy took {amount}. HP = {CurrentHealth}");

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
        GD.Print("Enemy died!");

        GetTree().CreateTimer(2f).Timeout += () => QueueFree();
	}

    // update label function (USED IN action behavior files)
    public void SetStateLabel(string text)
    {
        if (stateLabel != null)
            stateLabel.Text = text;
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
