using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestureGame : MonoBehaviour {

    static System.Random _rand = new System.Random();
    float timer = 30;
    static int score = 0;
    public Text scoreText;
    public Text countdown;
    public Image letterImage;
    public char currentLetter;
    public bool guessed = true;
    RecognizeGestures recognition;
    public KeyCode skip = KeyCode.N;
    public KeyCode exit = KeyCode.X;
    static int gestureNumber = 0;
	// Use this for initialization
	void Start () {
        recognition = new RecognizeGestures();
        scoreText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("lETTER " + getRandomLetter());
        if((int)timer <= 5)
        {
            countdown.color = Color.red;
        }
        countdown.text = ((int)timer).ToString();
        
        if (timer <= 0)
        {
            GameOver();
        }
        timer -= Time.deltaTime;
        

        if(guessed || Input.GetKeyDown(skip))
        {
            guessed = false;
            //ucitaj sliku i slovo
            currentLetter = getRandomLetter();
            LoadLetterImage();
            gestureNumber++;
        }
        
        
        if(CheckGesture())
        {
            guessed = true;
            score += 1;
            scoreText.text = (score*100).ToString();
        }

        if(Input.GetKeyDown(exit))
        {
            GameOver();
        }
        
	}

    public char getRandomLetter()
    {
        int num = _rand.Next(0, 26);
        char letter = (char)('a' + num);
        letter = char.ToUpper(letter);

        return letter;
    }

    public void GameOver()
    {
        
        SceneManager.LoadScene("Score");
    }

    public static string GameScore()
    {
        return (score*100).ToString();
    }

    public static string NumberOfGuessed()
    {
        return score + "/" + gestureNumber;
    }
    /**
        Method loads an appropriate letter image
        to the screen
    */
    public void LoadLetterImage()
    {
        string path = "./Assets/LeapMotion/Resources/";
        ArrayList image = new ArrayList();
        int n = 0;
        string[] files = System.IO.Directory.GetFiles(path);
        foreach(string file in files)
        {
            Debug.Log(file);
            if(file.EndsWith(".png") && 
                file.Substring(file.LastIndexOf("/")+1).StartsWith(currentLetter.ToString()))
            {
                //there may be a one hand and 2 hand variant of a gesture
                image.Add(file);
            }
        }
        
        string im = "";
        System.Random rand = new System.Random();
        int num = rand.Next(0, image.Count-1);
        im = image[num].ToString();
        im = im.Replace("./Assets/LeapMotion/Resources/", "").Replace(".png", "");

        Debug.Log(im);
        letterImage.sprite = (Resources.Load<Sprite>(im));
        
    }

    public bool CheckGesture()
    {
        
        return RecognizeGestures.recognizedGesture().Equals(currentLetter.ToString());
    }

}
