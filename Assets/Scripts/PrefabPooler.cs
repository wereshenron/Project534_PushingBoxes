using UnityEngine;
using System.Collections.Generic;

public class PrefabPooler : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10; 
    public float spacing = 2f;
    public int gridWidth = 5;

    private List<GameObject> _objectPool = new ();
    private List<Vector3> _spawnPositions = new();

    void Start()
    {
        GeneratePool();
    }

    void GeneratePool()
    {
        int row = 0, col = 0;
        _spawnPositions.Clear();

        for (int i = 0; i < poolSize; i++)
        {
            // Calculate position using row and column index
            Vector3 spawnPosition = gameObject.transform.position + new Vector3(col * spacing, 0, row * spacing);

            // Instantiate and store in pool
            GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.Euler(-90, 0, -90));
            _objectPool.Add(obj);

            // Update grid indices
            col++;
            if (col >= gridWidth)
            {
                col = 0;
                row++;
            }
        }
    }

     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        int row = 0, col = 0;

        for (int i = 0; i < poolSize; i++)
        {
            Vector3 spawnPosition = gameObject.transform.position + new Vector3(col * spacing, 0, row * spacing);
            Gizmos.DrawWireSphere(spawnPosition, 0.3f);

            col++;
            if (col >= gridWidth)
            {
                col = 0;
                row++;
            }
        }
    }
}
