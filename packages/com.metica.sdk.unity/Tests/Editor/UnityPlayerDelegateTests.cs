using System;
using NUnit.Framework;
using Metica.Ads;
using Metica.Ads.UnityPlayer;
using Metica.Core;

[TestFixture]
public class UnityPlayerDelegateTests
{
    private UnityPlayerDelegate _delegate;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Register a mock logger for tests
        Registry.RegisterIfNull<ILog>(new MockLogger());
    }

    [SetUp]
    public void SetUp()
    {
        _delegate = new UnityPlayerDelegate();
    }

    private class MockLogger : ILog
    {
        public LogLevel CurrentLogLevel { get; set; } = LogLevel.Off;
        public void D(Func<string> messageSupplier) { }
        public void E(Func<string> messageSupplier, Exception error = null) { }
        public void I(Func<string> messageSupplier) { }
        public void W(Func<string> messageSupplier) { }
    }

    // ===== BANNER TESTS =====

    [Test]
    public void LoadBanner_InvokesBannerAdLoadFailed()
    {
        // Arrange
        MeticaAdError receivedError = null;
        _delegate.BannerAdLoadFailed += (error) => receivedError = error;

        // Act
        _delegate.LoadBanner("test-banner-unit");

        // Assert
        Assert.IsNotNull(receivedError);
        Assert.AreEqual("Ads not supported in Unity Editor", receivedError.message);
        Assert.AreEqual("test-banner-unit", receivedError.adUnitId);
    }

    // ===== MREC TESTS =====

    [Test]
    public void LoadMrec_InvokesMrecAdLoadFailed()
    {
        // Arrange
        MeticaAdError receivedError = null;
        _delegate.MrecAdLoadFailed += (error) => receivedError = error;

        // Act
        _delegate.LoadMrec("test-mrec-unit");

        // Assert
        Assert.IsNotNull(receivedError);
        Assert.AreEqual("Ads not supported in Unity Editor", receivedError.message);
        Assert.AreEqual("test-mrec-unit", receivedError.adUnitId);
    }

    // ===== INTERSTITIAL TESTS =====

    [Test]
    public void LoadInterstitial_InvokesInterstitialAdLoadFailed()
    {
        // Arrange
        MeticaAdError receivedError = null;
        _delegate.InterstitialAdLoadFailed += (error) => receivedError = error;

        // Act
        _delegate.LoadInterstitial("test-interstitial-unit");

        // Assert
        Assert.IsNotNull(receivedError);
        Assert.AreEqual("Ads not supported in Unity Editor", receivedError.message);
        Assert.AreEqual("test-interstitial-unit", receivedError.adUnitId);
    }

    [Test]
    public void ShowInterstitial_InvokesInterstitialAdShowFailed()
    {
        // Arrange
        MeticaAd receivedAd = null;
        MeticaAdError receivedError = null;
        _delegate.InterstitialAdShowFailed += (ad, error) =>
        {
            receivedAd = ad;
            receivedError = error;
        };

        // Act
        _delegate.ShowInterstitial("test-interstitial-unit", "placement123", "customData");

        // Assert
        Assert.IsNotNull(receivedAd);
        Assert.IsNotNull(receivedError);
        Assert.AreEqual("test-interstitial-unit", receivedAd.adUnitId);
        Assert.AreEqual("placement123", receivedAd.placementTag);
        Assert.AreEqual("INTERSTITIAL", receivedAd.adFormat);
        Assert.AreEqual("Ads not supported in Unity Editor", receivedError.message);
    }

    // ===== REWARDED TESTS =====

    [Test]
    public void LoadRewarded_InvokesRewardedAdLoadFailed()
    {
        // Arrange
        MeticaAdError receivedError = null;
        _delegate.RewardedAdLoadFailed += (error) => receivedError = error;

        // Act
        _delegate.LoadRewarded("test-rewarded-unit");

        // Assert
        Assert.IsNotNull(receivedError);
        Assert.AreEqual("Ads not supported in Unity Editor", receivedError.message);
        Assert.AreEqual("test-rewarded-unit", receivedError.adUnitId);
    }

    [Test]
    public void ShowRewarded_InvokesRewardedAdShowFailed()
    {
        // Arrange
        MeticaAd receivedAd = null;
        MeticaAdError receivedError = null;
        _delegate.RewardedAdShowFailed += (ad, error) =>
        {
            receivedAd = ad;
            receivedError = error;
        };

        // Act
        _delegate.ShowRewarded("test-rewarded-unit", "placement456", null);

        // Assert
        Assert.IsNotNull(receivedAd);
        Assert.IsNotNull(receivedError);
        Assert.AreEqual("test-rewarded-unit", receivedAd.adUnitId);
        Assert.AreEqual("placement456", receivedAd.placementTag);
        Assert.AreEqual("REWARDED", receivedAd.adFormat);
        Assert.AreEqual("Ads not supported in Unity Editor", receivedError.message);
    }
}
