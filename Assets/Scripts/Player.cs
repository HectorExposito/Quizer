using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private const int MAX_MOVEMENT=7;//Biggest number that the player can obtain from the dice +1
    private int movement;//Number obtained from the dice
    private Collider2D playerCollider;
    [SerializeField]private Square playerBase;
    private Square nextSquare;//Next square of the board
    private Square actualSquare;//The square the player is in
    [SerializeField]private float movementDuration;//Time the player takes to move from the actual square to the next
    private bool isMoving;//Tells if the player is currently moving or not
    private bool waitingForDiceAnimation;//Tells if the player is waiting for the dice animation to end
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
    //Generates a random number that represents the square the player is going to move
    public void ThrowDice()
    {
        movement = Random.Range(1,MAX_MOVEMENT);
        waitingForDiceAnimation = true;
        dice.DiceAnimation(movement);
        MovePlayer();
    }
    //Moves the player
    private void MovePlayer()
    {
        isMoving = true;
        StartCoroutine(MovePlayerCoroutine());
    }

    IEnumerator MovePlayerCoroutine()
    {
        //Waits until the dice animation is over
        if (waitingForDiceAnimation)
        {
            yield return new WaitForSeconds(2);
            waitingForDiceAnimation = false;
        }
        //Moves the player to the next square
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        while (timeElapsed < movementDuration)
        {
            transform.position = Vector3.Lerp(startPosition, nextSquare.gameObject.transform.position, timeElapsed / movementDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = nextSquare.gameObject.transform.position;
        //Changes the actual and next square
        actualSquare = nextSquare;
        nextSquare = nextSquare.GetNextSquare();
        movement--;
        //If the player has movements left it will continue moving, otherwise, it does the action of the square
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
