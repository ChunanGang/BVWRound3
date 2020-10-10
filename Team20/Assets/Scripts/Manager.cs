using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class Manager : MonoBehaviour
{
	// ref
	public PlayerController [] players;
	public CanvasController canvas;

	// game logic 
	public bool gameStarted = false;
	public bool player1Ready = false;
	public bool player2Ready = false;

#if !DISABLE_AIRCONSOLE

	void Awake()
	{
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	/// <summary>
	/// We start the game if 2 players are connected and the game is not already running (activePlayers == null).
	/// 
	/// NOTE: We store the controller device_ids of the active players. We do not hardcode player device_ids 1 and 2,
	///       because the two controllers that are connected can have other device_ids e.g. 3 and 7.
	///       For more information read: http://developers.airconsole.com/#/guides/device_ids_and_states
	/// 
	/// </summary>
	/// <param name="device_id">The device_id that connected</param>
	void OnConnect(int device_id)
	{
		if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
		{
			if (AirConsole.instance.GetControllerDeviceIds().Count < 2)
			{
				canvas.setGameInfo("Need 2 players");
			}
			else
			{
				AirConsole.instance.SetActivePlayers(2);
				canvas.setGameInfo("Press A to get ready");
			}
		}
	}

	/// <summary>
	/// If the game is running and one of the active players leaves, we reset the game.
	/// </summary>
	/// <param name="device_id">The device_id that has left.</param>
	void OnDisconnect(int device_id)
	{
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
		if (active_player != -1)
		{
			// enough players reset let let them get ready
			if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
			{
				AirConsole.instance.SetActivePlayers(2);
				canvas.setGameInfo("Press A to get ready");
			}
			else
			{
				AirConsole.instance.SetActivePlayers(0);
				gameStarted = false;
				canvas.setGameInfo("PlayerLeft. Waiiting for more Plyers");
			}
			gameStarted = false;
			player1Ready = false;
			player2Ready = false;
		}
	}

	/// <summary>
	/// We check which one of the active players has moved the paddle.
	/// </summary>
	/// <param name="from">From.</param>
	/// <param name="data">Data.</param>
	/// The message if sent by controller.html
	void OnMessage(int device_id, JToken data)
	{
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
		// check player valid
		if (active_player != -1)
		{
			if (active_player == 0 || active_player == 1)
			{		
				// release
				if (!(bool)data["data"]["pressed"])
					players[active_player].stop();
				// press
				else
				{
					// attack button
					if ((float)data["data"]["attack"] == 1)
					{
						// use for player ready
						if (!gameStarted)
						{
							if (active_player == 0 && !player1Ready)
								player1Ready = true;
							else if (active_player == 1 && !player2Ready)
								player2Ready = true;
							// start game if both ready
							if (player1Ready && player2Ready)
							{
								gameStarted = true;
								canvas.setGameInfo("");
							}
						}
						// attck in game
						else
							players[active_player].attack();
					}
					// movement
					else
						players[active_player].move((float)data["data"]["move"]);
				}
				
			}
		}
	}


	void OnDestroy()
	{

		// unregister airconsole events on scene change
		if (AirConsole.instance != null)
		{
			AirConsole.instance.onMessage -= OnMessage;
		}
	}
#endif
}
