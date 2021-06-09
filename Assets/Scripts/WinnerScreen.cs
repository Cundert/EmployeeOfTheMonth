using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelloWorld;

public class WinnerScreen : MonoBehaviour
{
	Text screenText;
	int totalPlayers = 0;
	int deadPlayers = -999;

	public void AddAlivePlayer()
	{
		totalPlayers++;
	}

	public void AddDeadPlayer()
	{
		if (deadPlayers == -999)
			deadPlayers = 1;
		else
			deadPlayers++;
	}

	string DetermineWinner()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		string winner = "";
		foreach (GameObject player in players)
		{
			HelloWorldPlayer p = player.GetComponent<HelloWorldPlayer>();
			if (!p.isDead)
			{
				winner = p.PlayerName.Value;
				break;
			}
		}
		return winner;
	}

	// Start is called before the first frame update
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		screenText = GetComponent<Text>();
		screenText.text = "";
	}

    // Update is called once per frame
    void Update()
    {
        if (deadPlayers != -999)
		{
			if (totalPlayers == deadPlayers + 1)
			{
				string winnerPlayer = DetermineWinner();
				screenText.text = string.Format("{0} is the employee of the month!", winnerPlayer);
			}
		}
    }
}
