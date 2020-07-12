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
    public static Settings settings = new Settings();

    const int StartWait = 2;
    Vector3 RndmPstn;
    float width;
    float height;

    private void Start()
    {
        Application.targetFrameRate = 240;

        if (Application.platform == RuntimePlatform.Android)
        {
            Application.targetFrameRate = 120;
        }

        if (File.Exists(Application.persistentDataPath + "/settings.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/settings.sav", FileMode.Open);
            settings = (Settings)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            settings.Music = true;          // Music 
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                settings.Music = false;
            settings.SFX = true;            // SFX
            settings.ThreeLives = false;    // 3 Lives
            settings.HardMode = false;      // HardMode
            BinaryFormatter bf = new BinaryFormatter();
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
            StartCoroutine(Spawn2());
            Soundtrack.GetComponent<AudioSource>().Stop();
            Soundtrack.GetComponent<AudioSource>().clip = HardAudioClip;
            if (settings.Music)
                Soundtrack.GetComponent<AudioSource>().Play();
        }
    }

    private void Update()
    {
        if (restart == true)
            StartCoroutine(RestartM());
        scoreText.text = "Score: " + score;
        LivesText.text = "Lives:  " + Lives;
    }

    public void ToggleMusic()
    {
        if (settings.Music == true)
        {
            Soundtrack.SetActive(false);
            settings.Music = false;
        }
        else if (settings.Music == false)
        {
            Soundtrack.SetActive(true);
            settings.Music = true;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.sav");
        bf.Serialize(file, settings);
        file.Close();
    }

    public void ToggleSFX()
    {
        if (settings.SFX)
        {
            settings.SFX = false;
        }
        else
        {
            settings.SFX = true;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.sav");
        bf.Serialize(file, settings);
        file.Close();
    }

    public void ToggleLiv3s()
    {
        if (settings.ThreeLives)
        {
            LivesObject.SetActive(false);
            settings.ThreeLives = false;
        }
        else
        {
            LivesObject.SetActive(true);
            settings.ThreeLives = true;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.sav");
        bf.Serialize(file, settings);
        file.Close();
    }

    public void ToggleHardMode()
    {
        if (settings.HardMode)
        {
            settings.HardMode = false;
            StopAllCoroutines();
            StartCoroutine(Spawn());
            Soundtrack.GetComponent<AudioSource>().Stop();
            Soundtrack.GetComponent<AudioSource>().clip = DefaultAudioClip;
            if (settings.Music)
                Soundtrack.GetComponent<AudioSource>().Play();
        }
        else
        {
            settings.HardMode = true;
            StopAllCoroutines();
            StartCoroutine(Spawn2());
            Soundtrack.GetComponent<AudioSource>().Stop();
            Soundtrack.GetComponent<AudioSource>().clip = HardAudioClip;
            if (settings.Music)
                Soundtrack.GetComponent<AudioSource>().Play();
        }
        BinaryFormatter bf = new BinaryFormatter();
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
        Lives = 3;
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

    IEnumerator Spawn2()
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