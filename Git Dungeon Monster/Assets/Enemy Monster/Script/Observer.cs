using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//�N���X���C���X�y�N�^�[�ɕ\������ɂ�Serializable���K�v
//UnityEvent���p�����邱�Ƃň������X�N���v�g��œn�����Ƃ��ł���
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
