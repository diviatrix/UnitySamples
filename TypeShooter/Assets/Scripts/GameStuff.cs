using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class GameStuff : MonoBehaviour {
	
	String[] allWords = new String[500000];

	public float cooldown;
	float lastWordTime;
	public Text wordTextUI;
	public GameObject textPrefab;
	public List<GameObject> wordsInGame = new List<GameObject>();

	System.Random rnd = new System.Random();

	// Use this for initialization
	void Start () 
	{
		loadWords();
		lastWordTime = Time.time;		
	}

	// cycle for word spawning
	void SpawnWords()
	{	
		if ((lastWordTime + cooldown) > Time.time ) return;

		if (wordsInGame.Count < 10)
		{
			lastWordTime = Time.time;
			GameObject go = GameObject.Instantiate(textPrefab);

			string word = randomWord();
			
			go.transform.position = new Vector3(-10, UnityEngine.Random.Range(-3,5), -1);			
			go.GetComponent<TextMesh>().text = word;
			go.transform.name = word;
			Debug.Log(word.Length);	
			go.GetComponent<WordBehavior>().speed = 1/(float)word.Length;

			wordsInGame.Add(go);
		}
	}


	void FixedUpdate()
	{
		// this
		SpawnWords();

		// remove null elements from wordsingame list
		wordsInGame.Remove(null);
	}


	// load dictionary from file
	void loadWords()
	{
		string line; 
		// Open the text file using a stream reader.
        using (StreamReader sr = new StreamReader("Assets/Resources/Text/words_ru.txt"))
        {
			int i = 0;
	    // Read the stream to a string, and write the string to the console.
            while((line = sr.ReadLine()) != null)  
			{  
				//Debug.Log(line);  
				allWords[i] = line;
				i++;
			}
			
			String[] tempArray = new String[i];

			for(int a = 0; a <i; a++)
			{
				tempArray[a] = allWords[a];
			}
			allWords = tempArray;

			Debug.Log(allWords.Length);
        }			
	}

	// gives random word from dictionary
	string randomWord() 
	{	
		int id = rnd.Next(0,allWords.Length);
		return(allWords[id]);
	}

	// run every frame
	void Update () 
	{
		
	}
}
