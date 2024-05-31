using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EJ3 : MonoBehaviour
{
    public NetworkObject objetoTradeable;
    private bool validacionIntercambio;
    private bool peticionAceptada;

    private ulong idOrigen;
    private ulong idDestino;

    private int objetosEnInventario;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref idOrigen);
        serializer.SerializeValue(ref idDestino);
    }

    void Update()
    {
        Debug.Log("Pulsa espacio para disparar!");

        objetosEnInventario = 0;
        validacionIntercambio = true;
        peticionAceptada = true;
        Debug.Log("Objetos en inventario: " + objetosEnInventario);

        Debug.Log("Pulsa espacio para realizar la peticion de intercambio!");
        //Cliente1 realiza llamada al servidor para validar el intercambio
        if (Input.GetKeyDown("space"))
        {
            RealizarPeticionIntercambio(objetoTradeable, idOrigen, idDestino);
        }
    }

    public void RealizarPeticionIntercambio(NetworkObject objeto, ulong jugadorOrigen, ulong jugadorDestino)
    {
        ProcesarIntercambioServerRpc(objeto, jugadorOrigen, jugadorDestino);
    }

    public void AceptarPeticionIntercambio(NetworkObject objeto)
    {
        //Si se acepta, se llama al servido para realizar el intercambio
        if (peticionAceptada)
        {
            RealizarIntercambioServerRpc(objeto);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ProcesarIntercambioServerRpc(NetworkObject objeto, ulong jugadorOrigen, ulong jugadorDestino)
    {
       //Servidor valida el intercambio y se actualizan los inventarios
       if (validacionIntercambio)
       {

            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { jugadorDestino }
                }
            };

            //Servidor notifica al cliente objetivo de la peticion de intercambio
            NotificarSolicitudClientRpc(objeto, jugadorOrigen, clientRpcParams);
       } 
       else
       {
            Debug.Log("ERROR: Intercambio no realizado");
       }
    }

    [ServerRpc(RequireOwnership = false)]
    void RealizarIntercambioServerRpc(NetworkObject objeto)
    {
        //Servidor actualizan los inventarios
        objetosEnInventario++;

        //Servidor notifica a los clientes del resultado del intercambio
        NotificarResultadoIntercambioClientRpc(objetosEnInventario);
    }

    [ClientRpc]
    void NotificarSolicitudClientRpc(NetworkObject objeto, ulong jugadorOrigen, ClientRpcParams clientRpcParams = default)
    {
        //Se notifica al cliente objetivo la solicitud de intercambio
        Debug.Log("Has recibido una peticion de intercambio, ¿Desea aceptar?");

        //Cliente2 acepta el intercambio
        AceptarPeticionIntercambio(objeto);
    }

    [ClientRpc]
    void NotificarResultadoIntercambioClientRpc(int objetosInventario)
    {
        //Se notifica a los clientes 
        Debug.Log("Intercambio exitoso, ahora tienes: " + objetosEnInventario + " objetos en tu inventario;");
    }
}
