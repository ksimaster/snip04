using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAnimal : MonoBehaviour
{
    private string terrainTag = "Terrain";
    private float posX;
    private float posY;
    private float posZ;

    private float sideAreaSpawn = 200;
    public GameObject mapPoint;
    public GameObject player;


    private void Awake()
    {
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void Start()
    {
        posX = Random.Range(player.transform.position.x - sideAreaSpawn, player.transform.position.x + sideAreaSpawn);
        posY = 50;
        posZ = Random.Range(player.transform.position.z - sideAreaSpawn, player.transform.position.z + sideAreaSpawn);
        gameObject.transform.position = new Vector3(posX, posY, posZ);
    }

    private void Update()
    {
        if(gameObject.transform.position.y <= -300)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(terrainTag))
        {
            Debug.Log("Животное коснулось террейна!");
            //gameObject.transform.position += new Vector3(0, 0.01f,0);
            gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            Destroy(GetComponent<StandAnimal>());
        }
    }


}
