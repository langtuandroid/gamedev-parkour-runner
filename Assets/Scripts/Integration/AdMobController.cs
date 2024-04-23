using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Integration  
{
	/// <summary>
	/// https://github.com/googleads/googleads-mobile-unity/tree/main/samples/HelloWorld/Assets/Scripts
	/// https://github.com/googleads/googleads-mobile-unity/releases
	/// https://developers.google.com/admob/unity/interstitial#ios
	/// </summary>

	public class AdMobController : MonoBehaviour
	{
		public static AdMobController Instance { get; private set; }
		public event Action OnInterstitialAdClosed;
		public event Action GetReward;
		public bool ShowingBanner { get; private set; }

		private bool _noAds;
		public string noAdsKey = "NoAds";

		[SerializeField] private AdMobSettings _settings;
		[SerializeField] private bool IsProdaction;

		[SerializeField] private BannerViewController _bannerViewController;
		[SerializeField] private InterstitialAdController _interstitialAdController;
		[SerializeField] private RewardedAdController _rewardedAdController;
		

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
				MobileAds.Initialize(initStatus => { });
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			_bannerViewController.BannerId = IsProdaction ? _settings.BannerID : _settings.BannerTestID;
			_interstitialAdController.InterstitialId = IsProdaction ? _settings.InterstitialID : _settings.InterstitialTestID;
			_rewardedAdController.RewardedId = IsProdaction ? _settings.RewardedID : _settings.RewardedTestID;
			LoadAllAds();
		}

		private void LoadAllAds()
		{
			_noAds = PlayerPrefs.GetInt(noAdsKey, 0) == 1;
			Debug.Log("_noAds=" +_noAds);
			if (!_noAds)
			{
				//RequestBanner();
				RequestInterstitialAd();
			}

			RequestRewardedAd();
		}


		public void RemoveAds()
		{
			PlayerPrefs.SetInt(noAdsKey, 1);
			ShowBanner(false);
		}


// Banner	
		public void RequestBanner()
		{
			_bannerViewController.CreateBannerView();
			_bannerViewController.LoadAd();
			_bannerViewController.HideAd();
			ShowBanner(true);
		}

		public void ShowBanner(bool show)
		{
			_noAds = PlayerPrefs.GetInt(noAdsKey, 0) == 1;
			if (!_noAds)
			{
				if (show)
				{
					_bannerViewController.ShowAd();
					ShowingBanner = true;
				}
				else
				{
					_bannerViewController.HideAd();
					ShowingBanner = false;
				}
			}
		}

// Interstitial		
		public void RequestInterstitialAd()
		{
			_interstitialAdController.LoadAd();
		}

		public void ShowInterstitialAd()
		{
			_noAds = PlayerPrefs.GetInt(noAdsKey, 0) == 1;
			if (!_noAds)
			{
				RequestInterstitialAd();
				_interstitialAdController.ShowAd();
				// После показа интерстишл рекламы, вызываем событие OnAdClosed
				_interstitialAdController.OnAdClosed += () =>
				{
					OnInterstitialAdClosed?.Invoke();
				};
				
			}
		}

// Rewarded			
		public void RequestRewardedAd()
		{
			_rewardedAdController.LoadAd();
		}

		public void ShowRewardedAd()
		{
			_rewardedAdController.ShowAd();
			_rewardedAdController.GetRewarded += () =>
			{
				GetReward?.Invoke();
			};
			RequestRewardedAd();
			
		}

	}
}


