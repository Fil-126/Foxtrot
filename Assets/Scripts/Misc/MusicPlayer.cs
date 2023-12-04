using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static AudioSource source;
    private static bool created;
    void Start()
    {
        if (created)
        {
            Destroy(gameObject);
            return;            
        }

        created = true;
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
    }
    
}
