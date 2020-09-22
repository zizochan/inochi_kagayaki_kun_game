using System.Collections.Generic;

public static class Data
{
    // other
    public static string VERSION = "1.0.1";

    // スコア記録関連
    public static int tmpScore = 0; // シーン移動に使う

    // フラグ管理
    public static int status;
    public const int STATUS_INITIAL = 0; // 初期状態
    public const int STATUS_PLAY = 10; // プレイ中
    public const int STATUS_GAMEOVER = 99; // ゲームオーバー

    // cheat
    public const bool CHEAT_MUTEKI = false;

    // ゲームモード
    public const int GAME_MODE_A = 0;
    public const int GAME_MODE_B = 1;
    public static int GAME_MODE = GAME_MODE_A;

    public static void ChangeStatus(int value)
    {
        status = value;
    }

    public static bool IsGamePlay()
    {
        return status == STATUS_PLAY;
    }

    public static bool IsBgmStop()
    {
        return false;
    }
}
