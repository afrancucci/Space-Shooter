using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0 = Triple Shot 1 = Speed 2 = Shields
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate (Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.tag == "Player")
            {
                Player _player = other.transform.GetComponent<Player>();
                
                AudioSource.PlayClipAtPoint(_clip, new Vector3(0,0,-10));
                
                if (_player != null)
                {
    
                    switch(powerupID)
                    {
                        case 0:
                            _player.TripleShotActive();
                            break;
                        case 1:
                            _player.SpeedBoostActive();
                            break;
                        case 2:
                            _player.ShieldBoostActive();
                            break;
                        default:
                            Debug.Log("Default Value");
                            break;
                    }
                }
                
                Destroy(this.gameObject);
            }
    }
}
