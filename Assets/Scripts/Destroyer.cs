using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject BigExplosion;

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Mothership":
                {
                    GameController.Lives -= 1;
                    if (GameController.Lives <= 0)
                    {
                        GameController.restart = true;
                        Destroy(other.gameObject);
                        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
                            Destroy(ship);
                        Instantiate(BigExplosion, transform.position, transform.rotation);
                        if (GameController.settings.SFX)
                            GameObject.Find("BigExplosionSFX").GetComponent<AudioSource>().Play();
                    }
                    else if (GameController.settings.ThreeLives == true)
                    {
                        GameController.noShip = true;
                        GameController.Ships -= 1;
                        Destroy(gameObject);
                        Instantiate(Explosion, transform.position, transform.rotation);
                        if (GameController.settings.SFX)
                            GameObject.Find("SmallExplosionSFX").GetComponent<AudioSource>().Play();
                    }

                    break;
                }

            case "Player":
                GameController.score++;
                GameController.noShip = true;
                GameController.Ships -= 1;
                Destroy(gameObject);
                Instantiate(Explosion, transform.position, transform.rotation);
                if (GameController.settings.SFX)
                {
                    switch (transform.name)
                    {
                        case "SpaceshipFighter(Clone)":
                            GameObject.Find("VerySmallExplosionSFX").GetComponent<AudioSource>().Play();
                            break;
                        default:
                            GameObject.Find("SmallExplosionSFX").GetComponent<AudioSource>().Play();
                            break;
                    }
                }
                break;
        }
    }
}