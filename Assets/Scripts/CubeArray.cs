using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
public class CubeArray : MonoBehaviour
{
    public const int width = 10;
    public const int height = 17;

    bool[,] isCube = new bool[width, height];

    // 🔹 Call ONLY when a piece locks
    public void RebuildGrid()
    {
        isCube = new bool[width, height];

        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("LockedCube"))
        {
            int x = Mathf.RoundToInt(cube.transform.position.x);
            int y = Mathf.RoundToInt(cube.transform.position.y);

            if (x >= 0 && x < width && y >= 0 && y < height)
                isCube[x, y] = true;
        }
    }

    // 🔹 Pure check (NO side effects)
    public bool IsValidPosition(Transform group)
    {
        foreach (Transform block in group)
        {
            int x = Mathf.RoundToInt(block.position.x);
            int y = Mathf.RoundToInt(block.position.y);

            if (x < 0 || x >= width || y < 0)
                return false;

            if (y < height && isCube[x, y])
                return false;
        }
        return true;
    }



public void checkForFullLine()
{
    getCubePositionFromScene(); // rebuild grid FIRST

    List<int> fullLines = new List<int>();

    for (int y = 0; y < isCube.GetLength(1); y++)
    {
        bool full = true;
        for (int x = 0; x < isCube.GetLength(0); x++)
        {
            if (!isCube[x, y])
            {
                full = false;
                break;
            }
        }
        if (full) fullLines.Add(y);
    }

    if (fullLines.Count == 0)
        return;

    GetComponent<Highscore>().addPointsForLines(fullLines.Count);

    GameObject[] lockedCubes = GameObject.FindGameObjectsWithTag("LockedCube");

    List<GameObject> lineCubes = new List<GameObject>();

    foreach (GameObject cube in lockedCubes)
    {
        int y = Mathf.RoundToInt(cube.transform.position.y);
        if (fullLines.Contains(y))
            lineCubes.Add(cube);
    }

    //  DOTWEEN SEQUENCE
    Sequence seq = DOTween.Sequence();

    foreach (GameObject cube in lineCubes)
    {
        seq.Join(
            cube.transform
                .DOScale(Vector3.zero, 0.4f)
                .SetEase(Ease.InBack)
        );
    }

    seq.OnComplete(() =>
    {
        // DESTROY AFTER ANIMATION
        foreach (GameObject cube in lineCubes)
        {
            if (cube != null)
                Destroy(cube);
        }

        // DROP CUBES ABOVE
        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("LockedCube"))
        {
            int y = Mathf.RoundToInt(cube.transform.position.y);
            int drop = fullLines.Count(line => line < y);
            if (drop > 0)
            {
                cube.transform.position += Vector3.down * drop;
            }
        }

        GameObject.Find("FullLine")?.GetComponent<AudioSource>()?.Play();

        // rebuild grid after everything
        getCubePositionFromScene();
    });
}


//Update the cube array and return false if there is any intersection between two cubes
public bool getCubePositionFromScene()
    {
        isCube = new bool[10, 17]; foreach (GameObject cube in GameObject.FindGameObjectsWithTag("LockedCube"))
        {
            int x = (int)cube.transform.position.x; int y = (int)cube.transform.position.y; if (x >= 0 && x < isCube.GetLength(0) && y >= 0 && y < isCube.GetLength(1))
            {
                bool cubeSetted = isCube[x, y]; if (cubeSetted)
                {  //Two cubes have the same position --> intersection
                    return false;
                }
                else
                { isCube[x, y] = true; }
            }
            else
            { //Position is out of range, e.g. when we are trying to set down when it's not possible
                return false;
            }
        }
        return true;
    }

    void MoveDownAbove(int y)
    {
        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("LockedCube"))
        {
            if (Mathf.RoundToInt(cube.transform.position.y) > y)
                cube.transform.position += Vector3.down;
        }
    }
}
