using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class _Controller : Agent
{
   public float speed = 2.0f;
   public Rigidbody rb;
   float Continuous;
   public GameObject S_goal;
   public GameObject L_goal;
   public GameObject Obstacle;
   public Vector3 Init_Pos;
   string sceneName;
   public float forceMultiplier = 2.0f;
   // int MaxStep;
   private void Awake()
   {
        rb = GetComponent<Rigidbody>();
        Init_Pos = transform.position;
        sceneName = "Basic";
   }

   // private void FixedUpdate()
   // {
   //    if (Input.GetKey(KeyCode.A))
   //    {
   //       rb.AddForce(speed * Vector3.left);
   //    }
   //    if (Input.GetKey(KeyCode.D))
   //    {
   //       rb.AddForce(speed * Vector3.right);
   //    }
   // }  
   // public override void OnActionReceived(ActionBuffers actions)
   // {
   //    AddReward(-0.001f);
   //    var Discrete = actions.DiscreteActions[0];
   //    //float Continuous = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
      
   //    //Debug.Log("Discrete: " + Discrete);
   //   // Debug.Log("Continuous: "+Continuous);
   //   switch (Discrete)
   //   {
   //    case 0: break;
   //    case 2: rb.AddForce(speed * Vector3.left); Debug.Log("Left"); break;
   //    case 1: rb.AddForce(speed * Vector3.right); Debug.Log("Right"); break;
      
   //   }
   // }
   public override void OnActionReceived(ActionBuffers actionBuffers)
   {
    // Actions, size = 2
    Vector3 controlSignal = Vector3.zero;
    controlSignal.x = actionBuffers.ContinuousActions[0];
    controlSignal.z = actionBuffers.ContinuousActions[1];
    rb.AddForce(controlSignal * forceMultiplier);

   //  // Rewards
   //  float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

   //  // Reached target
   //  if (distanceToTarget < 1.42f)
   //  {
   //      SetReward(1.0f);
   //      EndEpisode();
   //  }

   //  // Fell off platform
   //  else if (this.transform.localPosition.y < 0)
   //  {
   //      EndEpisode();
   //  }
}

   public override void Heuristic(in ActionBuffers actionsOut)
   {
      // var discreateActionsOut = actionsOut.DiscreteActions;

      // if(Input.GetKey(KeyCode.D))
      // {
      //    discreateActionsOut[0] = 1;
      // }
      // else if (Input.GetKey(KeyCode.A))
      // {
      //    discreateActionsOut[0] = 2;
      // }
      var continuousActionsOut = actionsOut.ContinuousActions;
      continuousActionsOut[0] = Input.GetAxis("Horizontal");
      continuousActionsOut[1] = Input.GetAxis("Vertical");
   }
   public override void CollectObservations(VectorSensor sensor)
   {
      sensor.AddObservation(gameObject.transform.position.x);
      sensor.AddObservation(gameObject.transform.position.z);
      sensor.AddObservation(S_goal.transform.position.x);
      sensor.AddObservation(S_goal.transform.position.z);
      sensor.AddObservation(L_goal.transform.position.x);
      sensor.AddObservation(L_goal.transform.position.z);
      sensor.AddObservation(Obstacle.transform.position.x);
      sensor.AddObservation(Obstacle.transform.position.z);
      

      // sensor.AddObservation(S_goal.transform.position.x - gameObject.transform.position.x);
      // sensor.AddObservation(L_goal.transform.position.x - gameObject.transform.position.x);

      sensor.AddObservation(rb.velocity.x);
      sensor.AddObservation(rb.velocity.z);
      sensor.AddObservation(rb.angularVelocity.x);
      sensor.AddObservation(rb.angularVelocity.z);
      sensor.AddObservation(rb.rotation.x);
      sensor.AddObservation(rb.rotation.z);
      sensor.AddObservation(StepCount/MaxStep);
   }

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.CompareTag("Large_Ball"))
      {
         //Debug.Log("Large_Ball");
         AddReward(10f);
         EndEpisode();
         SceneManager.LoadScene(sceneName);
      }
      if (collision.gameObject.CompareTag("Small_Ball"))
      {
         //Debug.Log("Small_Ball");
         AddReward(1f);
         EndEpisode();
         SceneManager.LoadScene(sceneName);

      }
      if (collision.gameObject.CompareTag("Red_Ball"))
      {
         SetReward(-1f);
         EndEpisode();
         SceneManager.LoadScene(sceneName);
      }
      if (collision.gameObject.CompareTag("wall"))
      {
         SetReward(-1f);
         EndEpisode();
         SceneManager.LoadScene(sceneName);
      }
   }

      // public override void OnEpisodeBegin()
      // {
      //    transform.position = Init_Pos;
      // }
}
