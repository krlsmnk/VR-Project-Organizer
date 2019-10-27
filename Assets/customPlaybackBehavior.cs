using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Import the record and play namespace for access to the Recording class.
using EliCDavis.RecordAndPlay;

// Import the playback namespace for access to PlaybackBehavior
using EliCDavis.RecordAndPlay.Playback;

/// <summary>
/// This script is meant for providing playback controls through a gui to any
/// recording passed in. This class also logs custom events as they happen 
/// through the playback. This class demonstrates how you can implement the
/// IActorBuilder and IPlaybackCustomEventHandler for building custom playback.
/// </summary>
public class customPlaybackBehavior : MonoBehaviour, IActorBuilder, IPlaybackCustomEventHandler
{

  // The recording we are going to be playing back. Marked SerializeField
  // so it ican be assigned in the editor.
  [SerializeField]
  Recording recording;
 
  // A class scoped reference to PlaybackBehavior that is in control of 
  // animating the playback of the recording.
  PlaybackBehavior playbackBehavior;

  private headsetBacktrack backtrackScript;
  public bool renderTrail;  
 
  void Start()
  {
    // Create an instance of the playback behavior which will be
    // responsible for animating the recording we pass in. We also
    // tell it to use this class for both creating the actors that
    // will represent the subjects recorded, and to use this class
    // for responding to custom events in the recording.
    playbackBehavior = PlaybackBehavior.Build(recording, this, this, false);
  }
 
  // This method satisfies the IActorBuilder interface. The function takes 
  // the unique id for the subject, the name of the subject, and any 
  // assigned metadata. With that it should return a reference to a GameObject that 
  // exists in the current scene to act as a representation for the 
  // subject. There is an alternative to building the actor which 
  // includes an optional argument that is the handler for custom events
  // that occurred to the subject (not shown here).
  public Actor Build(int subjectId, string subjectName, Dictionary<string, string> metadata)
  {
    GameObject instance;
    if(subjectName == "Headset"){ 
        instance = Instantiate(Resources.Load("Vive Headset Model", typeof(GameObject))) as GameObject;
        
        //backtracking data    
        backtrackScript = instance.AddComponent<headsetBacktrack>();
        backtrackScript.Setup(recording.RecordingName);

            //trail renderer
            if (renderTrail) { 
                
                TrailRenderer trailRend = instance.AddComponent<TrailRenderer>();
                trailRend.receiveShadows = false;
                trailRend.startColor = Color.red;
                trailRend.endColor = Color.green; //new Color32( 143 , 0 , 254, 1 ); // Purple
                trailRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                trailRend.time = 600; //10 minutes
                trailRend.minVertexDistance = 0.1f;
                trailRend.startWidth = 1;
                trailRend.endWidth = 1;
                trailRend.material = GameObject.FindObjectOfType<headsetTrail>().gameObject.GetComponent<MeshRenderer>().material;
                
            }

    }     
    else if(subjectName.Contains("Controller")){
        instance = Instantiate(Resources.Load("Vive Controller Model", typeof(GameObject))) as GameObject;
        instance.AddComponent<controllerTracking>();
    }
    else if(subjectName == "TVPCamera"){
        instance = Instantiate(Resources.Load("Big Car", typeof(GameObject))) as GameObject;
        
        //try to remove tracking from headset
        Destroy(GameObject.FindObjectOfType<headsetBacktrack>().GetComponent<headsetBacktrack>());
        //backtracking data    
        backtrackScript = instance.AddComponent<headsetBacktrack>();
        backtrackScript.Setup(recording.RecordingName);

    }
    else instance = (GameObject.CreatePrimitive(PrimitiveType.Sphere));
    return new Actor(instance);
  }
 
  // This method satisfies the IPlaybackCustomEventHandler interface. This
  // function will be called as the current time in the playback 
  // progresses over the timestamp of the custom event. If the custom 
  // event belongs to a specific subject then details of the subject will
  // be passed in. Else the subject will be null and all you will be left 
  // with is the custom event itself.
  public void OnCustomEvent(SubjectRecording subject, CustomEventCapture customEvent)
  {
    if (subject == null)
    {
      Debug.LogFormat("Global Custom Event: {0} - {1}", customEvent.Name, customEvent.Contents);
    } 
    else
    {
      Debug.LogFormat("Custom Event For {0}: {1} - {2}", subject.SubjectName, customEvent.Name, customEvent.Contents);
    }
  }

  private void OnGUI()
  {
    if (playbackBehavior.CurrentlyStopped())
    {
      if (GUILayout.Button("Play"))
      {        
        playbackBehavior.Play();
      }
    }
    else if (playbackBehavior.CurrentlyPlaying())
    {
      if (GUILayout.Button("Pause"))
      {
        playbackBehavior.Pause();
      }
      if (GUILayout.Button("Stop"))
      {
        playbackBehavior.Stop();
        headsetBacktrack bt = GameObject.FindObjectOfType<headsetBacktrack>();
        if(bt!=null)bt.DoneRun();
      }
    }
    else if (playbackBehavior.CurrentlyPaused())
    {
      if (GUILayout.Button("Play"))
      {
        playbackBehavior.Play();
      }
      if (GUILayout.Button("Stop"))
      {
        playbackBehavior.Stop();
        headsetBacktrack bt = GameObject.FindObjectOfType<headsetBacktrack>();
        if(bt!=null)bt.DoneRun();
      }
    }
  }
   
}
