using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EliCDavis.RecordAndPlay.Record;
using VRTK;

namespace VRTK { 
namespace RecordAndPlay.Demo
{

public class recordAndPlayManager : MonoBehaviour {

        private Recorder recorder;
        public GameObject headset;
        public GameObject controllerLeft, controllerRight;
        private GameObject[] subjects;
        string nameOfRecording;


        void Start()
        {
            recorder = ScriptableObject.CreateInstance<Recorder>();

            int numberOfSubjects = 3;
            subjects = new GameObject[numberOfSubjects];

        }

        public void setupRecorder(string recordingName) { 
            headset = VRTK_DeviceFinder.HeadsetTransform().gameObject;
            subjects[0] = headset;
            controllerLeft = VRTK_DeviceFinder.GetControllerLeftHand();
            subjects[1] = controllerLeft;
            controllerRight = VRTK_DeviceFinder.GetControllerRightHand();
            subjects[2] = controllerRight;

            var subjectTransform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //int frameRate, string name, Dictionary<string, string> metadata, float minimumDelta
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            SubjectBehavior SBCLeft = SubjectBehavior.Build(controllerLeft, recorder, 30, "Left Controller", metaData, .001f);
            SubjectBehavior SBCRight = SubjectBehavior.Build(controllerRight, recorder, 30, "Right Controller", metaData, .001f);
            SubjectBehavior SBHeadset = SubjectBehavior.Build(headset, recorder, 30, "Headset", metaData, .001f);
            
            nameOfRecording = recordingName;

            recorder.Start();
        }

        private void OnGUI()
        {
            if (recorder.CurrentlyRecording()) { 
                if (GUILayout.Button("Save"))
                {
                    recorder.Finish().SaveToAssets(nameOfRecording);
                }                
            }
        }
        public void LogCustomEvent(string NameOfEvent, string DataToCapture)
            {
                recorder.CaptureCustomEvent(NameOfEvent, DataToCapture);
                //recorder.SetMetaData("KeyValue", "InformationToSet");
            }

}
}//end of namespace
}//end of VRTK namespace