using Godot;
using System;


public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    private AnimationPlayer animationPlayer;

    const float gravity = 20f;
    const float jumpPower = -650f;
    float velocity = 0.0f;
    Vector2 floor = new Vector2(0, -1);
    public double urges = 0f; 
    public float brainFog = 100f; 

    public float urgeHeal = 5f;  // TODO: decrease brain fog with time, and thus upgrade this at least

    private TextureProgress urgeBar;  

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetPhysicsProcess(true);
        animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        animationPlayer.Play("run");
        animationPlayer.PlaybackSpeed = 0.8f;
        urgeBar = GetParent().GetChild(4).GetChild(0).GetChild(0) as TextureProgress; 
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta) // call _PhysicsProcess if any 
    {
        Move(delta);
        Heal(delta);  
    }

    private void Move(float delta) // move and slide already takes into account delta
    {
        MoveAndSlide(new Vector2(0, velocity), floor);

        if (IsOnFloor())
        {
            animationPlayer.PlaybackSpeed = 0.8f;
            velocity = 0.1f;
            if (Input.IsActionJustPressed("ui_up"))
            {
                velocity = jumpPower;
            }

        }
        else
        {
            animationPlayer.PlaybackSpeed = 0.0f;
            velocity += gravity;
        }

    }

    private void Heal(double delta)
    {
        if(urges == 0)
        {
            return; 
        }

        urges -= urgeHeal * delta; 
        if(urges < 0)
        {
            urges = 0; 
        }

        urgeBar.Value = urges; 

        GD.Print("Player urges are: " + urges); 
    }


}
