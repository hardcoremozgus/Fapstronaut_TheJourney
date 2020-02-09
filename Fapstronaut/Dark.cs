using Godot;
using System;

public class Dark : Sprite
{
    // Declare member variables here. Examples:
    private float speed = 0f;
    bool doing = false;
    float targetAlpha = 0f;

    private float close = 0.05f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var color = Modulate;
        color.a = 0;
        Modulate = color;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        /*
        // Debug
        if (Input.IsActionJustPressed("ui_accept"))
        {
            Darken(0.75f, 0.5f);
        }*/


        if (doing)
        {
            Color color = Modulate;
            color.a += speed * delta;
            Modulate = color;
             
            if (Mathf.Abs(Modulate.a - targetAlpha) <= close)
            {
                color.a = targetAlpha;
                Modulate = color;
                doing = false;
            }
        }

    }

    public void Darken(float targetAlpha, float speed) // alpha from 0 to 1!! Recomended speed: around 0.5f
    {
        this.targetAlpha = targetAlpha;
        this.speed = speed;
        doing = true;
    }

}
