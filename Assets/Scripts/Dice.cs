using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private const int NUMBER_OF_CHANGES=10;
    private int actualChangesLeft;
    private int lastPosition=0;
    private int position = 0;

    private void Start()
    {
        actualChangesLeft = NUMBER_OF_CHANGES;
    }
    public void DiceAnimation(int number)
    {
        StartCoroutine(DiceAnimationCoroutine(number));
    }

    IEnumerator DiceAnimationCoroutine(int number)
    {
        do
        {
            position = Random.Range(0, sprites.Length);
        } while (lastPosition == position);
        lastPosition = position;
        actualChangesLeft--;
        yield return new WaitForSeconds(0.2f);
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
}
