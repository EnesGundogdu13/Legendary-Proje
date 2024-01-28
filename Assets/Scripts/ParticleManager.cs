using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ParticleDeactivate());
    }

    IEnumerator ParticleDeactivate()
    {
        yield return new WaitForSecondsRealtime(2);
        gameObject.SetActive(false);
    }
}





/*    
     
    //start ile Ienumerator kullan�m�n� hoca be�enmedi, de�i�tirilecek
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(2);
        gameObject.SetActive(false);
    }
*/