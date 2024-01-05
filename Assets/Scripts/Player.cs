using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private const int MAX_MOVEMENT=7;
    private int movement;
    private Collider2D playerCollider;
    [SerializeField]private Square playerBase;
    private Square nextSquare;
    private Square actualSquare;
    [SerializeField]private float movementDuration;
    private bool isMoving;
    private bool waitingForDiceAnimation;
    [SerializeField] private Dice dice;
    // Start is called before the first frame update
    void Start()
    {
        movement = 0;
        playerCollider = GetComponent<Collider2D>();
        transform.position = playerBase.gameObject.transform.position;
        nextSquare = playerBase.GetNextSquare();
        Debug.Log(nextSquare.gameObject.transform.position);
    }

    public void ThrowDice()
    {
        movement = Random.Range(1,MAX_MOVEMENT);
        waitingForDiceAnimation = true;
        dice.DiceAnimation(movement);
        MovePlayer();
    }

    private void MovePlayer()
    {
        isMoving = true;
        StartCoroutine(MovePlayerCoroutine());
    }

    IEnumerator MovePlayerCoroutine()
    {
        if (waitingForDiceAnimation)
        {
            yield return new WaitForSeconds(2);
            waitingForDiceAnimation = false;
        }
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        while (timeElapsed < movementDuration)
        {
            transform.position = Vector3.Lerp(startPosition, nextSquare.gameObject.transform.position, timeElapsed / movementDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = nextSquare.gameObject.transform.position;
        actualSquare = nextSquare;
        nextSquare = nextSquare.GetNextSquare();
        movement--;
        if (movement != 0)
        {
            MovePlayer();
        }
        else
        {
            isMoving = false;
            actualSquare.SquareAction();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&!isMoving)
        {
            ThrowDice();
        }
    }
}
