using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class HighOrLow : MonoBehaviour {

    [SerializeField]
    GameObject answer = null;
    [SerializeField]
    GameObject faceCard = null;
    [SerializeField]
    GameObject hiddenCard = null;

    [SerializeField]
    List<string> suits;
    [SerializeField]
    List<string> values;
    [SerializeField]
    List<string> suitHierarchy; 

    [SerializeField]
    Button high = null;
    [SerializeField]
    Button low = null;

    [SerializeField]
    List<SpecialCards> specialCards;

    [SerializeField]
    Button newGame = null;

    List<Card> deck;
    
    private Card[] hand;

    /** 
    * Initialization of buttons and deck
    **/
    void Start() {
        high.onClick.AddListener(highChoice);
        low.onClick.AddListener(lowChoice);
        newGame.onClick.AddListener(newGamePressed);
        deck = new List<Card>();
        hand = new Card[2];
        answer.SetActive(false);
        enableButtons(false);
    }

    /**
    * When the user presses the High button
    **/
    void highChoice() {
        answerCheck(1);
        Invoke("Draw", 2.0f);
    }

    /**
    * When the user presses the Low button
    **/
    void lowChoice() {
        answerCheck(-1);
        Invoke("Draw", 2.0f);
    }

    /** 
    * Checks the users answer against the next card
    * @param name="choice": if == 1 then the user selected high else if == -1 the user selected low
    **/
    void answerCheck(int choice) {
        enableButtons(false);
        int comp = choice*compareValues(hand[0],hand[1]);
        if(comp == -1) {
            setAnswer("Right");
        } else if(comp == 1) {
            setAnswer("Wrong");
        } else {
            int compSuit = compareSuits(hand[0],hand[1]);
            if(compSuit == 1) {
                setAnswer("Wrong");
            } else if (compSuit == -1) {
                setAnswer("Right");
            }
        }
        changeCardImage(hiddenCard, hand[1], false);
        
    }

    /** 
    * Disables or Enables high & low buttons
    * @param name="isEnabled": boolean to determine if the buttons will be active or disabled
    **/
    void enableButtons(bool isEnabled) {
        high.gameObject.SetActive(isEnabled);
        low.gameObject.SetActive(isEnabled);
    }

    /**
    * Compare function to compare card's numerical value
    * @param name="one": first card which will be compared to the second Card
    * @param name="two": second card
    * @return: 1 if card one is higher 
    *         -1 if card two is higher
    *          0 if they are the same value
    **/
    int compareValues(Card one, Card two) {
        int valone = Convert.ToInt32(one.value);
        int valtwo = Convert.ToInt32(two.value);
        if(valone > valtwo) {
            return 1;
        } else if(valtwo > valone) {
            return -1;
        } else {
            return 0;
        }
    }

    /**
    * Sets the answer that will appear on the screen
    * @param name="a": the string that will be displayed to the screen
    **/
    void setAnswer(string a) {
        answer.SetActive(true);
        answer.GetComponent<Text>().text = a;
    }

    /**
    * Compare function to comapre card's suit value determined by the suitHierarchy variable
    * @param name="one": first card which will be compared to the second Card
    * @param name="two": second card
    * @return: 1 if card one is higher suit
    *         -1 if card two is higher suit
    *          0 if they are the same suit
    **/
    int compareSuits(Card one, Card two) {
        int indexone = suitHierarchy.IndexOf(one.suit);
        int indextwo = suitHierarchy.IndexOf(two.suit);
        if(indexone < indextwo) {
            return 1;
        } else if(indextwo < indexone) {
            return -1;
        } else {
            return 0;
        }
    }

    /**
    * Changes the hand's images 
    * @param name="c": GameObject where the image applies to
    * @param name="card": The card we want to display
    * @param name="hidden": whether or not we want it to be hidden and display the cardback 
    **/
    void changeCardImage(GameObject c, Card card, bool hidden) {
        if(hidden) {
            c.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardArt/CardBack");
        } else {
            c.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CardArt/" + card.name);
        }
    }

    /** 
    * Creates the deck
    **/
    void createDeck() {
        foreach(string s in suits) {
            foreach(string v in values) {
                Card card = new Card(s,v);
                int count = match(card);
                if(count != 0) {
                    while(count != 0) {
                        deck.Add(card);
                        count--;
                    }
                } else {
                    deck.Add(card);
                }
            }
        }
    }

    /**
    * function that determines if a card matches the special card list
    * @param name="c": the card we are checking
    * @return: 0 if the card is not a special cardback
               card's chance value if it is a special card
    **/
    private int match(Card c) {
        foreach(SpecialCards s in specialCards) {
            if(s.card == c) {
                return s.chance;
            }
        }
        return 0;
    }

    /**
    * Shuffles the deck using Fisher-Yates shuffle algorithm
    **/
    void shuffleDeck() {
        for(int i = 0; i < deck.Count - 1; i++) {
            int j = UnityEngine.Random.Range(0,deck.Count);
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    /**
    * Function that handles what happens when drawing such as setting images or enabling buttons
    **/
    void Draw() {
        answer.SetActive(false);
        if(deck.Count == 0) {
            newGame.gameObject.SetActive(true);
            return;
        }
        hand[0] = DrawCard();
        hand[1] = DrawCard();
        changeCardImage(faceCard, hand[0], false);
        changeCardImage(hiddenCard, new Card("0","0"), true);
        enableButtons(true);
    }

    /**
    * Draws a card and returns that card
    * @return: the card to be drawn from the deck 
    **/
    Card DrawCard() {
        Card card = deck[0];
        if(match(card) != 0) {
            deck.RemoveAll(c => c == card);
            return card;
        }
        deck.RemoveAt(0);
        return card;
        
    }

    /**
    * creates a new game
    **/
    void newGamePressed() {
        newGame.gameObject.SetActive(false);
        createDeck();
        shuffleDeck();
        Draw();    
    }

    
}
