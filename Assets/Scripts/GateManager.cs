using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateManager : MonoBehaviour
{
    //gate yaz�s�
    public TextMeshPro GateNo;

    //gatelerde yazacak say�
    public int randomNumber;

    //�arpma i�lemi i�in bool
    public bool multiply;

    private void Start()
    {
        if(multiply)
        {
            randomNumber = Random.Range(1, 3);
            GateNo.text = "X " + randomNumber.ToString();
        }
        else
        {
            randomNumber = Random.Range(10, 100);
            print(randomNumber);
          /*  if(randomNumber %2  == 0)
            {
                randomNumber += 1;
                print("y�zdeli�in i�inde " + randomNumber);
            }*/

            GateNo.text = "+ " + randomNumber.ToString();
        }
    }
}
