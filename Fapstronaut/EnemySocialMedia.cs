using Godot;
using System;

public class EnemySocialMedia : Node2D
{
    bool doubleSpawned = false;
    int positionIndex = 0;
    // Declare member variables here. Examples:
    private float extraSpeed = -300f;

    private float dmg = 30f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        // Not Instanced from another 
        if (doubleSpawned == false)
        {
            // Position
            int value = new Random().Next(0, 3);
            Translate(new Vector2(0, value * ((GetViewport().Size.y - 2 * (GetViewport().Size.y / 4)) / 2)));

            // If player chad 
            if ((GetParent().GetChild(0) as Player).chad)
            {
                // 50% chance
                if (new Random().Next(0, 2) == 0)
                {
                    if (value != 1)
                    {
                        // Add another enemy of the same type
                        var scene = (PackedScene)ResourceLoader.Load("res://EnemySocialMedia.tscn");
                        var instance = scene.Instance();
                        EnemySocialMedia enemy = instance as EnemySocialMedia;
                        enemy.doubleSpawned = true;

                        // interpret the two position index so the 2 enemies aren't spawned in the same position
                        switch (value)
                        {
                            case 0:
                                {
                                    enemy.positionIndex = 1;
                                    break;
                                }
                            case 2:
                                {
                                    enemy.positionIndex = 1;
                                    break;
                                }
                        }

                        // Finally add it to the scene
                        GetParent().AddChild(instance);
                    }

                }
            }
        }
        else // Instanced from another
        {
            Translate(new Vector2(0, positionIndex * ((GetViewport().Size.y - 2 * (GetViewport().Size.y / 4)) / 2)));
        }

        // Sprite
        String path = "";
        if ((GetParent() as GameLogic).IsDerCoomerActive())
        {
            path = "res://sprites/socialMedia/DerCoomerSocialMedia.png"; 
        }
        else
        {
            int textValue = new Random().Next(0, 5);
            String[] paths = new String[5];
            paths[0] = "res://sprites/socialMedia/insta.png";
            paths[1] = "res://sprites/socialMedia/tinder.png";
            paths[2] = "res://sprites/socialMedia/4chan.png";
            paths[3] = "res://sprites/socialMedia/snapchat.png";
            paths[4] = "res://sprites/socialMedia/tiktok.png";
            path = paths[textValue];
        }

        var sprite = GetChild(0) as Sprite;
        sprite.Texture = ResourceLoader.Load(path) as Texture;

        // Level
        extraSpeed = (extraSpeed + (GetParent() as GameLogic).currentLevelUpPercentage * extraSpeed);
        var audio = GetNode("AudioStreamPlayer2D") as AudioStreamPlayer2D;
        audio.PitchScale += (audio.PitchScale * (GetParent() as GameLogic).currentLevelUpPercentage);
        dmg += (dmg * (GetParent() as GameLogic).currentLevelUpPercentage);


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
            gameLogic.DoDamageToEntity(dmg, body);
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
