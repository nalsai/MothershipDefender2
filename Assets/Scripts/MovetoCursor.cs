using UnityEngine;

public class MovetoCursor : MonoBehaviour
{
    Vector3 Pos;

    void Update()
    {
        Cursor.visible = false;

        if (Input.touchCount > 0)
            Pos = Input.GetTouch(0).position;
        else if (Input.mousePresent)
            Pos = Input.mousePosition;

        Pos.z = 5;
        transform.position = Camera.main.ScreenToWorldPoint(Pos);
    }
}