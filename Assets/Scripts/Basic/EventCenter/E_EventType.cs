public enum E_EventType
{
    // examples
    TEST_TYPE = 1,


    //ս�������ź�
    CALCULATE_BEGIN = 1000,
    FIGHT_TYPE = 1001, // �޶����������氤��ƴ���������ɹ� 0 1 2 3
    DAMAGE_CALC = 1002,
    ATTACK_FORCE = 1003,


    // ��Ϸ״̬�仯
    NEW_GAME_START = 2000,
    GAME_START = 2002,
    WIN = 2003,
    LOSE = 2004,
    SETTINGS = 2005,
    CLOSE_GAME = 2006,


    // UI���������
    PLAYER_DIE = 3000,
    ENEMY_DIE = 3001,
    PLAYER_HURT = 3002,
    BOSS_HURT = 3003,


    // ��Ұ������
    START_RECORD = 4000,   // ��ʼ��¼��ҵ�����
    PLAYER_INPUT_RES = 4001,   // ��Ұ������
    ADJUST_OFFSET = 4002,
    CAMERA_SHAKE = 4003,


    // ��ť�¼�
    LOAD_SCENE = 5000,
    NEXT_LEVEL = 5001,
    TRY_AGAIN = 5002,
    BACK_BACK = 5003,  // ������һ��ҳ����ǹرյ�ǰ����
    BACK_TO_MAIN_MENU = 5004,
    TYPER_GKD = 5005,


    // ս����ʾ
    // Ŀǰֻ����ǰ��֡���õ���ʾ
    BEAT_MIDPOINT_HINT = 6000,
    CREATE_BEAT_TIP_OBJ = 6001,
    BEATUP_AND_DURATION = 6002,
    NEXT_PLAYER_BEAT_TIME = 6003,

    //粒子摧毁
    PARTICLE_DESTROY = 7000,

    //选择奖励相关
    CHANGE_SKILL = 8000,
    VERIFY = 8001,
    CHANGE_PROPERTY = 8002,
    WIN_WINDOW = 8003,
    JUMP_CHANGE = 8004,
    TEST_SCENE = 8005,

    //战斗相关
    PLAYER_DAMAGE = 9000,
    ENEMY_DAMAGE = 9001,
    FULL_RECORVER = 9002,
    KILL_ENEMY = 9003,
    PAUSE_GAME = 9004,
    RESUME_GAME = 9005,
    USE_SKILL_SUCCESS = 9006,
    BUFF_SHOW = 9007,
    BUFF_REMOVE = 9008,
    BUFF_REFRESH = 9009,

    //描边显示
    LINE_COLOR = 10000,

}