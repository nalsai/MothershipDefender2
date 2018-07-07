
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject MusicToogle;
    public GameObject Soundtrack;
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
    public static int Ships;
    public static bool LivesA;
    public static bool noShip;
    public static bool restart;
    public static bool endpause;
    public static bool HardMode = false;
    public static bool SFX = true;
    public static bool Music = true;
    int StartWait = 2;
    Vector3 RndmPstn;
    float width;
    float height;

    private void Start()
    {
        Application.targetFrameRate = 240;

        if (Application.platform == RuntimePlatform.Android)
        {
            Application.targetFrameRate = 60;
        }
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Soundtrack.SetActive(false);
            MusicToogle.GetComponent<Toggle>().isOn = false;
            Application.targetFrameRate = 60;
        }
        else
            Soundtrack.SetActive(true);
        Ships = 3;

        if (File.Exists(Application.persistentDataPath + "/setttings.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/settings.sav", FileMode.Open);
            //SaveFile data = (SaveFile)bf.Deserialize(file);
            file.Close();
            //bool SetMusic = data.Music;
            //bool SetSFX = data.SFX;
            //bool SetLives = data.Lives;
            //bool SetHard = data.Hard;
            //bool SetFPS = data.FPS;
            print("File loaded from: " + Application.persistentDataPath);
        }
        else
        {
            print("File doesn't exist in: " + Application.persistentDataPath);
        }

        StartCoroutine(Spawn());
    }

    private void Update()
    {
        if (restart == true)
            StartCoroutine(RestartM());
        scoreText.text = "Score: " + score;
        LivesText.text = "Lives:  " + Lives;
    }

    public void EnableDisableSFX()
    {
        if (SFX == true)
        {
            SFX = false;
        }
        else if (SFX == false)
        {
            SFX = true;
        }
    }

    public void EnableDisableLives()
    {
        if (LivesA == true)
        {
            LivesA = false;
        }
        else if (LivesA == false)
        {
            LivesA = true;
        }
    }

    public void EnableDisableHardMode()
    {
        if (HardMode == true)
        {
            HardMode = false;
            StopAllCoroutines();
            StartCoroutine(Spawn());
        }
        else
        {
            HardMode = true;
            StopAllCoroutines();
            StartCoroutine(Spawn2());
        }
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
        Vector3 p = new Vector3();
        p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        width = p.x;
        height = p.z;
    }
}