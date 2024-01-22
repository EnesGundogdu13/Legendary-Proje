using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting.Dependencies.Sqlite;

public class PlayerManager : MonoBehaviour
{
    //player transformu
    public Transform player;

    //stickman say�s�
    private int numberOfStickmans;

    //karakter say�s�n� tutar. ek bilgi: player objesinin alt objesindeki textte tutar.
    [SerializeField] private TextMeshPro CounterTxt;

    //kopyalanacak obje
    [SerializeField] private GameObject stickMan;

    //*************************
    [Range(0f, 2f)]
    [SerializeField] float distanceFactor, radius;


    private void Start()
    {
        ///�uan bir i� yapm�yor.
        player = transform;

        // 1 say� eksiltmenin sebebi 'Count_Label' objesini g�rmezden gelmesi i�in(san�r�m)
        numberOfStickmans = transform.childCount - 1;

        CounterTxt.text = numberOfStickmans.ToString();
    }

    private void Update()
    {
        FormatStickMan();
    }

    //kopyalanan stickmanlar�n pozisyonu
    private void FormatStickMan()
    {
        for (int i = 1; i < player.childCount; i++)
        {
            //UFUFU WEWEWE ONYETEN WEWEWE UG�M�M� OSAS
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            //UFUFU WEWEWE ONYETEN WEWEWE UG�M�M� OSAS
            var z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var _NewPos = new Vector3(x, -0.4445743f, z);

            player.transform.GetChild(i).DOLocalMove(_NewPos, 1).SetEase(Ease.OutBack);
        }

    }


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
    }


}
