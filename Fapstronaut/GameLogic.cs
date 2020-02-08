using Godot;
using System;

public enum EntityTypes { COOMER, SOCIALMEDIA, POSTER, MAXTYPES };
public class GameLogic : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public float scrollSpeed = -200f;

    public float PI = 3.14159f;

    public float levelUpPercentatge = 0.05f,
    currentLevelUpPercentage = 0.05f;

    public float speedMultiplier = 1f;
    // Called when the node enters the scene tree for the first time.

    TextureProgress urgeBar;
    Tuple<String, float, float>[] spawnTimers; // scene string name, spawntime, currenttime

    public EntityTypes entityTypes;

    public override void _Ready()
    {
        urgeBar = GetChild(4).GetChild(0).GetChild(0) as TextureProgress;
        spawnTimers = new Tuple<String, float, float>[(int)EntityTypes.MAXTYPES];
        spawnTimers[0] = Tuple.Create("res://EnemyCoomer.tscn", 8f, 0f);
        spawnTimers[1] = Tuple.Create("res://EnemySocialMedia.tscn", 2f, 0f);
        spawnTimers[2] = Tuple.Create("res://Poster.tscn", 5f, 0f);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        SpawnEnemyLogic(delta);
    }

    void SpawnEnemyLogic(float delta)
    {
        for (int i = 0; i < spawnTimers.Length; ++i)
        {
            var last = spawnTimers[i];
            float currentTime = last.Item3 + delta;
            if (currentTime >= last.Item2)
            {
                currentTime = 0f;
                AddScene(spawnTimers[i].Item1, this);
            }
            spawnTimers[i] = Tuple.Create(last.Item1, last.Item2, currentTime);

        }

    }



    public void AddScene(String scenePath, Node parent)
    {
        var scene = (PackedScene)ResourceLoader.Load(scenePath);
        var instance = scene.Instance();
        parent.AddChild(instance);
    }

    public void DoDamageToEntity(float damage, Node entity)
    {

        if (entity.Name == "Player")
        {
            urgeBar.Value += damage;
            GD.Print("Player damaged!! Urges are: " + ((entity as Player).urges = urgeBar.Value));
            if (urgeBar.Value >= 100)
            {
                // TODO: death
            }
        }
    }

    public float GetCurrentSpeed()
    {
        return scrollSpeed * speedMultiplier;
    }

    public void OnCheckpoint()
    {
        currentLevelUpPercentage += levelUpPercentatge;

        Player player = (GetChild(0) as Player);
        if (player.postersArrived > 10)
        {
            GetTree().Quit(); // TODO: a win screen
            return;
        }

        if (player.chad == false)
        {
            player.LevelUp();
        }


        // Enemies spawned more often
        for (int i = 0; i < spawnTimers.Length; ++i)
        {
            var last = spawnTimers[i];
            spawnTimers[i] = Tuple.Create(last.Item1, last.Item2 - last.Item2 * levelUpPercentatge, last.Item3);
        }

    }

}
