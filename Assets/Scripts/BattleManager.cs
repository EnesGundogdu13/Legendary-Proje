using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    //karakteri hizaya sokmak i�in laz�m (gene scriptleri �orba ettim yaw...)
    [SerializeField] Transform player;


    #region atak kontrol
    public bool attackState;
    #endregion


    #region karakterler d��manlar� takip eder 
    public void PlayerOffence(Transform enemy, Transform _Player)
    {
        if (enemy.GetChild(1).childCount > 0)
            for (int i = 0; i < _Player.transform.childCount; i++)
            {                
                Vector3 _Distance = enemy.GetChild(1).GetChild(0).position - _Player.transform.GetChild(i).position;

                if (_Distance.magnitude < 5.5f)                   
                    //destan yaz�lm�� bu da�larda, kanlarla kapl� sava�larla! �i�ek a�m��t� bir yamac�nda, h�z�nl�yd� tek ba��na. bir bayrak kalkt� havaya "ZAFER!" diyen ���l�klarla!
                    //�i�ek solmu�tu gidenlerin kan�yla. son nefesini verirken kelimeler kurmaya �al��t� cabas�yla. a��z�ndan ��kan s�z �ok derindi ama anlayana "yapaca��n�z kodu g-"
                    _Player.transform.GetChild(i).position = Vector3.Lerp(_Player.transform.GetChild(i).position,
                         new Vector3(enemy.GetChild(1).GetChild(0).position.x, _Player.transform.GetChild(i).position.y, enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime);
            }
        else
        {
            attackState = false;
            Alignment(player);
            enemy.gameObject.SetActive(false);
            StartCoroutine(playerManager.FormatStickMan());

        }
    }
    #endregion

    #region d��manlar karakteri takip eder ve pozisyonunu playere �evirir
    public void EnemyOffence(Transform player, Transform enemy)
    {
        //sava� yoksa di�er komutlar� �al��t�rmaz
        if (!attackState)
            return;

        //sava� ba�lad�ysa enemyler playerin i�ine girer
        if (attackState && enemy.transform.childCount > 1)
        {

            Vector3 _PlayerDirection = player.position - enemy.position;

            for (int i = 0; i < enemy.transform.childCount; i++)
            {
                //d��manlar player transformuna rotasyonunu �evirir.
                enemy.transform.GetChild(i).rotation = Quaternion.Slerp(enemy.transform.GetChild(i).rotation, quaternion.LookRotation(_PlayerDirection, Vector3.up), Time.deltaTime);

                Vector3 _Distance = player.GetChild(0).position - enemy.transform.GetChild(i).position;
                
                if (_Distance.magnitude < 10.5f)    //player ile d��manlar�n aras�ndaki mesafe
                {
                    enemy.transform.GetChild(i).position = Vector3.Lerp(enemy.transform.GetChild(i).position, player.GetChild(1).position, Time.deltaTime * 2);
                }
            }
        }
    }

    #endregion

    #region D�E D�E D�E D�E D�E!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public void KillTheALL(GameObject red, GameObject blue)
    {
        Destroy(red.gameObject);
        Destroy(blue.gameObject);
    }
    #endregion



    //*********** BU KODUN YER� DE���EB�L�R MUHTEMELEN BURAYA UYGUN DE��L **********\\

    #region sava� sonras� rotasyon s�f�rlama giri�
    public void Alignment(Transform _Player)
    {
        for (int i = 1; i < _Player.transform.childCount; i++)
        {
            _Player.transform.GetChild(i).rotation = Quaternion.Slerp(_Player.transform.GetChild(i).rotation, quaternion.identity, Time.deltaTime);
        }
    }

    #endregion




}
