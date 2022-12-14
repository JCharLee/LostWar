using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayShoot : MonoBehaviour
{
    private RaycastHit hit; // 광선에 맞은 판정
    private const float _pivot = 0.02f;
    [SerializeField] private Transform FirePos_R;
    [SerializeField] private Transform FirePos_L;
    [SerializeField] private Transform FirePos_Cur;
    [SerializeField] private MoveBehaviour moveBehaviour;
    new AudioSource audio;
    AudioClip firesfx;
    void Start()
    {
        FirePos_Cur = Camera.main.transform;
        moveBehaviour = GetComponent<MoveBehaviour>();
        audio = GetComponent<AudioSource>();
        firesfx = Resources.Load<AudioClip>("Sound/AutoGun_3p_02");
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || FindObjectOfType<Health>().isdie) return;
        if (UIManager.instance.isAction || UIManager.instance.isPaused) return;
        Debug.DrawRay(FirePos_Cur.position, FirePos_Cur.forward * 25f, Color.green);
        if (moveBehaviour.usingWeapon == MoveBehaviour.UsingWeapon.long_dist && AimBehaviourBasic.aim)
        {
            if (Input.GetMouseButtonDown(0))
            Fire();
        }
    }
    void Fire()
    {
        audio.PlayOneShot(firesfx, 1f);

        if (Physics.Raycast(FirePos_Cur.position, FirePos_Cur.forward, out hit, 25f))
        {
            if (hit.collider.tag == "ENEMY")
            {
                object[] _params = new object[2];
                _params[0] = hit.point;
                _params[1] = 10f;
                hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
            }

            var Enemy = hit.transform.GetComponent<EnemyHealth>();
            if(Enemy != null)
            {
                Enemy.takeDamage(10f, hit.point);
            }
        }
    }
    public void MoveAim()
    {
        
        if (FirePos_Cur == FirePos_L)
        {
            FirePos_Cur = FirePos_R;
        }
        else if (FirePos_Cur == FirePos_R)
        {
            FirePos_Cur = FirePos_L; 
        }
    }


}
