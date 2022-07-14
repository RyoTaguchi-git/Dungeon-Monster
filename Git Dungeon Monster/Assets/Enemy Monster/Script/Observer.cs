using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//クラスをインスペクターに表示するにはSerializableが必要
//UnityEventを継承することで引数をスクリプト上で渡すことができる
// [System.Serializable] public class Event : UnityEvent<Collider> { }
public class Observer : MonoBehaviour
{
    [SerializeField]  Event StayEvent;
    [SerializeField] UnityEvent EnterEvent;
    [SerializeField] UnityEvent ExitEvent;
   
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerFriend"))
        {
            StayEvent!.Invoke(other);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerFriend"))
        {
            EnterEvent!.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerFriend"))
        {
            ExitEvent!.Invoke();
        }
    }
}
