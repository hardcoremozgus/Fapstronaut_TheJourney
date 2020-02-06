using Godot;
using System;

public class EnemySocialMedia : Node2D
{
    // Declare member variables here. Examples:
    private float extraSpeed = -300f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Position
        int value = new Random().Next(0, 3);
        Translate(new Vector2(0, value * ((GetViewport().Size.y - 2 * (GetViewport().Size.y / 4)) / 2)));


        // Sprite
        int textValue = new Random().Next(0, 4);
        String[] paths = new String[4];
        paths[0] = "res://sprites//socialMedia//insta.png";
        paths[1] = "res://sprites//socialMedia//tinder.png";
        paths[2] = "res://sprites//socialMedia//4chan.png";
        paths[3] = "res://sprites//socialMedia//snapchat.png";
        var sprite = GetChild(0) as Sprite;
        sprite.Texture = ResourceLoader.Load(paths[textValue]) as Texture;

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Translate(new Vector2((GetParent() as GameLogic).GetCurrentSpeed() + extraSpeed, 0) * delta);

        // TODO: destroy if left of screen
    }

    public void Collision(Node body)
    {
        GD.Print(body.Name);
        if (body.Name == "Player")
        {
            (GetParent() as GameLogic).DoDamageToEntity(30, body);
        }

    }

    public void OnScreenExit()
    {
        QueueFree();
    }

}
