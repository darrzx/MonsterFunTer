using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    private bool collided = false;
    public GameObject impact;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided && collision.gameObject.tag != "Player" && collision.gameObject.tag != "Bullet")
        {
            collided = true;

            var obj = Instantiate(impact, collision.contacts[0].point, Quaternion.identity) as GameObject;

            Destroy(gameObject);
            Destroy(obj, 2f);
        }
    }
}
