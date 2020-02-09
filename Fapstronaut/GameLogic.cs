using Godot;
using System;

public enum EntityTypes { COOMER, SOCIALMEDIA, POSTER, TWITCHTHOT, MAXTYPES };
public class GameLogic : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public float scrollSpeed = -200f;

    public int activeThots = 0; // very dirty
    public float PI = 3.14159f;

    public float levelUpPercentatge = 0.05f,
    currentLevelUpPercentage = 0.05f;

    public float speedMultiplier = 1f;
    // Called when the node enters the scene tree for the first time.

    TextureProgress urgeBar;
    Tuple<String, float, float>[] spawnTimers; // scene string name, spawntime, currenttime

    public EntityTypes entityTypes;

    float previousMusicTime = 0f;

    bool spawnsStopped = false;

    int spawnEnemyIndex = 666;

    public bool derCoomer = false, jewishBoss = false;

    public bool IsDerCoomerActive() => derCoomer;

    public override void _Ready()
    {
        urgeBar = GetChild(4).GetChild(0).GetChild(0).GetChild(0) as TextureProgress;
        spawnTimers = new Tuple<String, float, float>[(int)EntityTypes.MAXTYPES];
        spawnTimers[0] = Tuple.Create("res://EnemyCoomer.tscn", 15f, 0f);
        spawnTimers[1] = Tuple.Create("res://EnemySocialMedia.tscn", 3.5f, 0f);
        spawnTimers[2] = Tuple.Create("res://Poster.tscn", 30f, 0f);
        spawnTimers[3] = Tuple.Create("res://EnemyTwitchThot.tscn", 20f, 0f);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    
        if (spawnsStopped == false)
        {
            SpawnEnemyLogic(delta);
        }

    }

    void SpawnEnemyLogic(float delta)
    {
        for (int i = 0; i < spawnTimers.Length; ++i)
        {
            if ((spawnEnemyIndex != 666) && (i != spawnEnemyIndex))
            {
                continue;
            }

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
        GD.Print("Entity damaged!");

        if ((entity.Name == "Player") && ((entity as Player).godMode))
        {
            return;
        }

        // CAUTION: make sure there is an audio for new entities
        (entity.GetNode("Audio").GetNode("Damaged") as AudioStreamPlayer2D).Play();


        switch (entity.Name)
        {
            case "Player":
                {
                    (entity as Player).urges += damage;
                    if ((entity as Player).urges >= 100)
                    {
                        Finish(false); 
                    }
                    break;
                }
            case "EnemyTwitchThot":
                {
                    var enemy = entity as EnemyTwitchThot;
                    float life = enemy.life -= damage;
                    if (life <= 0)
                    {
                        activeThots--;
                        (GetNode("Player") as Player).Incapacitate(false);
                        enemy.QueueFree();
                    }
                    break;
                }
            case "EnemyDerCoomer":
                {
                    var enemy = entity as EnemyDerCoomer;
                    float life = enemy.life -= damage;
                    if (life <= 0)
                    {
                        BossTrigger(false, true);
                        (enemy.GetNode("Audio").GetNode("Theme") as AudioStreamPlayer2D).Stop();
                        enemy.QueueFree();
                    }

                    break;
                }

            case "EnemyJewishBoss":
                {
                    var enemy = entity as EnemyJewishBoss;
                    float life = enemy.life -= damage;
                    if (life <= 0)
                    {
                        BossTrigger(false, false);
                        (enemy.GetNode("Audio").GetNode("Theme") as AudioStreamPlayer2D).Stop();
                        enemy.QueueFree();
                        Finish(true); 
                    }

                    break;
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
        if (player.brainFog <= 0)
        {
            GetTree().Quit(); // TODO: a win screen (announcing you won because of not brain fog, after beating bosses)
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

        // Check for boss trigger
        if (player.postersArrived == 7)  // 2 Years = Der Coomer boss
        {
            BossTrigger(true, true);
        }
        else if (player.postersArrived == 10)  // 5 Years = Jewish boss
        {
            BossTrigger(true, false);
        }

    }



    public void BossTrigger(bool spawn, bool derCoomer)
    {
        if (spawn)
        {

            if (derCoomer)
            {
                this.derCoomer = true;
                spawnEnemyIndex = 1;
                (GetNode("Dark") as Dark).Darken(0.6f, 0.5f);
            }

            else
            {
                this.jewishBoss = true;
                spawnsStopped = true;
                (GetNode("Player") as Player).horziontalLocked = false; 
            }
            var music = GetNode("Music").GetNode("MusicChad") as AudioStreamPlayer2D;
            music.Stop();
            previousMusicTime = music.GetPlaybackPosition();
            String scene = (derCoomer) ? "res://EnemyDerCoomer.tscn" : "res://EnemyJewishBoss.tscn";
            AddScene(scene, this);

        }
        else
        {
            if (derCoomer)
            {
                this.derCoomer = false;
                spawnEnemyIndex = 666;
                (GetNode("Dark") as Dark).Darken(0f, -0.5f);
            }
            else
            {
                this.jewishBoss = false;
                spawnsStopped = false;
            }
            
            var music = GetNode("Music").GetNode("MusicChad") as AudioStreamPlayer2D;
            music.Play();
            music.Seek(previousMusicTime);


        }

    }

 
  
    private void Finish(bool win)
    {
        (GetParent() as MainScene).wonLastTime = win; 
        AddScene("res://WinLose.tscn", GetParent()); 
        QueueFree(); 
    }

}
