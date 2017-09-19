using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    private static bool gameMasterFoundOnce = false;
    private static GameMaster instance = null;

    public bool DebugOn = true;

    public static GameMaster Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType(typeof(GameMaster)) as GameMaster;
                if (instance == null)
                {
                    if (gameMasterFoundOnce)
                    {
                        return null;
                    }
                    instance = CreateGameMaster();
                    Debug.Log("AppMaster created!");
                }
                instance.transform.parent = null;
                Object.DontDestroyOnLoad(instance.gameObject);
            }
            gameMasterFoundOnce = true;
            return instance;
        }
    }

    private static GameMaster CreateGameMaster()
    {
        GameObject gO = new GameObject("GameMaster");
        GameMaster gM = gO.AddComponent<GameMaster>();

        return gM;
    }


    void Start ()
    {
		
	}
	
	void Update ()
    {



        /// DEBUG
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DebugOn = !DebugOn;
            Debug.Log("Debug: " + DebugOn);
        }
        if (Input.GetKeyDown(KeyCode.AltGr))
        {
            Time.timeScale = Time.timeScale == 1 ? 0.1f : 1f;
            Debug.Log("Timescale: " + Time.timeScale);
        }
	}
}
