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
    Tuple<String,float,float>[] enemySpawnTimers; // scene string name (enemy), spawntime, currenttime

    public uint enemyTypesCount = 1; // TODO: replace this with an enum
    public override void _Ready()
    {
         urgeBar = GetChild(4).GetChild(0).GetChild(0) as TextureProgress; 
         enemySpawnTimers = new Tuple<String,float,float>[enemyTypesCount]; // TODO: replace this with an enum
         enemySpawnTimers[0] = Tuple.Create("res://EnemyCoomer.tscn", 4f, 0f); 
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
     SpawnEnemyLogic(delta);      
 }

void SpawnEnemyLogic(float delta)
{
    for(int i = 0; i < enemySpawnTimers.Length; ++i)
    {
        var last = enemySpawnTimers[i]; 
        float currentTime = last.Item3 + delta; 
        if(currentTime >= last.Item2)
        {
            currentTime = 0f; 
            var scene = (PackedScene)ResourceLoader.Load(enemySpawnTimers[i].Item1);
            var instance = scene.Instance();
            AddChild(instance);

        }
        enemySpawnTimers[i] = Tuple.Create(last.Item1, last.Item2, currentTime); 

    }

    


}

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
