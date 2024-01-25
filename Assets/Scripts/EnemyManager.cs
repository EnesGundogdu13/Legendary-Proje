using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    GameManager gameManager;

    BattleManager battleManager;

    #region d��man prefabs ve Text

    [SerializeField] TextMeshPro CounterTxt;
    [SerializeField] GameObject enemyPrefabs;
    #endregion

    #region d��manlar�n hizas� i�in gereken de�i�kenler
    [Range(0f, 2f)][SerializeField] float distanceFactor, radius;
    #endregion

    [SerializeField] Transform player;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

        battleManager = GameObject.FindGameObjectWithTag("battleManager").GetComponent<BattleManager>();

        MakeEnemyStickman(Random.Range(20,35));

        FormatEnemyStickMan();
    }

    private void Update()
    {
        battleManager.EnemyOffence(player, transform);

    }

    #region kopyalanan d��man hizas�
    private void FormatEnemyStickMan()            //g�ze daha ho� g�r�ld��� i�in IEnumerator yap�ld�. E�ER AKS�L�K �IKARTIRSA GER� VO�D HAL�NE D�ND�R�LECEK
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //UFUFU WEWEWE ONYETEN WEWEWE UG�M�M� OSAS
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            //UFUFU WEWEWE ONYETEN WEWEWE UG�M�M� OSAS
            var z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var _NewPos = new Vector3(x, -0.4445743f, z);

            transform.GetChild(i).localPosition = _NewPos;
        }

    }
    #endregion

    #region stickman kopyalama i�lemleri
    //kopyalanacak d��man
    void MakeEnemyStickman(int randomEnemy)
    {

        for (int i = 0; i < randomEnemy; i++)
        {
            Instantiate(enemyPrefabs, transform.position, new Quaternion(0, 180, 0, 1), transform);
        }

        CounterTxt.text = (transform.childCount).ToString();            //adam dengesiz transform.childcount - 1 yap�yor ama countertxt yi bu scriptin alt objesi yapm�yor, benden daha k�t� matemati�i var ama cos sin biliyor. rez al�n tekrar alt objesi yapacak, ba�ka �ans� yok, ��nk� text adamlar� takip etmiyor.
    }
    #endregion


    #region d��manlar i�in animasyon
    private IEnumerator EnemyAnimation(Transform enemy)
    {
        if (gameManager.gameState)
        {
            //d��man ile karakterimiz anl�k bak���yor(e�er be�enilmezse ��kar�lacak).
            yield return new WaitForSeconds(0.35f);


            for (int i = 1; i < enemy.childCount; i++)
                enemy.GetChild(i).GetComponent<Animator>().SetBool("runner", true);                 //getcomponentden daha iyi ��z�m?

        }
        //oyun durursa
        else if (!gameManager.gameState)
            for (int i = 1; i < enemy.childCount; i++)
                enemy.GetChild(i).GetComponent<Animator>().SetBool("runner", false);
    }
    #endregion



    private void OnTriggerEnter(Collider other)
    {
        //telefonu yormamas� i�in updateye atmad�m ta��d�m
        if (other.CompareTag("blue"))
        {
            StartCoroutine(EnemyAnimation(transform));
          
        }

    }

}
