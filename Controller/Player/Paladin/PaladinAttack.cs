using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAttack : MonoBehaviour
{
    public BoxCollider sword;
    Collider c;
    public static bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        sword.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            sword.enabled = true;
        }
        else
        {
            sword.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Minion")
        {
            other.gameObject.GetComponent<EnemyController>().setCurrHealth(10);
        }
        else if (other.gameObject.tag == "Boss")
        {
            other.gameObject.GetComponent<BossController>().setCurrHealth(10);
        }
    }
}
