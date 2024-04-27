using System.Text;
using UnityEngine;

//Базовая статистика для существа которое живет и передвигаеться
public class GeneralLiveForm
{
    private float _HP, _maxHp;
    private float _magDef, _physDef, _atack;
    private bool _atackType; //0-phys 1-ma
    private float _speed;
    private GameObject _liveFormObject;

    public GeneralLiveForm(float HP, float magDef, float physDef, float atack, bool atackType, float speed, GameObject liveFormObject)
    {
        _HP = HP;
        _maxHp = _HP;
        _magDef = magDef;
        _physDef = physDef;
        _atack = atack;
        _atackType = atackType;
        _liveFormObject = liveFormObject;
        _speed = speed;
    }

    public bool GetAtackType() { return _atackType; }

    public GameObject GetGameObject() { return _liveFormObject; }

    public float GetAtack() { return _atack; }

    public float GetSpeed() { return _speed; }

    public float GetHP() { return _HP; }

    public void HPRegen(float HP)
    {
        if (HP + _HP < _maxHp)
        {
            _HP += HP;

        }
        else
            _HP = _maxHp;

    }

    public void ChangeTypeAction() =>
        _atackType = !_atackType;

    private bool DeadCheck() => _HP < 0 ? true : false;

    public bool GetDamage(bool damageType,float Damage, float ScalingDefDown) 
    {
        if (damageType)
            return GetMagDamage(Damage, ScalingDefDown);
                else
            return GetPhysDamage(Damage, ScalingDefDown);
    }
    public float GetExpectedDamage(bool damageType, float Damage, float ScalingDefDown)
    {
        if (damageType)
            return Damage - _magDef * (1 - ScalingDefDown);
        else
            return Damage - _physDef * (1 - ScalingDefDown);
    }


    private bool GetMagDamage(float magDamage, float ScalingDefDown)
    {
        float damage = (magDamage - _magDef * (1 - ScalingDefDown));
        if (damage > 0)
        {
            _HP -= damage;
            Debug.Log(_liveFormObject.name + " get magD " + damage + " HP left- "+ _HP);
            return DeadCheck();
        }
        else
            return false;
    }

    private bool GetPhysDamage(float physDamage, float ScalingDefDown)
    {
        float damage = (physDamage - _physDef * (1 - ScalingDefDown));
        if (damage > 0)
        {
            Debug.Log(_liveFormObject.name + " get physgD " + damage + " HP left- " + _HP);
            _HP -= damage;
            return DeadCheck();
        }
        return false;
    }


    public override string ToString()
    {

        return ("HP: " + _HP + "/" + _maxHp);

    }
}

