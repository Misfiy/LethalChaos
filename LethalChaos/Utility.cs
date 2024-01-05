using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LethalChaos;

public static class Utility
{
    public static T GetRandom<T>(this List<T> list) => list[UnityEngine.Random.Range(0, list.Count)];
    public static List<T> ToList<T>(this Array array)
    {
        List<T> list = new();
        foreach (T element in array)
            list.Add(element);
        return list;
    }

    public static List<T> CombineWith<T>(this List<T> list1, List<T> list2)
    {
        foreach (T value in list2)
            list1.Add(value);
        return list1;
    }
    
    // Ty Brutal Company 
    public static GameObject FindEnemyPrefabByType(Type enemyType, List<SpawnableEnemyWithRarity> enemyList, SelectableLevel newLevel)
    {
        foreach (SpawnableEnemyWithRarity enemy in enemyList)
        {
            if (enemy.enemyType.enemyPrefab.GetComponent(enemyType) != null)
            {
                return enemy.enemyType.enemyPrefab;
            }
        }
        foreach (SpawnableEnemyWithRarity enemy2 in newLevel.Enemies)
        {
            if (enemy2.enemyType.enemyPrefab.GetComponent(enemyType) != null)
            {
                return enemy2.enemyType.enemyPrefab;
            }
        }
        throw new("Enemy type " + enemyType.Name + " not found and could not be added.");
    }
    
    // Ty Brutal Company (again)
    public static EnemyAI SpawnEnemyOutside(Type enemyType, bool ForceOutside)
    {
        GameObject val;
        val = !ForceOutside ? FindEnemyPrefabByType(enemyType, RoundManager.Instance.currentLevel.OutsideEnemies, RoundManager.Instance.currentLevel) : FindEnemyPrefabByType(enemyType, RoundManager.Instance.currentLevel.Enemies, RoundManager.Instance.currentLevel);
        GameObject[] array = GameObject.FindGameObjectsWithTag("OutsideAINode");
        Vector3 position = array[UnityEngine.Random.Range(0, array.Length)].transform.position;
        GameObject obj = Object.Instantiate(val, position, Quaternion.identity);
        obj.GetComponentInChildren<NetworkObject>().Spawn(true);
        EnemyAI component = obj.GetComponent<EnemyAI>();
        if (ForceOutside)
        {
            component.enemyType.isOutsideEnemy = true;
            component.allAINodes = GameObject.FindGameObjectsWithTag("OutsideAINode");
            component.SyncPositionToClients();
        }
        RoundManager.Instance.SpawnedEnemies.Add(component);
        return component;
    }

    public static float CurrentScrapAmountMultiplier => RoundManager.Instance.currentLevel.PlanetName.Contains("Titan") ? 3f : 1.85f;
    public static float CurrentScrapValueMultiplier => RoundManager.Instance.currentLevel.PlanetName.Contains("Titan") ? 1.6f : 1.2f;
}