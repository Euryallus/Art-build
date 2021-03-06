﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// ## HUGO BAILEY
// ## Written: Prototype phase
// ## Purpose: Boss enemy variant
//
public class bossEnemy : Enemy
{
    private bool firing = false;
    private bool readyToFight = false;
    private GameObject missile;
    [SerializeField]
    private GameObject missileBlueprint;
    [SerializeField]
    private Transform bossHead;

    private float rotateSpeed = 3;

    [SerializeField]
    private float fireRate = 3;

    [SerializeField]
    private starStoneManager stoneManager;

    private starStoneManager.starStones currentStarStone;

    [SerializeField]
    private float purpleCooldown = 2f;
    private bool purpleReadyToFire = true;
    [SerializeField]
    private float purpleChargeTime = 4f;
    [SerializeField]
    private float purpleFireTime = 4f;

    [SerializeField]
    private float blueCooldown = 3f;
    private bool blueReadyToFire = true;

    private bool orangeReady = true;

    [SerializeField]
    private float bossDamage = 1f;

    private bool canMelee = true;

    [SerializeField]
    private GameObject purpleChargePart1;
    [SerializeField]
    private GameObject purpleChargePart2;

    [SerializeField]
    private GameObject flames;

    [SerializeField]
    private GameObject iceLanceBlueprint;

    private bool purpleFiring = false;

    public LineRenderer purpleLaser;

    private Vector3 beamDirection;

    [Header("Sounds")]
    [SerializeField]
    private string roar;

    [SerializeField]
    private string laserCharge;
    [SerializeField]
    private string laserFire;


    [SerializeField]
    private List<GameObject> healthPools = new List<GameObject>();
    private bool poolsActive = false;

    [SerializeField]
    private GameObject explosionOnDeath;

    public bossEnemy() : base(500f, 180, 4, 1.5f, 4)
    {
        // ## CLASS CONSTRUCTOR
        // ## Calls "Enemy" parent constructor with values assigned to the boss
    }

    protected override void Start()
    {
        stoneManager = GameObject.FindGameObjectWithTag("GeneratorManager").GetComponent<starStoneManager>();
        purpleLaser.gameObject.SetActive(false);

        purpleChargePart1.SetActive(false);
        purpleChargePart2.SetActive(false);

        flames.SetActive(false);

        StartCoroutine(wakeUP());

        foreach(GameObject pool in healthPools)
        {
            pool.SetActive(false);
        }

        base.Start();

    }

    protected override void Die()
    {
        foreach (GameObject pool in healthPools)
        {
            pool.SetActive(false);
        }

        AudioManager.instance.StopAllLoopingSoundEffects();
        AudioManager.instance.StopAllSoundEffects();

        AudioManager.instance.PlaySoundEffect3D(deathSoundName, transform.position, 3f, 0.95f, 1.05f);
        //SaveLoadManager code added by Joe - sets PlayerKilledEnemy/boss to 1 (i.e. true)
        //  so the fact that the player has killed a certain enemy type is saved. Used to show relevant info in the codex scene.
        int enemiesKilled = SaveLoadManager.instance.LoadIntFromPlayerPrefs("Counter_EnemiesKilled");
        SaveLoadManager.instance.SaveIntToPlayerPrefs("Counter_EnemiesKilled", enemiesKilled + 1);

        GameObject exposion = Instantiate(explosionOnDeath);
        exposion.transform.position = transform.position;

        Destroy(gameObject);
    }

    public override void Patrol()
    {
        currentState = State.Patrol;
    }

    public override void Investigate()
    {
        currentState = State.Patrol;
    }

