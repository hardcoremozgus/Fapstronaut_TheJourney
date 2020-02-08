using Godot;
using System;

public class Poster : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    private bool arrived = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        (GetNode("Label") as Label).Text = GetText(++(GetParent().GetChild(0) as Player).postersArrived - 1);
    }


    private String GetText(uint postersArrived)
    {

        String ret = "DAY";
        Label label = GetNode("Label") as Label;

        if (postersArrived <= 4)
        {
            ret += " ";
            ret += postersArrived * 30;

            if (postersArrived == 4)
            {
                label.RectGlobalPosition -= new Vector2(10, 0);
                label.RectScale = new Vector2(1.9f, 1.9f);
            }
        }
        else if (postersArrived == 5)
        {
            ret = "6 MONTHS";
            label.RectGlobalPosition -= new Vector2(20, 0);
            label.RectScale = new Vector2(1.6f, 1.6f);
        }
        else if (postersArrived == 6)
        {
            ret = "1 YEAR";
            label.RectScale = new Vector2(2f, 2f);
        }
        else
        {
            ret = (postersArrived - 5).ToString() + " YEARS";
            label.RectScale = new Vector2(1.8f, 1.8f);
        }

        return ret;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Translate(new Vector2((GetParent() as GameLogic).GetCurrentSpeed(), 0) * delta);
    }

    public void OnArrived(Node body)
    {
        if (body.Name != "Player")
        {
            return;
        }

        if (!arrived)
        {
            (GetNode("Audio").GetChild(new Random().Next(0, 2)) as AudioStreamPlayer2D).Play(); 
            GetNode("Area2D").QueueFree();
            (GetParent() as GameLogic).OnCheckpoint();
        }

        arrived = true;
    }

    public void OnScreenExit()
    {
        QueueFree();
    }

}

