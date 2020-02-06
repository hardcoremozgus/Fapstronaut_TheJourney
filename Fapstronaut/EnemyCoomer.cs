using Godot;
using System;

public class EnemyCoomer : Node2D
{
    // Declare member variables here. Examples:
    private float armSpeed = -10f; 
    private Vector2 speed; 
    private Sprite arm; 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arm = GetChild(0).GetChild(0) as Sprite; 
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
     arm.Rotate(armSpeed * delta); 
     Translate(new Vector2((GetParent() as GameLogic).GetCurrentSpeed(), 0) * delta); 
    
     // TODO: destroy if left of screen
  
 }


  public void ArmCollision(Node body)
  {
      GD.Print(body.Name); 
      if(body.Name == "Player" && !((body as Player).state == playerState.sliding)) // just ignore the player if sliding, instea
      {
          (GetParent() as GameLogic).DoDamageToEntity(20, body);
      }
      
  }

  public void OnScreenExit()
  {
      QueueFree(); 
  }
}
