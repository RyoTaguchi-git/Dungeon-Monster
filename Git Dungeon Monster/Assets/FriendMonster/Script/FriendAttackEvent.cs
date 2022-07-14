using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendAttackEvent : MonoBehaviour
{

    [SerializeField] public Event AttackEvent;
    [SerializeField] public Event SpAttackEvent;
    [SerializeField] private FriendMonster Friendscript;

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {

            switch (Friendscript.GetAttackstate())
            {
                case "Attack": AttackEvent!.Invoke(other); break;
                case "SpAttack": SpAttackEvent!.Invoke(other); break;
                default: break;
            }


        }


    }
}
