using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MotionController : MonoBehaviour
{
    private GameManager gameManager;

    private BattleManager battleManager;

    #region ray sistemi i�in gereken de�i�kenler
    private Camera cam;

    private float _touchPos;

    //t�klama kontrol�
    private bool TouchControl;
    #endregion

    #region hareket h�z de�i�kenleri
    [SerializeField] float xSpeed, roadSpeed;

    //yolun transformu
    [SerializeField] Transform roads;
    #endregion

    #region sa�a sola kayd�rma s�n�r� de�i�keni
    private float xBorder = 3;
    #endregion


    private void Start()
    {
        //ya b�yle tan�mlama �ekli mi olur #@=!%
        cam = Camera.main;

        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

        battleManager = GameObject.FindGameObjectWithTag("battleManager").GetComponent<BattleManager>();

    }

    void Update()
    {
        Swipe();
    }

    private void FixedUpdate()
    {
        //e�er oyun aktif de�ilse di�er komutlara girmez.
        if (!gameManager.gameState)
        {
            //olas� hatay� �nlemek ba�b�nda
            battleManager.attackState = false;

            return;
        }
        StartCoroutine(Movement());
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
    //hareket komutu        EK B�LG�: KARAKTER �LER� G�TMEYECEK YOL GER� GELECEK
    IEnumerator Movement()
    {
        //sava� ba�larsa
        if (battleManager.attackState)
        {
            roads.position = new(roads.position.x, roads.position.y, roads.position.z + 0 * Time.fixedDeltaTime);
            yield return new WaitForSeconds(0.25f);

            //******************** DE���MES� GEREKEB�L�R D��MANLAR HAREKET ETT���NDE TEST ED�LMES� GEREK�YOR!!!!!!!!!!!!!!!
            roads.position = new(roads.position.x, roads.position.y, roads.position.z - 1.5f * Time.fixedDeltaTime);


        }
        //yol ve alt objeleri geriye do�ru olmas� gereken h�zda hareket edecektir
        else
        {
            roads.position = new(roads.position.x, roads.position.y, roads.position.z + -roadSpeed * Time.fixedDeltaTime);

            #region sa�a ve sola ge�i� s�n�r�
            //�NEML�: e�er bu scriptin objesini de�i�tirmek gerekiyorsa transform.childcount de�i�tirilmesi gerekebilir.
            if (transform.childCount < 50)
                _touchPos = Mathf.Clamp(_touchPos, -xBorder, xBorder);
            else
                _touchPos = Mathf.Clamp(_touchPos, -2, 2);
            #endregion

            //player sa�a sola kayd�rma
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, _touchPos, Time.fixedDeltaTime * xSpeed), 0.4445743f, transform.position.z);
        }
    }
    #endregion
}

//bu script genel olarak: t�klad���m�z ekran pozisyonuna g�re hareket eder. e�er en sa� tarafa t�klarsak �ok h�zl�, ekran�n ortas�n�n birazc�k sa��na t�klarsak �ok yava� bir �ekilde sa�a gider.
//bu t�klamalar ray ile g�r�nmez olan bir plane objesinin ne taraf�na t�klad���m�za ba�l� olarak i�liyor.

//bu a. kodu�umun planesi o kadar g�r�nmezki hiyera�i b�l�m�nde bile yok, nas�l oluyor anlamad�m...