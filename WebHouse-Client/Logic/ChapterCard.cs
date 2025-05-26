using WebHouse_Client.Components;
using WebHouse_Client.Networking;

namespace WebHouse_Client.Logic;

public class ChapterCard : ILogicCard
{
    public Room.RoomName Chapter { get; } //Raumname der ChapterCard
    public int Steps { get; } //Anzahl an Schritten die der Spieler gehen darf nachdem er die Karte erfüllt hat
    public List<CardColor> Requirements { get; } //Die Farben die der Spieler anlegen muss
    public int Counter;
    public bool IsSpecial { get; } //Ob die Karte eine SpecialCard ist
    public List<EscapeCard> PlacedCards { get; } = new(); //Liste der EscapeCards die auf die ChapterCard gelegt wurden
    
    public IComponentCard? Component { get; private set; } //Die Komponente die die Karte darstellt

    public ChapterCard(Room.RoomName chapter, int steps, List<CardColor> requirements, bool isSpecial = false)
    {
        Chapter = chapter;
        Steps = steps;
        Requirements = requirements;
        IsSpecial = isSpecial;
    }
    //Erzeugt das Panel das die Karte darstellt
    public void CreateComponent()
    {
        if (Component != null)
            Component.Panel.Dispose();
        Component = new Components.ChapterCard(this);
    }
    
    //Überorüft ob die EscapeCard an die ChapterCard angelegt werden kann
    public bool DoesEscapeCardMatch(EscapeCard escapeCard)
    {
        return escapeCard.Type == EscapeCard.EscapeCardType.Normal && (Requirements.Contains(escapeCard.Color) || (IsSpecial && escapeCard.Room == Chapter)) && escapeCard.Number >= Counter;
    }
    //Legt eine EscapeCard auf die ChapterCard und löscht dabei die Anforderungen
    public void AddEscapeCard(EscapeCard escapeCard)
    {
        if (Component == null)
            return;
        if (!DoesEscapeCardMatch(escapeCard)) //Wenn die karte nicht angelegt werden kann wird die Methode beendet
            return;
        
        
        Counter = escapeCard.Number; //Aktualisiert den Counter,der anzeigt welche EscapeCards angelegt werden dürfen, der ChapterCard auf die Nummer der EscapeCard
        Requirements.Remove(IsSpecial ? CardColor.White : escapeCard.Color); //Entfernt die Farbe auf der ChapterCard die die gerade anglegte EscapeCard hat
        PlacedCards.Add(escapeCard);

        //Wird ausgeführt wenn die Karte keine Anforderungen mehr hat
        if (Requirements.Count == 0)
        {
            //Überprüft ob die Karte eine SpecialCard ist
            if (!IsSpecial)
            {
                //Wenn die Karte keine SpecialCard ist wird die ChapterCard gelöscht und der Ablagestapel auf der sie lag wird aktiviert
                ((Components.ChapterCard)Component).Pile.Panel.Enabled = true;
                ((Components.ChapterCard)Component).Pile.Panel.Visible = true;
                Component.Panel.Dispose();
                GameLogic.PlacedChapterCards.Remove(this);
            }
            
            //Wird vom Host audgeführt
            if (NetworkManager.Instance.LocalPlayer.IsHost)
            {
                NetworkManager.Rpc.MovePlayer(Steps); //Der Host bewegt den Spieler um die Anzahl an Schritten auf der ChapterCard 
                GameLogic.CurrentEscapeCards.AddRange(PlacedCards); //Fügt die EscapeCards die auf die ChapterCard gelegt wurden zu einer Liste hinzu
            }
        }
    }
}