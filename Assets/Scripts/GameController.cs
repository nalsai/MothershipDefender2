using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject MusicToogle;
    public GameObject Soundtrack;
    public GameObject SpaceshipFighter;
    public GameObject Spaceship2;
    public GameObject MotherShip;
    public GameObject Player;
    public GameObject Pause;
    public GameObject start;
    public GameObject Restart;
    public GameObject PauseButton;
    public GameObject SettingsButton;
    public Text scoreText;
    public Text LivesText;
    public int StartWait = 2;
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
    Vector3 RndmPstn;
    float width;
    float height;

    private void Start()
    {
        Vector3 p = new Vector3();
        p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        width = p.x;
        height = p.z;

        // deactivate Music on WebGL
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Soundtrack.SetActive(false);
            MusicToogle.GetComponent<Toggle>().isOn = false;
        }
        Ships = 3;
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        if (restart == true)
            StartCoroutine(RestartM());
        scoreText.text = "Score: " + score;
        LivesText.text = "Lives:  " + Lives;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Screen.orientation == ScreenOrientation.Portrait)
            {
                Camera.main.orthographicSize = (float)3.5;
                Vector3 p = new Vector3();
                p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                width = p.x;
                height = p.z;
            }
            else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                Camera.main.orthographicSize = (float)3.5;
                Vector3 p = new Vector3();
                p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                width = p.x;
                height = p.z;
            }
            else
            {
                Camera.main.orthographicSize = (float)2.81;
                Vector3 p = new Vector3();
                p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                width = p.x;
                height = p.z;
            }
        }
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
        Instantiate(MotherShip);
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
        Destroy(GameObject.Find("Mother Ship(Clone)"));
        GameObject[] AShips = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject Ship in AShips)
            Destroy(Ship);
        Cursor.visible = true;
    }
    public void EndPause()
    {
        PauseButton.SetActive(true);
        Pause.SetActive(false);
        Instantiate(MotherShip);
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

            // Choose where to spawn
            int choose;
            choose = Random.Range(0, 4);
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

            noShip = false;

            // Spawn
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(Spaceship2, RndmPstn, transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position, 100, 0.0F)));
            }
            else
            {
                Instantiate(SpaceshipFighter, RndmPstn, transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position, 100, 0.0F)));
            }
            Mover.speed += 1;
        }
    }
    
    IEnumerator Spawn2()
    {
        yield return new WaitForSeconds(StartWait);
        while (true)
        {
            yield return new WaitUntil(() => Ships <= 2);

            // Choose where to spawn
            int choose;
            choose = Random.Range(0, 4);
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

            Ships++;

            // Spawn
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(Spaceship2, RndmPstn, transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position, 100, 0.0F)));
            }
            else
            {
                Instantiate(SpaceshipFighter, RndmPstn, transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position, 100, 0.0F)));
            }
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
}