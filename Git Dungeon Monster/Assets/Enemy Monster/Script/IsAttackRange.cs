using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable] public class Event : UnityEvent<Collider> { }
public class IsAttackRange : MonoBehaviour
{

    [SerializeField] public Event AttackEvent;
    [SerializeField] public Event SpAttackEvent;
    [SerializeField] private EnemyMovement Enemyscript;

  


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerFriend")) 
        {

            switch (Enemyscript.GetAttackstate()) {
                case "Attack":  AttackEvent!.Invoke(other); break;
                case "SpAttack": SpAttackEvent!.Invoke(other);break;
                default: break;
            }
               

        }


    }
  


}
