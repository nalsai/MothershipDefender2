using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject BigExplosion;
    public GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mothership")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            Instantiate(BigExplosion, transform.position, transform.rotation);
            GameController.restart=true;
            if (GameController.SFX == true)
            {
                AudioSource audio = GameObject.Find("BigExplosionSFX").GetComponent<AudioSource>();
                audio.Play();
            }
        }
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            GameController.score++;
            GameController.noShip = true;
            Instantiate(Explosion, transform.position, transform.rotation);

            if(GameController.SFX == true && transform.name == "SpaceshipFighter(Clone)")
            {
                AudioSource audio = GameObject.Find("SmallExplosionSFX").GetComponent<AudioSource>();
                audio.Play();
            }
            else if(GameController.SFX == true)
            {
                AudioSource audio = GameObject.Find("VerySmallExplosionSFX").GetComponent<AudioSource>();
                audio.Play();
            }
        }
    }
}
