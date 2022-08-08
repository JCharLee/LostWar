using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public bool canAttack = false;
    private void OnTriggerEnter(Collider other)
    {
        if (canAttack)
        {
            if (other.gameObject.CompareTag("ENEMY"))
            {
                object[] _params = new object[2];
                _params[0] = null;
                _params[1] = 25f;
                other.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
            }

            var Enemy = other.GetComponent<EnemyHealth>();
            if(Enemy != null)
            {
                Enemy.takeDamage(25f, other.transform.position);
            }
        }
    }

}