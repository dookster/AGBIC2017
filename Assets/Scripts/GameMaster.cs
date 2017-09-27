using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    private static bool gameMasterFoundOnce = false;
    private static GameMaster instance = null;

    public bool DebugOn = true;

    public Camera mainCam;
    public CameraController camControl;

    public List<PlanetLevel> levels;
    public float levelLoadTime = 3f;
    public int currentLevelIndex = 0;

    public bool Paused = false;

    private NewGrub grub;

    private Coroutine nextLevelRoutine;
    private Coroutine floatToPlanetRoutine;

    private List<AudioSource> audioSources = new List<AudioSource>();

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
        grub = GameObject.Find("Grub").GetComponent<NewGrub>();
        currentLevelIndex = 0;
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

        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextLevel();
        }
	}

    public void LoadNextLevel()
    {
        Paused = true;

        if(nextLevelRoutine != null)
        {
            Debug.LogError("LOAD LEVEL routine not null");
        }
        else
        {
            StartCoroutine( LoadLevelRoutine(levels[currentLevelIndex == 1 ? 0 : 1]));
            grub.GoToPlanetLevel(levels[currentLevelIndex == 1 ? 0 : 1], levelLoadTime);
        }

    }

    IEnumerator LoadLevelRoutine(PlanetLevel level)
    {
        Color currentColor = mainCam.backgroundColor;
        float t = 0;
        while(t <= 1)
        {
            t += Time.deltaTime;
            mainCam.backgroundColor = Color.Lerp(currentColor, Color.black, t);
            levels[currentLevelIndex].planet.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(0, -100, 0), t);
            yield return 0;
        }
        t = 0;

        level.planet.gameObject.SetActive(true);
        level.planet.transform.position = new Vector3(0, -100, 0);

        while (t <= levelLoadTime)
        {
            t += Time.deltaTime;
            level.planet.transform.position = Vector3.Lerp(new Vector3(0, -100, 0), Vector3.zero, t / levelLoadTime);
            yield return 0;
        }
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            mainCam.backgroundColor = Color.Lerp(Color.black, level.spaceColor, t);
            yield return 0;
        }
        Paused = false;

        levels[currentLevelIndex].planet.gameObject.SetActive(false);

        grub.currentPlanet = level.planet;

        // Just switch back and forth
        currentLevelIndex = currentLevelIndex == 0 ? 1 : 0;

        nextLevelRoutine = null;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        AudioSource s = GetFreeAudioSource();
        s.volume = volume;
        s.clip = clip;
        s.Play();
    }

    private AudioSource GetFreeAudioSource()
    {
        foreach(AudioSource aS in audioSources)
        {
            if (!aS.isPlaying)
            {
                return aS;
            }
        }
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        audioSources.Add(newSource);
        newSource.spatialBlend = 0;
        return newSource;
    }

}

[System.Serializable]
public struct PlanetLevel
{
    public string title;
    public string message;
    public Color spaceColor;
    public Gradient juiceColor;
    public GameObject planet;
    public float eatTarget;
    public Transform grubTarget;
}
