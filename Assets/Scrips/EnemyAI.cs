using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private enum State{
        Roaming,
        ChaseTarget,
        GoingBackToStart,
    }
    private Vector3 target;

    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private State state;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    void Awake()
    {
        state = State.Roaming;
    }

    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void FixedUpdate()
    {
        if(path == null)
            return;

        target = roamPosition;
        
        float stopChaseDistance = 8f;
        switch(state){
            case State.Roaming:
                if(Vector3.Distance(transform.position, roamPosition) < nextWaypointDistance)
                    roamPosition = GetRoamingPosition();
                FindTarget();
                break;
            case State.ChaseTarget:
                roamPosition = Player.Instance.transform.position;
                if(Vector3.Distance(transform.position, Player.Instance.transform.position) > stopChaseDistance){
                    state = State.GoingBackToStart;
                    roamPosition = startingPosition;
                }
                break;
            case State.GoingBackToStart:
                if((Vector3.Distance(transform.position, startingPosition) < nextWaypointDistance) && state == State.GoingBackToStart)
                    state = State.Roaming;
                FindTarget();
                break;
        }

        if(currentWaypoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }else
            reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance){
            currentWaypoint++;
        }
    }

    void UpdatePath(){
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target, OnPathComplete);
    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }
    
    private Vector3 GetRoamingPosition(){
        return startingPosition + GetRandomDir() * Random.Range(1f, 5f);
    }

    public static Vector3 GetRandomDir(){
        return new Vector3(Random.Range(-1f, -1f), Random.Range(-1f, 1f)).normalized;
    }

    private void FindTarget(){
        float targetRange = 4f;
        if(Vector3.Distance(transform.position, Player.Instance.transform.position) < targetRange){
            roamPosition = Player.Instance.transform.position;
            state = State.ChaseTarget;
        }
    }
}
