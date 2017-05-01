﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {

	//Public fields
	public Color textColor; //Color of all GUI text
	public Player player; //The player object in the game
	//public Gradient healthBarGradient; //LEGACY: An editable gradient that determins health bar color
	public List<Sprite> healthBarTextures; //A list of all health bar textures in order

	//Private fields
	private TextMesh scoreText; //Text displaying the score
	private TextMesh livesText; //Text displaying the remaining lives
	private TextMesh healthTextLabel; //Text next to health bar
	//private GameObject healthMeter; //LEGACY: Game object that we scale to accurately scale it's child (the visible health bar)
	//private float fullHealthScale; //LEGACY: The initial unity xScale of the health meter (represents full health)
	private SpriteRenderer healthMeterBar; //The sprite renderer of the visible health bar

	// Use this for initialization
	void Start () {
		//Get all components
		scoreText = transform.Find ("ScoreText").GetComponent<TextMesh> ();
		livesText = transform.Find ("LivesText").GetComponent<TextMesh> ();
		healthTextLabel = transform.Find("HealthBar").transform.Find ("HealthLabel").GetComponent<TextMesh> ();
		//healthMeter = transform.Find("HealthBar").transform.Find ("HealthBarMeter").gameObject;
		//fullHealthScale = healthMeter.transform.localScale.x;
		//healthMeterBar = healthMeter.transform.Find("MeterBar") .GetComponent<SpriteRenderer> ();
		healthMeterBar = transform.Find("HealthBar").transform.Find("HealthBarMeter") .GetComponent<SpriteRenderer> ();


		ResetGui (); //Reset the look of everything, since it wont update until player updates it otherwise
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Update the GUI Score Display
	public void UpdateScoreDisplay(int score){
		scoreText.text = "Score: " + score;
	}

	//Update the GUI Health Display
	public void UpdateHealthDisplay(int health, int maxHealth){
		//Need to calculate the amount of health to display. 
		//We have an 8 cell health bar that can display arbitrary ranges
		int displayedHealth;
		//If at or below zero, draw zero
		if (health <= 0) {
			displayedHealth = 0;
		} 
		//Otherwise, calculate an approximation in a factor of 8 to display the remaining health percentage
		else {
			displayedHealth = (int)(((float)health / (float)maxHealth) * (float)(healthBarTextures.Count - 1));
		}

		//update display texture
		healthMeterBar.sprite = healthBarTextures [displayedHealth];

		#region OldHealthBarCode
		////Prevent the health bar from displaying a negative
		//if (health < 0) { health = 0; }
		//
		////Calculate the new scale of the health meter bar
		////Note: This is scaling the parent of the meter bar, which is at the left
		////edge of the bar. This results in a one directional scale.
		//float newHealthScale = (fullHealthScale / maxHealth) * health;
		//Vector3 newScale = healthMeter.transform.localScale;
		//newScale.x = newHealthScale;
		//healthMeter.transform.localScale = newScale;
		//
		////Update the color of the bar based on the gradient
		//healthMeterBar.color = healthBarGradient.Evaluate ((float)health / (float)maxHealth);
		#endregion
	}

	//Updates the GUI Lives Display
	public void UpdateLivesDisplay(int livesRemaining){
		livesText.text = "Lives Left: " + livesRemaining;
	}

	//Resets the GUI to it's initial state at the start of the game
	private void ResetGui(){
		//Reset all displays to their default state
		UpdateScoreDisplay (0);
		UpdateHealthDisplay (player.health, player.maxHealth);
		UpdateLivesDisplay (player.lives);

		//Set the colors of all the text meshes to the inspector selected color
		scoreText.color = textColor;
		livesText.color = textColor;
		healthTextLabel.color = textColor;
	}
}