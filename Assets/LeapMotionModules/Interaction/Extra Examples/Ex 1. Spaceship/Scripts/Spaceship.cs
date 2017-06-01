﻿using Leap.Unity;
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour {

  public
  #if UNITY_EDITOR
  new
  #endif
  Rigidbody rigidbody { get { return _body; } }

  public Vector3 velocity { get { return _velocity; } set { _velocity = value; } }

  private Vector3 _velocity;
  private Vector3 _angularVelocity;
  private float _mass = 10F;

  private Rigidbody _body;
  private Vector3 _accumulatedForce;
  private Vector3 _accumulatedTorque;

  public Vector3 ShipAlignedAngularVelocity {
    get { return Quaternion.Inverse(this.transform.rotation) * _angularVelocity; }
  }

  public Vector3 ShipAlignedVelocity {
    get { return Quaternion.Inverse(this.transform.rotation) * _velocity; }
  }

  void Awake() {
    _body = GetComponent<Rigidbody>();
    _body.mass = _mass;
  }

  void Start() {
    //PhysicsCallbacks.OnPrePhysics += onPrePhysics;
  }

  public void AddForce(Vector3 force) {
    _accumulatedForce += force;
  }

  public void AddForceAtPosition(Vector3 force, Vector3 position) {
    Vector3 toCenterOfMass = this.transform.TransformPoint(_body.centerOfMass) - position;

    float forceCMAngle = Vector3.Angle(force, toCenterOfMass);

    // Linear force
    Vector3 linForce = force * Mathf.Cos(forceCMAngle);
    _accumulatedForce += linForce;

    // Torque
    Vector3 torqueVector = force * Mathf.Sin(forceCMAngle) * toCenterOfMass.magnitude;

    _accumulatedTorque += Vector3.Cross(torqueVector, toCenterOfMass);
  }

  public void AddShipAlignedTorque(Vector3 shipAlignedTorque) {
    _accumulatedTorque += this.transform.rotation * shipAlignedTorque;
  }

  public void AddShipAlignedForce(Vector3 shipAlignedForce) {
    _accumulatedForce += this.transform.rotation * shipAlignedForce;
  }

  private void Update() {
    Vector3 acceleration = _accumulatedForce / _mass;
    _velocity += acceleration * Time.deltaTime;

    this.transform.position += _velocity * Time.deltaTime;
    this.rigidbody.velocity = _velocity;

    _accumulatedForce = Vector3.zero;

    //// torque + inertia tensor calculations (not working right)
    ////Quaternion worldInertiaTensorRot = this.transform.rotation * _body.inertiaTensorRotation;
    ////Vector3 eulerAcceleration = worldInertiaTensorRot * ((Quaternion.Inverse(worldInertiaTensorRot) * accumulatedTorque).CompDiv(Vector3.one/*_body.inertiaTensor*/));

    Vector3 eulerAcceleration = _accumulatedTorque;
    _angularVelocity += eulerAcceleration * Time.deltaTime;

    this.rigidbody.angularVelocity = _angularVelocity;
    this.transform.rotation = Quaternion.Euler(_angularVelocity * Time.deltaTime) * this.transform.rotation;

    _accumulatedTorque = Vector3.zero;
  }

}
