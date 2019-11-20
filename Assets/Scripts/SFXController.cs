using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    private void Start()
    {
        UnitsManager.UnitDetroyed += PlayDethSound;
        UnitsManager.UnitAttack += PlayAttackSound;
    }
    private void PlayAttackSound(Units a, Units b)
    {
        audioSource.Play();
    }
    private void PlayDethSound(Units obj)
    {
        audioSource.Play();
    }
    private void OnDestroy()
    {
        UnitsManager.UnitDetroyed -= PlayDethSound;
        UnitsManager.UnitAttack -= PlayAttackSound;
    }
}
