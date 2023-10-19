using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
 public void HideObject()
    {
        transform.gameObject.SetActive(false);
    }
   
}
