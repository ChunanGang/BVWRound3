using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class Manager : MonoBehaviour
{
	// Game data
	public PlayerController [] players;

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
			if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
			{
				startGame();
			}
			else
			{
				print( "NEED MORE PLAYERS");
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
			if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
			{
				startGame();
			}
			else
			{
				AirConsole.instance.SetActivePlayers(0);
				//ResetBall(false);
				print("PLAYER LEFT - will reste game and wait for enough players");
			}
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
		//print(data);
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
					// attack
					if ((float)data["data"]["attack"] == 1)
						players[active_player].attack();
					// movement
					else
						players[active_player].move((float)data["data"]["move"]);
				}
				
			}
		}
	}

	void startGame()
	{
		AirConsole.instance.SetActivePlayers(2);
		UpdateScoreUI();
	}


	void UpdateScoreUI()
	{
		
	}

	void FixedUpdate()
	{

		
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
