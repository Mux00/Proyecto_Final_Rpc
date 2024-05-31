using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EJ2 : MonoBehaviour
{
    private Vector3 posicionConstruccion;
    public NetworkObject edificio;
    private bool validacionConstruccion;
    private bool construccionRealizada;

    void Update()
    {
        //Cliente1 pulsa boton para rezalizar la construccion
        RealizarConstruccion(posicionConstruccion, edificio);
    }

    private void RealizarConstruccion(Vector3 posicionConstruccion, NetworkObject construccion)
    {
        //Se spawnea el objeto de la construccion para el cliente1
        Debug.Log("Pulsa espacio para construir!");
        if (Input.GetKeyDown("space"))
        {
            ConstruccionServerRpc(posicionConstruccion, construccion);
        }
    }

    [ClientRpc]
    void ConstruccionClientRpc()
    {
        Debug.Log("Posicion validada, se ha construido el edificio.");
    }

    [ServerRpc]
    void ConstruccionServerRpc(Vector3 posicion, NetworkObject construccion)
    {
        bool validacion = true;

        //Validacion de la posicion en la que se va a construir
        if (validacion)
        {
            validacionConstruccion = true;

            //El servidor spawnea el objeto edificio en la posicion indicada

            //Servidor notifica el resultado a los clientes por mensaje
            ConstruccionClientRpc();
        }
    }

}
