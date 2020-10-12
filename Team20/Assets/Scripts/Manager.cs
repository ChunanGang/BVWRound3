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
	public GameObject[] MinionsObjs;
	public GameObject finalBoss;
	public CanvasController canvas;

	// game logic 
	public int gameStage = 0; // -1: gameover page; 0: start menu, 1: playing video; 2: tutorial game; 3: video; 4: real game
	public bool gameStarted = false;
	public bool player1Ready = false;
	public bool player2Ready = false;
	public bool player1Dead = false;
	public bool player2Dead = false;

	private string ccurMoveDir;
	private bool movingToStageOne = false;
	private int skipVideoPress = 0; // press A 3 times to skip video

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
				if (AirConsole.instance.GetControllerDeviceIds().Count == 0)
					canvas.setGameInfo("No player connected");
				else if (AirConsole.instance.GetControllerDeviceIds().Count == 1)
					canvas.setGameInfo("Need one more player");
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
			// enough players reset  let them get ready
			if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
			{
				AirConsole.instance.SetActivePlayers(2);
				canvas.setGameInfo("Press A to get ready");
			}
			else
			{
				AirConsole.instance.SetActivePlayers(0);
				gameStarted = false;
				canvas.setGameInfo("PlayerLeft. Waiiting for more players");
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
				// handle inputs in differnet game stage
				if (gameStage == 0)
					stageZeroInput( data);
				else if (gameStage == 1)
					stageOneInput(data);
				else if (gameStage == 2 || gameStage == 4)
					inGameStageInput(active_player, data);
				else if (gameStage == -1)
					stageGameOverInput(data);
			}
		}
	}
	void stageGameOverInput(JToken data)
    {
		// --- attack button pressed --- //
		if ((string)data["element"] == "attackButton" && !movingToStageOne)
		{
			movingToStageOne = true;
			resetPlayers();
			canvas.moveToStageOne();
			//print("over to 1");
		}
	}

	void stageZeroInput(JToken data)
    {
		// --- attack button pressed --- //
		if ((string)data["element"] == "attackButton" && !movingToStageOne)
		{
			movingToStageOne = true;
			canvas.moveToStageOne();
		}
	}
	void stageOneInput(JToken data)
    {
		// --- attack button pressed --- //
		if ((string)data["element"] == "attackButton" )
		{
			skipVideoPress++;
			if (skipVideoPress == 6)
			{
				skipVideoPress = 0;
				canvas.skipVideo();
			}
		}
	}

	// for stage 2 and 4
	void inGameStageInput(int active_player, JToken data)
    {
		// stop if player dead
		if ((active_player == 0 && player1Dead) || (active_player == 1 && player2Dead))
			return;

		// --- attack button pressed --- //
		if ((string)data["element"] == "attackButton")
		{
			// use for player ready
			if (!gameStarted)
			{
				// ste to ready if was not ready
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

		// --- movepad pressed --- //
		else if ((string)data["element"] == "movepad")
		{
			// release
			if (!(bool)data["data"]["pressed"])
			{
				string value = data["data"].First.ToObject<string>();
				if (value == ccurMoveDir)
					players[active_player].stop();
			}
			// press -> movement
			else
			{
				ccurMoveDir = (string)data["data"]["key"];
				if (ccurMoveDir == "up")
					players[active_player].move(1);
				else if (ccurMoveDir == "down")
					players[active_player].move(-1);
				else if (ccurMoveDir == "left")
					players[active_player].move(-2);
				else if (ccurMoveDir == "right")
					players[active_player].move(2);

			}

		}
	}

	public void resetPlayers()
    {
		player1Ready = false;
		player2Ready = false;
		player1Dead = false;
		player2Dead = false;
		players[0].resetStatus();
		players[1].resetStatus();
		players[0].gameObject.SetActive(true);
		players[1].gameObject.SetActive(true);
	}
	public void resetMinions()
    {
		foreach(GameObject minions in MinionsObjs)
        {
			minions.SetActive(true);
			minions.GetComponent<MinionsController>().resetStatus();
        }
    } 
	public void resetBoss()
    {
		finalBoss.GetComponent<BossController>().resetStatus();
	}

	public void setGameOver()
    {
		gameStarted = false;
		movingToStageOne = false;
		canvas.setGameOver();
    }

	public void setPlayerDead(int playerID)
    {
		if (playerID == 1)
			player1Dead = true;
		else if (playerID == 2)
			player2Dead = true;
		// game over
		if(player1Dead && player2Dead)
        {
			setGameOver();
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
