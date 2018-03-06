 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetoCursor : MonoBehaviour {
    
    void Update ()
    {
        // make the Cursor invisible
        Cursor.visible = false;

        // make the Player Sphere move to the Cursor


        // Wo soll die Kugel sich hin bewwegen?
        // Für Touch: 
        //Touch touch = Input.GetTouch(0);
        //Vector3 Pos = touch.position;
        // Für Maus:
        Vector3 Pos = Input.mousePosition;

        Pos.z = 5; 
        transform.position = Camera.main.ScreenToWorldPoint(Pos);

    }
}
