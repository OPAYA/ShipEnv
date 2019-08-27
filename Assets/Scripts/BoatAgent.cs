using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using MLAgents;
using System;


public class BoatAgent : Agent
{
    
    public GameObject goal;
    public GameObject agent;
    public GameObject enemy;
    

    private Rigidbody2D rbBoat;
    private Rigidbody2D rbEnemy;
    private Rigidbody2D rbGoal;


    private Vector2 boatInitPos;
    private Quaternion boatInitRot;
    private Vector2 enemyInitPos;
    private Quaternion enemyInitRot;
    private Vector2 goalInitPos;
    
    
    private double preDist;
    private double curDist;
    private double Dist;
    private float Dist1;
    private float steeringPower = .3000f;
    private float direction;
    private float speed = 300000;
    public float TargetDistance = 100f;
    
    private float steeringAmount;

    private int EnemyRand;

    private int count=0;

    private Vector3 ResetPosBoat;
    private Vector3 ResetPosEnemy;


    
    public override void InitializeAgent()
    {
        base.InitializeAgent();
        boatInitPos = agent.transform.position;
        boatInitRot = agent.transform.rotation;
        enemyInitPos = enemy.transform.position;
        enemyInitRot = enemy.transform.rotation;

        goalInitPos = goal.transform.position;
        preDist = curDist = Math.Sqrt(Math.Pow((goal.transform.position.x - agent.transform.position.x), 2) + Math.Pow((goal.transform.position.y - agent.transform.position.y), 2));
        rbBoat = agent.GetComponent<Rigidbody2D>();
        rbEnemy = enemy.GetComponent<Rigidbody2D>();
    }

    public override void CollectObservations()
    {
        AddVectorObs(rbBoat.velocity);
        AddVectorObs(rbBoat.angularVelocity);

        Dist = Math.Sqrt(Math.Pow((goal.transform.position.x - agent.transform.position.x), 2) + Math.Pow((goal.transform.position.y - agent.transform.position.y), 2));
        float Dist1 = Convert.ToSingle(Dist);

        AddVectorObs(Dist1);
        

        RaycastHit2D hit = Physics2D.Raycast(agent.transform.position, agent.transform.forward);
        AddVectorObs(hit.distance);
        

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        
        int action = Mathf.FloorToInt(vectorAction[0]);
        
        switch (action)
        {
            case 1:
                //agent.transform.position = agent.transform.position + 1f * Vector3.up;
                rbBoat.AddRelativeForce(Vector2.up *speed);
                
                break;
            case 2:
                //agent.transform.position = agent.transform.position + 1f * Vector3.down;
                rbBoat.AddRelativeForce(Vector2.down * speed);
                break;

            case 3:
                //agent.transform.position = agent.transform.position + 1f * Vector3.RotateTowards;
                direction = Mathf.Sign(Vector3.Dot(rbBoat.velocity, rbBoat.GetRelativeVector(Vector2.up)));
                rbBoat.rotation += -0.5f * steeringPower * rbBoat.velocity.magnitude * direction;
                //rbBoat.AddRelativeForce(Vector3.right * rbBoat.velocity.magnitude / 2);
                
                break;
            case 4:
                
                direction = Mathf.Sign(Vector3.Dot(rbBoat.velocity, rbBoat.GetRelativeVector(Vector2.up)));
                rbBoat.rotation += 0.5f * steeringPower * rbBoat.velocity.magnitude * direction;
               // rbBoat.AddRelativeForce(Vector3.right * rbBoat.velocity.magnitude / 2);
                break;
        }
        
        curDist = Math.Sqrt(Math.Pow((goal.transform.position.x - agent.transform.position.x), 2) + Math.Pow((goal.transform.position.y - agent.transform.position.y),2));
        float calDist = Convert.ToSingle(preDist - curDist);
        AddReward(calDist/50);
        

        Ray2D ray = new Ray2D(agent.transform.position, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10f);
        Debug.DrawRay(ray.origin, ray.direction*10f, Color.red);
        //Debug.Log(hit.distance);
        



    }
    private void OnCollisionEnter2D(Collision2D collision)

    {
        
        if (collision.collider.tag == "stone")
        {
           
            SetReward(-0.1f);
            count = count + 1;
            if (count == 5)
            {
                SetReward(-0.5f);
                Done();
                count = 0;
            }
            //Done();
        }
        if (collision.collider.tag == "goal")
        {
            
            SetReward(1f);
            Done();
        }
    }

    public override void AgentReset()
    {
        agent.transform.position = boatInitPos;
        agent.transform.rotation = boatInitRot;
     

        rbBoat.velocity = Vector2.zero;
        rbBoat.angularVelocity =0f;
        

        preDist = curDist = Math.Sqrt(Math.Pow((goal.transform.position.x - agent.transform.position.x), 2) + Math.Pow((goal.transform.position.y - agent.transform.position.y), 2));

    }

    
}
