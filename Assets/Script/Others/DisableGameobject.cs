using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameobject : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    public void DisableObject()
    {
        target.SetActive(false);
    }
}
