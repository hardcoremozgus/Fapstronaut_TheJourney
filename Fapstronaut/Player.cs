using Godot;
using System;

public enum playerState { walking, sliding, jumping, kicking }
public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    private AnimationPlayer animationPlayer;

    const float gravity = 20f;
    const float jumpPower = -650f;
    float velocity = 0.0f;
    Vector2 floor = new Vector2(0, -1),
    idlePos = new Vector2(0,0);
    public double urges = 0f;
    public float brainFog = 100f;
    public float currentslideTime = 0f, slideTime = 1f;
    public float urgeHeal = 5f;  // TODO: decrease brain fog with time, and thus upgrade this at least
    private TextureProgress urgeBar;

    public playerState state;
    float slideHeight = 215;
    private GameLogic gameLogic;
    private CollisionShape2D collider, kick;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetPhysicsProcess(true);
        animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        animationPlayer.Play("run");
        animationPlayer.PlaybackSpeed = 0.8f;
        urgeBar = GetParent().GetChild(4).GetChild(0).GetChild(0) as TextureProgress;
        gameLogic = GetParent() as GameLogic;
        collider = GetChild(1) as CollisionShape2D;
        kick = GetNode("Kick") as CollisionShape2D; 
        state = playerState.walking;
        idlePos = GlobalPosition; 
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta) // call _PhysicsProcess if any 
    {
        Move(delta);
        Heal(delta);
    }

    private void Move(float delta) // move and slide already takes into account delta
    {
        // First move and slide, it will update the IsOnFloor() logic
        MoveAndSlide(new Vector2(0, velocity), floor);

        // Common in all floor states: still have some "y" speed so kinematic body works properly and detects floor
        if (IsOnFloor())
        {
            velocity = 0.1f;
            if (state == playerState.jumping)
            {
                ToIdle(); 
            }
        }

        // State
        switch (state)
        {
            case playerState.walking: // Can jump, slide or kick
                {
                    if (Input.IsActionJustPressed("ui_up"))
                    {
                        state = playerState.jumping;
                        gameLogic.speedMultiplier = 1.5f;
                        velocity = jumpPower;
                    }

                    if (Input.IsActionJustPressed("ui_down"))
                    {
                        state = playerState.sliding;
                        gameLogic.speedMultiplier = 2f;
                        collider.Translate(new Vector2(150, 350));
                        collider.Rotate(-80f * gameLogic.PI / 180f);
                        animationPlayer.Play("Slide");
                        animationPlayer.PlaybackSpeed = 0.0f;
                    }

                    if (Input.IsActionJustPressed("ui_accept"))
                    {
                        state = playerState.kicking;
                        animationPlayer.Play("Kick");
                        animationPlayer.PlaybackSpeed = 2.5f;
                    }


                    break;
                }

            case playerState.sliding:
                {
                    if (Input.IsActionJustReleased("ui_down") || ((currentslideTime += delta) >= slideTime))
                    {
                        ToIdle();
                        currentslideTime = 0f;
                        collider.Translate(new Vector2(-150, -350));
                        collider.Rotate(80f * gameLogic.PI / 180f);
                    }
                    break;
                }

            case playerState.jumping:
                {
                    gameLogic.speedMultiplier = 2f;
                    animationPlayer.PlaybackSpeed = 0.0f;
                    velocity += gravity;
                    break;
                }

        }

    }

    public void OnAnimationFinished(String name) // must do this here, as a signal, for the kick anim 
    {
        if (name == "Kick")
        {
            ToIdle();
        }

    }


    private void ToIdle()
    {
        animationPlayer.PlaybackSpeed = 0.8f;
        state = playerState.walking;
        animationPlayer.Play("run");
        gameLogic.speedMultiplier = 1f;
        GlobalPosition = idlePos; 
    }

    private void Heal(double delta)
    {
        if (urges == 0)
        {
            return;
        }

        urges -= urgeHeal * delta;
        if (urges < 0)
        {
            urges = 0;
        }

        urgeBar.Value = urges;
    }


}
