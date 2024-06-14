using UnityEditor;
using UnityEngine;
using Upgrades.Scriptable_Objects;

namespace Editor
{
    public class BoatSpeedUpgradeCreator : EditorWindow
    {
        private int startAcceleration;
        private int amountToCreate;
        private int maxAcceleration;
        private AnimationCurve accelerationCurve = new AnimationCurve();
        private bool roundLength;
        private int roundedLength;
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
        private string upgradeName;
        private string description;
        private string upgradePath;

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
            startAcceleration = EditorGUILayout.IntField("Starting Length", startAcceleration);
            maxAcceleration = EditorGUILayout.IntField("Max Length", maxAcceleration);
            accelerationCurve = EditorGUILayout.CurveField("Length Curve", accelerationCurve);
            roundLength = EditorGUILayout.Toggle("Round Length", roundLength);
            roundedLength = EditorGUILayout.IntField("Rounded Length", roundedLength);
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
                float acceleration =
                    Mathf.RoundToInt(accelerationCurve.Evaluate(i / (float)amountToCreate) *
                                     (maxAcceleration - startAcceleration) +
                                     startAcceleration);
                upgrade.acceleration = roundLength ? RoundTo(acceleration, roundedLength) : acceleration;
                
                //set max speed
                float maxSpeed =
                    Mathf.RoundToInt(maxSpeedCurve.Evaluate(i / (float)amountToCreate) * (maxMaxSpeed - startMaxSpeed) +
                                     startMaxSpeed);
                upgrade.maxSpeed = roundMaxSpeed ? RoundTo(maxSpeed, roundedMaxSpeed) : maxSpeed;
                
                //set cost
                int cost = Mathf.RoundToInt(costCurve.Evaluate(i / (float)amountToCreate) * (maxCost - startingCost) +
                                            startingCost);
                upgrade.cost = roundCost ? RoundTo(cost, roundedCost) : cost;
                upgrade.upgradeName = upgradeName;
                upgrade.description = description;
                upgrade.upgradeVisual = null;
                AssetDatabase.CreateAsset(upgrade, $"{upgradePath}/{upgrade.name}.asset");
            }

            AssetDatabase.SaveAssets();
        }

            
        private static int RoundTo(float number, int roundTo)
        {
            return (int)number / roundTo * roundTo;
        }
    }
}