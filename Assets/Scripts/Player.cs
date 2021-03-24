using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab; 
    [SerializeField]
    private GameObject _shieldsPrefab;
    [SerializeField]
    private GameObject _rightEnginePrefab;
    [SerializeField]
    private GameObject _leftEnginePrefab;
    [SerializeField]
    private AudioClip _laserAudio;
    private AudioSource _audioSource;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;
    private SpawnManager _spawnManager;
    private UIManager _UIManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL.");
        }

        if (_UIManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source On the Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserAudio;
        }
    }

    void Update()
    {
        CalculateMovement();    

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement() {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        
        // limito y fra 0 e -3.8f
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3 (-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3 (11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3 (0, 0.8f, 0) , Quaternion.identity);
        }
        
        _audioSource.Play();   
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldsPrefab.SetActive(false);
            return;
        }

        _lives -= 1;

        if (_lives == 2)
        {
            _rightEnginePrefab.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEnginePrefab.SetActive(true);
        }
        _UIManager.UpdateLives(_lives);


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _UIManager.GameOver();
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine (TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldBoostActive()
    {
        _isShieldActive = true;
        _shieldsPrefab.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _UIManager.UpdateScore(_score);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {    
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {    
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
}


