using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameState;        //true = oyun �al���yor




    private void Start()
    {
        gameState = true;
    }


    //oyun bitti�inde �al��acak komutlar
    public void GameWin()
    {
        gameState = false;
        print("oyunu KAZANDIN");

    }

}


//d�ne kadar bu script dopdoluydu vay bee