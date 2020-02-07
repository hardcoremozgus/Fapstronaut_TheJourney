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
        int textValue = new Random().Next(0, 5);
        String[] paths = new String[5];
        paths[0] = "res://sprites/socialMedia/insta.png";
        paths[1] = "res://sprites/socialMedia/tinder.png";
        paths[2] = "res://sprites/socialMedia/4chan.png";
        paths[3] = "res://sprites/socialMedia/snapchat.png";
        paths[4] = "res://sprites/socialMedia/tiktok.png";
        var sprite = GetChild(0) as Sprite;
        sprite.Texture = ResourceLoader.Load(paths[textValue]) as Texture;

        // Level
        extraSpeed = (extraSpeed + (GetParent() as GameLogic).currentLevelUpPercentage * extraSpeed);
        var audio = GetNode("AudioStreamPlayer2D") as AudioStreamPlayer2D;
        audio.PitchScale += (audio.PitchScale * (GetParent() as GameLogic).currentLevelUpPercentage);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Translate(new Vector2((GetParent() as GameLogic).GetCurrentSpeed() + extraSpeed, 0) * delta);

        // TODO: destroy if left of screen
    }

    public void Collision(Node body)
    {

        if (body.Name != "Player" && body.Name != "Kick")
        {
            return;
        }

        GameLogic gameLogic = GetParent() as GameLogic;
        if (body.Name == "Player")
        {
            gameLogic.DoDamageToEntity(30, body);
        }

        else if (body.Name == "Kick" && ((GetParent().GetChild(0)) as Player).state != playerState.kicking)
        {
            return;
        }

       (GetParent() as GameLogic).AddScene("res://DeathLogic.tscn", GetParent());
        QueueFree();
    }

    public void OnScreenExit()
    {
        QueueFree();
    }

}
