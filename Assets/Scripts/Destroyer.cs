using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject BigExplosion;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mothership")
        {
            GameController.Lives = GameController.Lives - 1;
            if (GameController.Lives == 0)
            {
                Instantiate(BigExplosion, transform.position, transform.rotation);
                Destroy(other.gameObject);
                GameObject[] Ships = GameObject.FindGameObjectsWithTag("Ship");
                foreach (GameObject Ship in Ships)
                    Destroy(Ship);
                GameController.restart = true;
                if (GameController.SFX == true)
                {
                    AudioSource audio = GameObject.Find("BigExplosionSFX").GetComponent<AudioSource>();
                    audio.Play();
                }
            }
            else if (GameController.LivesA == true)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                if (GameController.SFX == true)
                {
                    AudioSource audio = GameObject.Find("SmallExplosionSFX").GetComponent<AudioSource>();
                    audio.Play();
                }
                Destroy(gameObject);
                GameController.noShip = true;
                GameController.Ships -= 1;
            }
            else
            {
                Instantiate(BigExplosion, transform.position, transform.rotation);
                Destroy(other.gameObject);
                GameObject[] Ships = GameObject.FindGameObjectsWithTag("Ship");
                foreach (GameObject Ship in Ships)
                    Destroy(Ship);
                GameController.restart = true;
                if (GameController.SFX == true)
                {
                    AudioSource audio = GameObject.Find("BigExplosionSFX").GetComponent<AudioSource>();
                    audio.Play();
                }
            }

        }
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            GameController.score++;
            GameController.noShip = true;
            GameController.Ships -= 1;
            Instantiate(Explosion, transform.position, transform.rotation);

            if (GameController.SFX == true && transform.name == "SpaceshipFighter(Clone)")
            {
                AudioSource audio = GameObject.Find("SmallExplosionSFX").GetComponent<AudioSource>();
                audio.Play();
            }
            else if (GameController.SFX == true)
            {
                AudioSource audio = GameObject.Find("VerySmallExplosionSFX").GetComponent<AudioSource>();
                audio.Play();
            }
        }
    }
}