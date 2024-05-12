using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public TerrainGenerator terrainGanerator;
    [SerializeField] private Text scoreText;

    private int score = 0;
    private float lastXPosition;

    //private int score;
    private Animator animator;
    private bool isHopping;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x > lastXPosition)
        {
            score++;
            lastXPosition = transform.position.x;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        scoreText.text = "Score: " + score;

        if (!isHopping)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                float zDiference = 0;
                if (transform.position.z % 1 != 0)
                {
                    zDiference = Mathf.Round(transform.position.z) - transform.position.z;
                }
                MoveCharacter(new Vector3(1, 0, zDiference));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MoveCharacter(new Vector3(0, 0, 1));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveCharacter(new Vector3(0, 0, -1));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MoveCharacter(new Vector3(-1, 0, 0));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<CarScript>() != null)
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
        terrainGanerator.SpawnTerrain(false, transform.position);
    }
    public void FinishHop()
    {
        isHopping = false;
    }



}
