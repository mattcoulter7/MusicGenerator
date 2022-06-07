using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Standards;
using Melanchall.DryWetMidi.Tools;
public class MidiParser : MonoBehaviour
{
    string path;
    MidiFile midiFile;
    // Start is called before the first frame update
    void Start()
    {
        //path = EditorUtility.OpenFilePanel("Midi File", "", "mid");
        midiFile = MidiFile.Read(path);
    }

    void Update(){
        Debug.Log("");
    }
}
