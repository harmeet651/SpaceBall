using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour {

	public GameObject PlayerUnitPrefab;

	// Use this for initialization
	void Start () {
		if( isLocalPlayer == false )
        {
            // This object belongs to another player.
            return;
        }
        CmdSpawnMyUnit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//////////////////////////// COMMANDS
	// Commands are special functions that ONLY get executed on the server.

	[Command]
	void CmdSpawnMyUnit()
	{
	    // We are guaranteed to be on the server right now.
	    GameObject go = Instantiate(PlayerUnitPrefab);

	    //go.GetComponent<NetworkIdentity>().AssignClientAuthority( connectionToClient );

	    // Now that the object exists on the server, propagate it to all
	    // the clients (and also wire up the NetworkIdentity)
	    NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
	}
}
