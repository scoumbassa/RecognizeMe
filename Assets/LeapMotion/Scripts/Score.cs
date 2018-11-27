using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {

    public Text score;
    public Text guessed;
	
	// Update is called once per frame
	void Update () {
        score.text = GestureGame.GameScore();
        guessed.text = GestureGame.NumberOfGuessed();
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
