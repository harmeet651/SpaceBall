using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionController : NetworkBehaviour {

	public GameObject PlayerUnitPrefab;
	private TileManager tileManager;

	static int NoOfPlayers = 0;

	[SyncVar]
	public int playerNumber;

	// Use this for initialization
	void Start () {
		string playerName;
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
	    NoOfPlayers++;
	    Debug.Log("Number of players now is: " + NoOfPlayers);
	    GameObject go = Instantiate(PlayerUnitPrefab);
	    go.name = "Player" + NoOfPlayers;

	    //go.GetComponent<NetworkIdentity>().AssignClientAuthority( connectionToClient );

	    // Now that the object exists on the server, propagate it to all
	    // the clients (and also wire up the NetworkIdentity)
	    NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
	    //RpcSetPlayerName ("Player" + NoOfPlayers);
	    playerNumber = NoOfPlayers;
	}

	// [ClientRpc]
	// public void RpcSetPlayerName(string playerName){
		
	// 		tileManager = GameObject.FindWithTag("TileManager").GetComponent<TileManager>();
 //        	tileManager.setPlayerName(playerName);
		
	// }

}
