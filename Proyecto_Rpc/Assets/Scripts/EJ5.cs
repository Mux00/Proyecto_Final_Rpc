using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EJ5 : MonoBehaviour
{
    private Vector3 posicionCliente1;
    private Vector3 posicionCliente2;
    private int cliente2HP;
    private int cliente1DMG;

    ulong idOrigen;
    ulong idDestino;

    void Start()
    {
        cliente1DMG = 1;
        cliente2HP = 5;
    }

    void Update()
    {
        Debug.Log("Pulsa espacio para disparar!");
        //Cliente1 realiza un disparo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AtacarJugador(posicionCliente1, posicionCliente2, cliente1DMG);
        }
    }

    public void AtacarJugador(Vector3 posicion1, Vector3 posicion2, int dmg)
    {
        if (VerificarEnRango(posicion1, posicion2))
        {
            AplicarDa�oJugadorServerRpc(CalcularDa�oAtaque(dmg), cliente2HP);
        }
    }

    public bool VerificarEnRango(Vector3 posicion1, Vector3 posicion2)
    {
        //Se calcula con la distancia entre cliente1 y cliente2 para verificar si cliente1 esta en rango de da�ar a cliente2
        return true;
    }

    public int CalcularDa�oAtaque(int dmg)
    {
        //Se calcula el da�o que realizara el cliente1 al impactar con su ataque basado en sus estadisticas
        return dmg;
    }

    [ServerRpc(RequireOwnership = false)]
    void AplicarDa�oJugadorServerRpc(int dmg, int hpObjetivo)
    {
        //Servidor actuliza la vida de cliente 2
        hpObjetivo = hpObjetivo - dmg;

        //Servior notifica a los clientes que cliente2 ha recibido da�o
        NotificarDa�oAplicadoClientRpc(hpObjetivo);

    }

    [ClientRpc]
    void NotificarDa�oAplicadoClientRpc(int hpObjetivo)
    {
        Debug.Log("Cliente2 recibio da�o de Cliente1, su vida es ahora de: " + hpObjetivo);
    }
}
