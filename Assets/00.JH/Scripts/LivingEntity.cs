using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : GenericBehaviour
{
    protected float startHp = 100;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    public event Action onDeath; //사망 시 발동할 이벤트
    public bool canDamaged;


    //생명체가 활성화될 떄 상태를 리셋
    protected virtual void OnEnable()
    {
        //사망하지 않은 상태로 시작
        dead = false;
        //체력을 시작 체력으로 초기화
        health = startHp;
        canDamaged = true;
    }
    //피해를 받는 기능
    public virtual void OnDamage(object[] _params)
    {
        if (canDamaged)
        {
            //데미지만큼 체력 감소
            health -= (float)_params[1]; // health = health - damage;
            //체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
            if (health <= 0 && !dead)
            {
                Die();
            }
            
            StartCoroutine(DamageDelay());
        }
    }

    //체력을 회복 하는 기능은 책에 있는데 나는 플레이어 스텟 스크립트에서 적용할 것임

    //사망 처리
    public virtual void Die()
    {
        //onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }
    IEnumerator DamageDelay()
    {
        canDamaged = false;
        yield return new WaitForSeconds(0.5f);
        canDamaged = true;
    }
}