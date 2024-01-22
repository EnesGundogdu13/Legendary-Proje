using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RayTest : MonoBehaviour
{
    Camera cam;


    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        //t�klama varsa
        if (Input.touchCount > 0)
        {
            //t�klama inputunu tan�m�
            Touch _touch = Input.GetTouch(0);

            Vector3 a = _touch.position;

            //kameran�n a��s�ndaki _touch pozisyonunu al�r ve onu d�nya kordinat�na �evirir
            Ray _ray = cam.ScreenPointToRay(a);

            print(_ray + " ray");
            print(a);
        }
    }
}
