using System;
using Integration;
using MainControllers;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class IAPService : MonoBehaviour, IStoreListener 
    {
        private static IStoreController _storeController;
        private static IExtensionProvider _extensionsProvider;
        public static IAPService Instance { get; private set; }
        
        private const string subscriptionMonthProductID = "sub.gamedev.test.one.month";
        private const string subscriptionYearProductID = "sub.gamedev.test.one.year";
        private const string subscriptionForeverProductID = "sub.gamedev.test.forever ";
        
        public const string buy100Id = "test.gamedev.buy100";
        public const string buy300Id = "test.gamedev.buy300";
        public const string buy1000Id = "test.gamedev.buy1000";
        public const string buy3000Id = "test.gamedev.buy3000";

        [SerializeField]
        public Toggle _toggleMonth;
        [SerializeField]
        public Toggle _toggleYear;
        [SerializeField]
        public Toggle _toggleForever;
        
        [SerializeField]
        public Button _buySubscriptionButton;
        
        [SerializeField]
        private MainMenupr _mainMenupr;
        [SerializeField]
        private GameObject _subscriptionCanvas;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                if (_storeController == null)
                {
                    InitializePurchasing();
                    CheckSubscriptionStatus();
                }
                else
                {
                    string nameOfError = "error _storeController = null";
                    Debug.Log(nameOfError);
                }
            }
            else
            {
                Destroy(gameObject);
            }
            _buySubscriptionButton.onClick.AddListener(BuySubscription);
        }

        private void OnDestroy()
        {
            _buySubscriptionButton.onClick.RemoveListener(BuySubscription);
        }

        public void ShowSubscriptionPanel()
        {
            _subscriptionCanvas.SetActive(true);
            AdMobController.Instance.ShowBanner(false);
        }
        
        public void HideSubscriptionPanel()
        {
            _subscriptionCanvas.SetActive(false);
            AdMobController.Instance.ShowBanner(true);
        }
        
        
        private void CheckSubscriptionStatus()
        {
            // Product productMonth = GetProduct(subscriptionMonthProductID);
            // Product productYear = GetProduct(subscriptionYearProductID);
            // Product productForever = GetProduct(subscriptionForeversubProductID);
            //
            // if (productMonth != null && productMonth.hasReceipt )
            // {
            //     if (_subscriptionCanvas != null)
            //     {
            //         _subscriptionCanvas.SetActive(false);
            //     }
            // }
            // if (productYear != null && productYear.hasReceipt )
            // {
            //     if (_subscriptionCanvas != null)
            //     {
            //         _subscriptionCanvas.SetActive(false);
            //     }
            // }
            // if (productForever != null && productForever.hasReceipt )
            // {
            //     if (_subscriptionCanvas != null)
            //     {
            //         _subscriptionCanvas.SetActive(false);
            //     }
            // }
            string[] productIds = new string[] { subscriptionMonthProductID, subscriptionYearProductID, subscriptionForeverProductID };

            bool subscriptionActive = false;

            foreach (string productId in productIds)
            {
                Product product = GetProduct(productId);
                if (product != null && product.hasReceipt)
                {
                    subscriptionActive = true;
                    break;
                }
            }

            PlayerPrefs.SetInt(AdMobController.Instance.noAdsKey, subscriptionActive ? 1 : 0);
            
            if (subscriptionActive && _subscriptionCanvas != null)
            {
                HideSubscriptionPanel();
            }
        }


        public bool IsInitialized()
        {
            return _storeController != null && _extensionsProvider != null;
        }

        private void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(subscriptionMonthProductID, ProductType.Subscription);
            builder.AddProduct(subscriptionYearProductID, ProductType.Subscription);
            builder.AddProduct(subscriptionForeverProductID, ProductType.Subscription);
            
            builder.AddProduct(buy100Id, ProductType.Consumable);
            builder.AddProduct(buy300Id, ProductType.Consumable);
            builder.AddProduct(buy1000Id, ProductType.Consumable);
            builder.AddProduct(buy3000Id, ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }
        
        public Product GetProduct(string productID)
        {
            if (IsInitialized())
            {
                return _storeController.products.WithID(productID);
            }

            return null;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            
            Debug.Log("OnInitialized: SUCSESS");
            _storeController = controller;
            _extensionsProvider = extensions;
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
            else
            {
                Debug.LogError("No subscription type selected.");
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
            if (IsInitialized())
            {
                _storeController.InitiatePurchase(productId);
                Product product = _storeController.products.WithID(productId);

                if (product is {availableToPurchase: true})
                {
                    Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
                }
                else
                {
                    Debug.Log("Failed to purchase subscription. Product is not available.");
                }
            }
            else
            {
                Debug.Log("[STORE NOT INITIALIZED]");
            }
        }
        

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if (String.Equals(args.purchasedProduct.definition.id, subscriptionMonthProductID, StringComparison.Ordinal))
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
                AdMobController.Instance.RemoveAds();
                HideSubscriptionPanel();
            }
            else if (String.Equals(args.purchasedProduct.definition.id, subscriptionYearProductID, StringComparison.Ordinal))
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
                AdMobController.Instance.RemoveAds();
                HideSubscriptionPanel();
            }
            else if (String.Equals(args.purchasedProduct.definition.id, subscriptionForeverProductID, StringComparison.Ordinal))
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
                AdMobController.Instance.RemoveAds();
                HideSubscriptionPanel();
            }
            else if (String.Equals(args.purchasedProduct.definition.id, buy100Id, StringComparison.Ordinal))
            {
                _mainMenupr.AddDiamondpr(100);
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, buy300Id, StringComparison.Ordinal))
            {
                _mainMenupr.AddDiamondpr(300);
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, buy1000Id, StringComparison.Ordinal))
            {
                _mainMenupr.AddDiamondpr(1000);
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, buy3000Id, StringComparison.Ordinal))
            {
                _mainMenupr.AddDiamondpr(3000);
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            }
            else
            {
                Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
            }
        
            return PurchaseProcessingResult.Complete;
        }
        
        public void RestorePurchases()
        {
            if (IsInitialized() && _extensionsProvider != null)
            {
                Debug.Log("Restoring purchases...");

                _extensionsProvider.GetExtension<IAppleExtensions>()?.RestoreTransactions(OnRestoreComplete);
            }
            else
            {
                Debug.Log("[STORE NOT INITIALIZED]");
            }
            _subscriptionCanvas.SetActive(false);
        }

        private void OnRestoreComplete(bool success)
        {
            if (success)
            {
                Debug.Log("Purchases successfully restored.");
            }
            else
            {
                Debug.Log("Failed to restore purchases.");
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log(
                $"OnPurchaseFailed: FAIL. Products: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string? message)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }                
    }