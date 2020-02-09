using Godot;
using System;

public class Nose : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Player player;

    GameLogic gameLogic;
    float close = 10f;
    float speed = 50f;

    float scaleReduction = 0.1f;

    float minimumScale = 0.4f;
    float damage = 50f;

    float currentTime = 0f, totalTime = 2f;

    Vector2 lastSpeed;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameLogic = GetParent() as GameLogic;
        player = gameLogic.GetNode("Player") as Player;
        lastSpeed = (new Random().Next(0,2) == 0) ? new Vector2(-6f, -2f) : new Vector2(-4f, 4f); 

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

        Scale -= Scale * new Vector2(scaleReduction * delta, scaleReduction * delta);

        if(Scale.x <= minimumScale)
        {
            QueueFree(); 
            return; 
        }

        if ((currentTime += delta) >= totalTime)
        {
            currentTime = 0f;
            Vector2 targetPosition = player.GetCurrentPosition();
            lastSpeed = new Vector2(targetPosition - GlobalPosition).Normalized() * speed * delta;
            Translate(lastSpeed);
        }
        else
        {
            Translate(lastSpeed);
        }

    }

    public void OnCollision(Node body)
    {
        if (body.Name == "Player")
        {
            gameLogic.DoDamageToEntity(damage, player as Node);
            QueueFree();
        }
    }
}
