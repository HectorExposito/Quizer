using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsAnimations : MonoBehaviour
{
    private RectTransform buttonChildTransform;
    private Vector3 originalPosition;
    private Vector3 scale;
    private bool makingBigger;
    private bool mouseExited;
    private float movingDistance;
    private void Start()
    {
        buttonChildTransform = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        scale = new Vector3(1.1f,1.1f,1.1f);
        makingBigger = true;
        if (gameObject.transform.parent.GetComponent<GridLayout>() != null)
        {
            movingDistance = gameObject.transform.parent.GetComponent<GridLayout>().cellSize.y / 11;
        }
        else
        {
            movingDistance = gameObject.GetComponent<RectTransform>().sizeDelta.y / 11;
        }
        
    }
    public void OnMouseOver()
    {
        mouseExited = false;
        StartCoroutine(ButtonZooming());
    }

    private IEnumerator ButtonZooming()
    {
        if (makingBigger)
        {
            gameObject.transform.localScale =
                new Vector3(gameObject.transform.localScale.x + 0.01f, gameObject.transform.localScale.y + 0.01f, 1);
            if (gameObject.transform.localScale.x >= scale.x)
            {
                makingBigger = false;
            }
        }
        else
        {
            gameObject.transform.localScale =
                new Vector3(gameObject.transform.localScale.x - 0.01f, gameObject.transform.localScale.y - 0.01f, 1);
            if (gameObject.transform.localScale == Vector3.one)
            {
                makingBigger = true;
            }
        }
        yield return new WaitForSeconds(0.05f);
        if (!mouseExited)
        {
            StartCoroutine(ButtonZooming());
        }
    }

    public void OnMouseExit()
    {
        gameObject.transform.localScale = Vector3.one;
        makingBigger = true;
        mouseExited = true;
    }

    public void OnMouseDown()
    {
        originalPosition = buttonChildTransform.position;
        buttonChildTransform.position = 
            new Vector3(buttonChildTransform.position.x, buttonChildTransform.position.y-7f, buttonChildTransform.position.z);

    }

    public void OnMouseUp()
    {
        buttonChildTransform.position = originalPosition;
        gameObject.transform.localScale = Vector3.one;
    }
}
