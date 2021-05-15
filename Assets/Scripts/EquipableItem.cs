using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelloWorld;

[CreateAssetMenu(menuName = "Pickable Object")]
public class EquipableItem : ScriptableObject
{
	public Sprite image;
	public float speedBonus;
	public float attackDelayReducement;
	public float hpHeal;

	public void ChangeStats(HelloWorldPlayer player) {
		player.speed+=speedBonus;
		player.attackDelay-=attackDelayReducement;
		if (player.attackDelay<0.2f) player.attackDelay=0.2f;
	}
}