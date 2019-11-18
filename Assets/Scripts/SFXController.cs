using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [SerializeField]
    AudioClip AttackSound;

    [SerializeField]
    AudioClip DethSound;
    [SerializeField]
    AudioSource audioSource;

    private void Start()
    {
        UnitsManager.UnitDetroyed += ShowDestroyParticle;
        UnitsManager.UnitAttack += ShowUnitAttackParticle;
    }
    private void ShowUnitAttackParticle(Units a, Units b)
    {
        audioSource.clip = AttackSound;
        audioSource.Play();
    }
    private void ShowDestroyParticle(Units obj)
    {
        audioSource.clip = DethSound;
        audioSource.Play();
    }
    private void OnDestroy()
    {
        UnitsManager.UnitDetroyed -= ShowDestroyParticle;
        UnitsManager.UnitAttack -= ShowUnitAttackParticle;
    }
}
