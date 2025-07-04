//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherCurrentNode;

    public static Entitas.IMatcher<GameEntity> CurrentNode {
        get {
            if (_matcherCurrentNode == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CurrentNode);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCurrentNode = matcher;
            }

            return _matcherCurrentNode;
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

    public Code.Gameplay.Features.TrainAI.CurrentNode currentNode { get { return (Code.Gameplay.Features.TrainAI.CurrentNode)GetComponent(GameComponentsLookup.CurrentNode); } }
    public int CurrentNode { get { return currentNode.Value; } }
    public bool hasCurrentNode { get { return HasComponent(GameComponentsLookup.CurrentNode); } }

    public GameEntity AddCurrentNode(int newValue) {
        var index = GameComponentsLookup.CurrentNode;
        var component = (Code.Gameplay.Features.TrainAI.CurrentNode)CreateComponent(index, typeof(Code.Gameplay.Features.TrainAI.CurrentNode));
        component.Value = newValue;
        AddComponent(index, component);
        return this;
    }

    public GameEntity ReplaceCurrentNode(int newValue) {
        var index = GameComponentsLookup.CurrentNode;
        var component = (Code.Gameplay.Features.TrainAI.CurrentNode)CreateComponent(index, typeof(Code.Gameplay.Features.TrainAI.CurrentNode));
        component.Value = newValue;
        ReplaceComponent(index, component);
        return this;
    }

    public GameEntity RemoveCurrentNode() {
        RemoveComponent(GameComponentsLookup.CurrentNode);
        return this;
    }
}
