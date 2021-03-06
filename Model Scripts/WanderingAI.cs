﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour {

	public float wanderRadius;
	public float wanderTimer;

	private Transform target;
	public NavMeshAgent agent;
	private float timer;

	// Use this for initialization
	void onEnable() {
		//agent = GetComponent<NavMeshAgent> ();
		timer = wanderTimer;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer >= wanderTimer) {
			Vector3 newPos = RandomNavSphere (transform.position, wanderRadius, -1);
			agent.SetDestination (newPos);
			timer = 0;
		}
	}

	public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
		Vector3 randDirection = Random.insideUnitSphere * dist;

		randDirection += origin;

		NavMeshHit navHit;

		NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);

		return new Vector3 (navHit.position.x, 0f, navHit.position.z);
	}
}