    public override void Engage()
    {
        if (readyToFight)
        {

            currentStarStone = stoneManager.returnActive();
            
            //when player is visible, stop moving and turn to look at player

            Vector3 playerDirection = playerVector.normalized;
            playerDirection.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

            beamDirection = (player.transform.position - new Vector3(0, 1, 0) - bossHead.position);

            switch (currentStarStone)
            {

                case starStoneManager.starStones.None:
                    agent.SetDestination(transform.position);

                    break;

                case starStoneManager.starStones.Purple:
                    AudioManager.instance.StopLoopingSoundEffect("flameFiring");
                    AudioManager.instance.StopLoopingSoundEffect("flame Passive");

                    flames.SetActive(false);

                    poolsActive = false;
                    foreach (GameObject pool in healthPools)
                    {
                        pool.SetActive(false);
                    }

                    if (canSeePlayer != true)
                    {
                        if(playerDist > viewDistance)
                        {
                            agent.SetDestination(player.transform.position);
                        }
                        else
                        {
                            agent.SetDestination(transform.position);
                        }
                        
                    }
                    else
                    {
                        agent.SetDestination(transform.position);
                    }

                    if (purpleReadyToFire)
                    {
                        StartCoroutine(purpleCharge());
                    }

                    if (purpleFiring)
                    {
                        RaycastHit hitObj;

                        Vector3 endOfLaser = (bossHead.position + new Vector3(0, beamDirection.y, 0) + (transform.forward * playerDist));

                        

                        //Debug.DrawRay(bossHead.position, (endOfLaser - bossHead.position).normalized * playerDist * 1.2f);

                        if (Physics.Raycast(bossHead.position, (endOfLaser - bossHead.position).normalized, out hitObj, playerDist))
                        {
                            if (hitObj.transform.gameObject.CompareTag("Player"))
                            {
                                player.GetComponent<playerHealth>().takeDamage(bossDamage);
                            }

                            else if(hitObj.transform.gameObject != null)
                            {
                                endOfLaser = hitObj.point;
                            }
                            
                        }

                        purpleLaser.SetPosition(0, bossHead.position);
                        
                        purpleLaser.SetPosition(1, endOfLaser);
                    }
                    break;

                case starStoneManager.starStones.Orange:
                    AudioManager.instance.StopLoopingSoundEffect("Laser Shot");
                    AudioManager.instance.StopLoopingSoundEffect("flame Passive");

                    purpleLaser.gameObject.SetActive(false);
                    purpleChargePart1.SetActive(false);
                    purpleChargePart2.SetActive(false);

                    poolsActive = false;
                    foreach (GameObject pool in healthPools)
                    {
                        pool.SetActive(false);
                    }

                    if (orangeReady && canSeePlayer)
                    {
                        StartCoroutine(orangeFlames());
                    }

                    if (canSeePlayer && playerDist > 15)
                    {
                        agent.SetDestination(player.transform.position);
                    }
                    else if (canSeePlayer != true && playerDist > 15)
                    {
                        agent.SetDestination(player.transform.position);
                    }
                    else
                    {
                        agent.SetDestination(transform.position);
                    }

                    break;

                case starStoneManager.starStones.Blue:
                    AudioManager.instance.StopLoopingSoundEffect("flameFiring");

                    AudioManager.instance.StopLoopingSoundEffect("Laser Shot");

                    purpleLaser.gameObject.SetActive(false);
                    purpleChargePart1.SetActive(false);
                    purpleChargePart2.SetActive(false);

                    flames.SetActive(false);
                    poolsActive = false;
                    foreach (GameObject pool in healthPools)
                    {
                        pool.SetActive(false);
                    }

                    if (canSeePlayer & blueReadyToFire)
                    {
                        StartCoroutine(blueFire());
                    }

                    if(canSeePlayer && playerDist > 15)
                    {
                        agent.SetDestination(player.transform.position);
                    }
                    else if(canSeePlayer != true && playerDist > 15)
                    {
                        agent.SetDestination(player.transform.position);
                    }
                    else
                    {
                        agent.SetDestination(transform.position);
                    }

                    break;

                case starStoneManager.starStones.Pink:
                    AudioManager.instance.StopLoopingSoundEffect("flameFiring");
                    AudioManager.instance.StopLoopingSoundEffect("flame Passive");
                    AudioManager.instance.StopLoopingSoundEffect("Laser Shot");

                    purpleLaser.gameObject.SetActive(false);
                    purpleChargePart1.SetActive(false);
                    purpleChargePart2.SetActive(false);

                    flames.SetActive(false);

                    if(poolsActive == false)
                    {
                        foreach(GameObject pool in healthPools)
                        {
                            pool.SetActive(true);
                            pool.GetComponent<bossHealthPool>().setParent(gameObject.GetComponent<bossEnemy>());
                            
                        }

                        poolsActive = true;
                    }

                    if (canSeePlayer && playerDist > meleeDistance)
                    {
                        agent.SetDestination(player.transform.position);
                    }

                    if(canSeePlayer && playerDist <= meleeDistance)
                    {
                        agent.SetDestination(transform.position);

                        if (canMelee)
                        {
                            canMelee = false;
                            StartCoroutine(bossMelee());
                        }
                    }

                    if(canSeePlayer == false)
                    {
                        agent.SetDestination(player.transform.position);
                    }

                    break;
            }

        }
        //base.Engage(); //call base ENGAGE behaviour from parent class ('Enemy')
    }

    

