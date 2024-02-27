using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class GameEvent
    {

    }
}
/*
 * Server Events
 * 
 * ShuffleAllCards
 * DealRoleCards
 * DealTwoCharacterCards
 * PlayerDealsDamage
 * PlayerHits
 * PlayerNoHit
 * 
 * 
 * Client Events
 * Host Game
 * Join Game
 * Start Game
 * Select Card Packs
 * Select Number of Roles
 * Receive Role Card
 * Receive Character Cards
 * Choose Character
 * DrawFromDeck
 * DrawFromOtherHand
 * DrawFromInPlay
 * DrawFromDiscard
 * PlayFromHand
 * PlayFromInPlay
 * DiscardFromHand
 * DiscardFromInPlay
 * 
 * 
 * 
 GameStart
RolesDecided
	Sheriff, Deputy, Renegade, Outlaw
CharacterDecided	
PlayerPlaysCard
PlayerDrawsCardFromDeck
PlayerDrawsCardFromPlayer
PlayerDrawsCardFromDiscard
ShuffleDrawPile
ClearDiscardPile
	Except top card
BlueCardPlayed
	Guns, Status
BrownCardPlayed
	Bang, Missed, etc
GreenCardPlayed
PlayerTakesDamage
PlayerKilled
SheriffDies
DeputyDies
RenegadeDies
OutlawDies

CharacterEvents

 */
