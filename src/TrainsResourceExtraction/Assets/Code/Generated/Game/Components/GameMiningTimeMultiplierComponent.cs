//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMiningTimeMultiplier;

    public static Entitas.IMatcher<GameEntity> MiningTimeMultiplier {
        get {
            if (_matcherMiningTimeMultiplier == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MiningTimeMultiplier);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMiningTimeMultiplier = matcher;
            }

            return _matcherMiningTimeMultiplier;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Code.Gameplay.Features.Mining.MiningTimeMultiplier miningTimeMultiplier { get { return (Code.Gameplay.Features.Mining.MiningTimeMultiplier)GetComponent(GameComponentsLookup.MiningTimeMultiplier); } }
    public float MiningTimeMultiplier { get { return miningTimeMultiplier.Value; } }
    public bool hasMiningTimeMultiplier { get { return HasComponent(GameComponentsLookup.MiningTimeMultiplier); } }

    public GameEntity AddMiningTimeMultiplier(float newValue) {
        var index = GameComponentsLookup.MiningTimeMultiplier;
        var component = (Code.Gameplay.Features.Mining.MiningTimeMultiplier)CreateComponent(index, typeof(Code.Gameplay.Features.Mining.MiningTimeMultiplier));
        component.Value = newValue;
        AddComponent(index, component);
        return this;
    }

    public GameEntity ReplaceMiningTimeMultiplier(float newValue) {
        var index = GameComponentsLookup.MiningTimeMultiplier;
        var component = (Code.Gameplay.Features.Mining.MiningTimeMultiplier)CreateComponent(index, typeof(Code.Gameplay.Features.Mining.MiningTimeMultiplier));
        component.Value = newValue;
        ReplaceComponent(index, component);
        return this;
    }

    public GameEntity RemoveMiningTimeMultiplier() {
        RemoveComponent(GameComponentsLookup.MiningTimeMultiplier);
        return this;
    }
}
