using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;
using UnityEngine.UI;
using System;

public class GazeToActivateItem : MonoBehaviour {

	public AudioClip SoundToPlay;
	public float ActivationTime = 5f;
	bool canActivate = true;

	Image uiSelectionBar = null;

	public event Action OnOver;
	public event Action OnOut;   

	VRInteractiveItem item;
	MenuButton btn;
	SelectionSlider btn2;
	AudioSource audioSource;
	bool lastIsOver = false;
	float lastStartTime = 0;

	// Use this for initialization
	void Start () {
		item = GetComponent<VRInteractiveItem>();
		btn = GetComponentInChildren<MenuButton>();
		audioSource = GetComponent<AudioSource>();
		uiSelectionBar = GameObject.Find("UISelectionBar").GetComponent<Image>();
		uiSelectionBar.fillAmount = 0;

		StartCoroutine(CheckForGaze());
	}

	IEnumerator CheckForGaze()
	{
		while(Application.isPlaying)
		{
			yield return new WaitForSeconds(.15f);
			
			if (lastIsOver != item.IsOver)
			{
				if (item.IsOver && !audioSource.isPlaying)
				{
					audioSource.clip = SoundToPlay;
					audioSource.Play();

					lastStartTime = Time.time;
					canActivate = true;
				}
				else
				if (!item.IsOver && audioSource.isPlaying)
				{
					audioSource.Stop();
					uiSelectionBar.fillAmount = 0;
				}
			}

			if (item.IsOver)
			{
				lastIsOver = true;

				if (lastStartTime > 0 && ((lastStartTime + ActivationTime) <= Time.time))
				{
					if (canActivate)
					{
						lastStartTime = 0;
						canActivate = false;
						StartCoroutine(btn.ActivateButton());
					}
				}
			}
			else
			{
				lastIsOver = false;
				canActivate = true;
				lastStartTime = 0;
				uiSelectionBar.fillAmount = 0;
			}
		}
		yield return null;
	}

	private void Update()
	{
		if (lastIsOver)
		{
			var percentComplete = (Time.time - lastStartTime) / ActivationTime;
			uiSelectionBar.fillAmount = percentComplete;
		}
	}
}
