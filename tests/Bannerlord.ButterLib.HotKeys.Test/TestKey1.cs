using TaleWorlds.InputSystem;

namespace Bannerlord.ButterLib.HotKeys.Test
{
    public class TestKey1 : HotKeyBase
    {
        protected override string DisplayName { get; }
        protected override string Description { get; }
        protected override InputKey DefaultKey { get; }
        protected override string Category { get; }

        public TestKey1() : base(nameof(TestKey1))
        {
            DisplayName = "My Test Key";
            Description = "This is a test key.";
            DefaultKey = InputKey.Comma;
            Category = HotKeyManager.Categories[HotKeyCategory.CampaignMap];
        }

        protected override void OnReleased()
        {
            // You can also override methods relating to keypresses within the key itself.
        }
    }
}