using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingPanel : MonoBehaviour
{

    [SerializeField] GameObject image;
    private bool connected;
    [SerializeField]private float rotationSpeed;
    public void ConnectingToServer()
    {
        connected = false;
        image.transform.rotation = new Quaternion(0, 0, 0, 0);
        this.gameObject.SetActive(true);
    }

    public void AlreadyConnected()
    {
        connected = true;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!connected)
        {
            image.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
    }
}
