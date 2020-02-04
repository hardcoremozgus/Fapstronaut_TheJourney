using Godot;
using System;


public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    private AnimationPlayer animationPlayer;

    const float gravity = 1800f;
    const float jumpPower = -800f;

    float velocity = 0.0f;

    Vector2 floor = new Vector2(0, -1);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetPhysicsProcess(true);
        animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        animationPlayer.Play("run");
        animationPlayer.PlaybackSpeed = 0.8f;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta) // call _PhysicsProcess if any 
    {
        Move(delta);
    }


    private void Move(float delta)
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
            velocity += gravity * delta;
        }

    }

}
