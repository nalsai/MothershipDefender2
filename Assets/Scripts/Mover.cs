using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public static float speed = 10;

    void Update()
    {
        // Moves towards the Mother Ship
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(0, 0), speed / 10  * Time.deltaTime);
    }
}
