﻿using UnityEngine;
using System.Collections.Generic;

public class MemoryGame : MonoBehaviour {

	public enum Direction {Up, Down, Left, Right};
    public GameObject up_arrow = GameObject.Find("up_arrow");
    public GameObject down_arrow = GameObject.Find("down_arrow");
    public GameObject left_arrow = GameObject.Find("left_arrow");
    public GameObject right_arrow = GameObject.Find("right_arrow");

    public List<Direction> sequence;
	int numFramesSinceChange;
	const int numFramesBufferBetweenInput = 10;
	const int framesToRespond = 100;
	const int framesPromptDisplayed = 30;
	bool isUsersTurn;
	int currentIndex;
	bool gameInProgress = true;
	bool isInputBufferInterval = false;

	// Use this for initialization
	void Start () {
		numFramesSinceChange = 0;
		isUsersTurn = false;
		currentIndex = 0;
		sequence.Add (getRandomDirection ());
		sequence.Add (getRandomDirection ());
		sequence.Add (getRandomDirection ());
		sequence.Add (getRandomDirection ());
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameInProgress) {
			return;
		}
		if (isUsersTurn) {
			handleUsersTurn ();
		} else {
			controlPrompt ();
		}
	}

	private Direction getRandomDirection() {
		int result = Random.Range (1, 4);
		return (Direction)result;
	}

	private void givePrompt(Direction dir) {

	}

	private Direction translateArrowKeyToDirection() {
        //PH return value
		return Direction.Up;
	}

	private void handleUsersTurn() {
		if (isInputBufferInterval) {
            //PH code
		    inputBuffer ();
            return;
		}
		if (numFramesSinceChange == framesToRespond) {
			gameInProgress = false; //player took too long to respond
			return;
		}
		Direction input = translateArrowKeyToDirection ();
		if (input != null) {
			if (correctDirection(input)) {
                //PH code
				advanceUserInSequence ();
                return;
			} else {
				gameInProgress = false; //incorrect input.
				return;
			}
		}
		numFramesSinceChange++;
	}

	private void inputBuffer() {
		if (numFramesSinceChange == numFramesBufferBetweenInput) {
			numFramesSinceChange = 0; 
			isInputBufferInterval = false;
			return;
		}
		numFramesSinceChange++;
	}

	private bool correctDirection(Direction input) {
		return (input == sequence[currentIndex]);
	}

	private void advanceUserInSequence() {
		currentIndex++;
		numFramesSinceChange = 0;
		isInputBufferInterval = true;
		if (currentIndex == sequence.Count) {
			isUsersTurn = false;
			currentIndex = 0;
			sequence.Add (getRandomDirection ());
		}
	}

	private void controlPrompt() {
		if (numFramesSinceChange == framesPromptDisplayed) {//prepare to prompt next direction
			numFramesSinceChange = 0;
			currentIndex += 1;
			return;
		}
		if (currentIndex == sequence.Count) {//start User's Turn
			isUsersTurn = true;
			currentIndex = 0;
			numFramesSinceChange = 0;
			return;
		}
		givePrompt (sequence [currentIndex]);
		numFramesSinceChange++;
	}
}