using Godot;
using System;

public class Helper : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    private string b = "text";

    float time = 5f, currenTime = 0f;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
     public override void _Process(float delta)
     {
         if((currenTime += delta) >= time)
         {
             QueueFree(); 
         }
     }
}
