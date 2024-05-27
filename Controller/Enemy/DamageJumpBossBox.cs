using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageJumpBossBox : MonoBehaviour
{

    PlayerObjectController instance = PlayerObjectController.getInstance();
    private int ctr = 0;

    private void Update()
    {
        if (!BossController.isAttack)
        {
            ctr = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (BossController.isAttack)
        {
            ctr++;
            if (other.gameObject.tag == "Player" && ctr == 1)
            {
                instance.setCurrHealth(instance.getCurrHealth() - 50);
            }
        }
    }
}
