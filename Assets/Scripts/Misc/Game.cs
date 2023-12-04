
public static class Game
{
    public enum State
    {
        MainMenu,
        Pause,
        Death,
        Win,
        Play
    }

    public static State state = State.MainMenu;
    
    public static int level;
    
    public static bool musicOn = true;
    public static bool soundsOn = true;

    public static int enemiesKilled;
}
