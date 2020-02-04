using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    private AnimationPlayer animationPlayer; 
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
        animationPlayer.Play("run"); 
        animationPlayer.PlaybackSpeed = 0.8f; 
     }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
