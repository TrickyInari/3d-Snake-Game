using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float sprintSpeed = 8f;
    public float turnSpeed = 120f;
    public float bodySpacing = 1.2f;                  // Distance between body segments

    [Header("References")]
    public GameObject bodyPartPrefab;                  // Assign prefab for snake body segments

    private List<Transform> bodyParts = new List<Transform>();
    private List<Vector3> positions = new List<Vector3>(); // Store positions for body follow
    private int score = 0;                             // Player's score
    private FoodSpawner spawner;                        // Cached reference to FoodSpawner

    void Start()
    {
        positions.Add(transform.position);
        spawner = FindFirstObjectByType<FoodSpawner>();
        if (spawner == null)
        {
            Debug.LogWarning("FoodSpawner not found in the scene.");
        }
    }

    void Update()
    {
        HandleMovement();
        UpdateBodyPositions();
    }

    void HandleMovement()
    {
        float move = 0f;
        float turn = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            move = 1f;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            move = -1f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            turn = -1f;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            turn = 1f;

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        if (move != 0)
            transform.Translate(Vector3.forward * speed * Time.deltaTime * move);

        if (turn != 0)
            transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);
    }

    void UpdateBodyPositions()
    {
        if ((transform.position - positions[0]).magnitude > bodySpacing)
        {
            positions.Insert(0, transform.position);
            positions.RemoveAt(positions.Count - 1);
        }

        for (int i = 0; i < bodyParts.Count; i++)
        {
            Vector3 targetPos = positions[Mathf.Min(i + 1, positions.Count - 1)];
            bodyParts[i].position = Vector3.Lerp(bodyParts[i].position, targetPos, 0.5f);
        }
    }

    public void Grow()
    {
        Vector3 newPartPos = bodyParts.Count > 0 ?
            bodyParts[bodyParts.Count - 1].position :
            transform.position - transform.forward * bodySpacing;

        GameObject newPart = Instantiate(bodyPartPrefab, newPartPos, Quaternion.identity);
        newPart.tag = "Body";
        bodyParts.Add(newPart.transform);
        positions.Add(newPart.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();

            int value = 1;
            FoodValue valueScript = other.GetComponent<FoodValue>();
            if (valueScript != null)
                value = valueScript.scoreValue;

            score += value;

            if (spawner != null)
                spawner.SpawnFood();

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Body"))
        {
            Debug.Log("Game Over! Hit your own body. Final Score: " + score);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Public getter method to allow other scripts (e.g., GameManager) to get the current score
    public int GetScore()
    {
        return score;
    }
}