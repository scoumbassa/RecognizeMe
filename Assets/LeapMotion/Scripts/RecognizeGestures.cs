using UnityEngine;
using System.Collections;
using Leap;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System;
/**
    RecognizeGesture is used for gesture recognition.

*/
public class RecognizeGestures : MonoBehaviour {
    /** Frame */
    Frame frame;
    /** Controller */
    Controller controller;
    /** Location where gestures are saved */
    string path = "./Assets/Gestures/";
    string match;
    string nextMatch="";
    float timer = 0;
    static string gestureFound = "";
    public Text gesture;
    /** Leap recorder */
    LeapRecorder recorder;
    /** number of frames in gesture that should be tested*/
    int numberOfFrames;
    /** List of static gestures */
    List<StaticGesture> staticGestures = new List<StaticGesture>();

    // Use this for initialization
    void Start () {
        recorder = new LeapRecorder();
        controller = new Controller();
        gesture.text = "";
        ProcessAllFiles();
    }
	
	// Update is called once per frame
	void Update () {
        frame = controller.Frame();
        FindNewMatch();
	}

    /** Used to compare stored gestures with current frame
        and determine is there a gesture that is a match.
    */
    private string compareModelWithFrame(Frame frame)
    {

        
        float minDiff = 100;
        string gesture = "";
        foreach(StaticGesture g in staticGestures)
        {
            float diff = g.RealGestureModelDifference(frame);
            
            if(diff < minDiff)
            {
                minDiff = diff;
                gesture = g.gestureName;
            }
        }

        if(minDiff < 3)
        {
            return gesture;
        } else
        {
            return "No match";
        }
    }
    
    /**
        Method used for getting gesture data from all
        files that contain stored gestures.
    */
    void ProcessAllFiles()
    {
        
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            //files that are not bytes files should be skipped
            if(!file.EndsWith(".bytes"))
            {
                continue;
            }
            
            
            string gestName = file.Substring(file.LastIndexOf("/")+1, file.LastIndexOf(".")-file.LastIndexOf("/"));
            byte[] frameData = System.IO.File.ReadAllBytes(file);
            recorder.Load(frameData);

            numberOfFrames = recorder.GetFramesCount();

            List<Frame> recorderFrames = recorder.GetFrames();
            
            //if there is just one frame stored this is a static gesture/pose
            if(numberOfFrames == 1)
            {
                ProcessStaticGesture(recorderFrames[0], gestName);
            }

        }
    }
    /**
        Method used for processing static gesture.
        Here, a frame that was deserialized is stored
        in a list of static gestures.
    */
    void ProcessStaticGesture(Frame recorderFrame, string gestName)
    {
        StaticGesture gest = new StaticGesture(gestName);

        gest.numberOfHands = recorderFrame.Hands.Count;
        gest.hands = recorderFrame.Hands;

        staticGestures.Add(gest);
       
    }

    void FindNewMatch()
    {
        if ((int)timer == 0)
        {
            match = compareModelWithFrame(frame);
            if (!match.Equals("No match"))
            {
                timer = 2;
            }
            else
            {
                return;
            }
        }
        //match found -> wait before new gesture recognition
        if ((int)timer > 0)
        {
            timer -= Time.deltaTime;
            nextMatch = compareModelWithFrame(frame);
            if (nextMatch.Equals(match))
            {
                match = nextMatch;
            }
            else
            {
                timer = 0;
                return;
            }
        }
        if ((int)timer == 0)
        {
            if (nextMatch.Length > 0)
            {
                gestureFound = nextMatch;
                if (gestureFound.Length == 0)
                    return;
                
                gesture.text = "This is " + gestureFound;
                
            }
        }
        //Debug.Log("MATCH " + match);
    }

    public static string recognizedGesture()
    {
        
        if (gestureFound.Length > 0)
            return gestureFound[0].ToString();
        else return gestureFound;
    }
}
