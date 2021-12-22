static class DefineData
{
    public static float TEST_CRITICAL_RATE = 0.5f;
    public static int TEST_MY_WEAPON_MIN = 10;
    public static int TEST_MY_WEAPON_MAX = 30;

    public static int TEST_ENEMY_WEAPON_MIN = 400;
    public static int TEST_ENEMY_WEAPON_MAX = 500;
    


    public static int MY_LEVEL = 30;
    public static int ENEMY_LEVEL = 100;

    // Game Setting ----
    public static float FIGHT_DISTANCE = 1.5f;
    public static float RUN_SPEED = 3f;
    // -----------------


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