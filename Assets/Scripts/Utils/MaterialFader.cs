// MaterialFader.cs
// Created by ZachLHelms on  1/25/2015
// Purpose: Fades out a collection of materials that it pulls from renderers. Materials must have a color value to work. Object disabled after fade.
// Use: in inspector add all objects wanted to fade that have renderers/materials to objectRenderers list 
// 		when you want the object to disappear, call FadeOut(). 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaterialFader : MonoBehaviour {

	[SerializeField]
	private List<Renderer> objectRenderers;

	private List<Material> materialsToFade;

	[SerializeField]
	private float timeForFadeEffect;

	private float timeFadeElapsed;

	private bool bShouldFadeOut;

	private List<Color> colorsOfMaterial;

	// Use this for initialization
	void Start () {
		this.bShouldFadeOut = false;

		this.materialsToFade = this.objectRenderers.ConvertAll (r => r.material);
		this.colorsOfMaterial = this.materialsToFade.ConvertAll(m => m.color);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.bShouldFadeOut) {
			float percentComplete = this.timeFadeElapsed / this.timeForFadeEffect;
			if(percentComplete >= 1.0f) {
				this.SetAlphas(0);
				this.bShouldFadeOut = false;
				this.gameObject.SetActive(false);
				return;
			}
			float applicableAlpha = 1.0f - percentComplete;
			this.SetAlphas(applicableAlpha);
			this.timeFadeElapsed +=  Time.deltaTime;
		}
	}

	private void SetAlphas(float alpha) {
		for (int i = 0; i < this.objectRenderers.Count; ++i) {
			// this is dumb, but cant modify it as part of the array;
			Color mutableColor = this.colorsOfMaterial[i];
			mutableColor.a = alpha;
			this.colorsOfMaterial[i] = mutableColor;
			this.materialsToFade[i].color = this.colorsOfMaterial[i];
		}
	}

	public void FadeOut() {
		this.timeFadeElapsed = 0;
		this.bShouldFadeOut = true;
	}
}
