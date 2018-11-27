using UnityEngine;
using System.Collections;
using Leap;
using UnityEngine.UI;
using System;

public class Recognition : MonoBehaviour {
    public Text textu;
    Controller controller;
    Hand h;
    Frame frame;
    GUIText text;
	// Use this for initialization
	void Start () {
        controller = new Controller();
        text = new GUIText();
        frame = new Frame();
	}
	
	// Update is called once per frame
	void Update () {
        controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
        controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);

        
        /*if(frame.Hands.Count == 1)
        {
            h = frame.Hands[0];
            if(h.IsLeft)
            {
                Debug.Log("lijeva sam pa pevam");
            } else
            {
                Debug.Log("Desna sam pa pisem");
            }
        }*/
        
        //Debug.Log(frame.Gesture().GetType));
        frame = controller.Frame();
        GestureList gests = frame.Gestures();
        /*for(int i = 0; i < gests.Count; i++)
        {
            if (gests[i].Type == Gesture.GestureType.TYPECIRCLE)
                Debug.Log("Circle");
            if (gests[i].Type == Gesture.GestureType.TYPE_KEY_TAP)
                Debug.Log("Key tap");

        }*/
        if(checkA(frame.Hands[0]))
        {
            
            textu.text = "letter a";
            Debug.Log("This is letter A");
        } else
        {
            textu.text = "Not letter a";
        }


	}

    void ProcessHand(Hand hand)
    {
        Vector normal = hand.PalmNormal;
        Vector position = hand.PalmPosition;
        //pinch - thumb and any other finger
        float pinchStrength = hand.PinchStrength;

        //finger positions -> compared to one another and palm
        FingerList fingers = hand.Fingers;
        
    }

    bool checkA(Hand hand)
    {
        if (hand.GrabStrength <= 0.8)
            return false;
        Debug.Log("snaga moja "+hand.GrabStrength);
        Finger thumb = hand.Fingers[0];

        Vector thumbDir = thumb.Direction.Normalized;

        Vector otherFingerDir = hand.Fingers[1].Direction.Normalized;
        Debug.Log("dot product " + thumbDir.Dot(otherFingerDir));
        if (thumbDir.Dot(otherFingerDir) <= -0.7 && thumbDir.Dot(otherFingerDir)>= -0.9)
            return true;
        else return false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 5, 5), "Label text");
    }

}
