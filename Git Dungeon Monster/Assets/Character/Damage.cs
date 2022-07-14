using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject Player;
    PlayerController PlayerScript;

    private void Start()
    {
        PlayerScript = Player.GetComponent<PlayerController>();
        GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
          int AttackValue = PlayerScript.GetAttackValue();
           EnemyMovement script = other.gameObject.GetComponent<EnemyMovement>();
            script.Takendamage(Player,AttackValue);
            //Debug.Log(script);
        }


    }

}
