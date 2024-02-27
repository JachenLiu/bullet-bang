using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;

namespace Bang
{
    public abstract class CharacterCard : UnplayableCard
    {
		private string characterName;
		private string characterDescription;
		private int characterHealth;

		public int GetCharacterHealth()
		{
			return characterHealth;
		}
		public void SetHealth(int health)
		{
			characterHealth = health;
		}
		public string GetName()
		{
			return characterName;
		}
		public void SetName(string name)
		{
			characterName = name;
		}
		public string GetDescription()
		{
			return characterDescription;
		}
		public void SetDescription(string description)
		{
			characterDescription = description;
		}
	}
}

