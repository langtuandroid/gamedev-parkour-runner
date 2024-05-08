using System;
using System.Collections.Generic;
using Integration;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;
using Zenject;


public class IAPService : MonoBehaviour, IDetailedStoreListener 
    {
        private static IStoreController _storeController;
        private static IExtensionProvider _extensionsProvider;

        public event Action OnPurchaseDiamonds;
        private const string subscriptionMonthProductID = "sub.deletead.month";
        private const string subscriptionYearProductID = "sub.deletead.year";
        private const string subscriptionForeverProductID = "sub.deletead.foreveringame";
        
        public const string buy100Id = "buy.parkur.diamonds100";
        public const string buy300Id = "buy.parkur.diamonds300";
        public const string buy1000Id = "buy.parkur.diamonds1000";
        public const string buy3000Id = "buy.parkur.diamonds3000";

        [SerializeField]
        public Toggle _toggleMonth;
        [SerializeField]
        public Toggle _toggleYear;
        [SerializeField]
        public Toggle _toggleForever;
        
        [SerializeField]
        public Button _buySubscriptionButton;
        [SerializeField]
        public Button _closeSubpanel;
        
        [SerializeField]
        private GameObject _subscriptionCanvas;
        
        private AdMobController _adMobController;
       

        [Inject]
        private void Construct (AdMobController adMobController)
        {
            _adMobController = adMobController;
        }

        private void Awake()
        {
            if (_storeController == null)
            {
                InitializePurchasing();
            }
            else
            {
                string nameOfError = "error _storeController = null";
                Debug.Log(nameOfError);
            }
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            _buySubscriptionButton.onClick.AddListener(BuySubscription);
            _closeSubpanel.onClick.AddListener(HideSubscriptionPanel);
        }

        private void OnDisable()
        {
            _buySubscriptionButton.onClick.RemoveListener(BuySubscription);
            _closeSubpanel.onClick.RemoveListener(HideSubscriptionPanel);
        }

        public void ShowSubscriptionPanel()
        {
            var adsstatus = PlayerPrefs.GetInt("NoAds",0) == 1;
            if (adsstatus)
            {
                return;
            }
            _subscriptionCanvas.SetActive(true);
            _adMobController.ShowBanner(false);
        }
        
        public void HideSubscriptionPanel()
        {
            _subscriptionCanvas.SetActive(false);
            _adMobController.ShowBanner(true);
        }

        private void CheckSubscriptionStatus()
        {
            string[] productIds = { subscriptionForeverProductID,subscriptionMonthProductID, subscriptionYearProductID };

            bool subscriptionActive = false;

            foreach (string productId in productIds)
            {
                var subscriptionProduct = _storeController.products.WithID(productId);

                try
                {
                    var isSubscribed = IsSubscribedTo(subscriptionProduct);
                    string isSubscribedText = isSubscribed ? "You are subscribed" : "You are not subscribed";
                    Debug.Log("isSubscribedText = " + isSubscribedText);
                    subscriptionActive = isSubscribed;
                    if (subscriptionActive)
                    {
                        break;
                    }
                }
                catch (StoreSubscriptionInfoNotSupportedException)
                {
                    var receipt = (Dictionary<string, object>)MiniJson.JsonDecode(subscriptionProduct.receipt);
                    var store = receipt["Store"];
                    string isSubscribedText =
                        "Couldn't retrieve subscription information because your current store is not supported.\n" +
                        $"Your store: \"{store}\"\n\n" +
                        "You must use the App Store, Google Play Store or Amazon Store to be able to retrieve subscription information.\n\n" +
                        "For more information, see README.md";
                    Debug.Log("isSubscribedText = " + isSubscribedText);
                }
            }
            PlayerPrefs.SetInt(_adMobController.noAdsKey, subscriptionActive ? 1 : 0);
            PlayerPrefs.Save();
            if (subscriptionActive)
            {
                HideSubscriptionPanel();
            }
            else
            {
                ShowSubscriptionPanel();
            }
        }
        
        bool IsSubscribedTo(Product subscription)
        {
            // If the product doesn't have a receipt, then it wasn't purchased and the user is therefore not subscribed.
            if (subscription.receipt == null)
            {
                return false;
            }
            //The intro_json parameter is optional and is only used for the App Store to get introductory information.
            var subscriptionManager = new SubscriptionManager(subscription, null);
            // The SubscriptionInfo contains all of the information about the subscription.
            // Find out more: https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPSubscriptionProducts.html
            var info = subscriptionManager.getSubscriptionInfo();
            return info.isSubscribed() == Result.True;
        }

        public bool IsInitialized()
        {
            return _storeController != null && _extensionsProvider != null;
        }

        private void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule
#if UNITY_IOS
            .Instance(AppStore.AppleAppStore));
