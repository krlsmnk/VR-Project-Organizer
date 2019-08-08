using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EliCDavis.RecordAndPlay.Record;
using VRTK;
using System;

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

        void Update()
        {
            //if(Input.GetKey(KeyCode.R)) startRAP();
            if(Input.GetKey(KeyCode.S)) saveRecording();
        }

        public void setupRecorder(string recordingName) {                
            if(headset == null) headset = VRTK_DeviceFinder.HeadsetTransform().gameObject;
            subjects[0] = headset;
            if(controllerLeft == null) controllerLeft = VRTK_DeviceFinder.GetControllerLeftHand();
            subjects[1] = controllerLeft;
            if(controllerRight == null )controllerRight = VRTK_DeviceFinder.GetControllerRightHand();
            subjects[2] = controllerRight;

            var subjectTransform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //int frameRate, string name, Dictionary<string, string> metadata, float minimumDelta
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            SubjectBehavior SBCLeft = SubjectBehavior.Build(controllerLeft, recorder, 30, "Left Controller", metaData, .001f);
            SubjectBehavior SBCRight = SubjectBehavior.Build(controllerRight, recorder, 30, "Right Controller", metaData, .001f);
            SubjectBehavior SBHeadset = SubjectBehavior.Build(headset, recorder, 30, "Headset", metaData, .001f);
            
            nameOfRecording = recordingName;

            if (!recorder.CurrentlyRecording())recorder.Start();
            //else if(recorder.CurrentlyRecording())recorder.(); //CNG WAY TO STOP
        }

            /*
        private void OnGUI()
        {                      
                if (GUILayout.Button("Save"))
                {
                    saveRecording();
                }                         
        }
        */
        public void LogCustomEvent(string NameOfEvent, string DataToCapture)
            {
                recorder.CaptureCustomEvent(NameOfEvent, DataToCapture);
                //recorder.SetMetaData("KeyValue", "InformationToSet");
            }

            public void saveRecording()
            {
                //check if recording
                if (recorder.CurrentlyRecording()) { 

                //Sanitize Filename
                    var invalids = System.IO.Path.GetInvalidFileNameChars();
                    string newName = String.Join("_", nameOfRecording.Split(invalids, StringSplitOptions.RemoveEmptyEntries) ).TrimEnd('.');

                    Debug.Log("New Name: " + newName);
                    recorder.Finish().SaveToAssets(newName, "");
                }
            }

}
}//end of namespace
}//end of VRTK namespace