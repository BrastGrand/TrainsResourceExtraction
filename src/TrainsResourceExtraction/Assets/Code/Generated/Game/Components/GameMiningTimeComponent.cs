//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMiningTime;

    public static Entitas.IMatcher<GameEntity> MiningTime {
        get {
            if (_matcherMiningTime == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MiningTime);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMiningTime = matcher;
            }

            return _matcherMiningTime;
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

    public Code.Gameplay.Features.Mining.MiningTime miningTime { get { return (Code.Gameplay.Features.Mining.MiningTime)GetComponent(GameComponentsLookup.MiningTime); } }
    public float MiningTime { get { return miningTime.Value; } }
    public bool hasMiningTime { get { return HasComponent(GameComponentsLookup.MiningTime); } }

    public GameEntity AddMiningTime(float newValue) {
        var index = GameComponentsLookup.MiningTime;
        var component = (Code.Gameplay.Features.Mining.MiningTime)CreateComponent(index, typeof(Code.Gameplay.Features.Mining.MiningTime));
        component.Value = newValue;
        AddComponent(index, component);
        return this;
    }

    public GameEntity ReplaceMiningTime(float newValue) {
        var index = GameComponentsLookup.MiningTime;
        var component = (Code.Gameplay.Features.Mining.MiningTime)CreateComponent(index, typeof(Code.Gameplay.Features.Mining.MiningTime));
        component.Value = newValue;
        ReplaceComponent(index, component);
        return this;
    }

    public GameEntity RemoveMiningTime() {
        RemoveComponent(GameComponentsLookup.MiningTime);
        return this;
    }
}
