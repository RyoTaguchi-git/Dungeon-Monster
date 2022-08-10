using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Rockhit : MonoBehaviour
{

    private EnemyMovement Enemyscript;

  public void Set(EnemyMovement script)
    {
        Enemyscript = script;
      
    }


    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerFriend"))
        {
           
            Enemyscript.SpecialAttack(other);

        }
        
    }
  


}
