using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Properties")]
    public float speed = 2/0f;
    public Boundary boundaries;
    public float verticalPosition;
    public float verticalSpeed = 10.0f;
    public bool usingMobileInput = false;
    public ScoreManager scoreManager;

    [Header("Bullet Properties")]
    public Transform bulletSpawnPoint;
    public float fireRate = 0.2f;
    public BulletManager bulletManager;

    private Camera camera;

    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();

        camera = Camera.main;

        usingMobileInput = Application.platform == RuntimePlatform.Android ||
                            Application.platform == RuntimePlatform.IPhonePlayer;

        scoreManager = FindObjectOfType<ScoreManager>();

        InvokeRepeating("FireBullets", 0.0f, fireRate);
    }


    void Update()
    {
        if (usingMobileInput)
        {
            MobileInput();
        }
        else
        {
            ConventionalInput();
        }

        Move();

        if (Input.GetKeyDown(KeyCode.K))
        {
            scoreManager.AddPoints(10);
        }
    }

    public void MobileInput()
    {
        foreach (var touch in Input.touches)
        {
            var destination = camera.ScreenToWorldPoint(touch.position);
            transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * speed);
        }
    }
    public void ConventionalInput()
    {
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        transform.position += new Vector3(x, 0.0f, 0.0f);
    }

    public void Move()
    {
        float clampledPosition = Mathf.Clamp(transform.position.x, boundaries.min, boundaries.max);
        transform.position = new Vector2(clampledPosition, verticalPosition);
    }

    void FireBullets()
    {
        var bullet = bulletManager.GetBullet(bulletSpawnPoint.position, BulletDirection.UP);
    }
}
