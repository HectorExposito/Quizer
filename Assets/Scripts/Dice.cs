using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;//All the sprites with the different numbers of the dice
    [SerializeField] private const int NUMBER_OF_CHANGES=10;//Changes that the dice will do before showing the final result
    private int actualChangesLeft;//Number of changes left 
    private int lastPosition=0;//Saves the last number that was shown by the dice in order to not repeat it
    private int position = 0;//Saves the position of the number that is going to be shown
    [SerializeField] private GameManager gameManager;
    private void Start()
    {
        actualChangesLeft = NUMBER_OF_CHANGES;
    }
    public void DiceAnimation(int number)
    {
        StartCoroutine(DiceAnimationCoroutine(number));
    }

    //It changes the sprite of the dice to give the impression of a dice rolling
    IEnumerator DiceAnimationCoroutine(int number)
    {
        //It selects a random position until its different to the last one used
        do
        {
            position = Random.Range(0, sprites.Length);
        } while (lastPosition == position);

        lastPosition = position;
        actualChangesLeft--;
        yield return new WaitForSeconds(0.2f);
        //If there is no changes left, it shows the real number
        if (actualChangesLeft != 0)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[lastPosition];
            DiceAnimation(number);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = sprites[number-1];
            actualChangesLeft = NUMBER_OF_CHANGES;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (hit.collider != null && hit.collider.transform == this.transform)
            {
                gameManager.ThrowDice();
            }
        }
    }
}
