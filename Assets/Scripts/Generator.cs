using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int zombieNumber;
    public int gunNumber;
    public GameObject[] gunLibrary;


    // Start is called before the first frame update
    void Start()
    {
        generateZombie();
        generateGun();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // random generate zombies and distributed them
    void generateZombie()
    {
        int unit = zombieNumber / 10;

        for (int i = 0; i < unit; i ++) 
        {
            generateZombieHelper(1);
            generateZombieHelper(2);
        }
        for (int i = 0; i < (zombieNumber - 2 * unit); i ++) 
        {
            generateZombieHelper(3);
        }
    }


    void generateZombieHelper(int typeZombie) 
    {
        GameObject zombie;
        if (typeZombie == 1) 
        {
            //instantiate a new zombie
            zombie = Instantiate(Resources.Load("zombie", typeof(GameObject))) as GameObject;
        }
        else if (typeZombie == 2) 
        {
            //instantiate a new zombie
            zombie = Instantiate(Resources.Load("zombie2", typeof(GameObject))) as GameObject;
        }
        else
        {
            //instantiate a new zombie
            zombie = Instantiate(Resources.Load("zombie3", typeof(GameObject))) as GameObject;
        }
        //random zombie position
        float randomX = UnityEngine.Random.Range(-12f, 52f);
        float constantY = 0.01f;
        float randomZ = UnityEngine.Random.Range(-22f, 32f);
        zombie.transform.position = new Vector3(randomX, constantY, randomZ);

        //if the zombie gets positioned less than or equal to 3 scene units away from the camera we won't be able to shoot it
        //so keep repositioning the zombie until it is greater than 3 scene units away. 
        while (Vector3.Distance(zombie.transform.position, Camera.main.transform.position) <= 6) 
        {
            randomX = UnityEngine.Random.Range(-12f,52f);
            randomZ = UnityEngine.Random.Range(-22f,32f);
            zombie.transform.position = new Vector3(randomX, constantY, randomZ);
        }
    }

    // random generate guns and distributed them
    void generateGun()
    {
        for (int i = 0; i < gunNumber; i ++) 
        {
            int index = Random.Range(0, gunLibrary.Length);
            //instantiate a new gun
            GameObject gun = Instantiate(gunLibrary[index]) as GameObject;

            //random zombie position
            float randomX = UnityEngine.Random.Range(-12f, 52f);
            float randomY = UnityEngine.Random.Range(0.01f, 4f);
            float randomZ = UnityEngine.Random.Range(-22f, 32f);
            gun.transform.position = new Vector3(randomX, randomY, randomZ);
            gun.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }    
    }
}
