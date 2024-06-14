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
        private int amountToCreate;
        private float startAcceleration;
        private float maxAcceleration;
        private AnimationCurve accelerationCurve = new AnimationCurve();
        private bool roundAcceleration;
        private float roundedAcceleration;
        private int startingCost;
        private int maxCost;
        private float startMaxSpeed;
        private float maxMaxSpeed;
        private AnimationCurve maxSpeedCurve = new AnimationCurve();
        private bool roundMaxSpeed;
        private float roundedMaxSpeed;
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
            startAcceleration = EditorGUILayout.FloatField("Start Acceleration", startAcceleration);
            maxAcceleration = EditorGUILayout.FloatField("Max Length", maxAcceleration);
            accelerationCurve = EditorGUILayout.CurveField("Length Curve", accelerationCurve);
            roundAcceleration = EditorGUILayout.Toggle("Round Acceleration", roundAcceleration);
            roundedAcceleration = EditorGUILayout.FloatField("Rounded Acceleration", roundedAcceleration);
            GUILayout.Space(5f);
            GUILayout.Label("Max Speed", EditorStyles.boldLabel);
            startMaxSpeed = EditorGUILayout.FloatField("Starting Max Speed", startMaxSpeed);
            maxMaxSpeed = EditorGUILayout.FloatField("Max Max Speed", maxMaxSpeed);
            maxSpeedCurve = EditorGUILayout.CurveField("Max Speed Curve", maxSpeedCurve);
            roundMaxSpeed = EditorGUILayout.Toggle("Round Max Speed", roundMaxSpeed);
            roundedMaxSpeed = EditorGUILayout.FloatField("Rounded Max Speed", roundedMaxSpeed);
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
             float roundTo)
        {
            float value =
                Mathf.RoundToInt(curve.Evaluate(i / (float)amountToCreate) * (maxValue - startValue) + startValue);
            return shouldRound ? RoundTo(value, roundTo) : value;
        }

        private static int RoundTo(float number, float roundTo)
        {
            return (int)number / (int)roundTo * (int)roundTo;
        }
    }
}