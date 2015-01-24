// TextureSwapper.cs
// Created by ZachLHelms on  1/24/2015
// Purpose: This class is to switch the rendered texture on one material to a different texture.
// Use: in inspector place on object with material(alternatively, assign material to objectMaterial field)
// 		assign a texture that you want object to be able to swap to to the swapTexture field
// 		to make swap happen simply invoke SwapTexture method.
using UnityEngine;
using System.Collections;

public class TextureSwapper : MonoBehaviour {

	/// <summary>
	/// The texture we want to swap to.
	/// </summary>
	[SerializeField]
	private Texture swapTexture = null;
	
	[Header("Hover for more info")]
	[SerializeField]
	[Tooltip("This field can be left blank if it's on the same object as the material")]
	/// <summary>
	/// The objects material.
	/// </summary>
	private Material objectMaterial = null;

	[SerializeField]
	[Tooltip("This field can be left blank if it's on the same object as the material, or if you will only be swapping one way.")]
	/// <summary>
	/// texture that object initally has, can be used to swap back
	/// </summary>
	private Texture initialTexture = null;

	/// <summary>
	/// false = using initial texture
	/// true = using swapped texture
	/// </summary>
	private bool swapState = false;

	/// <summary>
	/// Creates a console log that adds information to help track down this object
	/// </summary>
	/// <param name="message">message to be logged.</param>
	/// <param name="error">If set to <c>true</c> will be an logged as an error.</param>
	private void LogSelf(string message, bool error) {
		if (error) {
			Debug.LogError (this.name + "/TextureSwapper: " + message);
		}
		else {
			Debug.Log (this.name + "/TextureSwapper: " + message);
		}
	}

	// Use this for initialization
	void Start () {
		// make sure swapTexture exists
		if (null == this.swapTexture) {
			this.LogSelf("Missing swap texture.", true);
			this.enabled = false;
			return;
		}

		// be sure we have or can find a material
		if (null == this.objectMaterial) {
			if (null != this.renderer
			&& null != this.renderer.sharedMaterial) {
				this.objectMaterial = this.renderer.sharedMaterial;
			}
			else {
				this.LogSelf("Could not find object material", true);
				this.enabled = false;
				return;
			}
		}

		// get an initial texture if one is not assigned, and exists
		if (null == this.initialTexture) {
			if(null != this.objectMaterial.mainTexture) {
				this.initialTexture = this.objectMaterial.mainTexture;
			}
		}
	}

	/// <summary>
	/// Toggles the texture to the texture not currently active
	/// </summary>
	public void SwapTexture() {
		// based on our swap flag, change the texture
		if (false == this.swapState) {
			this.objectMaterial.mainTexture = this.swapTexture;
		}
		else {
			if(null != this.initialTexture) {
				this.objectMaterial.mainTexture = this.initialTexture;
			}
			else {
				// notify of no texture
				this.LogSelf("TextureSwapper script does not know what the initial texture was.", true);
				return;
			}
		}
		// change our flag
		this.swapState = !this.swapState;
	}
}
