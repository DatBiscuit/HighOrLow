using System;

[Serializable]
public struct Card {

    public string suit,value,name;

    //Constructor to create a Card 
    public Card(string s, string v) {
        suit = s;
        value = v;
        name = s + v;
    }

    public static bool operator ==(Card a, Card b) {
        if(a.suit == b.suit) {
            if(a.value == b.value) {
                return true;
            }
        }

        return false;
    }

    public static bool operator !=(Card a, Card b) {
        return !(a == b);
    }
    
}
