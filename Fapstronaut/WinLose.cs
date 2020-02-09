using Godot;
using System;

public class WinLose : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if ((GetParent() as MainScene).wonLastTime == false)
        {

            var node = GetChild(0).GetNode("Logic");
            (node.GetNode("WinLabel") as Label).Hide();
            (node.GetNode("LoseLabel") as Label).Show();

        }

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("PlayAgain"))
        {
            var scene = (PackedScene)ResourceLoader.Load("res://FapstronautScene.tscn");
            var instance = scene.Instance();
            GetParent().AddChild(instance);
            QueueFree();
        }
        if (Input.IsActionJustPressed("Quit"))
        {
            GetTree().Quit();
            QueueFree();
        }

    }
}
