
using UnityEngine;

public class MovetoCursor : MonoBehaviour
{
    Vector3 Pos;

    void Update()
    {
        Cursor.visible = false;

        // make the Player Sphere move to the Cursor
        try
        {
            Pos = Input.GetTouch(0).position;
        }
        catch
        {
            Pos = Input.mousePosition;
        }
        Pos.z = 5;
        transform.position = Camera.main.ScreenToWorldPoint(Pos);
    }
}