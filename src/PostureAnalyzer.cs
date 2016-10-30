using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;
using System.Collections.Specialized;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class PostureAnalyzer
    {
        private string workoutName;
        private BitVector32 postureFlags;


        public PostureAnalyzer()
        {
            //Sets all flags in bit vector to false
        }

        public Dictionary<string,Vector3D> TrackRelevantLimbs(Body body, string passedWorkoutName)
        {
            this.workoutName = passedWorkoutName;
            switch (workoutName)
            {
                case "Shoulder_Press":
                    //Shoulder Press workout has currently 6 checks associated with it
                    return TrackShoulderLimbs(body);
                default:
                    return null;
            }
        }
        

        public BitVector32 AnalyzeWorkout(Body body, string passedWorkoutName, float workoutProgress)
        {
            postureFlags = new BitVector32(0);
            int armDepthFlag = BitVector32.CreateMask();
            int orthogonalLeftFlag = BitVector32.CreateMask(armDepthFlag);
            int orthogonalRightFlag = BitVector32.CreateMask(orthogonalLeftFlag);
            int armsLockedFlag = BitVector32.CreateMask(orthogonalRightFlag);
            int armRotationFlag = BitVector32.CreateMask(armsLockedFlag);
            Dictionary<string, Vector3D> limbs = this.TrackRelevantLimbs(body, passedWorkoutName);
            //1st bit represents depth of arms
            //2nd bit represents orthogonality of left arm
            //3rd bit represents orthogonality of right arm
            //4th bit represents whether arms are locked at rep climax
            //5th bit represents rotation of arms about upper arm axis
            if(passedWorkoutName == "Shoulder_Press")
            {
                //Start with first check
                if(workoutProgress >= 0 && workoutProgress <= 0.25)
                {
                    //Person is at beginning of the workout rep

                    //Check for arm depth
                    if(Math.Abs(body.Joints[JointType.ElbowLeft].Position.Z - body.Joints[JointType.ElbowRight].Position.Z) > 0.05)
                    {
                        postureFlags[armDepthFlag] = true;
                    }

                    //Check arms for orthogonality
                    Vector leftUpperArm = new Vector(limbs["leftUpperArm"].X, limbs["leftUpperArm"].Y);
                    Vector leftLowerArm = new Vector(limbs["leftLowerArm"].X, limbs["leftLowerArm"].Y);

                    Vector rightUpperArm = new Vector(limbs["rightUpperArm"].X, limbs["rightUpperArm"].Y);
                    Vector rightLowerArm = new Vector(limbs["rightLowerArm"].X, limbs["rightLowerArm"].Y);

                    //Check if arms is close to orthogonal
                    if (Math.Abs(Vector.AngleBetween(leftLowerArm,leftUpperArm) - 90.0) > 10.0)
                    {
                        //If arm is nowhere near orthogonal raise flag
                        postureFlags[orthogonalLeftFlag] = true;
                    }
                    if (Math.Abs(Vector.AngleBetween(rightLowerArm, rightUpperArm) - 90.0) > 10.0)
                    {
                        //If arm is not close to orthogonal raise flag
                        postureFlags[orthogonalRightFlag] = true;
                    }

                    //Check for rotation, retrieve z-coordinates of elbows and wrists, if the difference between them is larger than a certain threshold, arms are rotating
                    float leftArmRotation = Math.Abs(body.Joints[JointType.ElbowLeft].Position.Z - body.Joints[JointType.WristLeft].Position.Z);
                    float rightArmRotation = Math.Abs(body.Joints[JointType.ElbowRight].Position.Z - body.Joints[JointType.WristRight].Position.Z);
                    if (leftArmRotation > 0.05 || rightArmRotation > 0.05)
                    {
                        //If either arm is rotating too much
                        postureFlags[armRotationFlag] = true;
                    }


                }
                if (workoutProgress >= 0.5 && workoutProgress <= 1)
                {
                    //If at the end of the shoulder press rep

                    //Check arms for locking
                    Vector leftUpperArm = new Vector(limbs["leftUpperArm"].X, limbs["leftUpperArm"].Y);
                    Vector leftLowerArm = new Vector(limbs["leftLowerArm"].X, limbs["leftLowerArm"].Y);

                    Vector rightUpperArm = new Vector(limbs["rightUpperArm"].X, limbs["rightUpperArm"].Y);
                    Vector rightLowerArm = new Vector(limbs["rightLowerArm"].X, limbs["rightLowerArm"].Y);
                    if(Math.Abs(Vector.AngleBetween(leftLowerArm,leftUpperArm) - 0.0) < 15.0 || Math.Abs(Vector.AngleBetween(rightLowerArm,rightUpperArm) - 180.0) < 15.0)
                    {
                        postureFlags[armsLockedFlag] = true;
                    }

                }
            }


            return postureFlags;
        }

        private Dictionary<string,Vector3D> TrackShoulderLimbs(Body body)
        {
            Dictionary<string, Vector3D> shoulderLimbs = new Dictionary<string, Vector3D>();
            
            shoulderLimbs.Add("leftUpperArm", constructVector(JointType.ShoulderLeft, JointType.ElbowLeft, body));
            shoulderLimbs.Add("rightUpperArm", constructVector(JointType.ElbowRight, JointType.ShoulderRight, body));
            shoulderLimbs.Add("leftLowerArm", constructVector(JointType.ElbowLeft, JointType.WristLeft, body));
            shoulderLimbs.Add("rightLowerArm", constructVector(JointType.ElbowRight, JointType.WristRight, body));
            shoulderLimbs.Add("leftWristJoint", constructVector(JointType.WristLeft, JointType.HandLeft,body));
            shoulderLimbs.Add("rightWristJoint", constructVector(JointType.WristRight, JointType.HandRight,body));
            return shoulderLimbs;
        }
        private Vector3D constructVector(JointType point1, JointType point2, Body body)
        {
            Vector3D localVector = new Vector3D();
            localVector.X = body.Joints[point2].Position.X - body.Joints[point1].Position.X;
            localVector.Y = body.Joints[point2].Position.Y - body.Joints[point1].Position.Y;
            localVector.Z = body.Joints[point2].Position.Z - body.Joints[point1].Position.Z;
            return localVector;
        }

        
    }
}
