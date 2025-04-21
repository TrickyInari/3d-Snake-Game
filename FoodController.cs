using UnityEngine;

public class FoodController : MonoBehaviour
{
    public Vector3 spawnAreaMin = new Vector3(-20f, 0.5f, -20f);  // Set to (-20, ...) for 40x40 spawn
    public Vector3 spawnAreaMax = new Vector3(20f, 0.5f, 20f);    // Set to (20, ...) for 40x40 spawn

    public void Respawn()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        float y = spawnAreaMin.y; // Use the same y position for consistency
        transform.position = new Vector3(x, y, z);
    }

    void Start()
    {
        Respawn();
    }
}