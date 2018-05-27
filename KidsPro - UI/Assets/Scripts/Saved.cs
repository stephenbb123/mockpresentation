using UnityEngine;
using System.Collections;

public class Saved : MonoBehaviour
{
    

    public void Save()
    {
        gameObject.SetActive(true);
        new WaitForSecondsRealtime(1);
        gameObject.SetActive(false);
    }
}
