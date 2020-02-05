using Godot;
using System;

public class GameLogic : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public float scrollSpeed = -200f;

    public float speedMultiplier = 1f; 
    // Called when the node enters the scene tree for the first time.

    TextureProgress urgeBar; 
    public override void _Ready()
    {
         urgeBar = GetChild(4).GetChild(0).GetChild(0) as TextureProgress; 
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

public void DoDamageToEntity(float damage, Node entity)
{

    if(entity.Name == "Player")
    {
        urgeBar.Value += damage; 
        GD.Print("Player damaged!! Urges are: " + ((entity as Player).urges = urgeBar.Value));
        if(urgeBar.Value >= 100)
        {
            // TODO: death
        }
    }
}

public float GetCurrentSpeed()
{
    return scrollSpeed * speedMultiplier; 
}

}
