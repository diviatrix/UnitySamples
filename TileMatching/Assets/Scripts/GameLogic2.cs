using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameLogic2 : MonoBehaviour {
	
	// Game defines
	public GameObject tilePrefab;
	public int fieldHeight;
	public int fieldWidth;
	public int deathPairs;
	public int lives;
	public Sprite deathSprite,coverSprite;
	public Text panelText;

	// UI elements
	public GameObject pairCounterUI,livesCounterUI;	

	// privates
	private bool isPlaying = false;
	private float cameraSize;
	private int needDoubles;
	private bool canClick = true;
	private List <GameObject> CurrentCompare = new List<GameObject>();
	private int foundPairs;	
	private int winPairs;
	private int currentLives;
	private int currentDeathPairs;
	private List<GameObject> tileMap;
	private List<Sprite> textureList;
	

	// Use this for initialization
	void Start () 
	{
		// Delete this row before start
		var lol = new README();
		lol.Start();


		// load textures to List on game start
		LoadTextures();	
		StartGame();	
		HideUIPanel();						
	}

	public void StartGame()
	{	
		HideUIPanel();

		GeneratePlayField();

		// shuffle basic texture list to randomize for each game
		ShuffleTextures(textureList);	
		FillPlayField();

		StartCoroutine(ShowCards(tileMap, 0));
		StartCoroutine(HideCards(tileMap, 5));
		isPlaying = true;
		
	}

	void FlushGame()
	{
		// flush field
		foreach(GameObject go in tileMap)
		{
			Destroy(go);
		}
		tileMap = new List<GameObject>();

		// flush pairs
		currentDeathPairs = 0;	
		foundPairs = 0;

		// stop playing
		isPlaying = false;
	}


	// this fill playfield tiles with with images and params
	void FillPlayField()
	{		

		// prepare texture list
		int tileCount = fieldWidth * fieldHeight;

		// this is how many normal pair we need to generate
		winPairs = tileCount/2 - deathPairs;

		// debug for textures  count in folder.
		if(textureList.Count < winPairs)
		{
			Debug.LogError("There are not enough textures in Resources/Sprites folder");
			Debug.LogError("Change field size to smaller or add textures");
			Debug.LogError("Need textures: " + winPairs);
			Debug.LogError("Textures in folder: " + textureList.Count);
			return;
		}

		// prepare texture list
		List<Sprite> tempTexList = new List<Sprite>();

		// add  textures according to field size var from Editor
		for(int i = 0; i< winPairs; i++)
		{
			tempTexList.Add(textureList[i]);
			tempTexList.Add(textureList[i]);
		}

		// add death textures according to deathPairs var from Editor
		for(int i = 0; i< deathPairs; i++)
		{
			tempTexList.Add(deathSprite);
			tempTexList.Add(deathSprite);
		}

			
		// shuffle texture list
		ShuffleTextures(tempTexList);

		// fill field with texture list	
		for(int i = 0; i < tileCount; i++)
		{
			tileMap[i].GetComponent<Tile>().front = tempTexList[i]; // ?
		}		

		foreach (GameObject go in tileMap)
		{
			if (go.GetComponent<Tile>().front == deathSprite)
			{
				go.GetComponent<Tile>().isDeath = true;
			}
		}
	}

	// this generates playfield with params
	void GeneratePlayField()
	{	
		// create tile for each element in width*height 
		for (int width = 0; width < fieldWidth; width++){
			for (int height = 0; height < fieldHeight; height++){	
				
				//Debug.Log(width+ " " + height);	
				GameObject go = Instantiate(tilePrefab);

				// Change position and make it show spite
				go.transform.position = new Vector3(width-fieldWidth/2,height-fieldHeight/2,0);
				go.GetComponent<Tile>().back = coverSprite;
				go.AddComponent<BoxCollider>();

				// Add generated to list with all objects
				tileMap.Add(go);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		// leave cycle if not playing (in menu)
		if (!isPlaying) return;

		ClickHandler();
		CompareHandler();
		GameCycleHandler();
		UpdateUI();		
	}



	// load Textures from folder Resources/Sprites and add them to List with textures
    void LoadTextures()
	{
		var tex =  Resources.LoadAll("Sprites", typeof(Texture2D));
		foreach(Texture2D t in tex)
		{
			Sprite newSprite = Sprite.Create(t, new Rect(0.0f, 0.0f, t.width, t.height), new Vector2(0.5f, 0.5f), 100.0f);
			textureList.Add(newSprite);
		}
	}

	// Knuth shuffle algorithm :: courtesy of Wikipedia :)
	public void ShuffleTextures(List<Sprite> tex)
	{        
        for (int t = 0; t < tex.Count; t++)
        {
            Sprite tmp = tex[t];
            int r = Random.Range(t, tex.Count);
            tex[t] = tex[r];
            tex[r] = tmp;
        }
	}

	// just void to flip all tiles, mostly for debug
	public void SwitchAllTiles()
	{
		foreach (GameObject go in tileMap){
			go.GetComponent<Tile>().isOpen = !go.GetComponent<Tile>().isOpen;
		}
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
				if (hit.transform.GetComponent<Tile>().isOpen) return;
				AddToCompare(hit.transform.gameObject);
			}
		}
	}

	// UI Updater 
	void UpdateUI()
	{
		var pairCounter = winPairs-foundPairs;
		this.pairCounterUI.GetComponent<Text>().text = "Pairs left: " + pairCounter;

		var livesCounter = lives-currentDeathPairs;
		livesCounterUI.GetComponent<Text>().text = "Lives left: " + livesCounter;
	}

	// Game cycle, check if lose of win
	void GameCycleHandler()
	{
		if (lives-currentDeathPairs == 0)
		{
			GameOver();
		}
		if (foundPairs >= winPairs)
		{
			YouWin();
		}
	}

	// lose game
	void GameOver()
	{
		FlushGame();

		panelText.text = "YOU LOST THIS TIME, TRY AGAIN \n\n\n\n Click Start to begin game. \n You will need to find 15 pairs of cards. \n If you find pair of skulls, you'll be injured.\n Beware you have only two lives. ";
		Debug.Log("Game Over");
		ShowUIPanel();
		
			
	}

	// win game
	void YouWin()
	{
		FlushGame();

		panelText.text = "YOU WIN, CONGRATULATIONS! \n\n\n\n Click Start to begin game. \n You will need to find 15 pairs of cards. \n If you find pair of skulls, you'll be injured.\n Beware you have only two lives. ";
		Debug.Log("You Win");
		ShowUIPanel();	
	}

	// show and hide ui panel

	void ShowUIPanel()
	{
		panelText.transform.parent.gameObject.SetActive(true);
	}

	public void HideUIPanel()
	{
		panelText.transform.parent.gameObject.SetActive(false);
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
		tile.GetComponent<Tile>().isOpen = true;
	}

	// Open card compare
	void CompareHandler()
	{
		// if two cards open compare it
		if (CurrentCompare.Count == 2){

			// if same, leave them open
			if (CurrentCompare[0].GetComponent<Tile>().front == CurrentCompare[1].GetComponent<Tile>().front)
			{
				foundPairs +=1;
				// if one of them (inside this cycle both are same) has death texture, add +1 to deathpairs counter
				if (CurrentCompare[0].GetComponent<Tile>().front == deathSprite){
					currentDeathPairs+=1;
					foundPairs -=1;
				}
				// flush compare List in flip time
				CurrentCompare = new List<GameObject>();
				canClick = true;
			}

			// if not the same flip them back and flush list
			else 
			{
				canClick = false;
				StartCoroutine(HideCards(CurrentCompare, 1));
				CurrentCompare = new List<GameObject>();
			}
		} 		
	}

	// HideCards in someTime
	IEnumerator HideCards(List<GameObject> list, float seconds)
	{
		yield return new WaitForSeconds(seconds);

		foreach(GameObject go in list)
		{
			go.GetComponent<Tile>().isOpen = false;
		}
		canClick = true;
		Debug.Log("pew");
	}
	IEnumerator ShowCards(List<GameObject> list, float seconds)
	{
		yield return new WaitForSeconds(seconds);

		foreach(GameObject go in list)
		{
			go.GetComponent<Tile>().isOpen = true;
		}
		canClick = true;
		Debug.Log("pew");
	}
}
