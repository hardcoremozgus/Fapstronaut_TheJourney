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
    speed = (GetParent() as GameLogic).scrollSpeed * (GetParent() as GameLogic).speedMultiplier; 
    float magnitude = speed * delta; 
    accumulatedWidth += -magnitude; 
    childNode.Translate(new Vector2(magnitude, 0)); 

    if(accumulatedWidth >= GetViewportRect().Size.x)
    {
        childNode.Translate(new Vector2((float)GetViewportRect().Size.x + (accumulatedWidth - (float)GetViewportRect().Size.x), 0)); 
        accumulatedWidth = 0; 
    }
 }
}
