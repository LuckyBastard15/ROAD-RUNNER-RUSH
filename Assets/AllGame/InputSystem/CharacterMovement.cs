using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText; // Aseg�rate de tener una referencia a tu TextMeshProUGUI
    public float maxMovementL;
    public float maxMovementR;
    public TerrainGenerator terrainGenerator; // Aseg�rate de tener una referencia a tu generador de terreno

    private int score = 0;
    private bool isHopping = false;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        // Configurar acciones de movimiento
        playerInputActions.CharacterControls.MoveForward.performed += _ => MoveForward();
        playerInputActions.CharacterControls.MoveLeft.performed += _ => MoveLeft();
        playerInputActions.CharacterControls.MoveRight.performed += _ => MoveRight();
        playerInputActions.CharacterControls.MoveBackward.performed += _ => MoveBackward();
    }

    private void OnEnable()
    {
        playerInputActions.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.CharacterControls.Disable();
    }

    private void Update()
    {
        scoreText.text = "Score: " + score;
    }

    private void MoveForward()
    {
        if (!isHopping)
        {
            float zDiference = 0;
            if (transform.position.z % 1 != 0)
            {
                zDiference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            MoveCharacter(new Vector3(1, 0, zDiference));
        }
    }

    private void MoveLeft()
    {
        if (!isHopping && transform.position.z <= maxMovementL)
        {
            MoveCharacter(new Vector3(0, 0, 1));
        }
    }

    private void MoveRight()
    {
        if (!isHopping && transform.position.z >= maxMovementR)
        {
            MoveCharacter(new Vector3(0, 0, -1));
        }
    }

    private void MoveBackward()
    {
        if (!isHopping)
        {
            float distanceToBackEdge = terrainGenerator.lastRemovedTerrainPos.x + 4;
            if (transform.position.x > distanceToBackEdge)
            {
                MoveCharacter(new Vector3(-1, 0, 0));
            }
            else
            {
                Debug.Log("No hay suficiente terreno detr�s para moverse hacia atr�s.");
            }
        }
    }

    private void MoveCharacter(Vector3 direction)
    {
        // Implementa tu l�gica de movimiento aqu�
    }
}