#elif UNITY_ANDROID
            .Instance(AppStore.GooglePlay));
#else
            .Instance(AppStore.NotSpecified));
#endif

            builder.AddProduct(subscriptionMonthProductID, ProductType.Subscription);
            builder.AddProduct(subscriptionYearProductID, ProductType.Subscription);
            builder.AddProduct(subscriptionForeverProductID, ProductType.NonConsumable);
            
            builder.AddProduct(buy100Id, ProductType.Consumable);
            builder.AddProduct(buy300Id, ProductType.Consumable);
            builder.AddProduct(buy1000Id, ProductType.Consumable);
            builder.AddProduct(buy3000Id, ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }
        
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized: SUCSESS");
            _storeController = controller;
            _extensionsProvider = extensions;
            CheckSubscriptionStatus();
        }

        private void BuySubscription()
        {
            if (_toggleMonth.isOn)
            {
                BuyProductID(subscriptionMonthProductID);
            }
            else if (_toggleYear.isOn)
            {
                BuyProductID(subscriptionYearProductID);
            }
            else if (_toggleForever.isOn)
            {
                BuyProductID(subscriptionForeverProductID);
            }
        }
        
        public void BuyPack1()
        {
            BuyProductID(buy100Id);
        }

        public void BuyPack2()
        {
            BuyProductID(buy300Id);
        }

        public void BuyPack3()
        {
            BuyProductID(buy1000Id);
        }
        
        public void BuyPack4()
        {
            BuyProductID(buy3000Id);
        }

        public void BuyProductID(string productId)
        {
            _storeController.InitiatePurchase(productId);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            var product = args.purchasedProduct;
            if (product.definition.id == subscriptionMonthProductID)
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{product.definition.id}'");
                _adMobController.RemoveAds();
                HideSubscriptionPanel();
            }
            if (product.definition.id == subscriptionYearProductID)
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{product.definition.id}'");
                _adMobController.RemoveAds();
                HideSubscriptionPanel();
            }
            if (product.definition.id == subscriptionForeverProductID)
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{product.definition.id}'");
                _adMobController.RemoveAds();
                HideSubscriptionPanel();
            }
            if (product.definition.id == buy100Id)
            {
                PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 100);
                PlayerPrefs.Save();
                OnPurchaseDiamonds?.Invoke();
                Debug.Log($"ProcessPurchase: PASS. Product: '{product.definition.id}'");
            }
            if (product.definition.id == buy300Id)
            {
                PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 300);
                PlayerPrefs.Save();
                OnPurchaseDiamonds?.Invoke();
                Debug.Log($"ProcessPurchase: PASS. Product: '{product.definition.id}'");
            }
            if (product.definition.id == buy1000Id)
            {
                PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 1000);
                PlayerPrefs.Save();
                OnPurchaseDiamonds?.Invoke();
                Debug.Log($"ProcessPurchase: PASS. Product: '{product.definition.id}'");
            }
            if (product.definition.id == buy3000Id)
            {
                PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + 3000);
                PlayerPrefs.Save();
                OnPurchaseDiamonds?.Invoke();
                Debug.Log($"ProcessPurchase: PASS. Product: '{product.definition.id}'");
            }

            return PurchaseProcessingResult.Complete;
        }
        
        public void RestorePurchases()
        {
            if (IsInitialized())
            {
                Debug.Log("Restoring purchases...");
                _extensionsProvider.GetExtension<IAppleExtensions>()?.RestoreTransactions(OnRestore);
            }
            else
            {
                Debug.Log("[STORE NOT INITIALIZED]");
            }
        }

        private void OnRestore(bool success, string error)
        {
            var restoreMessage = "";
            if (success)
            {
                restoreMessage = "Restore Successful";
            }
            else
            {
                restoreMessage = $"Restore Failed with error: {error}";
            }
            Debug.Log(restoreMessage);
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log($"OnPurchaseFailed: {product}. {failureDescription}");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"OnPurchaseFailed: FAIL. Products: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string? message)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }                
    }
