using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : MonoBehaviour
{

    private PlayerObjectController instance = PlayerObjectController.getInstance();
    private bool playerDmg, enemyDmg;
    Collider c;

    // Start is called before the first frame update
    private void Start()
    {
        c = null;
        playerDmg = false;
        enemyDmg = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Minion" && !enemyDmg)
        {
            c = other;
            enemyDmg = true;
            //StartCoroutine(Damage(other));
            Invoke("EnemyDmg", 0.2f);
        }
        else if (other.gameObject.CompareTag("Boss") && !enemyDmg)
        {
            c = other;
            enemyDmg = true;
            Invoke("BossDmg", 0.2f);
        }
        else if (other.gameObject.CompareTag("Player") && !playerDmg)
        {
            playerDmg = true;
            Invoke("PlayerDmg", 0.2f);
            //StartCoroutine(PlayerDamage(other));
        }
    }

    void EnemyDmg()
    {
        if (enemyDmg && c != null)
        {
            c.gameObject.GetComponent<EnemyController>().setCurrHealth(10);
            enemyDmg = false;
            c = null;
        }
    }

    void BossDmg()
    {
        if (enemyDmg && c != null)
        {
            c.gameObject.GetComponent<BossController>().setCurrHealth(10);
            enemyDmg = false;
            c = null;
        }
    }

    void PlayerDmg()
    {
        if (playerDmg)
        {
            instance.setCurrHealth(instance.getCurrHealth() - 5);
            playerDmg = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Minion")
        {
            enemyDmg = false;
            c = null;
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            enemyDmg = false;
            c = null;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            playerDmg = false;
        }
    }
}
