using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting.Dependencies.Sqlite;
using Unity.Mathematics;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] BattleManager battleManager;

    [SerializeField] PlayerAnimator playerAnimator;

    #region ge�it i�in gereken de�i�kenler

    //stickman say�s�
    private int numberOfStickmans;

    //karakter say�s�n� tutar. ek bilgi: player objesinin alt objesindeki textte tutar.
    [SerializeField] private TextMeshPro CounterTxt;

    //kopyalanacak obje
    [SerializeField] private GameObject stickMan;

    #endregion

    #region karakterlerin pozisyonlar� i�in gereken de�i�kenler
    [Range(0f, 2f)][SerializeField] float distanceFactor, radius;
    #endregion

    #region sava�aca�� d��man� takip etmesi i�in de�i�ken
    private Transform enemy;
    #endregion

    private void Start()
    {

        #region ge�itle alakal�
        numberOfStickmans = transform.childCount - 1;       // 1 say� eksiltmenin sebebi 'Count_Label' objesini g�rmezden gelmesi i�in(san�r�m)
        CounterTxt.text = numberOfStickmans.ToString();
        #endregion

    }

    private void Update()
    {
        //bu kod sat�r� burada ge�icidir. OnTriggerEnter e ta��nabilir(yada ucuza ka��p burada b�rak�r�m :P)
        playerAnimator.PlayerAnimation(transform);

        #region player sava� ba�lad�ysa: d��man� takip eder, d��man bittiyse atak durumu false d�ner. d��mana do�ru rotasyonunu �evirir.
        battleManager.PlayerOffence(enemy, transform);
        LookEnemy();
        #endregion
    }

    #region d��mana do�ru pozisyon alma i�lemi
    private Vector3 enemyDirection;
    //e�er sava� ba�lad�ysa karakterlerimiz d��mana do�ru bakar
    void LookEnemy()
    {
        //sava� ba�lamad�ysa di�er komutlara girmez
        if (!battleManager.attackState)
            return;

        //D�ZELT�LD� AMA B DA �IKAB�L�R
        //  Transform _Enemy = GameObject.FindGameObjectWithTag("enemyManager").transform.GetChild(0);
        //her saniye bu i�lemi yapmas�n� istemedi�im i�in b�yle bir �nlem ald�m(performans artt�rmak ba�b�nda)
        if (enemyDirection == Vector3.zero)
            enemyDirection = enemy.position - transform.position;

        //karakter say�s�n� g�steren ve kameran�n d�nmemesi i�in bu scripte ba�l� objenin alt objelerine h�kmediyorum
        for (int i = 1; i < transform.childCount; i++)
        {
            //transform rotation yap�s�n� ��renmem gerekiyor..
            transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 5);
        }

    }
    #endregion

    #region stickman hizaya getirme i�lemi
    //kopyalanan stickmanlar�n pozisyonu
    public void FormatStickMan()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            //UFUFU WEWEWE ONYETEN WEWEWE UG�M�M� OSAS
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            //UFUFU WEWEWE ONYETEN WEWEWE UG�M�M� OSAS
            var z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var _NewPos = new Vector3(x, -0.4445743f, z);

            
            transform.GetChild(i).DOLocalMove(_NewPos, 1.5f).SetEase(Ease.OutBack);
        }
    }
    #endregion

    #region sava� sonras� karakterlerin rotasyon s�f�rlanmas�
    public void Alignment()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, quaternion.identity, Time.deltaTime);
        }
    }

    #endregion


    #region stickman kopyalama i�lemleri
    //stickman kopyalama
    private void MakeStickMan(int number)
    {
        //int 1 den ba�lamay�nca yanl�� sonu� ��k�yor aw
        for (int i = 1; i < number; i++)
        {
            Instantiate(stickMan, transform.position, Quaternion.identity, transform);
        }
        numberOfStickmans = transform.childCount - 1;

        CounterTxt.text = numberOfStickmans.ToString();

        //stickmanlar�n pozisyonu
        FormatStickMan();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gate"))
        {
            //gate_l nin boxcolliderini kapat�r(bunu unutmazsam destroy olarak de�i�tirece�im)
            other.transform.parent.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            //gate_r nin boxcolliderini kapat�r
            other.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;

            //�arpt��� objenin scriptini "_gateManager" olarak miras almam�za olanak sa�lar
            GateManager _gateManager = other.GetComponent<GateManager>();

            if (_gateManager.multiply)
            {
                MakeStickMan(numberOfStickmans * _gateManager.randomNumber);
            }
            else
            {
                MakeStickMan(numberOfStickmans + _gateManager.randomNumber);
            }
        }
        #endregion

        #region d��man g�rd�yse sald�r� moduna ge�er
        if (other.CompareTag("playerDetected"))
        {
            battleManager.attackState = true;

            //d��mana bakmak i�in transformunu buradan al�yorum(muhtemelen daha yere yerle�tirilebilirdi bu kod).
            enemy = other.transform;
        }
    }
    #endregion

}


//Transform = player kodunu ��kartt�m hi� bir i�e yaram�yordu. gerekirse tekrar eklerim.