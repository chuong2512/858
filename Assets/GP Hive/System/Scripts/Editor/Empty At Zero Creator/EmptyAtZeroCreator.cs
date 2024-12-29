using UnityEditor;

namespace GPHive.System.Editor.EmptyAtZeroCreator
{
    public class EmptyAtZeroCreator
    {
        const string _Space = EmptyCreator.Space;

        const string featureName = EmptyCreator.CreateEmpty + EmptyCreator.At + _Space + EmptyCreator.Zero;
        const string pathName = EmptyCreator.GameObjectStr + EmptyCreator.Slash + featureName + _Space + shortcutName;
        const string shortcutName = EmptyCreator.AltSymbol + EmptyCreator.ShortcutLetter;

        [MenuItem(pathName, false, -1)]
        public static void CreateEmptyAtZero(MenuCommand menuCommand)
        {
            EmptyCreator.CreateEmptyGameObject(featureName, true, false, menuCommand);
        }
    }
}
