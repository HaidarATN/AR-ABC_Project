/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System.Collections;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// 
/// Changes made to this file could be overwritten when upgrading the Vuforia version. 
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    public AudioSource[] allSource;

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

        allSource = GameObject.FindObjectsOfType<AudioSource>();
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    public GameObject instructionText;
    public AudioClip newclip;

    public bool isTracked;
    public AudioClip oldClip;
    AudioSource asc;

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        isTracked = true;
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);
        var animationComponents = GetComponentsInChildren<Animator>(true);
        var audioSourceComponents = GetComponentsInChildren<AudioSource>(true);

        foreach (AudioSource asc in allSource)
            asc.Stop();

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        // Start Anim
        foreach (var component in animationComponents)
            component.Play("Entry");

        // Play Sound
        foreach (var component in audioSourceComponents)
        {
            asc = component;
            oldClip = asc.clip;
            component.clip = newclip;
            component.loop = false;
            component.Play();
        }

        StartCoroutine(CheckSound());
        instructionText.SetActive(false);
    }


    protected virtual void OnTrackingLost()
    {
        isTracked = false;
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);
        var audioSourceComponents = GetComponentsInChildren<AudioSource>(true);

        // Stop Sound
        foreach (var component in audioSourceComponents)
            component.Stop();

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;

        foreach (AudioSource asc in allSource)
            asc.Stop();

        if(oldClip!=null)
        asc.clip = oldClip;

        instructionText.SetActive(true);
    }

    IEnumerator CheckSound()
    {
        while (asc.isPlaying)
        {
            print("hehe");
            yield return new WaitForSeconds(0.1f);
        }

        if (!asc.isPlaying && isTracked)
        {
            asc.clip = oldClip;
            asc.loop = true;
            asc.Play();
            yield break;
        }
    }

    #endregion // PROTECTED_METHODS
}






/////////////////////////////////////// Newer Version
///*==============================================================================
//Copyright (c) 2019 PTC Inc. All Rights Reserved.

//Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
//All Rights Reserved.
//Confidential and Proprietary - Protected under copyright and other laws.
//==============================================================================*/

//using UnityEngine;
//using Vuforia;

///// <summary>
///// A custom handler that implements the ITrackableEventHandler interface.
/////
///// Changes made to this file could be overwritten when upgrading the Vuforia version.
///// When implementing custom event handler behavior, consider inheriting from this class instead.
///// </summary>
//public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
//{
//    #region PROTECTED_MEMBER_VARIABLES

//    protected TrackableBehaviour mTrackableBehaviour;
//    protected TrackableBehaviour.Status m_PreviousStatus;
//    protected TrackableBehaviour.Status m_NewStatus;

//    #endregion // PROTECTED_MEMBER_VARIABLES

//    #region UNITY_MONOBEHAVIOUR_METHODS

//    protected virtual void Start()
//    {
//        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
//        if (mTrackableBehaviour)
//            mTrackableBehaviour.RegisterTrackableEventHandler(this);
//    }

//    protected virtual void OnDestroy()
//    {
//        if (mTrackableBehaviour)
//            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
//    }

//    #endregion // UNITY_MONOBEHAVIOUR_METHODS

//    #region PUBLIC_METHODS

//    /// <summary>
//    ///     Implementation of the ITrackableEventHandler function called when the
//    ///     tracking state changes.
//    /// </summary>
//    public void OnTrackableStateChanged(
//        TrackableBehaviour.Status previousStatus,
//        TrackableBehaviour.Status newStatus)
//    {
//        m_PreviousStatus = previousStatus;
//        m_NewStatus = newStatus;

//        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName +
//                  " " + mTrackableBehaviour.CurrentStatus +
//                  " -- " + mTrackableBehaviour.CurrentStatusInfo);

//        if (newStatus == TrackableBehaviour.Status.DETECTED ||
//            newStatus == TrackableBehaviour.Status.TRACKED ||
//            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
//        {
//            OnTrackingFound();
//        }
//        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
//                 newStatus == TrackableBehaviour.Status.NO_POSE)
//        {
//            OnTrackingLost();
//        }
//        else
//        {
//            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
//            // Vuforia is starting, but tracking has not been lost or found yet
//            // Call OnTrackingLost() to hide the augmentations
//            OnTrackingLost();
//        }
//    }

//    #endregion // PUBLIC_METHODS

//    #region PROTECTED_METHODS

//    protected virtual void OnTrackingFound()
//    {
//        if (mTrackableBehaviour)
//        {
//            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
//            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
//            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);
//            var audioSource = mTrackableBehaviour.GetComponentsInChildren<AudioSource>(true);
//            var gameObject = mTrackableBehaviour.GetComponentsInChildren<GameObject>(true);

//            // Enable rendering:
//            foreach (var component in rendererComponents)
//                component.enabled = true;

//            // Enable colliders:
//            foreach (var component in colliderComponents)
//                component.enabled = true;

//            // Enable canvas':
//            foreach (var component in canvasComponents)
//                component.enabled = true;

//            foreach (var component in audioSource)
//                component.Play();

//            foreach (var component in gameObject)
//                component.SetActive(true);
//        }
//    }


//    protected virtual void OnTrackingLost()
//    {
//        if (mTrackableBehaviour)
//        {
//            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
//            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
//            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);
//            var gameObject = mTrackableBehaviour.GetComponentsInChildren<GameObject>(true);

//            // Disable rendering:
//            foreach (var component in rendererComponents)
//                component.enabled = false;

//            // Disable colliders:
//            foreach (var component in colliderComponents)
//                component.enabled = false;

//            // Disable canvas':
//            foreach (var component in canvasComponents)
//                component.enabled = false;

//            foreach (var component in gameObject)
//                component.SetActive(true);
//        }
//    }

//    #endregion // PROTECTED_METHODS
//}

