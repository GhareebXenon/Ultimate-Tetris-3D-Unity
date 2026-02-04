using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float timestep = 0.2f;
    private float time;
    private bool fastDrop;
    // The actual group which can rotate and will move down
    public GameObject actualGroup;
    public void startGame() { actualGroup = GetComponent<GroupSpawner>().SpawnCurrent(); }
    void Start()
    {
        // Spawn first piece
        //startGame();

        // Update next preview
        GetComponent<NextPiecePreview>()?.UpdatePreview();
    }

    void Update()
    {
        if (actualGroup == null)
            return;

        // Handle automatic fall
        time += Time.deltaTime;
        if (time > timestep)
        {
            time = 0;
            Move(Vector3.down);
        }
        if (fastDrop) { timestep = 0.05f; }
        else { SetNewSpeed(); }
            
        CheckForInput();
    }

    void CheckForInput()
    {
        if (actualGroup == null)
            return;

        // Rotate
        if (Input.GetKeyDown(KeyCode.R))
            actualGroup.GetComponent<Rotation>().rotateRight();
        if (Input.GetKeyDown(KeyCode.L))
            actualGroup.GetComponent<Rotation>().rotateLeft();

        // Move sideways
        if (Input.GetKeyDown(KeyCode.A))
            Move(Vector3.left);
        if (Input.GetKeyDown(KeyCode.D))
            Move(Vector3.right);

        if (Input.GetKeyDown(KeyCode.S))
        {
            FastDropOn();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            FastDropOff();
        }
        // Update cube positions
        GetComponent<CubeArray>().getCubePositionFromScene();
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }


    public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void RotateLeft()
    {
        actualGroup?.GetComponent<Rotation>().rotateLeft();
    }

    public void RotateRight()
    {
        actualGroup?.GetComponent<Rotation>().rotateRight();
    }
    public void FastDropOn()
    {
        fastDrop = true;
    }

    public void FastDropOff()
    {
        fastDrop = false;
    }

    // Normal speed based on level
    public void SetNewSpeed()
    {
        float baseSpeed = (10 - GetComponent<Highscore>().level) * 0.05f;
        timestep = Mathf.Max(baseSpeed, 0.1f); // Ensures it never goes faster than 0.1s per drop
    }

    void Move(Vector3 dir)
    {
        actualGroup.transform.position += dir;

        // Check collision with grid
        if (!GetComponent<CubeArray>().IsValidPosition(actualGroup.transform))
        {
            actualGroup.transform.position -= dir;
            GameObject.Find("CantMove")?.GetComponent<AudioSource>()?.Play();

            if (dir == Vector3.down)
                SpawnNew();
        }
    }

    void SpawnNew()
    {
        actualGroup.GetComponent<Rotation>().isActive = false;

        // Lock the blocks
        foreach (Transform block in actualGroup.transform)
            block.tag = "LockedCube";

        GroupSpawner spawner = GetComponent<GroupSpawner>();

        // Advance queue before spawning new piece
        spawner.AdvanceQueue();

        // Spawn next piece
        actualGroup = spawner.SpawnCurrent();
        actualGroup.GetComponent<Rotation>().isActive = true;

        // Update preview
        GetComponent<NextPiecePreview>()?.UpdatePreview();

        // Check spawn collision (Game Over)
        if (GetComponent<CubeArray>().IsValidPosition(actualGroup.transform))
        {
            GetComponent<CubeArray>().checkForFullLine();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }
}
