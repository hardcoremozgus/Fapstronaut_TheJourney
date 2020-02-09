using Godot;
using System;

public class EnemyTwitchThot : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    Player player;
    bool spawning = true;
    float close = 10f;
    float speed = 100f;

    public float life = 100f; 
    float damageTime = 1f, damageCurrentTime = 0f; 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetParent().GetNode("Player") as Player; 
         
        // If another thot is incapacitating the player, just don't spawn another one :) 
        if((GetParent() as GameLogic).activeThots != 0)
        {
            QueueFree();
            return;  
        }

        (GetParent() as GameLogic).activeThots++; 

        Position = new Vector2(player.GetCurrentPosition().x, GetViewportRect().Size.y); 
        (GetNode("AnimatedSprite") as AnimatedSprite).Play();

        // Level
        life += (life * (GetParent() as GameLogic).currentLevelUpPercentage);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

        Vector2 targetPosition = player.GetKickPosition(); 
        if (spawning)
        {
            Translate(new Vector2(targetPosition - Position).Normalized() * speed * delta);

            if ((Position - targetPosition).Length() <= close)
            {
                Position = targetPosition;
                spawning = false;
                player.Incapacitate(true); 
          
            }
        }
        else
        {
            Position = targetPosition; 
        }

    }
}
