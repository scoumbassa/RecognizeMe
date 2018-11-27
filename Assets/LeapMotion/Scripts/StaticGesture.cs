using UnityEngine;
using System.Collections;
using Leap;
using System;
/**
* This class represents a static gesture - i.e.
* a gesture consisting of only one frame.
*/
public class StaticGesture {
    public int numberOfHands { get; set; }
    
    public HandList hands { get; set; }
    public FingerList fingers { get; set; }

    public string gestureName { get; set;}
    public StaticGesture(string name)
    {
        gestureName = name;
    }
    /**
        Calculates the distance/difference between the
        previously recorded gesture and gesture captured
        by the Leap Motion controller.
    */
    public float RealGestureModelDifference(Frame frame)
    {
        /*this is used to determine how much the current gesture
        provided by the user is different from the stored gesture.
        The gesture with the least amount of difference is most likely
        to be the gesture we are looking for.*/
        float totalSumOfDifference = 0;

        /*compare number of hands in the field - if number of hands is not
        the same - this cannot be the right gesture. That is why a large number
        should be returned.*/
        if(numberOfHands != frame.Hands.Count)
        {
            return 100;
        }

        /*check every hand in the frame -> hands should be recognized as
        left most and right most. In this case, no more than 2 hands is expected*/
        if(numberOfHands == 1)
        {
            Hand modelHand = hands[0];
            Hand currentHand = frame.Hands[0];

            totalSumOfDifference = CheckHand(totalSumOfDifference, modelHand, currentHand);
        } 

        if(numberOfHands == 2)
        {
            Hand modelHandLeft = hands.Leftmost;
            Hand currentHandLeft = frame.Hands.Leftmost;

            Hand modelHandRight = hands.Rightmost;
            Hand currentHandRight = frame.Hands.Rightmost;

            totalSumOfDifference =
                (CheckHand(totalSumOfDifference, modelHandLeft, currentHandLeft) +
                CheckHand(totalSumOfDifference, modelHandRight, currentHandRight)) / 2;
        }

        return totalSumOfDifference;
    }

    private float compareBonePositionToHandNormal(Vector bone, Vector normal)
    {
        return bone.Dot(normal);
    }

    private float checkExtendedFingers(Finger model, Finger current)
    {
        
        if (model.IsExtended == current.IsExtended)
        {
            return 0;
        }
        return 0.1f;
    }

    private float CheckHand(float totalSumOfDifference, Hand modelHand, Hand currentHand)
    {
        //compare grab strenght - to determine if for example the fist is closed or not
        float grabDifference =
           Mathf.Abs(modelHand.GrabStrength - currentHand.GrabStrength);

        totalSumOfDifference += grabDifference;

        //compare pinch strength - to determine if there is a pinch -> i.e. pinch
        //between thumb and any other finger
        float pinchDifference =
            Mathf.Abs(modelHand.PinchStrength - currentHand.PinchStrength);
        totalSumOfDifference += pinchDifference;

        //check extended fingers
        for (int i = 0; i < 5; i++)
        {
            float ext = checkExtendedFingers(modelHand.Fingers[i], currentHand.Fingers[i]);
            totalSumOfDifference += ext;

        }

        //check relative position of every finger (every bone in finger) to hand normal
        for (int i = 0; i < 5; i++)
        {
            Bone modelBone;
            Bone currentBone;
            foreach (Bone.BoneType boneType in (Bone.BoneType[])Enum.GetValues(typeof(Bone.BoneType)))
            {
                modelBone = modelHand.Fingers[i].Bone(boneType);
                currentBone = currentHand.Fingers[i].Bone(boneType);

                float modelBoneNormal = compareBonePositionToHandNormal(
                        modelBone.Direction.Normalized,
                        modelHand.PalmNormal.Normalized);

                float currentBoneNormal = compareBonePositionToHandNormal(
                        currentBone.Direction.Normalized,
                        currentHand.PalmNormal.Normalized);
                totalSumOfDifference += Mathf.Abs(currentBoneNormal - modelBoneNormal);
            }
        }

        return totalSumOfDifference;
    }

}
