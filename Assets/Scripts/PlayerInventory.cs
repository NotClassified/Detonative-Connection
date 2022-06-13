using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int batteries;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Battery"))
        {
            Destroy(other.gameObject);
            batteries++;
        }
    }
}
