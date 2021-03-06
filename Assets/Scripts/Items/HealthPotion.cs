﻿using UnityEngine;
using System;

[Serializable]
public class HealthPotion : Consumable{

    [SerializeField]
    private float RestoreXHP;

    /**
     * ConsumeEffectOn(Unit u)
     * @param Unit u - Restore \RestoreXHP\ hp to this unit, up to the calculated max hp
     */
    override public void ConsumeEffectOn(Unit u) {
        if (u.Hp + RestoreXHP > u.CalculateMaxHp()) {
            u.Hp = u.CalculateMaxHp();
        } else {
            u.Hp += RestoreXHP;
        }
        if (u.GetType() == typeof(Player)) {
            Player p = u as Player;
            p.GetHPBar().UpdateBar(p.Hp,p.CalculateMaxHp());
        }
    }
}