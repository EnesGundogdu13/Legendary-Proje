using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MotionController : MonoBehaviour
{
    #region ray sistemi i�in gereken de�i�kenler
    //private yap�lacak
    [SerializeField] Camera cam;

    float _touchPos;

    //t�klama kontrol�
    bool TouchControl;
    #endregion

    #region hareket h�z de�i�kenleri
    [SerializeField] float xSpeed, zSpeed;
    #endregion

    #region sa�a sola kayd�rma s�n�r� de�i�keni
    [SerializeField] float xBorder = 3;
    #endregion
    private void Start()
    {
        //ya b�yle tan�mlama �ekli mi olur #@=!%
        cam = Camera.main;
    }

    void Update()
    {
        Swipe();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    #region Motion(hareket) i�lemlemleri
    //kayd�rma
    void Swipe()
    {
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);

            //t�klad���nda
            if (_touch.phase == TouchPhase.Began)
            {
                TouchControl = true;
            }
            //elini kald�rd���nda
            if (_touch.phase == TouchPhase.Ended || _touch.phase == TouchPhase.Canceled)
            {
                TouchControl = false;

            }
        }


        if (TouchControl)
        {
            //AMACIN NE SEN�N YA
            Plane _plane = new(Vector3.up, 0);          //buradaki 0 ne i�e yar�yor bilmiyorum ama 9999 yap�nca karakter �ok fazla h�zla sa�a veya sola gidiyor.

            //ekrandaki t�klama pozisyonunu _ray a ekler(san�r�m)
            Ray _ray = cam.ScreenPointToRay(Input.GetTouch(0).position);        //ray bu t�klama pozisyonunu d�nya kordinat�na �eviriyor, origin ve direction olarak 2 farkl� kordinat veriyor(san�r�m)


            //e�er _ray plane'ye �arparsa(san�r�m)
            if (_plane.Raycast(_ray, out float distance))   //out float distance k�sm� ise t�klad���m�z pozisyona ba�l� olarak de�er d�nd�r�yor.
            {
                _touchPos = _ray.GetPoint(distance).x;      // "�arpma noktas�n� al ve x koordinat�n� al"  chat gpt a��klamas�, hi� bir �ey anlamad�m.
            }
        }
    }
    //hareket komutu
    void Movement()
    {
        //sa�a ve sola ge�i� s�n�r�
        _touchPos = Mathf.Clamp(_touchPos, -xBorder, xBorder);

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, _touchPos, Time.fixedDeltaTime * xSpeed), 0.5f, transform.position.z + zSpeed * Time.fixedDeltaTime);
    }
    #endregion
}

//bu script genel olarak: t�klad���m�z ekran pozisyonuna g�re hareket eder. e�er en sa� tarafa t�klarsak �ok h�zl�, ekran�n ortas�n�n birazc�k sa��na t�klarsak �ok yava� bir �ekilde sa�a gider.
//bu t�klamalar ray ile g�r�nmez olan bir plane objesinin ne taraf�na t�klad���m�za ba�l� olarak i�liyor.

//bu a. kodu�umun planesi o kadar g�r�nmezki hiyera�i b�l�m�nde bile yok, nas�l oluyor anlamad�m...