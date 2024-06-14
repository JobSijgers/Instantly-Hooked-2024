using UnityEditor;
using UnityEngine;
using Upgrades.Scriptable_Objects;

namespace Editor
{
    public class RodUpgradeCreator : EditorWindow
    {
        private string upgradeName;
        private string description;
        private string upgradePath;
        private int amountToCreate;
        private float startReelSpeed;
        private float maxReelSpeed;
        private AnimationCurve reelSpeedCurve = new AnimationCurve();
        private bool roundReelSpeed;
        private float roundedReelSpeed;
        private int startingCost;
        private int maxCost;
        private float startDropSpeed;
        private float maxDropSpeed;
        private AnimationCurve dropSpeedCurve = new AnimationCurve();
        private bool roundDropSpeed;
        private float roundedDropSpeed;
        private AnimationCurve costCurve = new AnimationCurve();
        private bool roundCost;
        private int roundedCost;


        [MenuItem("Window/Upgrade Creator/Reel Speed")]
        public static void ShowWindow()
        {
            GetWindow(typeof(RodUpgradeCreator));
        }

        void OnGUI()
        {
            GUILayout.Label("Boat Speed Upgrade Creator", EditorStyles.boldLabel);
            GUILayout.Space(5f);
            upgradeName = EditorGUILayout.TextField("Upgrade Name", upgradeName);
            description = EditorGUILayout.TextField("Description", description);
            upgradePath = EditorGUILayout.TextField("Upgrade Path", upgradePath);
            amountToCreate = EditorGUILayout.IntField("Amount to Create", amountToCreate);
            GUILayout.Space(5f);
            GUILayout.Label("Reel Speed", EditorStyles.boldLabel);
            startReelSpeed = EditorGUILayout.FloatField("Start Reel Speed", startReelSpeed);
            maxReelSpeed = EditorGUILayout.FloatField("Max Reel Speed", maxReelSpeed);
            reelSpeedCurve = EditorGUILayout.CurveField("Reel Speed Curve", reelSpeedCurve);
            roundReelSpeed = EditorGUILayout.Toggle("Round Reel Speed", roundReelSpeed);
            roundedReelSpeed = EditorGUILayout.FloatField("Rounded Reel Speed", roundedReelSpeed);
            GUILayout.Space(5f);
            GUILayout.Label("Drop Speed", EditorStyles.boldLabel);
            startDropSpeed = EditorGUILayout.FloatField("Starting Drop Speed", startDropSpeed);
            maxDropSpeed = EditorGUILayout.FloatField("Max Drop Speed", maxDropSpeed);
            dropSpeedCurve = EditorGUILayout.CurveField("Drop Speed Curve", dropSpeedCurve);
            roundDropSpeed = EditorGUILayout.Toggle("Round Drop Speed", roundDropSpeed);
            roundedDropSpeed = EditorGUILayout.FloatField("Rounded Drop Speed", roundedDropSpeed);
            GUILayout.Space(5f);
            GUILayout.Label("Cost", EditorStyles.boldLabel);
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
                ReelSpeedUpgrade upgrade = CreateInstance<ReelSpeedUpgrade>();
                upgrade.name = upgradeName + " " + i;

                //set acceleration
                upgrade.reelSpeed = CalculateAndRound(startReelSpeed, maxReelSpeed, reelSpeedCurve, i,
                    roundReelSpeed, roundedReelSpeed);

                //set max speed
                upgrade.dropSpeed = CalculateAndRound(startDropSpeed, maxDropSpeed, dropSpeedCurve, i, roundDropSpeed,
                    roundedDropSpeed);

                //set cost
                upgrade.cost =
                    Mathf.RoundToInt(costCurve.Evaluate(i / (float)amountToCreate) * (maxCost - startingCost) +
                                     startingCost);
                upgrade.cost = roundCost ? (int)RoundTo(upgrade.cost, roundedCost) : upgrade.cost;
                upgrade.upgradeName = upgradeName;
                upgrade.description = description;
                upgrade.upgradeVisual = null;
                AssetDatabase.CreateAsset(upgrade, $"{upgradePath}/{upgrade.name}.asset");
            }

            AssetDatabase.SaveAssets();
        }

        private float CalculateAndRound(float startValue, float maxValue, AnimationCurve curve, int i, bool shouldRound,
            float roundTo)
        {
            float value = curve.Evaluate(i / (float)amountToCreate) * (maxValue - startValue) + startValue;
            return shouldRound ? RoundTo(value, roundTo) : value;
        }

        private static float RoundTo(float number, float roundTo)
        {
            return Mathf.Floor(number / roundTo) * roundTo;
        }
    }
}