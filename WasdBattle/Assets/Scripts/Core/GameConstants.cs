namespace WasdBattle.Core
{
    /// <summary>
    /// Oyun sabitleri
    /// </summary>
    public static class GameConstants
    {
        // Scene isimleri
        public const string SCENE_MAIN_MENU = "MainMenu";
        public const string SCENE_CHARACTER_SELECT = "CharacterSelect";
        public const string SCENE_COMBAT = "Combat";
        public const string SCENE_LOBBY = "Lobby";
        
        // Layer'lar
        public const string LAYER_PLAYER = "Player";
        public const string LAYER_UI = "UI";
        
        // Tag'ler
        public const string TAG_PLAYER = "Player";
        public const string TAG_ENEMY = "Enemy";
        
        // Progression
        public const int MAX_LEVEL = 100;
        public const int STARTING_ELO = 1000;
        public const int MIN_ELO = 0;
        public const int MAX_ELO = 3000;
        
        // Combat
        public const int MAX_ROUNDS = 10;
        public const float TURN_DURATION = 10f;
        public const float COMBO_BASE_TIME = 3f;
        
        // Economy
        public const int STARTING_GOLD = 100;
        public const int STARTING_METAL = 50;
        public const int STARTING_CRYSTAL = 50;
        public const int STARTING_RUNE = 10;
        public const int STARTING_ESSENCE = 5;
        
        // Matchmaking
        public const float MATCHMAKING_TIMEOUT = 60f;
        public const int MAX_PLAYERS_PER_MATCH = 2;
        
        // Network
        public const int NETWORK_TICK_RATE = 30;
        public const float NETWORK_TIMEOUT = 30f;
        
        // UI
        public const float UI_FADE_DURATION = 0.3f;
        public const float NOTIFICATION_DURATION = 3f;
        
        // Audio
        public const float DEFAULT_MASTER_VOLUME = 1f;
        public const float DEFAULT_MUSIC_VOLUME = 0.7f;
        public const float DEFAULT_SFX_VOLUME = 1f;
    }
}

