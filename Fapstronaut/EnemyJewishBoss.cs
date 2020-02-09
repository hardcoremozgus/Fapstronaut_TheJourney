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

    float launchTime = 5f, currentLaunchTime = 0f;

    JewishBossPhase phase;

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


        if (life <= (totalLife - phaseLife))
        {
            phase = JewishBossPhase.MINES;
        }
    }

    private void MinesLogic(float delta)
    {
        if (life <= (totalLife - 2 * phaseLife))
        {
            phase = JewishBossPhase.BOTH;
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
        if(sprite.Animation == "damaged")
        {
            sprite.Play("idle"); 
        }
    }
}
