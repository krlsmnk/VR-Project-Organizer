using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EliCDavis.RecordAndPlay.Record;
using VRTK;
using System;
using CAVS.ProjectOrganizer.Controls;
using CAVS.ProjectOrganizer.Scenes.Showcase;

namespace VRTK { 
namespace RecordAndPlay.Demo
{

public class recordAndPlayManager : MonoBehaviour {

        private Recorder recorder;
        public GameObject headset;
        public GameObject controllerLeft, controllerRight;
        public GameObject tvpCameraGO;
        private GameObject[] subjects;
        string nameOfRecording;
        bool tvp;


        void Start()
        {           
            recorder = ScriptableObject.CreateInstance<Recorder>();
            int numberOfSubjects = 3;         
            subjects = new GameObject[numberOfSubjects];

        }

        public void ifTVP()
        {
            tvp = true;
            int numberOfSubjects = 4;         
            subjects = new GameObject[numberOfSubjects];
        }        

        void Update()
        {
            //if(Input.GetKey(KeyCode.R)) startRAP();
            if(Input.GetKey(KeyCode.S)) saveRecording();
        }

        public void setupRecorder(string recordingName) {
         if(GameObject.FindObjectOfType<SceneManagerBehavior>().Recording)
         { 
            if(headset == null) headset = VRTK_DeviceFinder.HeadsetTransform().gameObject;
            subjects[0] = headset;
            if(controllerLeft == null) controllerLeft = VRTK_DeviceFinder.GetControllerLeftHand();
            subjects[1] = controllerLeft;
            if(controllerRight == null )controllerRight = VRTK_DeviceFinder.GetControllerRightHand();
            subjects[2] = controllerRight;

            if(tvp){    
                 tvpCameraGO = GameObject.FindGameObjectWithTag("TVPCamera");
                    try{subjects[3] = tvpCameraGO; }
                    catch {Debug.Log("no camera found");}
                }

            //CNG var subjectTransform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //int frameRate, string name, Dictionary<string, string> metadata, float minimumDelta
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            SubjectBehavior SBCLeft = SubjectBehavior.Build(controllerLeft, recorder, 30, "Left Controller", metaData, .001f);
            SubjectBehavior SBCRight = SubjectBehavior.Build(controllerRight, recorder, 30, "Right Controller", metaData, .001f);
            SubjectBehavior SBHeadset = SubjectBehavior.Build(headset, recorder, 30, "Headset", metaData, .001f);
            
           if(tvp) { 
            SubjectBehavior SBCamera = SubjectBehavior.Build(tvpCameraGO, recorder, 30, "TVPCamera", metaData, .001f);
           }

            nameOfRecording = recordingName;

            if (!recorder.CurrentlyRecording())recorder.Start();
                    //else if(recorder.CurrentlyRecording())recorder.(); //CNG WAY TO STOP
         }
         else{
             Debug.Log("Not recording!!");
             }

           //Get rid of random cube
                GameObject randomCube = GameObject.Find("Cube");
                if(randomCube.transform.position.x == 0) Destroy(randomCube);
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

                    //UnityEditor.EditorApplication.isPlaying = false;
                }

            }

}
}//end of namespace
}//end of VRTK namespace