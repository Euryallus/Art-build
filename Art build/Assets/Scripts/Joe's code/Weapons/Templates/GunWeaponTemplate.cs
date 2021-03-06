﻿using UnityEngine;

//------------------------------------------------------\\
//  Template that defines a type of gun that can        \\
//  be edited in the inspector by creating a            \\
//  ScriptableObject and adjusting its properties       \\
//------------------------------------------------------\\
//      Written by Joe for proof of concept phase       \\
//      and modified/optimised for prototype phase      \\
//------------------------------------------------------\\

[CreateAssetMenu(fileName = "Gun Template", menuName = "Weapon Template/Gun")]
public class GunWeaponTemplate : WeaponTemplate
{
    #region Properties

    //See tooltips for comments on each of the following priperties:

    [Header("Gun Properties")]

    [SerializeField]
    [Tooltip("The offset of the weapon GameObject from its parent when aiming down sights")]
    private Vector3 m_aimDownSightOffset;

    [SerializeField]
    [Tooltip("Prefab for particles to be spawned when this gun is fired")]
    private GameObject m_fireParticles;

    [SerializeField]
    [Tooltip("Objects within this range can be hit")]
    private float m_range;

    [SerializeField]
    [Tooltip("If true, this gun will fire continually while the fire button is held. If false, it will fire a single time for each button press.")]
    private bool m_continuousFire;

    [SerializeField]
    [Tooltip("Total ammo for this gun on level start")]
    private int m_totalStartAmmo;

    [SerializeField]
    [Tooltip("Number of available bullets after reloading")]
    private int m_magazineSize;

    [SerializeField]
    [Tooltip("The amount of time taken to reload this gun (seconds)")]
    private float m_reloadTime;

    [SerializeField]
    [Tooltip("If true, this weapon can be used to as a melee weapon to hit entities at a defined short range")]
    private bool m_canUseAsMelee;

    [SerializeField]
    [Tooltip("Maximum distance from the target where this gun will be effective as a melee weapon")]
    private float m_meleeRange;

    [SerializeField]
    [Tooltip("Minimum damage given to the enemy/player when using this gun a a melee weapon")]
    private int m_minMeleeAttackDamage;

    [SerializeField]
    [Tooltip("Maximum damage given to the enemy/player when using this gun a a melee weapon")]
    private int m_maxMeleeAttackDamage;

    [SerializeField]
    [Tooltip("Minimum time between each usage of this gun as a melee weapon")]
    private float m_meleeInterval;

    [SerializeField]
    [Tooltip("Intensity of recoil effect when shooting")]
    private float m_recoilIntensity;

    //Set in custom editor:

    //Name of sound effect to be played when using this gun as a melee weapon
    [SerializeField]
    [HideInInspector]
    public string m_meleeSound;

    //Volume of sound effect to be played when using this gun as a melee weapon
    [SerializeField]
    [HideInInspector]
    public float m_meleeSoundVolume;

    //Name of sound effect to be played when this gun is reloaded
    [SerializeField]
    [HideInInspector]
    public string m_reloadSound;

    //Volume of sound effect to be played when this gun is reloaded
    [SerializeField]
    [HideInInspector]
    public float m_reloadSoundVolume;

    //Name of sound effect to be played when an object is hit by a bullet
    [SerializeField]
    [HideInInspector]
    public string m_objectHitSound;

    //Volume of sound effect to be played when an object is hit by a bullet
    [SerializeField]
    [HideInInspector]
    public float m_objectHitSoundVolume;

    #endregion

    #region Getters

    //Getters
    //=======

    public Vector3 GetAimDownSightOffset()
    {
        return m_aimDownSightOffset;
    }
    public GameObject GetFireParticles()
    {
        return m_fireParticles;
    }
    public float GetRange()
    {
        return m_range;
    }
    public bool GetContinuousFire()
    {
        return m_continuousFire;
    }
    public int GetTotalStartAmmo()
    {
        return m_totalStartAmmo;
    }
    public int GetMagazineSize()
    {
        return m_magazineSize;
    }
    public float GetReloadTime()
    {
        return m_reloadTime;
    }
    public bool GetCanUseAsMelee()
    {
        return m_canUseAsMelee;
    }
    public float GetMeleeRange()
    {
        return m_meleeRange;
    }
    public float GetMeleeInterval()
    {
        return m_meleeInterval;
    }
    public int GetMinMeleeAttackDamage()
    {
        return m_minMeleeAttackDamage;
    }
    public int GetMaxMeleeAttackDamage()
    {
        return m_maxMeleeAttackDamage;
    }
    public float GetRecoilIntensity()
    {
        return m_recoilIntensity;
    }

    #endregion
}