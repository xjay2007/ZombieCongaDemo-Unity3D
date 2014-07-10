using UnityEngine;
using System.Collections;

//@script RequireComponent(AudioSource)
[RequireComponent(typeof(AudioSource))]

public class XJVideoController : MonoBehaviour {

	public MovieTexture movie;

	// Use this for initialization
	void Start () {
		// Screen 

		Debug.Log("Screen Size = " + Screen.width + ", " + Screen.height +",movie.size = " + movie.width + "," + movie.height);

		// Movie
		renderer.material.mainTexture = movie as MovieTexture;
		audio.clip = movie.audioClip;
		movie.Play();
		audio.Play();
	}

	void OnMouseDown() {
		movie.Stop();
		audio.Stop();
		JumpToNextScene();
	}

	void Update () {
		if (!movie.isPlaying) {
			JumpToNextScene();
		}
	}

	private void JumpToNextScene () {
		Application.LoadLevel("CongaScene");
	}
}
