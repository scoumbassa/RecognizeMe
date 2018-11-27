using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Leap;
using LMWidgets;

public class Menu : MonoBehaviour {

    public void OnPlay()
    {
        SceneManager.LoadScene("Gestures");
    }

    public void OnRecord()
    {
        SceneManager.LoadScene("RecordAndPlayback");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
