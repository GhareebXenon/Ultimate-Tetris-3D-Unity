using UnityEngine;

public class GroupSpawner : MonoBehaviour
{
    public GameObject[] groups;

    int currentIndex;
    int nextIndex;

    void Awake()
    {
        currentIndex = Random.Range(0, groups.Length);
        nextIndex = Random.Range(0, groups.Length);
    }

    public GameObject SpawnCurrent()
    {
        return Instantiate(
            groups[currentIndex],
            new Vector3(3, 14, 0),
            Quaternion.identity
        );
    }

    public void AdvanceQueue()
    {
        currentIndex = nextIndex;
        nextIndex = Random.Range(0, groups.Length);
    }

    public GameObject GetNextPrefab()
    {
        return groups[nextIndex];
    }
}