    private IEnumerator purpleCharge()
    {
        purpleReadyToFire = false;
        purpleChargePart1.SetActive(true);

        AudioManager.instance.PlayLoopingSoundEffect("Laser Loop", false, transform.position, "Laser Shot", 1, 0f, 0.5f);

        yield return new WaitForSeconds(purpleChargeTime);
        AudioManager.instance.StopLoopingSoundEffect("Laser Shot");

        AudioManager.instance.PlayLoopingSoundEffect("Laser Loop", false, transform.position, "Laser Shot", 1, 0.8f, 1.3f);
        AudioManager.instance.PlaySoundEffect3D("Laser Shot", transform.position, 2, 1, 1);
        purpleChargePart2.SetActive(true);
        purpleFiring = true;
        purpleLaser.gameObject.SetActive(true);
        rotateSpeed = 3;
        yield return new WaitForSeconds(purpleFireTime);

        AudioManager.instance.StopLoopingSoundEffect("Laser Shot");
        purpleFiring = false;
        rotateSpeed = 3;
        purpleLaser.gameObject.SetActive(false);

        purpleChargePart1.SetActive(false);
        purpleChargePart2.SetActive(false);

        yield return new WaitForSeconds(purpleCooldown);
        purpleReadyToFire = true;
    }

    private IEnumerator blueFire()
    {
        blueReadyToFire = false;

        AudioManager.instance.PlaySoundEffect3D("IceCronch", transform.position, 3f, 0.9f, 1.1f);

        GameObject iceLance = Instantiate(iceLanceBlueprint);
        iceLance.transform.position = transform.position + (transform.right * 2) + (transform.up * 1.5f);
        iceLance.GetComponent<coneScript>().takeParentPos(iceLance.transform.position - transform.position, gameObject);

        yield return new WaitForSeconds(2);
        iceLance.GetComponent<coneScript>().takeDirection((player.transform.position - iceLance.transform.position));
        
        yield return new WaitForSeconds(blueCooldown);
        
        blueReadyToFire = true;
    }

    private IEnumerator orangeFlames()
    {

        AudioManager.instance.PlayLoopingSoundEffect("FlameFire", false, transform.position, "flameFiring", 1, 0.5f, 1.3f);
        flames.SetActive(true);
        orangeReady = false;

        yield return new WaitForSeconds(5);
        AudioManager.instance.StopLoopingSoundEffect("flameFiring");
        flames.SetActive(false);

        AudioManager.instance.PlayLoopingSoundEffect("flamePassive", false, transform.position, "flame Passive", 1.1f, 1f, 1.3f);
        yield return new WaitForSeconds(3);
        AudioManager.instance.StopLoopingSoundEffect("flame Passive");
        orangeReady = true;
    }

    private IEnumerator wakeUP()
    {
        yield return new WaitForSeconds(4);
        AudioManager.instance.PlaySoundEffect3D(roar, transform.position, 5, 1, 1);
        yield return new WaitForSeconds(2);
        readyToFight = true;
    }

    private IEnumerator bossMelee()
    {
        player.GetComponent<playerHealth>().takeDamage(meleeHitDamage);

        AudioManager.instance.PlaySoundEffect3D("EnemyMeleeHit", transform.position);
        yield return new WaitForSeconds(2);

        canMelee = true;
    }

}
