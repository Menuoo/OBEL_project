using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance { get; private set; }

    [SerializeField] PlayerController player;

    int currentID = 0;
    public Dictionary<int, EnemyBase> enemyList { get; private set; }


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            currentID = 0;
            enemyList = new Dictionary<int, EnemyBase>();
        }
    }

    private void OnEnable()
    {
        SceneControl.SceneChangeEvent += ClearEnemies;
    }

    private void OnDisable()
    {
        SceneControl.SceneChangeEvent -= ClearEnemies;
    }

    public int AddEnemy(EnemyBase toAdd)
    { 
        enemyList.Add(currentID++, toAdd);

        Debug.Log(toAdd.name + " added to Enemy List");

        return currentID - 1;
    }

    public void RemoveEnemy(int id)
    { 
        enemyList.Remove(id);
    }

    public void ClearEnemies(bool state)
    { 
        enemyList.Clear();
    }

    public PlayerController GetPlayer() => player;
}



/*public static class EnemyManager
{
    static int currentID = 0;
    public static Dictionary<int, EnemyBase> enemyList = new Dictionary<int, EnemyBase>();

    public static int AddEnemy(EnemyBase toAdd)
    {
        enemyList.Add(currentID++, toAdd);

        return currentID - 1;
    }

    public static void RemoveEnemy(int id)
    { 
        enemyList.Remove(id);
    }
}*/
