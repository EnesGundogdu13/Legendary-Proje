using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    #region atak kontrol
    public bool attackState;
    #endregion

    private Vector3 playerDistance;
    private Vector3 enemyDistance;

    #region karakterler d��manlar� takip eder ve e�er d��man kalmad�ysa yap�lacak i�lemler
    public void PlayerOffence(Transform enemyZone, Transform _Player)
    {
        //sava� yoksa di�er komutlar� �al��t�rmaz
        if (!attackState)
            return;

        //sava� ba�lad�ysa playerler enemy i�ine girer
        if (enemyZone.GetChild(0).childCount > 1)
            for (int i = 1; i < _Player.childCount; i++)
            {
                #region karakterin rotasyonunu d��manlara �evirir
                for (int j = 1; j < enemyZone.GetChild(0).childCount; j++)
                {
                    enemyDistance = enemyZone.GetChild(0).GetChild(j).position - _Player.GetChild(i).position;
                }

                _Player.GetChild(i).rotation = Quaternion.Slerp(_Player.GetChild(i).rotation, Quaternion.LookRotation(enemyDistance, Vector3.up), Time.fixedDeltaTime * 10);

                #endregion

                #region karakterlerimiz d��manlar� takip eder
                enemyZone.GetChild(0).GetChild(0).gameObject.SetActive(true);


                if (enemyDistance.magnitude < 10f)      //d��man ile aras�ndaki mesafe
                    _Player.GetChild(i).position = Vector3.Lerp(_Player.GetChild(i).position, enemyZone.GetChild(0).GetChild(1).position, Time.fixedDeltaTime / 5);     //d��mana do�ru ilerler


                //text 1. karakteri takip eder(bug fix)
                _Player.GetChild(0).position = new Vector3(_Player.GetChild(0).position.x, _Player.GetChild(0).position.y, Mathf.Lerp(_Player.GetChild(0).position.z, _Player.GetChild(1).position.z, Time.fixedDeltaTime));
                #endregion
            }
        //d��man bittiyse
        else
        {
            attackState = false;
            enemyZone.gameObject.SetActive(false);
            playerManager.FormatStickMan();

            print("d��man bitti");
        }
    }
    #endregion

    #region d��manlar karakteri takip eder, rotasyonunu karaktere �evirir ve kaybedildiyse yap�lacak i�lemler
    public void EnemyOffence(Transform player, Transform enemy)          //player = playerManager
    {
        //sava� yoksa di�er komutlar� �al��t�rmaz
        if (!attackState)
            return;

        //2 taraftan birisi nefes al�yorsa:
        if (enemy.childCount > 1 && player.childCount > 1)
            for (int i = 1; i < enemy.childCount; i++)
            {
                #region d��manlar�n rotasyonunu karaktere �evirir                
                for (int j = 1; j < player.childCount; j++)
                {
                    //d��manlar player transformuna rotasyonunu �evirmek i�in gereken kod(hi� bir �ey anlamad�m pozisyon ile rotasyonu ayarlamak nas�l yaw)
                    playerDistance = player.GetChild(j).position - enemy.GetChild(i).position;
                }
                enemy.GetChild(i).rotation = Quaternion.Slerp(enemy.GetChild(i).rotation, quaternion.LookRotation(playerDistance, Vector3.up), Time.fixedDeltaTime * 10);
                #endregion


                #region karakteri takip eder
                if (playerDistance.magnitude < 10f)     //player ile d��manlar�n aras�ndaki mesafe
                {
                    enemy.GetChild(i).position = Vector3.Lerp(enemy.GetChild(i).position, player.GetChild(1).position, Time.fixedDeltaTime * 2);

                    //text 1. enemyi takip eder(bug fix)
                    enemy.GetChild(0).position = new Vector3(Mathf.Lerp(enemy.GetChild(0).position.x, enemy.GetChild(1).position.x, Time.fixedDeltaTime),
                        enemy.GetChild(0).position.y, Mathf.Lerp(enemy.GetChild(0).position.z, enemy.GetChild(1).position.z, Time.fixedDeltaTime));
                }
                #endregion
            }
        #region oyun kaybedildi�inde �al��acak kodlar
        else if (enemy.childCount > 1)
        {
            #region oyunu bitirir
            GameManager gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
            gameManager.gameState = false;
            attackState = false;
            player.GetChild(0).gameObject.SetActive(false);
            gameManager.LoseMenu.SetActive(true);
            #endregion

            print("d��man kazand�");
        }
        #endregion
    }

    #endregion

    #region D�E D�E D�E D�E D�E!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public IEnumerator KillTheALL(GameObject target)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(target.gameObject);
        playerManager.stickmanList.Remove(target);

    }
    #endregion
}
