using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public GameObject pairCounterUI,livesCounterUI;
	
	public List<GameObject> Tiles;
	public Texture2D backTexture;
	public List<Texture2D> TileTextures;
	public Text panelText;

	public int foundPairs = 0;

	public float tilesShowTime, tileFlipTime;
	bool canClick = true;

	public int deathPairs, currentDeathPairs, winPairs;
	List <GameObject> CurrentCompare = new List<GameObject>();

	// Game resetter, triggers from ui button
	public void StartGame () {
		CurrentCompare = new List<GameObject>();
		ShuffleGo(Tiles);
		StartCoroutine(HideCards(Tiles, tilesShowTime));
		HideUIPanel();
		foundPairs = 0;
		currentDeathPairs = 0;
		
	}


	// methods to hide and show parent panel for UI text
	void HideUIPanel()
	{
		panelText.transform.parent.gameObject.SetActive(false);
	}

	void ShowUIPanel()
	{
		panelText.transform.parent.gameObject.SetActive(true);
	}

	// Knuth shuffle algorithm :: courtesy of Wikipedia :)
	public void ShuffleGo(List<GameObject> go)
	{
        
        for (int t = 0; t < go.Count; t++)
        {
            GameObject tmp = go[t];
            int r = Random.Range(t, go.Count);
            go[t] = go[r];
            go[r] = tmp;
        }

		SetTextureForAllList(go);
	}

	void SetTextureForAllList(List<GameObject> go)
	{
		for (int i = 0; i < go.Count; i++)
		{
			Tiles[i].GetComponent<Renderer>().material.mainTexture = TileTextures[i];
		}
	}

	// HideCards in someTime
	IEnumerator HideCards(List<GameObject> list, float seconds)
	{
		yield return new WaitForSeconds(seconds);

		foreach(GameObject go in list)
		{
			go.GetComponent<Renderer>().material.mainTexture = backTexture;
		}
		canClick = true;
	}
	
	// Update is called once per frame
	void Update () {		
		ClickHandler();
		CompareHandler();
		GameCycleHandler();
		UpdateUI();
	}

	// hardcode shit
	void UpdateUI()
	{
		var pairCounter = winPairs-foundPairs;
		this.pairCounterUI.GetComponent<Text>().text = "Pairs left: " + pairCounter;

		var deathCounter = deathPairs-currentDeathPairs;
		livesCounterUI.GetComponent<Text>().text = "Lives left: " + deathCounter;
	}

	// Game cycle, start end game and so on
	void GameCycleHandler()
	{
		if (currentDeathPairs >= deathPairs)
		{
			GameOver();
		}
		if (foundPairs >= winPairs)
		{
			YouWin();
		}
	}

	// Open card compare
	void CompareHandler()
	{
		// if two cards open compare it
		if (CurrentCompare.Count == 2){

			// if same, leave them open
			if (CurrentCompare[0].GetComponent<Renderer>().material.mainTexture.name == CurrentCompare[1].GetComponent<Renderer>().material.mainTexture.name)
			{
				foundPairs +=1;
				// if one of them (inside this cycle both are same) has death texture, add +1 to deathpairs counter
				if (CurrentCompare[0].GetComponent<Renderer>().material.mainTexture.name == "x"){
					currentDeathPairs+=1;
					foundPairs -=1;
				}
				// flush compare List in flip time
				CurrentCompare = new List<GameObject>();
			}

			// if not the same flip them back and flush list
			else 
			{
				canClick = false;
				StartCoroutine(HideCards(CurrentCompare, tileFlipTime));
				CurrentCompare = new List<GameObject>();
			}
		} 		
	}

	

	// add tile to compare and leave if compare is full
	void AddToCompare (GameObject tile)
	{
		if (CurrentCompare.Count >= 2){
			return;
		}

		CurrentCompare.Add(tile);		
		OpenClickedTile(tile);

	}


	// turn tile to see image
	void OpenClickedTile(GameObject tile)
	{
		for(int i = 0; i < Tiles.Count; i++)
		{
			if (tile.gameObject == Tiles[i])
			{
				tile.GetComponent<Renderer>().material.mainTexture = TileTextures[i];
			}
		}
	}

	void GameOver()
	{
		panelText.text = "YOU LOST THIS TIME, TRY AGAIN \n\n\n\n Click Start to begin game. \n You will need to find 15 pairs of cards. \n If you find pair of skulls, you'll be injured.\n Beware you have only two lives. ";
		Debug.Log("Game Over");
		ShowUIPanel();		
	}

	void YouWin()
	{
		panelText.text = "YOU WIN, CONGRATULATIONS! \n\n\n\n Click Start to begin game. \n You will need to find 15 pairs of cards. \n If you find pair of skulls, you'll be injured.\n Beware you have only two lives. ";
		Debug.Log("You Win");
		ShowUIPanel();
	}

	// Mouse click handler
	void ClickHandler()
	{
		// leave cycle if cant click
		if (!canClick) return;

		if (Input.GetMouseButtonDown(0))
		{	
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        	RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100))
			{
				AddToCompare(hit.transform.gameObject);
			}
		}
	}
}
