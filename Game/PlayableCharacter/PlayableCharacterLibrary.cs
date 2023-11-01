using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [CreateAssetMenu(fileName = "PlayableCharacterLibrary", menuName = "ScriptableObject/PlayableCharacterLibrary", order = 1)]
    public class PlayableCharacterLibrary : DataLibrary<PlayableCharacter, PlayableCharacterType> {
    }
}