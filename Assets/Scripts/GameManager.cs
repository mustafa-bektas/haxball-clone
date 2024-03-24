using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int RedScore;
    public int BlueScore;
    public bool RedScored;
    public bool BlueScored;
    public bool gameEnded;
    public bool postGoal;

    private GameObject _ball;
    private GameObject _player;
    private GameObject _enemy;
    private TextMeshProUGUI _scoreTableText;
    private TextMeshProUGUI _goalScoredText1;
    private TextMeshProUGUI _goalScoredText2;
    private TextMeshProUGUI _goalScoredText3;
    private TextMeshProUGUI _gameOverText; // Reference to the GameOverText object

    private AudioSource _redGoalAudio;
    private AudioSource _blueGoalAudio;

    void Start()
    {
        RedScore = 0;
        BlueScore = 0;
        _ball = GameObject.Find("Ball");
        _scoreTableText = GameObject.Find("ScoreTableText").GetComponent<TextMeshProUGUI>();
        _goalScoredText1 = GameObject.Find("GoalScoredText_1").GetComponent<TextMeshProUGUI>();
        _goalScoredText2 = GameObject.Find("GoalScoredText_2").GetComponent<TextMeshProUGUI>();
        _goalScoredText3 = GameObject.Find("GoalScoredText_3").GetComponent<TextMeshProUGUI>();
        _goalScoredText1.enabled = false;
        _goalScoredText2.enabled = false;
        _goalScoredText3.enabled = false;
        _gameOverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>(); // Get the GameOverText component
        _gameOverText.enabled = false; // Hide the GameOverText when the game starts
        UpdateScoreText();
        _redGoalAudio = GameObject.Find("RedGoal").GetComponent<AudioSource>();
        _blueGoalAudio = GameObject.Find("BlueGoal").GetComponent<AudioSource>();
        Button resetGameButton = GameObject.Find("ResetGameButton").GetComponent<Button>();
        resetGameButton.onClick.AddListener(ResetGame);

        _player = GameObject.Find("Player");
        _enemy = GameObject.Find("Enemy");
    }

    void Update()
    {
        if(RedScore == 3 && !gameEnded)
        {
            Debug.Log("Red Wins!");
            gameEnded = true;
            StartCoroutine(ShowGameOverText("RED WINS!", Color.red));
            _ball.GetComponent<Renderer>().enabled = false;
        }
        else if(BlueScore == 3 && !gameEnded)
        {
            Debug.Log("Blue Wins!");
            gameEnded = true;
            StartCoroutine(ShowGameOverText("BLUE WINS!", Color.blue));
            _ball.GetComponent<Renderer>().enabled = false;
        }

        if (RedScored && !gameEnded)
        {
            Debug.Log("Red Scored!");
            _ball.GetComponent<Renderer>().enabled = false;
            _ball.GetComponent<Collider2D>().enabled = false;
            _ball.transform.position = new Vector3(0, 0, 0);
            _ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            RedScore++;
            _redGoalAudio.Play();
            StartCoroutine(ShowGoalScoredText(Color.red));
            Invoke("ResetBall", 2f);
            RedScored = false;
            UpdateScoreText();
        }
        else if (BlueScored && !gameEnded)
        {
            Debug.Log("Blue Scored!");
            _ball.GetComponent<Renderer>().enabled = false;
            _ball.GetComponent<Collider2D>().enabled = false;
            _ball.transform.position = new Vector3(0, 0, 0);
            _ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            BlueScore++;
            _blueGoalAudio.Play();
            StartCoroutine(ShowGoalScoredText(Color.blue));
            Invoke("ResetBall", 2f);
            BlueScored = false;
            UpdateScoreText();
        }
    }

    private void ResetGame()
    {
        RedScore = 0;
        BlueScore = 0;
        ResetBall();
        gameEnded = false;
        UpdateScoreText();
    }

    private void ResetBall()
    {
        if (!gameEnded)
        {
            _ball.transform.position = new Vector3(0, 0, 0);
            _ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _ball.GetComponent<Renderer>().enabled = true;
            _ball.GetComponent<Collider2D>().enabled = true;

            _player.transform.position = new Vector3(-5, 0, 0);
            _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            _enemy.transform.position = new Vector3(5, 0, 0);
            _enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void UpdateScoreText()
    {
        _scoreTableText.text = $"<color=red>Red {RedScore}</color><color=black>-</color><color=blue>{BlueScore} Blue</color>";
    }

    private IEnumerator ShowGameOverText(string message, Color color)
    {
        _gameOverText.text = message;
        _gameOverText.enabled = true;

        for (int i = 0; i < 3; i++)
        {
            _gameOverText.color = color;
            yield return new WaitForSeconds(1f);
            _gameOverText.color = Color.white;
            yield return new WaitForSeconds(1f);
        }

        _gameOverText.enabled = false;
    }

    private IEnumerator ShowGoalScoredText(Color color)
    {
        _goalScoredText1.enabled = true;
        _goalScoredText2.enabled = true;
        _goalScoredText3.enabled = true;

        for (int i = 0; i < 5; i++)
        {
            _goalScoredText1.color = color;
            _goalScoredText2.color = color;
            _goalScoredText3.color = color;
            yield return new WaitForSeconds(0.2f);
            _goalScoredText1.color = Color.white;
            _goalScoredText2.color = Color.white;
            _goalScoredText3.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }

        _goalScoredText1.enabled = false;
        _goalScoredText2.enabled = false;
        _goalScoredText3.enabled = false;
    }
}