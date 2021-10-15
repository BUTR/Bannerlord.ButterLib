using System;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.DynamicAPI
{
    // Instance Class
    [DynamicAPIClass("Relationshit")]
    internal class RelationshitAPI
    {
        // Instance Method
        [DynamicAPIMethod("IsLoverOf")]
        public bool IsLoverOf(Hero hero1, Hero hero2) => false;

        // Static Method
        [DynamicAPIMethod("IsLoverOf")]
        public static bool IsLoverOfStatic(Hero hero1, Hero hero2) => false;
    }

    internal static class Example
    {
        public static void ExampleM()
        {
            var instance = DynamicAPI.RequestAPIClass("Relationshit");
            var isLoverOf = instance?.RequestMethod<Func<Hero, Hero, bool>>("IsLoverOf");
            var instanceValue = isLoverOf?.Invoke(Hero.MainHero, Hero.MainHero);

            var isLoverOfStatic = DynamicAPI.RequestAPIMethod<Func<Hero, Hero, bool>>("IsLoverOf");
            var staticValue = isLoverOfStatic?.Invoke(Hero.MainHero, Hero.MainHero);
        }
    }

    internal static class DynamicAPI
    {
        public static DynamicAPIInstance? RequestAPIClass(string @class)
        {
            return new(null!);
        }

        public static TDelegate? RequestAPIMethod<TDelegate>(string method, params Type[] types) where TDelegate : Delegate
        {
            if (method is null) throw new ArgumentNullException(nameof(method));
            return null;
        }
    }
    internal class DynamicAPIInstance
    {
        public object Instance { get; }

        internal DynamicAPIInstance(object instance)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public TDelegate? RequestMethod<TDelegate>(string method, params Type[] types) where TDelegate : Delegate
        {
            if (method is null) throw new ArgumentNullException(nameof(method));
            return null;
        }
    }
}