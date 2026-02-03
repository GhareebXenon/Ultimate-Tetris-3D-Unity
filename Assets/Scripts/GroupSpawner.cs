using UnityEngine;

public class GroupSpawner : MonoBehaviour
{
    public GameObject[] groups;

    private int currentIndex;
    private int nextIndex;

    void Awake()
    {
        currentIndex = Random.Range(0, groups.Length);
        nextIndex = Random.Range(0, groups.Length);
    }

    // Spawn current WITHOUT changing next yet
    public GameObject SpawnCurrent()
    {
        GameObject obj = Instantiate(
            groups[currentIndex],
            new Vector3(3, 14, 0),
            Quaternion.identity
        );

        return obj;
    }

    // Advance queue AFTER piece is locked
    public void AdvanceQueue()
    {
        currentIndex = nextIndex;
        nextIndex = Random.Range(0, groups.Length);
    }

    // Preview always shows NEXT
    public GameObject GetNextPrefab()
    {
        return groups[nextIndex];
    }
}
