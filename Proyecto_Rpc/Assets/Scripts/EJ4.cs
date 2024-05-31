using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EJ4 : MonoBehaviour
{
    private int hpCliente1;
    private int maxHpCliente1;
    private bool validacionCura;

    private ulong idClienteOrigen;

    void Start()
    {
        hpCliente1 = 25;
        maxHpCliente1 = 50;
        validacionCura = true;
    }

    void Update()
    {
        Debug.Log("Pulsa espacio para solicitar curacion!");
        //Cliente1 solicita curacion
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SolicitarCuracion(idClienteOrigen, hpCliente1, maxHpCliente1);
        }
    }

    public void SolicitarCuracion(ulong jugadorOrigen, int hpCliente, int maxHpCliente)
    {
        //Llamada al servidor para solicitar curacion
        SolicitarCuracionServerRpc(jugadorOrigen, hpCliente, maxHpCliente);
    }

    public void RealizarCuracion(ulong jugadorOrigen, int hpCliente, int maxHpCliente)
    {
        int hpCurada = 10;

        RealizarCuracionServerRpc(jugadorOrigen, hpCliente, maxHpCliente, hpCurada);
    }


    [ServerRpc]
    void SolicitarCuracionServerRpc(ulong jugadorOrigen, int hpCliente, int maxHpCliente)
    {
        //Servidor verifica que Cliente1 pueda ser curado
        if(validacionCura) 
        {
            //Servidor notifica a los demas clientes que Cliente1 esta pidiendo curacion
            NotificarPeticionCuracionClientRpc(jugadorOrigen, hpCliente, maxHpCliente);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void RealizarCuracionServerRpc(ulong jugadorOrigen, int hpCliente, int maxHpCliente, int curacion)
    {
        //Servidor actualiza la vida de cliente1
        hpCliente = hpCliente + curacion;

        //Servidor notifica a los clientes el resultado de la curacion
        NotificarCuracionClienteRpc(hpCliente, maxHpCliente);
    }

    [ClientRpc]
    void NotificarPeticionCuracionClientRpc(ulong jugadorOrigen, int hpCliente, int maxHpCliente)
    {
        //Mensaje para los clientes
        Debug.Log("Un jugador esta a " + hpCliente + "/" + maxHpCliente + "hp y solictia curacion!");

        //Asumimos que el cliente2 pulsa un boton para curar a cliente1
        RealizarCuracion(jugadorOrigen, hpCliente, maxHpCliente);
    }

    [ClientRpc]
    void NotificarCuracionClienteRpc(int hpCliente, int maxHpCliente)
    {
        Debug.Log("Cura exitosa, cliente2 te ha curado y ahora tu vida actual ese de " + hpCliente + "/" + maxHpCliente);
    }
}
