using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    public float shootPower = 10.0f;
    public float extraRadius = 0.2f;
    private int numSegments = 100;
    private LineRenderer lineRenderer;
    private Rigidbody2D _playerRb;
    private Collider2D _ballCollider;
    private Rigidbody2D _ballRb;

    // Start is called before the first frame update
    void Start()
    {
        var materials =
        _playerRb = GetComponent<Rigidbody2D>();
        _ballCollider = GameObject.Find("Ball").GetComponent<Collider2D>();
        _ballRb = _ballCollider.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // Initialize the LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.015f;
        lineRenderer.endWidth = 0.015f;
        lineRenderer.positionCount = numSegments + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.sortingOrder = spriteRenderer.sortingOrder;
        lineRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        DrawShootingRadius();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        _playerRb.velocity = new Vector2(horizontalInput * playerSpeed, verticalInput * playerSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && IsPlayerNearBall())
        {
            Shoot(_ballCollider);
        }
    }

    bool IsPlayerNearBall()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius + extraRadius);
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

    void DrawShootingRadius()
    {
        float deltaTheta = (2f * Mathf.PI) / numSegments;
        float theta = 0f;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float radius = GetComponent<CircleCollider2D>().radius + extraRadius;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}