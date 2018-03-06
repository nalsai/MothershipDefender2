using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject SpaceshipFighter;
    public GameObject Spaceship2;
    public GameObject MotherShip;
    public GameObject Player;
    public GameObject start;
    public GameObject Restart;
    public GameObject Pause;
    public GameObject PauseButton;
    public GameObject SettingsButton;
    public Text scoreText;
    public int StartWait = 2;
    public static int score;
    public static bool noShip;
    public static bool restart;
    public static bool endpause;
    public static bool SFX = true;
    public static bool Music = true;
    Vector3 RndmPstn;

    public void EnableDisableSFX()
    {
        if (SFX == true)
        {
            SFX = false;
        }
        else
        {
            SFX = true;
        }
    }
    private void Update()
    {
        if (restart == true)
        {
            StartCoroutine(RestartM());
        }
        scoreText.text = "Score: " + score;
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
        Mover.speed = 10;
        StartCoroutine(Spawn());
        noShip = true;
    }
    public void PauseGame()
    {
        PauseButton.SetActive(false);
        noShip = false;
        Pause.SetActive(true);
        Destroy(GameObject.Find("Player(Clone)"));
        Destroy(GameObject.Find("Mother Ship(Clone)"));
        Destroy(GameObject.Find("SpaceshipFighter(Clone)"));
        Destroy(GameObject.Find("Spaceship2(Clone)"));
        Cursor.visible = true;
    }
    public void EndPause()
    {
        PauseButton.SetActive(true);
        Pause.SetActive(false);
        Instantiate(MotherShip);
        Instantiate(Player);
        noShip = true;
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(StartWait);
        while (true)
        {
            yield return new WaitUntil(() => noShip == true);
            // Choose where to spawn
            
            Vector3 p = new Vector3();
            Camera c = Camera.main;

            p = c.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            float width = p.x;
            float height = p.z;
            
            Debug.Log(height);
            Debug.Log(width);
            Debug.Log(Random.Range(-height, height));

            int choose;
            choose = Random.Range(0, 4);
            if (choose == 0)
            {
                Debug.Log("0");
                RndmPstn.x = Random.Range(-width, width);
                RndmPstn.z = height;
            }
            if (choose == 1)
            {
                Debug.Log("1");
                RndmPstn.x = Random.Range(-width, width);
                RndmPstn.z = -height;
            }
            if (choose == 2)
            {
                Debug.Log("2");
                RndmPstn.x = width;
                RndmPstn.z = Random.Range(-height, height);
            }
            if (choose == 3)
            {
                Debug.Log("3");
                RndmPstn.x = -width;
                RndmPstn.z = Random.Range(-height, height);
            }
            RndmPstn.y = 0;

            noShip = false;

            // Spawn
            int hallo = Random.Range(0, 4);
            if (hallo == 0)
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
        restart = false;
        PauseButton.SetActive(false);
        Destroy(GameObject.Find("Player(Clone)"));
        yield return new WaitForSeconds(1);
        Cursor.visible = true;
        SettingsButton.SetActive(true);
        Restart.SetActive(true);
    }
}