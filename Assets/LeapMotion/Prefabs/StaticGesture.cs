using UnityEngine;
using System.Collections;
using Leap;

public class StoreGesture : MonoBehaviour {

    Controller controller;
    bool readNew = false;
    int frames = 0;
	// Use this for initialization
	void Start () {
        controller = new Controller();
       
	}
	
	// Update is called once per frame
	void Update () {

        //get the latest frame 
        Frame frame = controller.Frame();

        //get data representation of hand - decide which hand it is (if left you have to transform it to right)
        //default hand for gestures is right
        //is it left or right hand will also be saved with gesture data
        HandList hands = frame.Hands;
        
        //is it a gesture consisting of one or two hands
        switch(hands.Count)
        {
            case 1: processOneHandGesture();
                break;
            case 2: processTwoHandGesture();
                break;
            default: Debug.Log("This application supports gestures with 1 or 2 hands only");
                //ovo ce ici na ekran
                break;

        }
        if(hands.Count == 1)
        {
            //one hand gesture - decide left or right
            if(hands[0].IsRight)
            {
                //check if gesture already exists

                //just store data
                //TO DO - serialization? 
                //maybe put timer to know when to capture the frame?
                //or just forget about serialization and remember basic hand points


            } else
            {
                //rotate data to right -> check if exists -> store
            }
        }
        
	}


    void processOneHandGesture()
    {
        //all the data that is important
    }

    void processTwoHandGesture()
    {

    }

    void getFrameData(Frame frame)
    {
        byte[] serialized = frame.Serialize;
    }
}
