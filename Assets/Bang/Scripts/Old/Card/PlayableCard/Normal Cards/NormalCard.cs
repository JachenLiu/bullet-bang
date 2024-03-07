using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
	public enum Suit
	{
		Spade, Heart, Club, Diamond
	};
	public enum Number
	{
		Ace = 1,
		Two = 2,
		Three = 3,
		Four = 4,
		Five = 5,
		Six = 6,
		Seven = 7,
		Eight = 8,
		Nine = 9,
		Ten = 10,
		Jack = 11,
		Queen = 12,
		King = 13
	};
	public abstract class NormalCard : PlayableCard
    {
		//every blue brown and green card has a number and a suit
		private string cardName;
		private Number cardNumber;
		private Suit cardSuit;

		public string GetCardName()
		{
			//associate with gameobject
			return cardName;
		}
		public void SetCardName(string cardName)
		{
			this.cardName = cardName;
		}
		public Number GetCardNumber()
		{
			return cardNumber;
		}
		public void SetCardNumber(Number cardNumber)
		{
			this.cardNumber = cardNumber;
		}

		public Suit GetCardSuit()
		{
			return cardSuit;
		}

		public void SetCardSuit(Suit cardSuit)
		{
			this.cardSuit = cardSuit;
		}


		public bool IsInHand()
		{
			//is card in player hand?
			return false;
		}

		public bool IsInPlay()
		{
			//in play or not for player?
			return false;
		}
	}
}
