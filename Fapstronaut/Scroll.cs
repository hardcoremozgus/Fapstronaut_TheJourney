using Godot;
using System;

public class Scroll : Node2D
{
    // Declare member variables here. Examples:
   private float accumulatedWidth; 

   private float speed; 
   Sprite childNode; 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        childNode = GetChild(0) as Sprite;
        speed = (GetParent() as GameLogic).scrollSpeed; // CAUTION: assumes "Root" as parent
        accumulatedWidth = 0; 
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
    float magnitude = speed * delta; 
    accumulatedWidth += -magnitude; 
    childNode.Translate(new Vector2(magnitude, 0)); 

    if(accumulatedWidth >= OS.WindowSize.x)
    {
        childNode.Translate(new Vector2((float)OS.WindowSize.x + (accumulatedWidth - (float)OS.WindowSize.x), 0)); 
        accumulatedWidth = 0; 
    }
 }
}
