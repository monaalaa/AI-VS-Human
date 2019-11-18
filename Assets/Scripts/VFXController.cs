using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField]
    ParticleSystem AttackPrefab;

    [SerializeField]
    ParticleSystem DethPrefab;

    private void Start()
    {
        UnitsManager.UnitDetroyed += ShowDestroyParticle;
        UnitsManager.UnitAttack += ShowUnitAttackParticle;
    }
    private void ShowUnitAttackParticle(Units a, Units b)
    {
      Instantiate(AttackPrefab, b.transform.position, new Quaternion());
    }
    private void ShowDestroyParticle(Units obj)
    {
        if (obj != null)
        {
            Vector3 pos = new Vector3(obj.transform.position.x, obj.transform.position.y + 0.5f, obj.transform.position.z);
            Instantiate(DethPrefab, pos, new Quaternion());
        }
    }
    private void OnDestroy()
    {
        UnitsManager.UnitDetroyed -= ShowDestroyParticle;
        UnitsManager.UnitAttack -= ShowUnitAttackParticle;
    }
}
