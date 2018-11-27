using UnityEngine;
using System.Collections;
using Leap;
using UnityEngine.UI;
/**
    RecordGestures is used for gesture recording. After the defined gesture is
    recorded, it is saved as a .bytes file on appropriate location.
*/
public class RecordGestures : MonoBehaviour {
    
    /**String representing gesture name*/
    private string gestureString = "";
    /**Signifies end of recording */
    public KeyCode saveGestureName = KeyCode.S;
    /**When 'N' pressed new recording starts after a countdown*/
    public KeyCode newGestureName = KeyCode.N;
    /**Writes the location where gesture is saved*/
    public Text gestText;
    /**is it a dynamic gesture - i.e. more than one frame*/
    bool dynamicGesture = false;
    /** Countdown time*/
    public const float TIME = 5;
    /**timer*/
    private float timer = TIME;
    /**is it time to record a gesture*/
    bool record = false;
    /**where the gesture is stored*/
    private string path;

    void OnGUI()
    {
        GUI.Label(new Rect(400, 25, 50, 70), "Enter gesture name");
        gestureString = GUI.TextField(new Rect(500, 25, 100, 30), gestureString);
        dynamicGesture = GUI.Toggle(new Rect(50, 25, 100, 30), dynamicGesture, "Dynamic");
        //GUI.Label(new Rect(505, 75, 100, 100), gestureString);
        
        if(record)
        {
            
            if (timer > 0)
                GUI.Label(new Rect(150, 25, 300, 30), "Recording gesture in " + ((int)timer).ToString());
            else
                GUI.Label(new Rect(150, 25, 300, 30), "Recording gesture...Click 'S' to stop and save");
        }
        
    }
	// Use this for initialization
	void Start () {
        HandController.Main.StopRecording();
        //recorder.state = RecorderState.Stopped;
    }
	
	// Update is called once per frame
	void Update () {

        //countdown before recording a pose/gesture
        if(record) 
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if (timer < 0)
            {
                HandController.Main.ResetRecording();
                HandController.Main.Record();
                record = false;
                
            }
        }
        
	
        if(Input.GetKeyDown(newGestureName))
        {
            recordNewGesture();
            
            gestText.text = "";
        }
        if(Input.GetKeyDown(saveGestureName))
        {
            saveGesture();
            HandController.Main.StopRecording();

        }
	}

    void recordNewGesture()
    {
        record = true;
        timer = TIME;
    }
    /** Saves gesture on appropriate location */
    void saveGesture()
    {
        path = HandController.Main.FinishAndSaveRecording
            ("./Assets/Gestures/" + gestureString + ".bytes", dynamicGesture);
        
        gestText.text = "Saved to \n" + path;
    }
}
