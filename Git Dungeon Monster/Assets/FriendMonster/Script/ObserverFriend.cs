using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ObserverFriend : MonoBehaviour
{
    [SerializeField] Event StayEvent;
    [SerializeField] UnityEvent EnterEvent;
    [SerializeField] UnityEvent ExitEvent;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            StayEvent!.Invoke(other);
          
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnterEvent!.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            ExitEvent!.Invoke();

        }
    }
}
