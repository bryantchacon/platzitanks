using System;
using UnityEngine;

//Esta es una clase propia del juego que es independiente de todas las que heredan de monobehaviour y por eso se indica Serializable para que se muestre en el inspector
[Serializable]
public class TankManager
{
    public Color playerColor;
    public Transform spawnPoint;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public string coloredPlayerText;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public int wins;


    private TankMovement movement;
    private TankShoot shoot;
    private GameObject canvas;


    public void Setup()
    {
        movement = instance.GetComponent<TankMovement>();
        shoot = instance.GetComponent<TankShoot>();
        //Obtiene el game object del canvas del tanque, que es donde estan el circulo de salud y la flecha de disparo, de los anteriores no obtiene el game object porque no son elementos visibles dentro del juego
        canvas = instance.GetComponentInChildren<Canvas>().gameObject;

        movement.playerNumber = playerNumber;
        shoot.playerNumber = playerNumber;

        //HTML enriquecido para que el texto del jugador sea de su color
        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

        //Nope
        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        //Nope too
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = playerColor;
        }
    }


    public void DisableControl()
    {
        movement.enabled = false;
        shoot.enabled = false;

        canvas.SetActive(false);
    }

    public void EnableControl()
    {
        movement.enabled = true;
        shoot.enabled = true;

        canvas.SetActive(true);
    }


    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
}