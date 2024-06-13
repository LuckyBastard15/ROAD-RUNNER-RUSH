using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Asegúrate de importar TextMesh Pro
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    [SerializeField] private TextMeshProUGUI scoreText; // Cambia a TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI highScoreText; // Cambia a TextMeshProUGUI

    private int score = 0;
    private int highScore = 0;
    private float lastXPosition;

    private Animator animator;
    private bool isHopping;

    [SerializeField] private float maxMovementL = 15.0f;
    [SerializeField] private float maxMovementR = -15.0f;
    public float minDistanceToTerrain = 1.0f;

    private bool isInvincible = false;
    private float invincibilityDuration = 5f;

    private PlayerInputActions playerInputActions;

    public GameObject restartMenu;

    void Start()
    {
        animator = GetComponent<Animator>();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.CharacterControls.MoveForward.performed += OnMoveForward;
        playerInputActions.CharacterControls.MoveLeft.performed += OnMoveLeft;
        playerInputActions.CharacterControls.MoveRight.performed += OnMoveRight;
        playerInputActions.CharacterControls.MoveBackward.performed += OnMoveBackward;
    }

    private void OnDisable()
    {
        playerInputActions.CharacterControls.MoveForward.performed -= OnMoveForward;
        playerInputActions.CharacterControls.MoveLeft.performed -= OnMoveLeft;
        playerInputActions.CharacterControls.MoveRight.performed -= OnMoveRight;
        playerInputActions.CharacterControls.MoveBackward.performed -= OnMoveBackward;
        playerInputActions.Disable();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "" + highScore;
    }

    private bool IsObstacleInDirection(Vector3 direction)
    {
        RaycastHit hit;
        float rayDistance = 1.0f;

        if (Physics.Raycast(transform.position, direction, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }

        if (Physics.Raycast(transform.position + Vector3.left * 0.5f, direction, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }

        if (Physics.Raycast(transform.position + Vector3.right * 0.5f, direction, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }

        if (Physics.Raycast(transform.position + Vector3.back * 0.5f, direction, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnMoveForward(InputAction.CallbackContext context)
    {
        if (!isHopping && !IsObstacleInDirection(Vector3.right))
        {
            float zDifference = 0;
            if (transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            MoveCharacter(new Vector3(1, 0, zDifference));
        }
    }

    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (!isHopping && transform.position.z <= maxMovementL && !IsObstacleInDirection(Vector3.forward))
        {
            MoveCharacter(new Vector3(0, 0, 1));
        }
    }

    private void OnMoveRight(InputAction.CallbackContext context)
    {
        if (!isHopping && transform.position.z >= maxMovementR && !IsObstacleInDirection(Vector3.back))
        {
            MoveCharacter(new Vector3(0, 0, -1));
        }
    }

    private void OnMoveBackward(InputAction.CallbackContext context)
    {
        Debug.Log("Intentando moverse hacia atrás."); // Mensaje de depuración

        if (!isHopping && !IsObstacleInDirection(Vector3.left))
        {
            float distanceToBackEdge = terrainGenerator.lastRemovedTerrainPos.x + 4;
            Debug.Log("Distancia al borde trasero: " + distanceToBackEdge); // Mensaje de depuración

            if (transform.position.x > distanceToBackEdge)
            {
                Debug.Log("Moviendo el personaje hacia atrás."); // Mensaje de depuración
                MoveCharacter(new Vector3(-1, 0, 0));
            }
            else
            {
                Debug.Log("No hay suficiente terreno detrás para moverse hacia atrás."); // Mensaje de depuración
            }
        }
        else
        {
            Debug.Log("No se puede mover hacia atrás debido a un obstáculo o el jugador está saltando."); // Mensaje de depuración
        }
    }


    private void FixedUpdate()
    {
        if (transform.position.x > lastXPosition)
        {
            score++;
            lastXPosition = transform.position.x;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            StartCoroutine(ActivateInvincibility());
            Destroy(other.gameObject);
        }
    }

    private IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Collider enemyCollider = enemy.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }
        }

        yield return new WaitForSeconds(invincibilityDuration);

        foreach (GameObject enemy in enemies)
        {
            Collider enemyCollider = enemy.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                enemyCollider.enabled = true;
            }
        }

        isInvincible = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<CarScript>() != null)
        {
            if (collision.collider.GetComponent<CarScript>().isLog)
            {
                transform.parent = collision.collider.transform;
            }
        }
        else
        {
            transform.parent = null;
        }
    }

    private void MoveCharacter(Vector3 difference)
    {
        animator.SetTrigger("Hop");
        isHopping = true;
        transform.position = (transform.position + difference);
        terrainGenerator.SpawnTerrain(false, transform.position);
    }

    public void FinishHop()
    {
        isHopping = false;
    }

    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }

    public void OnDestroy()
    {
        FindObjectOfType<MenuController>().ShowDeathMenu();
    }
}
