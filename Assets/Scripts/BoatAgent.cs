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
    private float steeringPower = .3000f;
    private float direction;
    private float speed = 30000;
    
    private float steeringAmount;

    private int EnemyRand;

    private Vector3 ResetPosBoat;
    private Vector3 ResetPosEnemy;


    
    public override void InitializeAgent()
    {
        boatInitPos = agent.transform.position;
        boatInitRot = agent.transform.rotation;
        enemyInitPos = enemy.transform.position;
        enemyInitRot = enemy.transform.rotation;

        goalInitPos = goal.transform.position;
        preDist = 100000;
        rbBoat = agent.GetComponent<Rigidbody2D>();
        rbEnemy = enemy.GetComponent<Rigidbody2D>();
    }

    public override void CollectObservations()
    {
        AddVectorObs(agent.transform.position.x);
        AddVectorObs(agent.transform.position.y);
        AddVectorObs(agent.transform.rotation.z);
        AddVectorObs(agent.transform.position.z);

        AddVectorObs(enemy.transform.position.x);
        AddVectorObs(enemy.transform.position.y);
        AddVectorObs(enemy.transform.rotation.z);
        AddVectorObs(enemy.transform.position.z);

        AddVectorObs(goal.transform.position.x);
        AddVectorObs(goal.transform.position.y);
        AddVectorObs(goal.transform.rotation.z);

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        
        int action = Mathf.FloorToInt(vectorAction[0]);
        
        switch (action)
        {
            case 1:
                rbBoat.AddRelativeForce(Vector3.up * speed);
                
                break;
            case 2:
                rbBoat.AddRelativeForce(Vector3.down * speed);
                break;

            case 3:
                
                direction = Mathf.Sign(Vector3.Dot(rbBoat.velocity, rbBoat.GetRelativeVector(Vector3.up)));
                rbBoat.rotation += -0.5f * steeringPower * rbBoat.velocity.magnitude * direction;
                rbBoat.AddRelativeForce(Vector3.right * rbBoat.velocity.magnitude / 2);
                
                break;
            case 4:
                
                direction = Mathf.Sign(Vector3.Dot(rbBoat.velocity, rbBoat.GetRelativeVector(Vector3.up)));
                rbBoat.rotation += 0.5f * steeringPower * rbBoat.velocity.magnitude * direction;
                rbBoat.AddRelativeForce(Vector3.right * rbBoat.velocity.magnitude / 2);
                break;
        }
        
        curDist = Math.Sqrt(Math.Pow((goal.transform.position.x - agent.transform.position.x), 2f) + Math.Pow((goal.transform.position.y - agent.transform.position.y),2));
        
        if (curDist < preDist)
        {
            AddReward(2f);
            preDist = curDist;
            //Debug.Log(preDist);
        }
        else if(curDist > preDist)
        {
            AddReward(-2f);
            //Debug.Log(curDist);
        }

        if (curDist < 2f) 
        {
            AddReward(10f);
            Done();
        }
        //if(agent.transform.position.x == rock.transform.position.x && agent)


        void OnCollisionEnter2D(Collision2D collision)

        {
            Debug.Log("insert");
            if (collision.collider.tag == "stone")
            {
                Debug.Log("Collision stone");
                SetReward(-5f);
                Done();
            }
            if (collision.gameObject.tag == "goal")
            {
                Debug.Log("Collsion goal");
                SetReward(10f);
                Done();
            }
        }

    }
    
    public override void AgentReset()
    {
        agent.transform.position = boatInitPos;
        agent.transform.rotation = boatInitRot;
        enemy.transform.position = enemyInitPos;
        enemy.transform.rotation = enemyInitRot;

        rbBoat.velocity = Vector2.zero;
       
        rbEnemy.velocity = Vector2.zero;

        preDist = 100000f;
        
    }

    
}
