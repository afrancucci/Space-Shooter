using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RestartText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
       _scoreText.text = "Score: 0";
       _GameOverText.gameObject.SetActive(false);
       _RestartText.gameObject.SetActive(false);
       _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

       if (_gameManager == null)
       {
         Debug.LogError("GameManager is NULL");
       }
    }

    public void UpdateScore(int PlayerScore) 
    {
        _scoreText.text = "Score: " + PlayerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        _GameOverText.gameObject.SetActive(true);
        _RestartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {

        while (true)
        {
            _GameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _GameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
