using System;
using TMPro.Examples;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnemyController : MonoBehaviour
{
    public float playerSpeed = 3.0f;
    public float shootPower = 15.0f;
    public float shootingDistance = 10.0f; // Adjust this value to increase or decrease the shooting distance

    public Vector2 targetWaypoint;

    private float shootTimer = 0.0f;
    private float shootDelay = 1.0f;

    private Rigidbody2D _playerRb;
    private Rigidbody2D _ballRb;
    private Collider2D _ballCollider;

    private float angleInDegrees;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _ballCollider = GameObject.Find("Ball").GetComponent<Collider2D>();
        _ballRb = _ballCollider.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > shootDelay && CanShoot())
        {
            shootTimer = 0.0f;
            Shoot(_ballCollider);
        }

        targetWaypoint = CalculateTargetWaypoint();
        MoveToTargetWaypoint(targetWaypoint);
    }

    bool CanShoot()
    {
        if (!IsEnemyNearBall())
        {
            return false;
        }

        Vector2 shootDirection = (_ballCollider.transform.position - transform.position).normalized;

        if (WillShotHitBlueGoal(_ballCollider, shootDirection) || !IsEnemyFarFromRedGoalAndPlayer())
        {
            return false;
        }

        float randomFloat = UnityEngine.Random.Range(0f, 1f);

        if ((angleInDegrees >= 90 && angleInDegrees <= 110 && randomFloat <= 0.2) ||
            (angleInDegrees > 110 && angleInDegrees <= 130 && randomFloat <= 0.4) ||
            (angleInDegrees > 130 && angleInDegrees <= 150 && randomFloat <= 0.7) ||
            (angleInDegrees > 150 && angleInDegrees <= 180))
        {
            return true;
        }

        return false;
    }

    void MoveToTargetWaypoint(Vector2 targetWp)
    {
        float step = playerSpeed * Time.deltaTime;
        Vector2 aiPosition = transform.position;

        Vector2 walkDirection = ((Vector2)_ballRb.transform.position - aiPosition).normalized;
        var isWalkingTowardsOwnGoal = WillShotHitBlueGoal(_ballCollider, walkDirection);

        Vector2 bottomLineDirection = new Vector2(1, 0);

        float dotProduct = Vector2.Dot(walkDirection, bottomLineDirection);
        float angleInRadians = Mathf.Acos(dotProduct);
        angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        if (!isWalkingTowardsOwnGoal)
        {
            transform.position = Vector2.MoveTowards(aiPosition, targetWaypoint, step);
        }
        else
        {
            int randomChoice = UnityEngine.Random.Range(0, 2);
            if (randomChoice == 0)
            {
                transform.position = Vector2.MoveTowards(aiPosition, new Vector2(targetWaypoint.x, targetWaypoint.y-1f), step);
            }
            else
            {
                transform.position = Vector2.MoveTowards(aiPosition, new Vector2(targetWaypoint.x, targetWaypoint.y+1f), step);
            }
        }
    }

    Vector2 CalculateTargetWaypoint()
    {
        Vector2 redGoalPosition = GameObject.Find("RedGoal").transform.position;
        Vector2 ballPosition = _ballRb.position;

        Vector2 directionFromRedGoalToBall = (ballPosition - redGoalPosition).normalized;
        targetWaypoint = ballPosition + directionFromRedGoalToBall * 0.3f;

        return targetWaypoint;
    }

    bool WillShotHitBlueGoal(Collider2D ballCollider, Vector2 shootDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(ballCollider.transform.position, shootDirection);
        if (hit.collider != null)
        {
            return hit.collider.gameObject.name == "BlueGoal";
        }
        return false;
    }

    bool IsEnemyNearBall()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius + 0.1f);
        foreach (var col in colliders)
        {
            if (col.gameObject == _ballCollider.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void Shoot(Collider2D collision)
    {
        Vector2 shootDirection = (collision.transform.position - transform.position).normalized;
        _ballRb.AddForce(shootDirection * shootPower, ForceMode2D.Impulse);
    }

    bool IsEnemyFarFromRedGoalAndPlayer()
    {
        Vector2 redGoalPosition = GameObject.Find("RedGoal").transform.position;
        Vector2 playerPosition = GameObject.Find("Player").transform.position;
        Vector2 enemyPosition = transform.position;

        float distanceToRedGoal = Vector2.Distance(enemyPosition, redGoalPosition);
        float distanceToPlayer = Vector2.Distance(enemyPosition, playerPosition);

        return distanceToRedGoal < 13 || distanceToPlayer < 7;
    }
}