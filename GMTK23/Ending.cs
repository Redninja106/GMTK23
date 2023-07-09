namespace GMTK23;

class Ending 
{
    public static Ending DeathFromAbove { get; } = new("Death From Above", "The player was crushed with a boulder.");
    public static Ending RoleRevised { get; } = new("Roles Revised?", "You didn't do your job.");
    public static Ending AvatarAsphyxiation { get; } = new("Avatar Asphyxiation", "The player asphyxiated in a cave.");
    public static Ending DampenedDreams { get; } = new("Dampened Dreams", "My disappointment is immesurable, and my day is ruined.");
    public static Ending NoOneHeard { get; } = new("But No One Heard", "The player was crushed by a tree.");
    public static Ending ThatWasEasy { get; } = new("Well, That Was Easy", "The player got a lit torch from a lit tree.");
    public static Ending StickySituation { get; } = new ("Sticky Situation", "The player's stick was wet.");
    public static Ending ItsLikeBrawl { get; } = new ("It's Like Brawl!", "The player tripped and fell.");
    public static Ending CelestialKidnapping { get; } = new("Celestial Kidnapping", "The player ascended into the heavens.");
    public static Ending QuickAndPainful { get; } = new("Quick and Painful", "The player spontaneously combusted.");
    public static Ending ShouldveRespawn { get; } = new("No Way Home", "Should've set a respawn point.");

    public string Title;
    public string Description;

    public Ending(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
