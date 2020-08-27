using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coneScript : MonoBehaviour
{
    private Vector3 direction;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float damage = 5f;

    private bool fire = false;

    private GameObject player;

    private Vector3 bossVector;
    private GameObject boss;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void takeParentPos(Vector3 parentPosition, GameObject parent)
    {
        bossVector = parentPosition;
        boss = parent;
    }

    public void takeDirection(Vector3 inpDirection)
    {
        direction = inpDirection;
        fire = true;

    }

    private void FixedUpdate()
    {
        Vector3 Rotation = player.transform.position - transform.position;

        Vector3 playerVector = gameObject.transform.position - player.transform.position;
        //playerVector.x = playerVector.x;
        //playerVector.y = -playerVector.y;
        //
        //float x = playerVector.x;
        //playerVector.x = -playerVector.z;
        //playerVector.z = x;

        Quaternion lookRotation = Quaternion.LookRotation(playerVector.normalized);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3);


        if (fire)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + direction, speed * Time.deltaTime);
        }
        else
        {
            transform.position = boss.transform.position + bossVector;
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<playerHealth>().takeDamage(damage);
            collision.gameObject.GetComponent<playerMovement>().slowEffect(0.75f, 3f);
        }

        if(collision.gameObject.CompareTag("Enemy") == false && fire)
        {
            Destroy(gameObject);
        }

    }
}
