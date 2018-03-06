using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
	void Update () {
        // Rotates towards the Mother Ship
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position, 100, 0.0F));
    }
}
