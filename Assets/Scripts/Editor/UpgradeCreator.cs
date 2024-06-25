using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Linq;
using Upgrades.Scriptable_Objects;

namespace Editor
{
    public class UpgradeCreator : EditorWindow
    {
        private class UpgradeStatField
        {
            private readonly string statName;
            private readonly FieldInfo field;
            private float start;
            private float increment;
            private float exponent;

            public UpgradeStatField(string statName, FieldInfo field)
            {
                this.statName = statName;
                this.field = field;
            }

            public void Draw()
            {
                GUILayout.Space(5f);
                GUILayout.Label(statName, EditorStyles.boldLabel);
                start = EditorGUILayout.FloatField("Min", start);
                increment = EditorGUILayout.FloatField("Increment", increment);
                exponent = EditorGUILayout.FloatField("Exponent", exponent);
            }

            public void SetValue(Upgrade upgrade, int index)
            {
                // Calculate the value based on the index
                float value = start + Mathf.Pow(index, exponent) * increment;
                // Set the value based on the field type
                if (field.FieldType == typeof(int))
                {
                    field.SetValue(upgrade, (int)value);
                }
                else if (field.FieldType == typeof(float))
                {
                    field.SetValue(upgrade, value);
                }
            }
        }

        private enum CreationType
        {
            Create,
            Update
        }
        
        private string upgradeName;
        private string description;
        private string upgradePath;
        private CreationType creationType;
        private int amountToCreate;

        private Upgrade selectedUpgrade;
        public Upgrade[] existingUpgrades;
        private SerializedObject serializedObject;
        private SerializedProperty existingUpgradesProperty;

        private readonly UpgradeStatField costField = new("Cost", typeof(Upgrade).GetField("cost"));
        private readonly List<UpgradeStatField> upgradeStatFields = new();

        [MenuItem("Window/Upgrade Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(UpgradeCreator));
        }

        private void OnEnable()
        {
            // Create a serialized object for the window
            serializedObject = new SerializedObject(this);
            existingUpgradesProperty = serializedObject.FindProperty("existingUpgrades");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Select Upgrade"))
            {
                ShowUpgradeMenu();
            }

            if (selectedUpgrade == null)
                return;
            
            GUILayout.Label("Selected Upgrade: " + selectedUpgrade.GetType().Name, EditorStyles.largeLabel);
            upgradeName = EditorGUILayout.TextField("Upgrade Name", upgradeName);
            description = EditorGUILayout.TextField("Description", description);
            creationType = (CreationType)EditorGUILayout.EnumPopup("Creation Type", creationType);
            switch (creationType)
            {
                case CreationType.Create:
                    DrawCreateFields();
                    break;
                case CreationType.Update:
                    DrawUpdateExistingFields();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GUILayout.Space(5f);
            costField.Draw();
            DrawStatFields();
            GUILayout.Space(5f);
            if (GUILayout.Button("Create Upgrades"))
            {
                CreateUpgrades();
            }
        }

        // Draw the stat fields for the selected upgrade
        private void DrawStatFields()
        {
            foreach (UpgradeStatField field in upgradeStatFields)
            {
                field.Draw();
            }
        }

        // Show a context menu with all upgrade types
        private void ShowUpgradeMenu()
        {
            GenericMenu menu = new GenericMenu();
            // Get all types that inherit from Upgrade
            Type upgradeType = typeof(Upgrade);
            Assembly assembly = Assembly.GetAssembly(upgradeType);
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.IsSubclassOf(upgradeType));

            // Add each type to the menu
            foreach (Type type in types)
            {
                menu.AddItem(new GUIContent(type.Name), false, OnUpgradeSelected, type);
            }

            menu.ShowAsContext();
        }

        // Handle the selection of an upgrade
        private void OnUpgradeSelected(object selectedData)
        {
            // Cast the selected data to a Type
            Type type = (Type)selectedData;
            selectedUpgrade = (Upgrade)ScriptableObject.CreateInstance(type);
            //create empty array of existing upgrades
            existingUpgrades = Array.Empty<Upgrade>();
            CreateStatFields();
        }

        // Create the stat fields for the selected upgrade
        private void CreateStatFields()
        {
            // Get all fields in the selected upgrade
            upgradeStatFields.Clear();
            FieldInfo[] fields = selectedUpgrade.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                // Skip the all fields from base class field
                if (field.DeclaringType == typeof(Upgrade))
                    continue;
                // Create a stat field based on the field type
                UpgradeStatField statField = new UpgradeStatField(field.Name, field);
                upgradeStatFields.Add(statField);
            }
        }
        
        // Draw the fields for updating existing upgrades
        private void DrawUpdateExistingFields()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(existingUpgradesProperty);
            serializedObject.ApplyModifiedProperties();
        }
        
        // Draw the fields for creating new upgrades
        private void DrawCreateFields()
        {
            amountToCreate = EditorGUILayout.IntField("Amount to Create", amountToCreate);
        }

        // Create the upgrades based on the selected upgrade
        private void CreateUpgrades()
        {
            List<UpgradeStatField> allFields = upgradeStatFields.ToList();
            allFields.Add(costField);
            switch (creationType)
            {
                case CreationType.Create:
                    // Get the path to save the upgrades
                    string absolutePath = EditorUtility.SaveFolderPanel("Save Upgrade", "", "");
                    string relativePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
                    // Generate the upgrades at the specified path
                    GenerateUpgrades(selectedUpgrade, allFields.ToArray(), relativePath, amountToCreate);
                    break;
                case CreationType.Update:
                    // Update the existing upgrades
                    UpdateExistingUpgrades(existingUpgrades, allFields.ToArray());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Update the existing upgrades with the new values
        private void UpdateExistingUpgrades(Upgrade[] upgrades, UpgradeStatField[] stats)
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                SetUpgradeValues(upgrades[i], stats, i);
            }
        }

        // Generate the upgrades at the specified path
        private void GenerateUpgrades(Upgrade upgrade, UpgradeStatField[] stats, string path, int amount)
        {
            Type upgradeType = upgrade.GetType();
            for (int i = 0; i < amount; i++)
            {
                Upgrade createdUpgrade = (Upgrade)ScriptableObject.CreateInstance(upgradeType);
                SetUpgradeValues(createdUpgrade, stats, i);
                createdUpgrade.name = $"{upgradeName} {i}";
                string assetPath = $"{path}/{createdUpgrade.name}.asset";
                AssetDatabase.CreateAsset(createdUpgrade, assetPath);
            }

            AssetDatabase.SaveAssets();
        }

        // Set the values of the upgrade based on the stat fields
        private void SetUpgradeValues(Upgrade upgrade, UpgradeStatField[] stats, int index)
        {
            upgrade.upgradeName = upgradeName;
            upgrade.description = description;
            foreach (UpgradeStatField stat in stats)
            {
                stat.SetValue(upgrade, index);
            }
        }
    }
}