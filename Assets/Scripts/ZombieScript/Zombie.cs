
using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
  //declare the transform of our goal (where the navmesh agent will move towards) and our navmesh agent (in this case our zombie)
  public GameObject ScoreText;

  private Transform goal;
  private UnityEngine.AI.NavMeshAgent agent;
  private Animation animation;
  private bool died;

  private ScoreSc scriptSc;

  // Use this for initialization
  void Start () {
  
    //create references
    died = false;
    goal = Camera.main.transform;
    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    animation = GetComponent<Animation>();
    //set the navmesh agent's desination equal to the main camera's position (our first person character)
    agent.destination = goal.position;
    //start the walking animation
    animation.Play ("Z_Walk_InPlace");
  }


  void Update() 
  {
    if (!died) {
      goal = Camera.main.transform;
    }
    else
    {
      goal = agent.transform;
    }
    agent.destination = goal.position;

    // attack
    if (!died && Vector3.Distance (agent.transform.position, Camera.main.transform.position) <= 3.1f) {
      animation.Play ("Z_Attack"); 
    } 
    if (!died && Vector3.Distance (agent.transform.position, Camera.main.transform.position) >= 3.2f) {
      animation.Play ("Z_Walk_InPlace"); 
    }

  }

  void UpdateScore()
  {
    ScoreText =  GameObject.Find("ScoreValue");
    // ui, score
    scriptSc = ScoreText.GetComponent<ScoreSc>();
    scriptSc.scoreValue += 5;
  }


  //for this to work both need colliders, one must have rigid body, and the zombie must have is trigger checked.
  void OnTriggerEnter (Collider col)
  {
    if(col.gameObject.CompareTag("bullet"))
    {
        //first disable the zombie's collider so multiple collisions cannot occur
      GetComponent<CapsuleCollider>().enabled = false;
      //destroy the bullet
      Destroy(col.gameObject);
      //stop the zombie from moving forward by setting its destination to it's current position
      agent.destination = gameObject.transform.position;
      //stop the walking animation and play the falling back animation
      GetComponent<AudioSource>().Play();
      animation.Stop ();
      animation.Play ("Z_FallingBack");
      died = true;

      //instantiate a blood
      GameObject blood = Instantiate(Resources.Load("blood", typeof(GameObject))) as GameObject;
      blood.transform.position = gameObject.transform.position;

      //destroy this zombie in six seconds.
      Destroy (gameObject, 6);
      Destroy (blood, 6);
      // update score
      UpdateScore();
      //instantiate a new zombie
      GameObject zombie = Instantiate(Resources.Load("zombie", typeof(GameObject))) as GameObject;

      //set the coordinates for a new vector 3
      float randomX = UnityEngine.Random.Range (-12f,52f);
      float constantY = .01f;
      float randomZ = UnityEngine.Random.Range (-22f,32f);
      //set the zombies position equal to these new coordinates
      zombie.transform.position = new Vector3 (randomX, constantY, randomZ);

      //if the zombie gets positioned less than or equal to 3 scene units away from the camera we won't be able to shoot it
      //so keep repositioning the zombie until it is greater than 3 scene units away. 
      while (Vector3.Distance (zombie.transform.position, Camera.main.transform.position) <= 3) {
        
        randomX = UnityEngine.Random.Range (-12f,52f);
        randomZ = UnityEngine.Random.Range (-22f,32f);

        zombie.transform.position = new Vector3 (randomX, constantY, randomZ);
      }
    }
  }

}


