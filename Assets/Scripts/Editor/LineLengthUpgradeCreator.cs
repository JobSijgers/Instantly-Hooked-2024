using UnityEditor;
using UnityEngine;
using Upgrades.Scriptable_Objects;

namespace Editor
{
    public class LineLengthUpgradeCreator : EditorWindow
    {
        private int startingLength;
        private int amountToCreate;
        private int maxLength;
        private AnimationCurve lengthCurve = new AnimationCurve();
        private bool roundLength;
        private int roundedLength;
        private int startingCost;
        private int maxCost;
        private AnimationCurve costCurve = new AnimationCurve();
        private bool roundCost;
        private int roundedCost;
        private string upgradeName;
        private string description;
        private string upgradePath;

        [MenuItem("Window/Generate Upgrades")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LineLengthUpgradeCreator));
        }

        void OnGUI()
        {
            GUILayout.Label("Line Length Upgrade Creator", EditorStyles.boldLabel);
            upgradeName = EditorGUILayout.TextField("Upgrade Name", upgradeName);
            description = EditorGUILayout.TextField("Description", description);
            upgradePath = EditorGUILayout.TextField("Upgrade Path", upgradePath);
            startingLength = EditorGUILayout.IntField("Starting Length", startingLength);
            maxLength = EditorGUILayout.IntField("Max Length", maxLength);
            amountToCreate = EditorGUILayout.IntField("Amount to Create", amountToCreate);
            lengthCurve = EditorGUILayout.CurveField("Length Curve", lengthCurve);
            roundLength = EditorGUILayout.Toggle("Round Length", roundLength);
            roundedLength = EditorGUILayout.IntField("Rounded Length", roundedLength);
            startingCost = EditorGUILayout.IntField("Starting Cost", startingCost);
            maxCost = EditorGUILayout.IntField("Max Cost", maxCost);
            costCurve = EditorGUILayout.CurveField("Cost Curve", costCurve);
            roundCost = EditorGUILayout.Toggle("Round Cost", roundCost);
            roundedCost = EditorGUILayout.IntField("Rounded Cost", roundedCost);
            if (GUILayout.Button("Create Upgrades"))
            {
                CreateUpgrade();
            }
        }

        private void CreateUpgrade()
        {
            for (int i = 0; i < amountToCreate; i++)
            {
                LineLengthUpgrade upgrade = CreateInstance<LineLengthUpgrade>();
                upgrade.name = upgradeName + " " + i;
                int lineLength =
                    Mathf.RoundToInt(lengthCurve.Evaluate(i / (float)amountToCreate) * (maxLength - startingLength) +
                                     startingLength);
                upgrade.lineLength = roundLength ? RoundTo(lineLength, roundedLength) : lineLength;

                int cost = Mathf.RoundToInt(costCurve.Evaluate(i / (float)amountToCreate) * (maxCost - startingCost) +
                                            startingCost);
                upgrade.cost = roundCost ? RoundTo(cost, roundedCost) : cost;
                upgrade.upgradeName = upgradeName + " " + upgrade.lineLength + "m";
                upgrade.description = description;
                upgrade.upgradeVisual = null;
                AssetDatabase.CreateAsset(upgrade, $"{upgradePath}/{upgrade.name}.asset");
            }

            AssetDatabase.SaveAssets();
        }

        private static int RoundTo(int number, int roundTo)
        {
            return number / roundTo * roundTo;
        }
    }
}