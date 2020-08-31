using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveBossEnemy : Enemy
{

    [SerializeField]
    private GameObject missileTemplate;

    private bool readyToFire = true;


    public waveBossEnemy() : base(500f, 180, 4, 1.5f, 4)
    {
        
    }

    public override void Patrol()
    {
        currentState = State.Engage;
        //base.Patrol();
    }

    public override void Investigate()
    {
        currentState = State.Engage;
        //base.Investigate();
    }

    public override void Engage()
    {

        Vector3 playerDirection = playerVector.normalized;
        playerDirection.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3);

        if (canSeePlayer)
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }

        if(canSeePlayer && readyToFire)
        {
            StartCoroutine(fire());
        }
    }

    private IEnumerator fire()
    {
        readyToFire = false;
        GameObject missile = Instantiate(missileTemplate);
        missile.transform.position = transform.position + (transform.forward * 1.5f);

        yield return new WaitForSeconds(3);
        readyToFire = true;
    }
}
