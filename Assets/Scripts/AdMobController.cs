using System;
using UnityEngine;
using UnityEngine.Events;
using GoogleMobileAds.Api;

public class AdMobController : MonoBehaviour
{

#if UNITY_ANDROID
    [SerializeField] private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    [SerializeField] private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    [SerializeField] private string _adUnitId = "unused";
#endif
    [SerializeField] private float TiempoDeCiclo;
    [SerializeField] private GameObject BottonAdd;
    
    [SerializeField] private UnityEvent OnRewardOpen;
    [SerializeField] private UnityEvent OnReward;
    [SerializeField] private UnityEvent OnRewardFail;

    private RewardedAd _rewardedAd;
    private float _tiempo = 0;
    private bool _contando = false;
    private bool _disponible = false;
    private bool _pausa = false;
    private bool _reward = false;


    private void Start()
    {
        MobileAds.Initialize(initStatus => { print("MobileAds SDK is initialized."); LoadRewardedAd(); });
    }
    private void Update()
    {
        if (_reward) 
        {
            _reward = false;
            BottonAdd.SetActive(false);
            OnReward?.Invoke();
        }
        if (_pausa) { return; }
        if (!_contando) { return; }

        _tiempo += Time.deltaTime;
        if (_tiempo >= TiempoDeCiclo) 
        {
            if (_disponible)
            {
                _tiempo = 0;
                _contando = false;
                BottonAdd.SetActive(true);
            }
        }
    }

    public void IniciarCuenta() { _contando = true; }
    public void MostrarAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) => { print(String.Format("Rewarded ad rewarded the user. Type: {0}, amount: {1}.", reward.Type, reward.Amount)); });
        }
    }
    public void Pausar() { _pausa = true; }
    public void Reanudar() { _pausa = false; }

    private void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        print("Loading the rewarded ad.");

        AdRequest adRequest = new();

        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null) { print($"Rewarded ad failed to load an ad with error : {error}"); return; }
            print($"Rewarded ad loaded with response : {ad.GetResponseInfo()}");
            _rewardedAd = ad;
            RegisterReloadHandler(ad);
            _disponible = true;
        });
    }
    private void RegisterReloadHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () => 
        {
            _contando = true;
            _disponible = false; 
            LoadRewardedAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) => 
        {
            print($"Rewarded ad failed to open full screen content with error : {error}");
            _contando = true;
            _disponible = false;
            LoadRewardedAd(); 
            OnRewardFail?.Invoke();
        };
        ad.OnAdImpressionRecorded += () => 
        {
            _reward = true; 
            _contando = false;
            _disponible = false;
            BottonAdd.SetActive(false);
        };
    }
    private void OnDestroy()
    {
        _rewardedAd.Destroy();
    }
}
