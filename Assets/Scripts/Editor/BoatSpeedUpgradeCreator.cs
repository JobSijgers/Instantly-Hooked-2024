using UnityEditor;
using UnityEngine;
using Upgrades.Scriptable_Objects;

namespace Editor
{
    public class BoatSpeedUpgradeCreator : EditorWindow
    {
        private string upgradeName;
        private string description;
        private string upgradePath;
        private int startAcceleration;
        private int amountToCreate;
        private int maxAcceleration;
        private AnimationCurve accelerationCurve = new AnimationCurve();
        private bool roundAcceleration;
        private int roundedAcceleration;
        private int startingCost;
        private int maxCost;
        private int startMaxSpeed;
        private int maxMaxSpeed;
        private AnimationCurve maxSpeedCurve = new AnimationCurve();
        private bool roundMaxSpeed;
        private int roundedMaxSpeed;
        private AnimationCurve costCurve = new AnimationCurve();
        private bool roundCost;
        private int roundedCost;


        [MenuItem("Window/Upgrade Creator/Boat Speed")]
        public static void ShowWindow()
        {
            GetWindow(typeof(BoatSpeedUpgradeCreator));
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
            GUILayout.Label("Acceleration", EditorStyles.boldLabel);
            startAcceleration = EditorGUILayout.IntField("Start Acceleration", startAcceleration);
            maxAcceleration = EditorGUILayout.IntField("Max Length", maxAcceleration);
            accelerationCurve = EditorGUILayout.CurveField("Length Curve", accelerationCurve);
            roundAcceleration = EditorGUILayout.Toggle("Round Acceleration", roundAcceleration);
            roundedAcceleration = EditorGUILayout.IntField("Rounded Acceleration", roundedAcceleration);
            GUILayout.Space(5f);
            GUILayout.Label("Max Speed", EditorStyles.boldLabel);
            startMaxSpeed = EditorGUILayout.IntField("Starting Max Speed", startMaxSpeed);
            maxMaxSpeed = EditorGUILayout.IntField("Max Max Speed", maxMaxSpeed);
            maxSpeedCurve = EditorGUILayout.CurveField("Max Speed Curve", maxSpeedCurve);
            roundMaxSpeed = EditorGUILayout.Toggle("Round Max Speed", roundMaxSpeed);
            roundedMaxSpeed = EditorGUILayout.IntField("Rounded Max Speed", roundedMaxSpeed);
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
                ShipSpeedUpgrade upgrade = CreateInstance<ShipSpeedUpgrade>();
                upgrade.name = upgradeName + " " + i;

                //set acceleration
                upgrade.acceleration = CalculateAndRound(startAcceleration, maxAcceleration, accelerationCurve, i,
                    roundAcceleration, roundedAcceleration);

                //set max speed
                upgrade.maxSpeed = CalculateAndRound(startMaxSpeed, maxMaxSpeed, maxSpeedCurve, i, roundMaxSpeed,
                    roundedMaxSpeed);

                //set cost
                upgrade.cost =
                    Mathf.RoundToInt(costCurve.Evaluate(i / (float)amountToCreate) * (maxCost - startingCost) +
                                     startingCost);
                upgrade.cost = roundCost ? RoundTo(upgrade.cost, roundedCost) : upgrade.cost;
                upgrade.upgradeName = upgradeName;
                upgrade.description = description;
                upgrade.upgradeVisual = null;
                AssetDatabase.CreateAsset(upgrade, $"{upgradePath}/{upgrade.name}.asset");
            }

            AssetDatabase.SaveAssets();
        }

        private float CalculateAndRound(float startValue, float maxValue, AnimationCurve curve, int i, bool shouldRound,
            int roundTo)
        {
            float value =
                Mathf.RoundToInt(curve.Evaluate(i / (float)amountToCreate) * (maxValue - startValue) + startValue);
            return shouldRound ? RoundTo(value, roundTo) : value;
        }

        private static int RoundTo(float number, int roundTo)
        {
            return (int)number / roundTo * roundTo;
        }
    }
}