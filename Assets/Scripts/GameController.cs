
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
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
    public static int Ships = 3;
    public static bool noShip;
    public static bool restart;
    public static bool endpause;

    public GameObject MusicToogle;
    public GameObject SFXToogle;
    public GameObject Liv3sToogle;
    public GameObject HardModeToogle;
    public static bool Music = true;
    public static bool SFX = true;
    public static bool Liv3s;
    public static bool HardMode;

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

        // Load Settings
        //PlayerPrefs.SetString("FirstRun", "true");
        if (PlayerPrefs.GetString("FirstRun") == "false" == false)
        {
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.SetInt("SFX", 1);
            PlayerPrefs.SetInt("Liv3s", 0);
            PlayerPrefs.SetInt("HardMode", 0);
            PlayerPrefs.SetString("FirstRun", "false");
        }

        SFX = PlayerPrefs.GetInt("SFX") == 1 ? true : false;
        Music = PlayerPrefs.GetInt("Music") == 1 ? true : false;
        Liv3s = PlayerPrefs.GetInt("Liv3s") == 1 ? true : false;
        HardMode = PlayerPrefs.GetInt("HardMode") == 1 ? true : false;

        Liv3sToogle.GetComponent<Toggle>().isOn = Liv3s;
        HardModeToogle.GetComponent<Toggle>().isOn = HardMode;
        MusicToogle.GetComponent<Toggle>().isOn = Music;

        Debug.Log(SFX);
        if(SFX == true)
        {
            SFXToogle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            SFXToogle.GetComponent<Toggle>().isOn = false;
        }
        Debug.Log(SFX);

        StartCoroutine(Spawn());
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
        if (Music == true)
        {
            Music = false;
        }
        else if (Music == false)
        {
            Music = true;
        }
        PlayerPrefs.SetInt("Music", Music ? 1 : 0);
    }

    public void ToggleSFX()
    {
        if (SFX == true)
        {
            SFX = false;
            PlayerPrefs.SetInt("SFX", 0);
        }
        else if (SFX == false)
        {
            SFX = true;
            PlayerPrefs.SetInt("SFX", 1);
        }
        //PlayerPrefs.SetInt("SFX", SFX ? 1 : 0);
    }

    public void ToggleLiv3s()
    {
        if (Liv3s == true)
        {
            Liv3s = false;
        }
        else if (Liv3s == false)
        {
            Liv3s = true;
        }
        PlayerPrefs.SetInt("Liv3s", Liv3s ? 1 : 0);
    }

    public void ToggleHardMode()
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
        PlayerPrefs.SetInt("HardMode", HardMode ? 1 : 0);
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