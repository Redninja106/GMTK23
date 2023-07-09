namespace GMTK23;

class Ending 
{
    public static Ending DeathFromAbove = new("Death from Above", "Crush the player with a boulder");
    public static Ending RoleRevised = new("Roles revised", "The player lit the torch problem-free");
    public static Ending AvatarAsphyxiation = new("Avatar asphyxiation", "The player asphyxiated in a cave.");
    public static Ending Dissappointment = new("My disappointment is immesurable, and my day is ruined", "The player's fire (mysteriously) went out.");
    public static Ending NoOneHeard = new("But no one heard", "The player is crushed by a tree.");
    public static Ending ThatWasEasy = new("Well, that was easy.", "Avatar gets lit torch from lit tree.");
    public static Ending DampenedDreams = new ("Dampened dreams", "The player's stick was wet.");
    public static Ending LikeABrawl = new ("It's like Brawl!", "The player tripped and fell.");
    public static Ending CelestialKidnapping = new("Celestial Kidnapping", "The player ascended into the heavens.");
    public static Ending QuickAndPainful = new("Quick and painful", "The player spontaneously combusted.");
    public static Ending ShouldveRespawn = new("Should've set a respawn point", "The player get locked out of the cave.");

    public string Title;
    public string Description;

    public Ending(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
