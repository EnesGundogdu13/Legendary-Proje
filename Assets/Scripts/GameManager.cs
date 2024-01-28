using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] BattleManager battleManager;


    //true = oyun �al���yor
    public bool gameState;

    //oyun durdurdu�unda attack da pasif oluyor, oyunu tekrar ba�latt���nda e�er sava�ta ise tekrar aktif olmas� gerekiyor.
    public bool attackDebug;


    //A.�
    public Button StartButton, RetryButton, WinButton, StopButton, StopMenuContentsButton;
    public GameObject StartMenu, LoseMenu, WinMenu, StopMenuContents;
    private void Start()
    {
        gameState = false;

        #region A.�
        if (StartMenu != null)
        {
            StartButton.onClick.AddListener(StartButtonClicked);
        }
        if (LoseMenu != null)
        {
            RetryButton.onClick.AddListener(RetryButtonClicked);
        }
        if (WinMenu != null)
        {
            WinButton.onClick.AddListener(WinButtonClicked);
        }
        StopButton.onClick.AddListener(StopButtonClicked);
        if (StopMenuContentsButton != null)
        {
            StopMenuContentsButton.onClick.AddListener(StopMenuButtonClicked);
        }
        #endregion
    }
    #region A.� metot i�levleri i�in yorum sat�r� ekler misin, anlarken zorland�m :D
    void StartButtonClicked()
    {

        StartMenu.SetActive(!StartMenu.activeSelf);
        gameState = true;
    }
    void RetryButtonClicked()
    {
        gameState = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void WinButtonClicked()
    {
        gameState = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void StopButtonClicked()
    {
        attackDebug = battleManager.attackState;
        gameState = false;
        battleManager.attackState = false;
        StopMenuContents.SetActive(true);

    }
    void StopMenuButtonClicked()
    {
        gameState = true;
        battleManager.attackState = attackDebug;
        StopMenuContents.SetActive(false);
    }

    #endregion

    //oyun bitti�inde �al��acak komutlar
    public void GameWin()
    {
        gameState = false;
        print("oyunu KAZANDIN");

    }

}