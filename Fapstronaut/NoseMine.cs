using Godot;
using System;

public class NoseMine : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    float speed = 50f; 
    int Loffset = 400, Roffset = 50; 
    float damage = 20f; 
    Tuple<float, float> scaleRange; 
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        scaleRange = Tuple.Create(0.5f, 1.5f); 
        GlobalPosition = new Vector2(new Random().Next(Loffset, (int)GetViewportRect().Size.x - Roffset), 0);

        var random = new RandomNumberGenerator(); 
        random.Seed = OS.GetTicksMsec(); 
        float scale = random.RandfRange(scaleRange.Item1, scaleRange.Item2); 
        Scale = new Vector2(scale, scale);      
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Translate(new Vector2(0, speed) * delta);
    }

     public void OnCollision(Node body)
    {
        if (body.Name == "Player")
        {
            (GetParent() as GameLogic).DoDamageToEntity(damage, (GetParent().GetNode("Player")));
            QueueFree();
        }
    }

}
