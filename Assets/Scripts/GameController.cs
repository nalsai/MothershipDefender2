using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [System.Serializable]
    public class Settings
    {
        public bool Music;
        public bool SFX;
        public bool ThreeLives;
        public bool HardMode;
    }

    public GameObject Soundtrack;
    public GameObject LivesObject;
    public GameObject SpaceshipFighter;
    public GameObject Spaceship2;
    public GameObject Mothership;
    public GameObject Player;
    public GameObject Pause;
    public GameObject start;
    public GameObject Restart;
    public GameObject PauseButton;
    public GameObject SettingsButton;
    public Text scoreText;
    public Text LivesText;
    public static int score;
    public static int Lives;
    public static int Ships = 3;
    public static bool noShip;
    public static bool restart;
    public static bool endpause;

    public AudioClip DefaultAudioClip;
    public AudioClip HardAudioClip;
    public GameObject MusicToogle;
    public GameObject SFXToggle;
    public GameObject ThreeLivesToggle;
    public GameObject HardModeToggle;
    public static Settings settings = new();

    const int StartWait = 2;
    Vector3 RndmPstn;
    float width;
    float height;

    private void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value * 2;

        if (Application.platform == RuntimePlatform.Android)
        {
            Application.targetFrameRate = -1;
        }

        if (File.Exists(Application.persistentDataPath + "/settings.sav"))
        {
            BinaryFormatter bf = new();
            FileStream file = File.Open(Application.persistentDataPath + "/settings.sav", FileMode.Open);
            settings = (Settings)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            settings.Music = true;
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                settings.Music = false;
            settings.SFX = true;
            settings.ThreeLives = false;
            settings.HardMode = false;

            BinaryFormatter bf = new();
            FileStream file = File.Create(Application.persistentDataPath + "/settings.sav");
            bf.Serialize(file, settings);
            file.Close();
        }

        MusicToogle.GetComponent<Toggle>().SetIsOnWithoutNotify(settings.Music);
        Soundtrack.SetActive(settings.Music);

        SFXToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(settings.SFX);

        ThreeLivesToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(settings.ThreeLives);
        LivesObject.SetActive(settings.ThreeLives);

        HardModeToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(settings.HardMode);
        if (!settings.HardMode)
            StartCoroutine(Spawn());
        else
        {
            StartCoroutine(SpawnHardMode());
            Soundtrack.GetComponent<AudioSource>().Stop();
            Soundtrack.GetComponent<AudioSource>().clip = HardAudioClip;
            if (settings.Music)
                Soundtrack.GetComponent<AudioSource>().Play();
        }
    }

    private void Update()
    {
        if (restart)
            StartCoroutine(RestartM());
        scoreText.text = "Score: " + score;
        LivesText.text = "Lives:  " + Lives;
    }

    public void ToggleMusic()
    {
        settings.Music = !settings.Music;
        Soundtrack.SetActive(settings.Music);

        BinaryFormatter bf = new();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.sav");
        bf.Serialize(file, settings);
        file.Close();
    }

    public void ToggleSFX()
    {
        settings.SFX = !settings.SFX;

        BinaryFormatter bf = new();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.sav");
        bf.Serialize(file, settings);
        file.Close();
    }

    public void ToggleThreeLives()
    {
        settings.ThreeLives = !settings.ThreeLives;
        LivesObject.SetActive(settings.ThreeLives);
        
        BinaryFormatter bf = new();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.sav");
        bf.Serialize(file, settings);
        file.Close();
    }

    public void ToggleHardMode()
    {
        settings.HardMode = !settings.HardMode;
        StopAllCoroutines();
        Soundtrack.GetComponent<AudioSource>().Stop();

        if (settings.HardMode)
        {
            StartCoroutine(SpawnHardMode());
            Soundtrack.GetComponent<AudioSource>().clip = HardAudioClip;
        }
        else
        {
            StartCoroutine(Spawn());
            Soundtrack.GetComponent<AudioSource>().clip = DefaultAudioClip;
        }
        if (settings.Music)
            Soundtrack.GetComponent<AudioSource>().Play();

        BinaryFormatter bf = new();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.sav");
        bf.Serialize(file, settings);
        file.Close();
    }

    public void StartGame()
    {
        Destroy(GameObject.Find("Spaceship(Clone)"));
        Destroy(GameObject.Find("Explosion(Clone)"));
        Destroy(GameObject.Find("ExplosionMobile(Clone)"));
        Instantiate(Mothership);
        Instantiate(Player);
        SettingsButton.SetActive(false);
        PauseButton.SetActive(true);
        Restart.SetActive(false);
        start.SetActive(false);
        score = 0;
        if (settings.ThreeLives)
        {
            Lives = 3;
        }
        else
        {
            Lives = 1;
        }
        Mover.speed = 10;
        noShip = true;
        Ships = 0;
    }
    
    public void PauseGame()
    {
        PauseButton.SetActive(false);
        noShip = false;
        Ships = 3;
        Pause.SetActive(true);
        Destroy(GameObject.Find("Player(Clone)"));
        Destroy(GameObject.Find("Mothership(Clone)"));
        GameObject[] AShips = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject Ship in AShips)
            Destroy(Ship);
        Cursor.visible = true;
    }

    public void EndPause()
    {
        PauseButton.SetActive(true);
        Pause.SetActive(false);
        Instantiate(Mothership);
        Instantiate(Player);
        noShip = true;
        Ships = 0;
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(StartWait);
        while (true)
        {
            yield return new WaitUntil(() => noShip == true);

            CheckScreenDimensions();

            // Choose where to spawn
            int choose = Random.Range(0, 4);
            if (choose == 0)
            {
                RndmPstn.x = Random.Range(-width, width);
                RndmPstn.z = height;
            }
            if (choose == 1)
            {
                RndmPstn.x = Random.Range(-width, width);
                RndmPstn.z = -height;
            }
            if (choose == 2)
            {
                RndmPstn.x = width;
                RndmPstn.z = Random.Range(-height, height);
            }
            if (choose == 3)
            {
                RndmPstn.x = -width;
                RndmPstn.z = Random.Range(-height, height);
            }
            RndmPstn.y = 0;

            // Spawn
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(Spaceship2, RndmPstn, transform.rotation);
            }
            else
            {
                Instantiate(SpaceshipFighter, RndmPstn, transform.rotation);
            }
            noShip = false;
            Mover.speed += 1;
        }
    }

    IEnumerator SpawnHardMode()
    {
        yield return new WaitForSeconds(StartWait);
        while (true)
        {
            yield return new WaitUntil(() => Ships < 3);

            CheckScreenDimensions();

            // Choose where to spawn
            int choose = Random.Range(0, 4);
            if (choose == 0)
            {
                RndmPstn.x = Random.Range(-width, width);
                RndmPstn.z = height;
            }
            if (choose == 1)
            {
                RndmPstn.x = Random.Range(-width, width);
                RndmPstn.z = -height;
            }
            if (choose == 2)
            {
                RndmPstn.x = width;
                RndmPstn.z = Random.Range(-height, height);
            }
            if (choose == 3)
            {
                RndmPstn.x = -width;
                RndmPstn.z = Random.Range(-height, height);
            }
            RndmPstn.y = 0;

            // Spawn
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(Spaceship2, RndmPstn, transform.rotation);
            }
            else
            {
                Instantiate(SpaceshipFighter, RndmPstn, transform.rotation);
            }
            Ships++;
            Mover.speed += 1;
        }
    }

    IEnumerator RestartM()
    {
        noShip = false;
        Ships = 3;
        restart = false;
        PauseButton.SetActive(false);
        Destroy(GameObject.Find("Player(Clone)"));
        yield return new WaitForSeconds(1);
        Cursor.visible = true;
        SettingsButton.SetActive(true);
        Restart.SetActive(true);
    }

    void CheckScreenDimensions()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        width = p.x;
        height = p.z;
    }
}