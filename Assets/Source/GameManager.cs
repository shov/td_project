using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string PLAYER_TAG = "Player";
    public static string ENEMY_TAG = "Enemy";


    // Connected with Camps
    // Manage game state
    //  - Play / Pause
    //  - Win / Defeat

    public Camp playerCamp;
    public Camp enemyCamp;

    private void Start()
    {
        /*playerCamp.onDefeat += OnPlayerDefeat;
        enemyCamp.onDefeat += OnEnemyDefeat;*/

        // Build the main towers
        playerCamp.BuildTower();
        enemyCamp.BuildTower();
    }



    // UI actions
    public void PlayerSpawnASwordman()
    {
        playerCamp.SpawnUnit(Camp.EUnitType.Swordman);
    }

    public void PlayerBuildTower()
    {
        playerCamp.BuildTower();
    }

    public void EnemySpawnASwordman()
    {
        enemyCamp.SpawnUnit(Camp.EUnitType.Swordman);
    }

    public void EnemyBuildTower()
    {
        enemyCamp.BuildTower();
    }
}
