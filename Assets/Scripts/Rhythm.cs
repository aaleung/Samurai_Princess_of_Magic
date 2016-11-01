﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Rhythm : MonoBehaviour {

    public enum Action { tap, swipe_up, swipe_down, swipe_left, swipe_right };
    public List<Action> sequence;
    public List<Action> target;

    public int difficulty;
    public int mutation_index;
    public int prompt_index;
    public bool correct;
    public bool valid_input;
    public bool activated;
    public bool triggered;
    public bool looping;
    public float inputDelay = 2.0f;

    public GameObject swipe_up;
    public GameObject swipe_down;
    public GameObject swipe_left;
    public GameObject swipe_right;
    public GameObject tap;


    // Use this for initialization
    void Start () {
        print("starting late start");
        mutation_index = 0;
        prompt_index = 0;
        //difficulty = 3;
        correct = true;
        looping = false;
        activated = false;
        triggered = false;
	    for (int i = 0; i < difficulty; i++)
        {
            int val = Random.Range(1, 4);
            target.Add((Action)val);
            sequence.Add(Action.tap);
        }

	}
	
	// Update is called once per frame
	void Update () {
        //start prompt
        if (Input.GetKeyDown("return") && !triggered)
        {
            triggered = true;
            activated = true;
            StartCoroutine(show_sequence());
        }

        //take user input
        else if (valid_input) { 
            if (Input.GetKeyDown("space")) {
                compare_user_input(Action.tap);
            }
            else if (Input.GetKeyDown("up")) {
                compare_user_input(Action.swipe_up);
            }
            else if (Input.GetKeyDown("down")) {
                compare_user_input(Action.swipe_down);
            }
            else if (Input.GetKeyDown("left")) {
                compare_user_input(Action.swipe_left);
            }
            else if (Input.GetKeyDown("right")) {
                compare_user_input(Action.swipe_right);
            }
        }

        //keep running prompt
        if (activated && !looping)
        {
            StartCoroutine(show_sequence());
        }

	}


    public void compare_user_input(Action act) {
        valid_input = false;
        if(sequence[prompt_index] != act)
        {
            correct = false;
        }
        if (correct && prompt_index == sequence.Count - 1)
        {
            print("it should mutate");
            mutate_sequence();
        }
        print("correct: " + correct);
        return;
    }

    public void mutate_sequence() {
        print("mutation");
        sequence[mutation_index] = target[mutation_index];
        mutation_index++;
        //change to random index
    }

    public IEnumerator show_sequence()
    {
        looping = true;
        correct = true;

        //print("entered coroutine");
        if(sequence == null)
        {
            //print("null sequence");
        }
        print(sequence.Count);
        int i = 0;
        while (i < sequence.Count)
        {
            prompt_index = i;
            print("sequence: " + i);
            prompt_user(sequence[i]);
            valid_input = true;
            yield return new WaitForSeconds(inputDelay);
            disable_prompts();
            valid_input = false;
            yield return new WaitForSeconds(0.2f);
            i++;
        }
        looping = false;
    }


    public void disable_prompts() {
        swipe_up.GetComponent<SpriteRenderer>().enabled = false;
        swipe_down.GetComponent<SpriteRenderer>().enabled = false;
        swipe_left.GetComponent<SpriteRenderer>().enabled = false;
        swipe_right.GetComponent<SpriteRenderer>().enabled = false;
        tap.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void prompt_user(Action act) {
        disable_prompts();
        switch (act) {
            case Action.tap:
                tap.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Action.swipe_up:
                swipe_up.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Action.swipe_down:
                swipe_down.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Action.swipe_left:
                swipe_left.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case Action.swipe_right:
                swipe_right.GetComponent<SpriteRenderer>().enabled = true;
                break;
            default:
                print("ERROR: unrecognized action");
                break;
        }
    }
}