using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;

public class GazeToActivateItem : MonoBehaviour {

	public AudioClip SoundToPlay;
	public float ActivationTime = 5f;
	bool canActivate = true;

	VRInteractiveItem item;
	MenuButton btn;
	AudioSource audioSource;
	bool lastIsOver = false;
	float lastStartTime = 0;

	// Use this for initialization
	void Start () {
		item = GetComponent<VRInteractiveItem>();
		btn = GetComponentInChildren<MenuButton>();
		audioSource = GetComponent<AudioSource>();

		StartCoroutine(CheckForGaze());
	}

	IEnumerator CheckForGaze()
	{
		while(Application.isPlaying)
		{
			yield return new WaitForSeconds(.35f);
			
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
			}
		}
		yield return null;
	}	
}
