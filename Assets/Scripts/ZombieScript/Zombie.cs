
using UnityEngine;
using System.Collections;

public class Zombie: MonoBehaviour 
{
 
  public GameObject ScoreText;

  private Transform goal;
  private UnityEngine.AI.NavMeshAgent agent;
  private Animation animation;
  private bool died;

  private ScoreSc scriptSc;


  void Start() {

    died = false;
    goal = Camera.main.transform;
    agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    animation = GetComponent<Animation>();

    //set the zombie's desination equal to the player's position 
    agent.destination = goal.position;

    animation.Play("Z_Walk_InPlace");
  }


  void Update() 
  {
    if (!died) 
    {
      goal = Camera.main.transform;
    }
    else
    {
      goal = agent.transform;
    }
    agent.destination = goal.position;

    // add attack
    if (!died && Vector3.Distance(agent.transform.position, Camera.main.transform.position) <= 3.1f)
    {
      animation.Play("Z_Attack"); 
    } 
    if (!died && Vector3.Distance(agent.transform.position, Camera.main.transform.position) >= 3.2f) 
    {
      animation.Play("Z_Walk_InPlace"); 
    }

  }

  // update score
  void UpdateScore()
  {
    ScoreText =  GameObject.Find("ScoreValue");
    // ui, score
    scriptSc = ScoreText.GetComponent<ScoreSc>();
    scriptSc.scoreValue += 5;
  }


  //when shooting on zombie
  void OnTriggerEnter(Collider col)
  {
    if(col.gameObject.CompareTag("bullet"))
    {
      //first disable the zombie's collider so multiple collisions cannot occur
      GetComponent<CapsuleCollider>().enabled = false;

      // shoot on zombie
      Destroy(col.gameObject);
      agent.destination = gameObject.transform.position;
      animation.Stop();
      animation.Play("Z_FallingBack");
      died = true;
      //play died music
      GetComponent<AudioSource>().Play();

      //instantiate a blood
      GameObject blood = Instantiate(Resources.Load("blood", typeof(GameObject))) as GameObject;
      blood.transform.position = gameObject.transform.position;

      //destroy this zombie and blood in six seconds.
      Destroy(gameObject, 6);
      Destroy(blood, 6);
      // update score
      UpdateScore();
      
      //instantiate a new zombie
      GameObject zombie = Instantiate(Resources.Load("zombie", typeof(GameObject))) as GameObject;

      //random zombie position
      float randomX = UnityEngine.Random.Range(-12f, 52f);
      float constantY = 0.01f;
      float randomZ = UnityEngine.Random.Range(-22f, 32f);
      zombie.transform.position = new Vector3(randomX, constantY, randomZ);

      //if the zombie gets positioned less than or equal to 3 scene units away from the camera we won't be able to shoot it
      //so keep repositioning the zombie until it is greater than 3 scene units away. 
      while (Vector3.Distance(zombie.transform.position, Camera.main.transform.position) <= 3) 
      {
        randomX = UnityEngine.Random.Range(-12f,52f);
        randomZ = UnityEngine.Random.Range(-22f,32f);
        zombie.transform.position = new Vector3(randomX, constantY, randomZ);
      }
    }
  }
}


