using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerObjectController instance = PlayerObjectController.getInstance();
    private int ctr = 0;

    private void Update()
    {
        if (!EnemyController.isAttack)
        {
            ctr = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EnemyController.isAttack)
        {
            ctr++;
            if (other.gameObject.tag == "Player" && ctr == 1)
            {
                instance.setCurrHealth(instance.getCurrHealth() - 10);
            }
        }
    }
}