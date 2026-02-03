using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Movement : MonoBehaviour {
	public float timestep = 0.2F; 
	float time;

    void Start()
    {
        

        
        GetComponent<NextPiecePreview>().UpdatePreview();
    }
    //The actual group which can rotate and will move down
    public GameObject actualGroup; 

	public void startGame(){
		actualGroup = this.gameObject.GetComponent<GroupSpawner> ().SpawnCurrent ();
	}
    //Move down in interval of timestep
    void Update()
    {
        if (actualGroup == null)
            return; //  do nothing until a group exists

        time += Time.deltaTime;
        if (time > timestep)
        {
            time = 0;
            move(Vector3.down);
        }

        checkForInput();
    }


    void checkForInput()
    {
        if (actualGroup == null) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            actualGroup.GetComponent<Rotation>().rotateRight();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            actualGroup.GetComponent<Rotation>().rotateLeft();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            move(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            move(Vector3.right);
        }

        if (Input.GetKey(KeyCode.S))
        {
            timestep = 0.05F;
        }
        else
        {
            setNewSpeed();
        }

        GetComponent<CubeArray>().getCubePositionFromScene();
    }



    //Speed increasement found at http://www.colinfahey.com/tetris/tetris.html 5.10 
    public void setNewSpeed(){
		timestep = ((10 - gameObject.GetComponent<Highscore> ().level) * 0.05F);
	}

	void move(Vector3 pos){
		actualGroup.transform.position += pos; 
		if (!gameObject.GetComponent<CubeArray> ().getCubePositionFromScene ()) {
			actualGroup.transform.position -= pos; 
			GameObject.Find("CantMove").GetComponent<AudioSource>().Play();
			if(pos == Vector3.down){
				spawnNew (); 
			}
		}
	}

    //Handle spawning a new group and check if there is any intersection after spawning
    private void spawnNew()
    {
        foreach (Transform block in actualGroup.transform)
            block.tag = "Cube";

        GroupSpawner spawner = GetComponent<GroupSpawner>();

        // advance queue BEFORE spawning
        spawner.AdvanceQueue();

        actualGroup = spawner.SpawnCurrent();

        // update preview AFTER advance
        GetComponent<NextPiecePreview>().UpdatePreview();

        if (!GetComponent<CubeArray>().getCubePositionFromScene())
        {
            Application.LoadLevel(Application.loadedLevelName);
        }
        else
        {
            GetComponent<CubeArray>().checkForFullLine();
        }
    }
}