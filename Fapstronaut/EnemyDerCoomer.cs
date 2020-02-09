using Godot;
using System;

public enum DerCoomerState { HIDE, APPEAR }
public class EnemyDerCoomer : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private float currentAppearTime = 0f, appearTime = 5f, nextAppearTime = 0f;
    private int randomExtraTimeRange = 3;
    public DerCoomerState state;

    bool cancelPlayer = false;

    float damage = 50f;

    float animationSpeedMulti = 1.05f; 

    public float life = 200f;

    int reactionFrames = 5;
    AnimatedSprite sprite;

    Player player;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetParent().GetNode("Player") as Player;
        sprite = GetNode("AnimatedSprite") as AnimatedSprite;
        state = DerCoomerState.HIDE;
        DoHide(false);
        SetNextTime();
    }

    void SetNextTime()
    {
        currentAppearTime = 0f;
        nextAppearTime = appearTime + (float)new System.Random().Next(0, randomExtraTimeRange + 1);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (state)
        {
            case DerCoomerState.HIDE:
                {
                    if ((currentAppearTime += delta) >= nextAppearTime)
                    {
                        state = DerCoomerState.APPEAR;
                        cancelPlayer = true;
                        (GetNode("Audio").GetNode("Appear") as AudioStreamPlayer2D).Play();
                        sprite.Visible = true;
                        sprite.Play("attack");
                        (GetNode("Light2D") as Light2D).Enabled = true;
                        SetNextTime();
                    }

                    break;
                }

            case DerCoomerState.APPEAR:
                {
                    if (player.state == playerState.kicking)
                    {
                        GD.Print("Player hits Der Coomer!");
                        cancelPlayer = false;
                        sprite.SpeedScale *= animationSpeedMulti; // harder each time
                    }
                  
                    break;
                }

        }
    }

    public void OnAnimationFinish()
    {
        if (state == DerCoomerState.APPEAR)
        {
            GD.Print("Der coomer goes to sleep");
            DoHide(true);
        }

    }

    private void DoHide(bool doDamage = false)
    {
        state = DerCoomerState.HIDE;
        sprite.Visible = false;
        sprite.Frame = 0;
        sprite.Stop();
        (GetNode("Light2D") as Light2D).Enabled = false;

        if (doDamage)
        {
            if (cancelPlayer) // Do damage to the player
            {
                (GetParent() as GameLogic).DoDamageToEntity(damage, player as Node);
            }
            else // Do damage to myself
            {
                (GetParent() as GameLogic).DoDamageToEntity(player.damage, this);
            }
        }

    }

}
