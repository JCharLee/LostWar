using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;// 아이템 정보를 담은 네임 스페이스

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameData", order = 0)]
public class GameDataObject : ScriptableObject
{
    public GameData gameData = new GameData();

    //능력치들
    public int Level { get; set; } = 1;// 레벨
    public int Level_max { get; }=30;// 최대 레벨
    private float exp;// 경험치
    private float maxHp = 1000f;
    private float hp;
    private float maxSp = 100f;
    private float sp;
    public float Exp// 경험치 프로퍼티
    {
        get
        {
            return exp;// 경험치 반환
        }
        set
        {
            exp = value;// 들어오는 값을 받아들인다
            if (exp >= Exp_require)// 경험치가 요구치보다 크다면
            {
                exp -= Exp_require;// 요구치 만큼 경험치 값을 뺀다
                Level++;// 레벨이 오른다
                Exp_require *= 1.2f;// 요구치가 1.2배 증가한다
                UIManager.instance.UpdateLevel(Level);// 게임 매니저의 레벨 업데이트 함수 실행
            }
        }
    }
    public float Exp_require { get; set; } = 100f;// 경험치 요구치
    public int Str { get; set; } = 5;// 힘 스탯
    public int Agi { get; set; } = 5;// 민첩 스탯
    public int Con { get; set; } = 5;// 컨디션 스탯
    public int Vit { get; set; } = 5;// 바이탈 스탯
    public int Status_max { get; } = 30;// 스탯 맥스치
    public int Status_own { get; set; } = 5;// 현재 스탯
    public float MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
            if (Hp > maxHp)
            {
                Hp -= (Hp - maxHp);
                UIManager.instance.DisplayHpBar();
            }
        }
    }
    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            hp = Mathf.Clamp(hp, 0f, MaxHp);
        }
    }
    public float MaxSp
    {
        get
        {
            return maxSp;
        }
        set
        {
            maxSp = value;
            if (Sp > maxSp)
            {
                Sp -= (Sp - maxSp);
                UIManager.instance.DisplaySpBar();
            }
        }
    }
    public float Sp
    {
        get
        {
            return sp;
        }
        set
        {
            sp = value;
            sp = Mathf.Clamp(sp, 0f, MaxSp);
        }
    }
    public float Dam { get; set; } = 5;// 데미지
    public float Weight { get; set; } = 0;// 무게 스탯
    public float Max_weight { get; } = 100;// 최대 무게치
    public float Def { get; set; } = 5;// 방어 스탯
    //가진 아이템들
    public List<Item> shortWeapon = new List<Item>();// 근거리 무기 리스트
    public List<Item> longWeapon = new List<Item>();// 장거리 무기 리스트
    public List<Item> shoes = new List<Item>();// 신발 리스트
    public List<Item> top = new List<Item>();// 상의 리스트
    public List<Item> bottoms = new List<Item>();// 하의 리스트
    public List<Item> hpPotion = new List<Item>();
    public List<Item> spPotion = new List<Item>();
    //현재 장착중인 아이템들
    public Weapon shortWeapon_C;// 현재 근거리 무기
    public Weapon longWeapon_C;// 현재 장거리 무기
    public Clothes shoes_C;// 현재 신발
    public Clothes top_C;// 현재 상의
    public Clothes bottoms_C;// 현재 하의
}