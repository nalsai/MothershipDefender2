using UnityEngine;

public class Mover : MonoBehaviour
{
    public static float speed = 10;

    private void Start()
    {
        // Rotate towards the Mother Ship
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position, 100f, 0.0F));
    }
    void Update()
    {
        // Move towards the Mother Ship
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(0, 0), speed / 10 * Time.deltaTime);
    }
}
