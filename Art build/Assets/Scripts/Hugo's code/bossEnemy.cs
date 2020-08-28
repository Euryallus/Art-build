using System.Collections;
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

                        AudioManager.instance.PlaySoundEffect2D(laserFire, 1, 0.8f, 1.3f);

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

                    break;
            }

        }
        //base.Engage(); //call base ENGAGE behaviour from parent class ('Enemy')
    }

    private IEnumerator purpleCharge()
    {
        purpleReadyToFire = false;
        purpleChargePart1.SetActive(true);
        AudioManager.instance.PlaySoundEffect2D(laserCharge,  1.5f, 0.4f, 0.8f);

        yield return new WaitForSeconds(purpleChargeTime);

        purpleChargePart2.SetActive(true);
        purpleFiring = true;
        purpleLaser.gameObject.SetActive(true);
        rotateSpeed = 3;
        yield return new WaitForSeconds(purpleFireTime);

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
        flames.SetActive(true);
        orangeReady = false;

        yield return new WaitForSeconds(5);
        flames.SetActive(false);

        yield return new WaitForSeconds(3);
        orangeReady = true;
    }

    private IEnumerator wakeUP()
    {
        yield return new WaitForSeconds(6);
        AudioManager.instance.PlaySoundEffect2D(roar, 3, 1, 1);
        readyToFight = true;
    }

}
