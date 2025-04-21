using System.Collections.Generic;
using UnityEngine;

[System.Serializable]  // Allows this class to be editable in the Inspector
public class SpawnableFood
{
    public GameObject prefab;
    public int weight = 1;      // The higher, the more likely (acts as a % if total is 100)
}

public class FoodSpawner : MonoBehaviour
{
    public List<SpawnableFood> foodTypes = new List<SpawnableFood>();
    public Vector3 spawnAreaMin = new Vector3(-20f, 0.5f, -20f);
    public Vector3 spawnAreaMax = new Vector3(20f, 0.5f, 20f);

    public int maxFoodCount = 3;  // Maximum number of food pieces at a time
    private List<GameObject> currentFoods = new List<GameObject>();

    void Start()
    {
        // Spawn initial batch of food
        SpawnMultipleFood(maxFoodCount);
    }

    // Call this to spawn a single food piece if under max count
    public void SpawnFood()
    {
        // Remove null references from currentFoods (food got eaten/destroyed)
        currentFoods.RemoveAll(item => item == null);

        if (currentFoods.Count >= maxFoodCount)
            return; // already max food spawned

        SpawnSingleFood();
    }

    // Spawns up to count food pieces
    public void SpawnMultipleFood(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnFood();
        }
    }

    // Internal method to spawn one food piece at a random position
    private void SpawnSingleFood()
    {
        GameObject prefabToSpawn = GetRandomFood();
        if (prefabToSpawn == null)
            return;

        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        float y = spawnAreaMin.y;
        Vector3 spawnPos = new Vector3(x, y, z);

        GameObject food = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        currentFoods.Add(food);
    }

    // Weighted random selection of food prefab
    GameObject GetRandomFood()
    {
        int totalWeight = 0;
        foreach (var food in foodTypes)
            totalWeight += food.weight;
        if (totalWeight == 0)
            return null;

        int rand = Random.Range(1, totalWeight + 1);
        foreach (var food in foodTypes)
        {
            rand -= food.weight;
            if (rand <= 0)
                return food.prefab;
        }
        return null; // fallback
    }
}