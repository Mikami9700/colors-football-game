﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SmartLocalization;
using ArabicSupport;

public class MainMenu : MonoBehaviour
{

    public string iOSURL = "";
    public string ANDROIDURL = "";
    public string fbPage = ""; //use "fb://page/pageID" instead of http:// eg:- ("fb://page/315797608481737")
    public string moreGames;

    private AudioSource sound;

    public Text bestScore,gameNameP1,gameNameP2;
    [SerializeField]
    private Sprite[] soundBtnSprites; //1 for off and 0 for on
    public Button playBtn, leaderboardBtn, rateBtn, fbLikeBtn, soundBtn, moreGamesBtn, noAdsBtn, slideBtn;
    public string gameScene,leaderScene,accountScene;

    [SerializeField]
    private Animator slideButtonAnim;

    private bool hidden;
    private bool canTouchSlideButton;


    // Use this for initialization
    void Start()
    {
        bestScore.text = "" + GameManager.instance.hiScore;
        canTouchSlideButton = true;
        hidden = true;

	
		string language = LanguageManager.Instance.GetSystemLanguageEnglishName ();
		if (LanguageManager.Instance.IsLanguageSupportedEnglishName (language)) {
			LanguageManager.Instance.ChangeLanguage (LanguageManager.Instance.GetDeviceCultureIfSupported ());
		} else {
			LanguageManager.Instance.ChangeLanguage ("en");
		}

		//LanguageManager.Instance.ChangeLanguage ("ar");


		if (true || LanguageManager.Instance.GetDeviceCultureIfSupported () != null && 
			LanguageManager.Instance.GetDeviceCultureIfSupported ().languageCode.Equals ("ar")) {

			gameNameP1.text = ArabicFixer.Fix (LanguageManager.Instance.GetTextValue ("GameName1"));
			gameNameP2.text = ArabicFixer.Fix (LanguageManager.Instance.GetTextValue ("GameName2"));
		} else {
			gameNameP1.text = LanguageManager.Instance.GetTextValue ("GameName1");
			gameNameP2.text = LanguageManager.Instance.GetTextValue ("GameName2");
		}


        sound = GetComponent<AudioSource>();
        playBtn.GetComponent<Button>().onClick.AddListener(() => { PlayBtn(); });    //play
        rateBtn.GetComponent<Button>().onClick.AddListener(() => { RateBtn(); });    //rate
        noAdsBtn.GetComponent<Button>().onClick.AddListener(() => { NoAdsBtn(); });    //noAds
        
        leaderboardBtn.GetComponent<Button>().onClick.AddListener(() => { LeaderboardBtn(); });    //leaderboard
        fbLikeBtn.GetComponent<Button>().onClick.AddListener(() => { FBlikeBtn(); });    //facebook
        soundBtn.GetComponent<Button>().onClick.AddListener(() => { SoundBtn(); });    //sound
        moreGamesBtn.GetComponent<Button>().onClick.AddListener(() => { MoregamesBtn(); });    //more games
        slideBtn.GetComponent<Button>().onClick.AddListener(() => { SlideBtn(); });    //slide

        if (GameManager.instance.isMusicOn)
        {
			AudioListener.volume = 1;
            //MusicController.instance.PlayBgMusic();
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = soundBtnSprites[0];

        }
        else
        {
			AudioListener.volume = 0;
            //MusicController.instance.StopBgMusic();
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = soundBtnSprites[1];

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayBtn()
    {
        sound.Play();
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
        SceneManager.LoadScene(gameScene);
#else
        Application.LoadLevel(gameScene);
#endif
    }

    void RateBtn()
    {
        sound.Play();
#if UNITY_IPHONE
		Application.OpenURL(iOSURL);
#endif

#if UNITY_ANDROID
        Application.OpenURL(ANDROIDURL);
#endif
        GameManager.instance.showRate = false;
        GameManager.instance.Save();
    }

    void SoundBtn()
    {

//		PlayBtn();
//        sound.Play();


      if (GameManager.instance.isMusicOn)
        {
			AudioListener.volume = 0;
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = soundBtnSprites[1];
            //MusicController.instance.StopBgMusic();
            GameManager.instance.isMusicOn = false;
            GameManager.instance.Save();
			sound.Play();

        }
        else
        {
			AudioListener.volume = 1;
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = soundBtnSprites[0];
            //MusicController.instance.PlayBgMusic();
            GameManager.instance.isMusicOn = true;
            GameManager.instance.Save();

        }
    }

    void FBlikeBtn()
    {
        sound.Play();
        Application.OpenURL(fbPage);
    }

    void LeaderboardBtn()
    {
        if(GameManager.instance.isUserRegistered)
		   SceneManager.LoadScene(leaderScene);
        else {
           SceneManager.LoadScene(accountScene);
        }
    }

    void MoregamesBtn()
    {
        sound.Play();
        Application.OpenURL(moreGames);
    }

    void NoAdsBtn()
    {
        sound.Play();
        //Purchaser.instance.BuyNoAds();
    }

    void SlideBtn()
    {
        sound.Play();
        StartCoroutine(DisableSlideBtnWhilePlayingAnimation());
    }

    IEnumerator DisableSlideBtnWhilePlayingAnimation()
    {
        if (canTouchSlideButton)
        {
            if (hidden)
            {
                canTouchSlideButton = false;
                slideButtonAnim.Play("SlideIn");
                hidden = false;
                yield return new WaitForSeconds(1.2f);
                canTouchSlideButton = true;

            }
            else
            {
                canTouchSlideButton = false;
                slideButtonAnim.Play("SlideOut");
                hidden = true;
                yield return new WaitForSeconds(1.2f);
                canTouchSlideButton = true;

            }

        }
    }

}