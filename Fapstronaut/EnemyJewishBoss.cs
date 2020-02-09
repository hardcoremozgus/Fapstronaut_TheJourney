using Godot;
using System;

public enum JewishBossPhase { LAUNCH, MINES, BOTH }
public class EnemyJewishBoss : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public bool playerInside = false;
    float totalLife = 1000f;
    public float life = 1000f;
    float phaseLife;

    float launchTime = 4f, currentLaunchTime = 0f,
    mineLaunchTime = 3f, currentMineLaunchTime = 0f;

    JewishBossPhase phase;

    AudioStreamPlayer2D phaseAudio;
    Player player;

    GameLogic gameLogic;

    AnimatedSprite sprite;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        phaseLife = life * 0.333f;
        gameLogic = GetParent() as GameLogic;
        player = gameLogic.GetNode("Player") as Player;
        sprite = GetNode("Head").GetNode("Sprite") as AnimatedSprite;
        phaseAudio = GetNode("Audio").GetNode("Phase") as AudioStreamPlayer2D;
        phase = JewishBossPhase.LAUNCH;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (phase)
        {
            case JewishBossPhase.LAUNCH:
                {
                    LaunchLogic(delta);
                    break;
                }
            case JewishBossPhase.MINES:
                {
                    MinesLogic(delta);
                    break;
                }
            case JewishBossPhase.BOTH:
                {
                    LaunchLogic(delta);
                    MinesLogic(delta);
                    break;
                }
        }
    }

    private void LaunchLogic(float delta)
    {
        if ((currentLaunchTime += delta) >= launchTime)
        {
            currentLaunchTime = 0;
            gameLogic.AddScene("res://Nose.tscn", gameLogic as Node);
        }

        if (phase == JewishBossPhase.LAUNCH)
        {
            if (life <= (totalLife - phaseLife))
            {
                phaseAudio.Play();
                currentLaunchTime = 0;
                phase = JewishBossPhase.MINES;
            }
        }

    }

    private void MinesLogic(float delta)
    {

        if ((currentMineLaunchTime += delta) >= mineLaunchTime)
        {
            currentMineLaunchTime = 0;
            gameLogic.AddScene("res://NoseMine.tscn", gameLogic as Node);
        }

        if (phase == JewishBossPhase.MINES)
        {
            if (life <= (totalLife - 2 * phaseLife))
            {
                phaseAudio.Play();
                currentMineLaunchTime = 0;
                phase = JewishBossPhase.BOTH;
            }
        }
    }


    public void OnCollision(Node body)
    {
        if ((body.Name == "Kick"))
        {
            playerInside = true;

        }
    }

    public void OnCollisionExit(Node body)
    {
        if (body.Name == "Kick")
        {
            playerInside = false;
        }
    }

    public void RecieveDamage()
    {
        gameLogic.DoDamageToEntity(player.damage, this);
        sprite.Play("damaged");
    }


    public void OnAnimationFinished()
    {
        if (sprite.Animation == "damaged")
        {
            sprite.Play("idle");
        }
    }
}
