using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private Collider2D _goalCollider;
    private Collider2D _ballCollider;
    private GameManager _gameManager;

    [SerializeField] ParticleSystem _goalExplosion;

    // Start is called before the first frame update
    void Start()
    {
        _goalCollider = GetComponent<Collider2D>();
        _ballCollider = GameObject.Find("Ball").GetComponent<Collider2D>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // This method is called when another collider enters the trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == _ballCollider)
        {
            CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();

            if (transform.name == "RedGoal")
            {
                _gameManager.BlueScored = true;
                _goalExplosion.Play();
                StartCoroutine(cameraShake.Shake(2.0f, 0.2f));
            }
            else if (transform.name == "BlueGoal")
            {
                _gameManager.RedScored = true;
                _goalExplosion.Play();
                StartCoroutine(cameraShake.Shake(2.0f, 0.2f));

            }
        }
    }
}