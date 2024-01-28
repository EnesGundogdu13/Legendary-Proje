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


    #region karakterler d��manlar� takip eder ve e�er d��man kalmad�ysa yap�lacak i�lemler
    public void PlayerOffence(Transform enemy, Transform _Player)
    {
        //sava� yoksa di�er komutlar� �al��t�rmaz
        if (!attackState)
            return;

        //sava� ba�lad�ysa playerler enemy i�ine girer
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
        //d��man bittiyse
        else
        {
            attackState = false;
            enemy.gameObject.SetActive(false);
            playerManager.FormatStickMan();


        }
    }
    #endregion

    #region d��manlar karakteri takip eder
    public void EnemyOffence(Transform player, Transform enemy)
    {
        //sava� yoksa di�er komutlar� �al��t�rmaz
        if (!attackState)
            return;

        //2 taraftan birisi nefes al�yorsa:
        if (enemy.transform.childCount > 1 && player.transform.childCount > 1)
            for (int i = 0; i < enemy.transform.childCount; i++)
            {
                Vector3 _Distance = player.GetChild(1).position - enemy.transform.GetChild(i).position;

                if (_Distance.magnitude < 10.5f)    //player ile d��manlar�n aras�ndaki mesafe
                    enemy.transform.GetChild(i).position = Vector3.Lerp(enemy.transform.GetChild(i).position, player.GetChild(1).position, Time.deltaTime * 2);
            }

        #region oyun kaybedildi�inde �al��acak kodlar
        else if (enemy.transform.childCount > 1)
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
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(target.gameObject);
        playerManager.stickmanList.Remove(target);

    }
    #endregion
}
