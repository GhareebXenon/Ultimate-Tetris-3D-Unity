using UnityEngine;

public class EnableSecondDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            
        }

        
    }


}
