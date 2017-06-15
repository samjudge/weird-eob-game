﻿using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class ItemFactory : MonoBehaviour
{
    //Weapons
    public Weapon Fist;
    public Weapon Fangs;
    public Weapon Sword;

    //Items
    public Item Key;

    public Item MakeItem(string name)
    {
        FieldInfo Property = this.GetType().GetField(name);
        Item ItemPrefab = Property.GetValue(this) as Item;
        Item nItem = Instantiate(ItemPrefab, new Vector3(0,0,0), Quaternion.Euler(15, 180, 0)) as Item;
        return nItem;
    }
}