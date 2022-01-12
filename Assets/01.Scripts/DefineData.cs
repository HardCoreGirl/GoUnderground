static class DefineData
{
    public static float TEST_CRITICAL_RATE = 0.5f;

    public static int TEST_FLOOR = 3;
    public static int TEST_MY_WEAPON_INDEX = 1;

    public static int TEST_ENEMY_WEAPON_INDEX = 1;

    public static int MY_LEVEL = 30;
    public static int ENEMY_LEVEL = 1;

    // Game Setting ----
    public static float FIGHT_DISTANCE = 1.5f;
    public static float RUN_SPEED = 3f;
    // -----------------

    // UI Borad --------
    public static int UI_INGAME_BOARD_DEV = 0;
    public static int UI_INGAME_BOARD_PLAYING = 1;
    // -----------------

    // InGame State ----
    public static int INGAME_READY = 0;
    public static int INGAME_PLAY = 1;
    // -----------------

    public static int CHARACTER_TYPE_PLAYER = 0;
    public static int CHARACTER_TYPE_ENEMY = 10;


    public static int STATE_READY = 0;
    public static int STATE_ACTIVE = 1;
    public static int STATE_DEATH = 2;

    public static float TIME_SKILL_BUFF = 2.5f;

    public static int SKILL_STATE_HIT_CANCEL = 0;   // 공격당함, 캔슬됨
    public static int SKILL_STATE_NO_HIT_CANCEL = 1;   // 공격안당함, 캔슬됨
    public static int SKILL_STATE_HIT_NO_CANCEL = 2;   // 공격당함, 캔슬안됨
    public static int SKILL_STATE_NO_HIT_NO_CANCEL = 3;   // 공격 안담함, 캔슬 안됨

    public static int HIT_STATE_NORMAL = 0;   // 공격당함, 캔슬됨
    public static int HIT_STATE_NO_HIT_CANCEL = 1;   // 공격안당함, 캔슬됨
    public static int HIT_STATE_HIT_NO_CANCEL = 2;   // 공격당함, 캔슬안됨
    public static int HIT_STATE_NO_HIT_NO_CANCEL = 3;   // 공격 안담함, 캔슬 안됨

}