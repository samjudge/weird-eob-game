﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Blocker, Openable {

    [SerializeField]
    private Player Player;
    [SerializeField]
    private Hand Hand;
    [SerializeField]
    private string NeedsKeyOfName = "";
    [SerializeField]
    private bool StartOpen = false;

    private bool IsOpened = false;
    private bool Locked = true;
    private bool InMotion = false;

    private void Start() {
        if (this.Player == null) {
            this.Player = GameObject.Find("Player").GetComponent<Player>() as Player;
        }
        if (this.Hand == null) {
            this.Hand = GameObject.Find("UI/UIBase/Hand").GetComponent<Hand>() as Hand;
        }
        if (StartOpen) {
            this.ToggleOpen();
        }
    }

    public bool IsLocked() {
        return Locked;
    }

    public void ToggleLock() {
        Locked = false;
    }

    public bool CanOpen() {
        return !InMotion;
    }

    public bool CanClose() {
        return !InMotion;
    }

    public void Open() {
        if (!IsOpened && CanOpen()) {
            IsOpened = !IsOpened;
            Player.GetActionLog().WriteNewLine("the door creeks open...");
            StartCoroutine(Slide(this.transform.position.y - 1));
        }
    }

    public void Close() {
        if (IsOpened && CanClose()) {
            IsOpened = !IsOpened;
            Player.GetActionLog().WriteNewLine("the door closes.");
            StartCoroutine(Slide(this.transform.position.y + 1));
        }
    }

    public void ToggleOpen() {
        if (!IsOpened) {
            Open();
        } else {
            Close();
        }
    }

    void OnMouseDown() {
        //attempt to unlock if locked
        if (IsLocked()) {
            if (this.NeedsKeyOfName != "") {
                GameObject Held = this.Hand.GetHeld();
                if (Held != null) {
                    Item Item = this.Hand.GetHeld().GetComponent<Item>() as Item;
                    if (Item.GetName() != NeedsKeyOfName) {
                        Player.GetActionLog().WriteNewLine("the key doesn't seem to fit.");
                    } else {
                        ToggleLock();
                    }
                } else {
                    Player.GetActionLog().WriteNewLine("the door is locked tight.");
                    return;
                }
            } else {
                ToggleLock();
            }
        }
        //open door if unlocked
        if ((this.transform.position - Player.transform.position).sqrMagnitude <= 1.2){
            if (!IsLocked()) {
                Open();
            }
        }
    }

    private IEnumerator Slide(float YDestination) {
        this.InMotion = true;
        Vector3 target = new Vector3(
            this.transform.position.x,
            YDestination,
            this.transform.position.z
        );
        float t = 0;
        Vector3 origin = this.transform.position;
        while ((this.transform.position - target).sqrMagnitude > Vector3.kEpsilon) {
            t += Time.deltaTime;
            this.transform.position = Vector3.Lerp(origin,target,t);
            yield return null;
        }
        this.InMotion = false;
        yield return null;
    }
}
