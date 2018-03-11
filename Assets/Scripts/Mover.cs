using UnityEngine;

public class Mover : MonoBehaviour
{
    public static float speed = 10;

    void Update()
    {
        // Rotates towards the Mother Ship
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position, 100, 0.0F));
        // Moves towards the Mother Ship
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(0, 0), speed / 10 * Time.deltaTime);
    }
}
