using Godot;
using System;

public class DeathLogic : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
     private AudioStreamPlayer2D audioPlayer; 
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioPlayer = GetChild(0) as AudioStreamPlayer2D; 
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 
 public void OnAudioFinish()
 {
     QueueFree(); 
 }
 
}
