using Godot;
using System;
using System.Collections.Generic;

public enum playerState { walking, sliding, jumping, kicking }
public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    private AnimationPlayer animationPlayer;

    float gravity = 20f;
    float jumpPower = -550f;
    float velocity = 0.0f;
    Vector2 floor = new Vector2(0, -1),
    idlePos = new Vector2(0, 0);
    public double urges = 0f;
    public float brainFog = 100f;

    public bool incapacitated = false;

    public bool chad = false;
    uint jumpCount = 0;
    public float damage = 30f;
    public float currentslideTime = 0f, slideTime = 1f;
    public float urgeHeal = 5f;  // TODO: decrease brain fog with time, and thus upgrade this at least
    private TextureProgress urgeBar, brainFogBar;

    public playerState state;
    float slideHeight = 215;
    private GameLogic gameLogic;
    private CollisionShape2D collider, kick;

    private Dictionary<String, AudioStreamPlayer2D> fxs;

    public uint postersArrived = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetPhysicsProcess(true);
        animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        animationPlayer.Play("run");
        animationPlayer.PlaybackSpeed = 0.8f;
        urgeBar = GetParent().GetChild(4).GetChild(0).GetChild(0) as TextureProgress; // very dirty :o
        brainFogBar = GetParent().GetChild(4).GetChild(1).GetChild(0) as TextureProgress; // very dirty :o
        gameLogic = GetParent() as GameLogic;
        collider = GetChild(1) as CollisionShape2D;
        kick = GetNode("Kick") as CollisionShape2D;
        state = playerState.walking;
        idlePos = GlobalPosition;
        fxs = new Dictionary<String, AudioStreamPlayer2D>();
        fxs.Add("Jump", GetNode("Audio").GetNode("Jump") as AudioStreamPlayer2D);
        fxs.Add("Slide", GetNode("Audio").GetNode("Slide") as AudioStreamPlayer2D);
        fxs.Add("Kick", GetNode("Audio").GetNode("Kick") as AudioStreamPlayer2D);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta) // call _PhysicsProcess if any 
    {
        Move(delta);
        Heal(delta);
    }

    private void Move(float delta) // move and slide already takes into account delta
    {
        /*
        // Debug
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            MakeChad();
        }*/

        // First move and slide, it will update the IsOnFloor() logic
        MoveAndSlide(new Vector2(0, velocity), floor);

        // Common in all floor states: still have some "y" speed so kinematic body works properly and detects floor
        if (IsOnFloor())
        {
            velocity = 0.1f;
            if (state == playerState.jumping)
            {
                StopJumping();
            }
        }


        // State
        switch (state)
        {
            case playerState.walking: // Can jump, slide or kick
                {
                    if (incapacitated == false)
                    {
                        if (Input.IsActionJustPressed("ui_up"))
                        {
                            Jump();
                        }

                        if (Input.IsActionJustPressed("ui_down"))
                        {
                            state = playerState.sliding;
                            gameLogic.speedMultiplier = 2f;
                            collider.Translate(new Vector2(150, 350));
                            collider.Rotate(-80f * gameLogic.PI / 180f);
                            animationPlayer.Play("Slide");
                            animationPlayer.PlaybackSpeed = 0.0f;
                            fxs["Slide"].Play();
                        }

                    }

                    if (Input.IsActionJustPressed("ui_accept"))
                    {
                        state = playerState.kicking;
                        animationPlayer.Play("Kick");
                        animationPlayer.PlaybackSpeed = 2.5f;
                        fxs["Kick"].Play();
                    }

                    break;
                }

            case playerState.sliding:
                {
                    if (Input.IsActionJustReleased("ui_down") || ((currentslideTime += delta) >= slideTime))
                    {
                        StopSliding();
                    }
                    break;
                }

            case playerState.jumping:
                {
                    gameLogic.speedMultiplier = 2f;
                    animationPlayer.PlaybackSpeed = 0.0f;
                    velocity += gravity;

                    // Double Jump

                    if (chad)
                    {
                        if (Input.IsActionJustPressed("ui_up") && jumpCount < 2)
                        {

                            Jump();
                        }
                    }

                    break;
                }

        }

    }
    private void Jump()
    {
        jumpCount++;
        state = playerState.jumping;
        gameLogic.speedMultiplier = 1.5f;
        velocity = jumpPower;
        fxs["Jump"].Play();
    }

    public void OnAnimationFinished(String name) // must do this here, as a signal, for the kick anim 
    {
        if (name == "Kick")
        {
            StopKicking();
        }

    }

    private void StopSliding()
    {
        fxs["Slide"].Stop();
        currentslideTime = 0f;
        collider.Translate(new Vector2(-150, -350));
        collider.Rotate(80f * gameLogic.PI / 180f);
        ToIdle();
    }

    private void StopJumping()
    {
        jumpCount = 0;
        fxs["Jump"].Stop();
        ToIdle();
    }

    private void StopKicking()
    {
        fxs["Kick"].Stop();

        // Game Logic already checks if thot life <= 0 and de-incapacitates the player
        if (incapacitated)
        {
            Node thot = GetParent().GetNode("EnemyTwitchThot");
            if (thot != null)
            {
                gameLogic.DoDamageToEntity(damage, thot);
            }

        }
        ToIdle();
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

    public void LevelUp()
    {
        float percentatge = (GetParent() as GameLogic).levelUpPercentatge;
        brainFogBar.Value = (brainFog -= (brainFog * percentatge));
        jumpPower += (jumpPower * percentatge);
        slideTime += (slideTime * percentatge);
        damage += (damage * percentatge);

        if (postersArrived == 6)
        {
            MakeChad();
        }

    }


    private void MakeChad()
    {
        chad = true;

        // Delete old sprites and animation player
        GetNode("AnimationPlayer").QueueFree();
        GetNode("Torso").QueueFree();

        var chadNode = GetNode("Chad");
        (chadNode.GetNode("Torso") as Sprite).Visible = true;
        animationPlayer = chadNode.GetNode("AnimationPlayer") as AnimationPlayer;

        // More Life
        urgeBar.MaxValue *= 2d;

        // Music
        (GetParent().GetNode("Music").GetNode("Music") as AudioStreamPlayer2D).Stop();
        (GetParent().GetNode("Music").GetNode("MusicChad") as AudioStreamPlayer2D).Play();

        // Reset stuff
        ResetState();

    }

    public void ResetState()
    {
        switch (state)
        {
            case playerState.walking:
                {
                    ToIdle();
                    break;
                }
            case playerState.sliding:
                {
                    StopSliding();
                    break;
                }
            case playerState.jumping:
                {
                    StopJumping();
                    break;
                }
            case playerState.kicking:
                {
                    StopKicking();
                    break;
                }
        }
    }


    public void Incapacitate(bool incapacitated)
    {
        this.incapacitated = incapacitated;
        if (incapacitated == false)
        {
            ResetState();
        }
        gameLogic.scrollSpeed += (incapacitated ? 50 : -50);
        animationPlayer.PlaybackSpeed = ((incapacitated) ? 0.8f : 0.5f);
    }

    public Vector2 GetCurrentPosition()
    {
        return (GetNode("CollisionShape2D") as CollisionShape2D).GlobalPosition;
    }

    public Vector2 GetKickPosition()
    {
        return (GetNode("Kick").GetNode("CollisionShape2D") as CollisionShape2D).GlobalPosition;
    }

}
