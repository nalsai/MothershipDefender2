using UnityEngine;

public class MovetoCursor : MonoBehaviour
{
    Vector3 Pos;

    void Update()
    {
        // make the Cursor invisible
        Cursor.visible = false;

        // make the Player Sphere move to the Cursor
        if (Application.platform == RuntimePlatform.Android)
        {
            Touch touch = Input.GetTouch(0);
            Pos = touch.position;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Touch touch = Input.GetTouch(0);
            Pos = touch.position;
        }
        else
        {
            Pos = Input.mousePosition;
        }
        Pos.z = 5;
        transform.position = Camera.main.ScreenToWorldPoint(Pos);
    }
}