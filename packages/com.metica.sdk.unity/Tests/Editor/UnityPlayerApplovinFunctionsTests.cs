using System;
using NUnit.Framework;
using Metica.Ads;
using Metica.Ads.UnityPlayer;
using Metica.Core;

[TestFixture]
public class UnityPlayerApplovinFunctionsTests
{
    private MeticaApplovinFunctions _max;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Registry.RegisterIfNull<ILog>(new MockLogger());
    }

    [SetUp]
    public void SetUp()
    {
        var delegate_ = new UnityPlayerDelegate();
        _max = delegate_.Max;
    }

    private class MockLogger : ILog
    {
        public LogLevel CurrentLogLevel { get; set; } = LogLevel.Off;
        public void D(Func<string> messageSupplier) { }
        public void E(Func<string> messageSupplier, Exception error = null) { }
        public void I(Func<string> messageSupplier) { }
        public void W(Func<string> messageSupplier) { }
    }

    // ===== DEVICE INFO TESTS =====

    [Test]
    public void IsTablet_ReturnsFalse()
    {
        Assert.IsFalse(_max.IsTablet());
    }

    [Test]
    public void GetAdaptiveBannerHeight_ReturnsZero()
    {
        Assert.AreEqual(0, _max.GetAdaptiveBannerHeight(320));
    }

    // ===== AUDIO CONTROL TESTS =====

    [Test]
    public void IsMuted_ReturnsFalse()
    {
        Assert.IsFalse(_max.IsMuted());
    }

    [Test]
    public void SetMuted_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _max.SetMuted(true));
        Assert.DoesNotThrow(() => _max.SetMuted(false));
    }

    // ===== CONSENT & PRIVACY TESTS =====

    [Test]
    public void HasUserConsent_ReturnsFalse()
    {
        Assert.IsFalse(_max.HasUserConsent());
    }

    [Test]
    public void IsUserConsentSet_ReturnsFalse()
    {
        Assert.IsFalse(_max.IsUserConsentSet());
    }

    [Test]
    public void GetConsentFlowUserGeography_ReturnsUnknown()
    {
        Assert.AreEqual(MaxSdkBase.ConsentFlowUserGeography.Unknown, _max.GetConsentFlowUserGeography());
    }

    [Test]
    public void ConsentDialogState_ReturnsUnknown()
    {
        Assert.AreEqual(MaxSdkBase.ConsentDialogState.Unknown, _max.ConsentDialogState());
    }

    [Test]
    public void ShowCmpForExistingUser_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _max.ShowCmpForExistingUser());
    }

    // ===== SDK STATE TESTS =====

    [Test]
    public void IsSuccessfullyInitialized_ReturnsTrue()
    {
        Assert.IsTrue(_max.IsSuccessfullyInitialized());
    }

    [Test]
    public void CountryCode_ReturnsEmptyString()
    {
        Assert.AreEqual("", _max.CountryCode());
    }

    // ===== DEBUGGER & PARAMETER TESTS =====

    [Test]
    public void ShowMediationDebugger_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _max.ShowMediationDebugger());
    }

    [Test]
    public void SetExtraParameter_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _max.SetExtraParameter("key", "value"));
        Assert.DoesNotThrow(() => _max.SetExtraParameter("key", null));
    }
}
