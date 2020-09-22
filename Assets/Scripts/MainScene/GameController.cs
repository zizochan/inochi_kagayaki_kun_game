using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    float score;
    float restCreateEnemyTime;

    GameObject enemyObject;
    TextController scoreText;

    private AudioSource audioSource;
    public AudioClip bombSound;
    float soundVolume = 1f;

    const int INITIAL_ENEMY_COUNT = 1;
    const float INITIAL_REST_CREATE_ENEMY_TIME = 5f;
    const float MIN_CREATE_ENEMY_TIME = 0.7f;
    const float DEFAULT_ENEMY_Y_POSITION = 9.3f;
    const float DEFAULT_ENEMY_SCALE = 3.5f;

    int enemyCreatePosition = 0; // どの位置で作ったか

    public CameraShake shake;

    // Start is called before the first frame update
    void Start()
    {
        Data.ChangeStatus(Data.STATUS_INITIAL);

        FadeManager.FadeIn(0.5f);

        SetInstances();
        ResetGame();

        CreateInitialEnemies();

        Data.ChangeStatus(Data.STATUS_PLAY);
    }

    void ResetGame()
    {
        score = 0f;
        Data.tmpScore = 0;

        restCreateEnemyTime = INITIAL_REST_CREATE_ENEMY_TIME;

        enemyCreatePosition = 0;
    }

    void SetInstances()
    {
        enemyObject = (GameObject)Resources.Load("KagayakiKun");
        scoreText = GameObject.Find("ScoreText").GetComponent<TextController>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        if (!Data.IsGamePlay())
        {
            return;
        }

        PastTime(deltaTime);
        AddScore(deltaTime);
    }

    void PastTime(float deltaTime)
    {
        restCreateEnemyTime -= deltaTime;
        if (restCreateEnemyTime < 0f)
        {
            CreateEnemy();
        }
    }

    void CreateEnemy()
    {
        string objectName = "enemy";
        float posX = ChoiceEnemyX();
        float posY = DEFAULT_ENEMY_Y_POSITION;
        float scaleX = DEFAULT_ENEMY_SCALE * 1.1f; // 少し横幅にしておく
        float scaleY = DEFAULT_ENEMY_SCALE;
        float rotation = ChoiceEnemyRotation();

        GameObject enemy = CreateObject(enemyObject, posX, posY, objectName, scaleX, scaleY, rotation);

        ResetRestCreateEnemyTime();
    }

    void ResetRestCreateEnemyTime()
    {
        restCreateEnemyTime = GetNextRestCreateEnemyTime();
    }

    float GetNextRestCreateEnemyTime()
    {
        float nextTime = INITIAL_REST_CREATE_ENEMY_TIME;
        float minTime = MIN_CREATE_ENEMY_TIME;

        nextTime -= (GetTrueScore() * 0.01f);

        if (nextTime < minTime)
        {
            nextTime = minTime;
        }

        return nextTime;
    }

    private GameObject CreateObject(GameObject baseObject, float x, float y, string objectName = null, float scaleX = 1f, float scaleY = 1f, float rotation = 0f)
    {
        Quaternion rote = Quaternion.Euler(0f, 0f, rotation);
        GameObject instance = (GameObject)Instantiate(baseObject, new Vector2(x, y), rote);
        instance.transform.localScale = new Vector2(scaleX, scaleY);

        if (objectName != null)
        {
            instance.name = objectName;
        }

        return instance;
    }

    float ChoiceEnemyX()
    {
        float minX;
        float maxX;

        switch(enemyCreatePosition)
        {
            // 次は中奥に作る
            case 1:
                minX = 2f;
                maxX = 9f;
                enemyCreatePosition = 2;
                break;
            // 次は右に作る
            case 2:
                minX = -4f;
                maxX = 4f;
                enemyCreatePosition = 0;
                break;
            // 次は左に作る
            default:
                minX = -9f;
                maxX = -2f;
                enemyCreatePosition = 1;
                break;
        }

        return Random.Range(minX, maxX);
    }

    float ChoiceEnemyRotation()
    {
        return Random.Range(0f, 360f);
    }

    public void ExecuteGameOver()
    {
        if (Data.CHEAT_MUTEKI)
        {
            return;
        }

        Data.ChangeStatus(Data.STATUS_GAMEOVER);

        PlaySound(bombSound);
        shake.Shake(3f, 0.5f);

        Data.tmpScore = GetTrueScore();

        FadeManager.FadeOut("GameOverScene", 2f);
    }

    void PlaySound(AudioClip clip)
    {
        if (Data.IsBgmStop())
        {
            return;
        }

        audioSource.PlayOneShot(clip, soundVolume);
    }

    void AddScore(float deltaTime)
    {
        score += deltaTime;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        string text = "SCORE: " + GetTrueScore().ToString();
        scoreText.SetText(text);
    }

    int GetTrueScore()
    {
        return (int)(score * 5);
    }

    void CreateInitialEnemies()
    {

        for (int i = 0; i < INITIAL_ENEMY_COUNT; i++)
        {
            CreateEnemy();
        }

        // ２体目はすぐに作る
        restCreateEnemyTime = INITIAL_REST_CREATE_ENEMY_TIME * 0.5f;
    }
}
