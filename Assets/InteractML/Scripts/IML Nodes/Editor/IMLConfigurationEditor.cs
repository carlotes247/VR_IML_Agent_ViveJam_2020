﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using XNodeEditor;
#endif
namespace InteractML
{
    [CustomNodeEditor(typeof(IMLConfiguration))]
    public class IMLConfigurationEditor : NodeEditor
    {
        #region Variables        

        public bool showInputOutputMatrix;
        public bool showModelOutput;

        /// <summary>
        /// Reference to the node itself
        /// </summary>
        private IMLConfiguration m_IMLConfigNode;

        #endregion

        #region Unity Messages

        /// <summary> 
        /// Called whenever the xNode editor window is updated 
        /// </summary>
        public override void OnBodyGUI()
        {
            // Override label width for this node (since some names are quite long)
            LabelWidth = 184;

            base.OnBodyGUI();

            m_IMLConfigNode = (target as IMLConfiguration);

            // SHOW TRAINING STATUS 
            ShowTrainingStatus();

            // SHOW TOTAL NUMBER OF TRAINING EXAMPLES
            // Account for dtw
            if (m_IMLConfigNode.LearningType == IMLSpecifications.LearningType.DTW)
            {
                EditorGUILayout.LabelField("Total Num Training Series: ", m_IMLConfigNode.TotalNumTrainingData.ToString());
            }
            // classification/regression
            else
            {
                EditorGUILayout.LabelField("Total Num Training Examples: ", m_IMLConfigNode.TotalNumTrainingData.ToString());
            }

            EditorGUILayout.Space();
            // SHOW MODEL OUTPUT
            ShowModelOutput();
            EditorGUILayout.Space();

            // SHOW KEYBOARD SHORTCUTS
            // Show information about runtime keys for interaction
            GUIStyle styleGUIBox = new GUIStyle(GUI.skin.box);
            styleGUIBox.richText = true;
            if (m_IMLConfigNode.KeyboardControl)
            {
                GUILayout.Box("<b>Runtime Keys:</b>\n <b>Train:</b> " + m_IMLConfigNode.TrainingKey.ToString() 
                    + " | <b>Run:</b> " + m_IMLConfigNode.RunningKey, 
                    styleGUIBox);
                //GUILayout.Box("<b>Runtime Keys:</b>\n <b>Run:</b> R", styleGUIBox);

                // Show key configs
                m_IMLConfigNode.TrainingKey = (KeyCode) EditorGUILayout.EnumFlagsField(m_IMLConfigNode.TrainingKey);
                m_IMLConfigNode.RunningKey = (KeyCode) EditorGUILayout.EnumFlagsField(m_IMLConfigNode.RunningKey);
                EditorGUILayout.Space();

            }

            // SHOW TRAIN BUTTON 
            ShowTrainModelButton();

            // SHOW RUN BUTTON
            ShowRunModelButton();

            // Init model button (to debug the model not working)
            if (GUILayout.Button("Reset Model"))
            {
                m_IMLConfigNode.ResetModel();
            }

            // SHOW FEATURE SELECTION MATRIX
            ShowFeatureSelectionMatrix();

            // Errors with needed connections
            if (m_IMLConfigNode.ExpectedOutputList.Count == 0)
            {
                EditorGUILayout.HelpBox("There are no Expected Outputs Configured!", MessageType.Error);
            }

            if (m_IMLConfigNode.InputFeatures.Count == 0)
            {
                EditorGUILayout.HelpBox("There are no Realtime Input Features Connected!", MessageType.Error);

            }
        }

        #endregion

        #region Methods

        private void ShowTrainingStatus()
        {
            string trainingStatusText = m_IMLConfigNode.ModelStatus.ToString();
            GUIStyle trainingStatusLabelStyle = new GUIStyle(EditorStyles.label);
            switch (m_IMLConfigNode.ModelStatus)
            {
                case IMLSpecifications.ModelStatus.Untrained:
                    trainingStatusLabelStyle.normal.textColor = Color.red;
                    break;
                case IMLSpecifications.ModelStatus.Training:
                    trainingStatusLabelStyle.normal.textColor = Color.yellow;
                    break;
                case IMLSpecifications.ModelStatus.Trained:
                    trainingStatusLabelStyle.normal.textColor = Color.green;
                    break;
                case IMLSpecifications.ModelStatus.Running:
                    trainingStatusLabelStyle.normal.textColor = Color.green;
                    break;
                default:
                    break;
            }
            EditorGUILayout.LabelField("Training Status: ", trainingStatusText, trainingStatusLabelStyle);

        }

        private void ShowTrainModelButton()
        {
            // Only run button logic when rapidlib reference not null
            if (m_IMLConfigNode.Model != null)
            {
                string nameButton = "";

                if (m_IMLConfigNode.Training)
                    nameButton = "STOP Training Model";
                else
                    nameButton = "Train Model";

                // Disable button if model is Running OR Trainig 
                if (m_IMLConfigNode.Running || m_IMLConfigNode.Training)
                    GUI.enabled = false;
                if (GUILayout.Button(nameButton))
                {
                    m_IMLConfigNode.TrainModel();
                }
                // Always enable it back at the end
                GUI.enabled = true;

            }
            // If rapidlib reference is null we draw a disabled button
            else
            {
                GUI.enabled = false;
                if (GUILayout.Button("Train Model"))
                {
                    //m_TrainingExamplesNode.ToggleCollectExamples();
                }
                GUI.enabled = true;

            }

        }

        private void ShowRunModelButton()
        {
            if (m_IMLConfigNode.Model != null)
            {
                string nameButton = "";

                if (m_IMLConfigNode.Running)
                {
                    // DTW
                    if (m_IMLConfigNode.LearningType == IMLSpecifications.LearningType.DTW)
                    {
                        nameButton = "STOP Populating Running Series & Run";
                    }
                    // Classification/Regression
                    else
                    {
                        nameButton = "STOP Running";
                    }
                }
                else
                {
                    // DTW
                    if (m_IMLConfigNode.LearningType == IMLSpecifications.LearningType.DTW)
                    {
                        nameButton = "Populate Running Series";
                    }
                    // Classification/Regression
                    else
                    {
                        nameButton = "Run";
                    }
                }

                // Disable button if model is Trainig OR Untrained
                if (m_IMLConfigNode.Training || m_IMLConfigNode.Untrained)
                    GUI.enabled = false;
                if (GUILayout.Button(nameButton))
                {
                    m_IMLConfigNode.ToggleRunning();
                }
                // Always enable it back at the end
                GUI.enabled = true;

            }
            // If rapidlib reference is null we draw a disabled button
            else
            {
                GUI.enabled = false;
                if (GUILayout.Button("Run"))
                {
                    //m_TrainingExamplesNode.ToggleCollectExamples();
                }
                GUI.enabled = true;
            }


        }

        private void ShowFeatureSelectionMatrix()
        {
                IMLConfiguration IMLConfigNode = (IMLConfiguration)target;
                EditorGUILayout.Space();

                showInputOutputMatrix = EditorGUILayout.Foldout(showInputOutputMatrix, "Feature Selection Matrix ");
                if (showInputOutputMatrix)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < 1; i++)
                    //for (int i = 0; i < IMLConfigNode.ExpectedInputList.Count; i++)
                    {

                        EditorGUI.indentLevel = 0;

                        GUIStyle tableStyle = new GUIStyle("box");
                        tableStyle.padding = new RectOffset(24, 10, 10, 10);
                        tableStyle.margin.left = 24;

                        GUIStyle headerColumnStyle = new GUIStyle();
                        headerColumnStyle.fixedWidth = 35;

                        GUIStyle columnStyle = new GUIStyle();
                        columnStyle.fixedWidth = 60;

                        GUIStyle rowStyle = new GUIStyle();
                        rowStyle.fixedHeight = 25;

                        GUIStyle rowHeaderStyle = new GUIStyle();
                        rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

                        GUIStyle columnHeaderStyle = new GUIStyle();
                        columnHeaderStyle.fixedWidth = 30f;
                        columnHeaderStyle.fixedHeight = 25.5f;

                        GUIStyle columnLabelStyle = new GUIStyle();
                        columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
                        columnLabelStyle.alignment = TextAnchor.MiddleCenter;
                        columnLabelStyle.fontStyle = FontStyle.Bold;
                        columnLabelStyle.fontSize = 10;

                        GUIStyle cornerLabelStyle = new GUIStyle();
                        cornerLabelStyle.fixedWidth = 42;
                        cornerLabelStyle.alignment = TextAnchor.MiddleRight;
                        cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
                        cornerLabelStyle.fontSize = 10;
                        cornerLabelStyle.padding.top = -5;

                        GUIStyle rowLabelStyle = new GUIStyle();
                        rowLabelStyle.fixedWidth = 25;
                        rowLabelStyle.alignment = TextAnchor.MiddleRight;
                        rowLabelStyle.fontStyle = FontStyle.Bold;
                        rowLabelStyle.fontSize = 10;

                        GUIStyle toggleStyle = new GUIStyle("toggle");
                        rowStyle.margin.left = 14;

                        // Draw background box with a softer color
                        Color oldColor = GUI.backgroundColor;
                        Color newColor = new Color(1, 1, 1, 0.8f);
                        GUI.backgroundColor = newColor;
                        EditorGUILayout.BeginHorizontal(tableStyle);
                        GUI.backgroundColor = oldColor;

                        // These are the nested for loops that draw the matrix
                        for (int x = -1; x < IMLConfigNode.ExpectedOutputList.Count; x++)
                        {
                            EditorGUILayout.BeginVertical((x == -1) ? headerColumnStyle : columnStyle);
                            for (int y = -1; y < IMLConfigNode.ExpectedInputList.Count; y++)
                            {
                                // Draws the top left info label
                                if (x == -1 && y == -1)
                                {
                                    EditorGUILayout.BeginVertical(rowHeaderStyle);
                                    EditorGUILayout.LabelField("", cornerLabelStyle);
                                    EditorGUILayout.EndHorizontal();
                                }
                                // Draws the label for the row (Inputs)
                                else if (x == -1)
                                {
                                    EditorGUILayout.BeginVertical(columnHeaderStyle);
                                    EditorGUILayout.LabelField("Input " + y, rowLabelStyle);
                                    EditorGUILayout.EndHorizontal();
                                }
                                // Draws the label for the column (Outputs)
                                else if (y == -1)
                                {
                                    EditorGUILayout.BeginVertical(rowHeaderStyle);
                                    EditorGUILayout.LabelField("Output " + x, columnLabelStyle);
                                    EditorGUILayout.EndHorizontal();
                                }

                                // Draws the check box in the feature to feature matrix
                                if (x >= 0 && y >= 0)
                                {
                                    EditorGUILayout.BeginHorizontal(rowStyle);
                                    // This should change an option in a matrix inside the IMLConfiguration component
                                    EditorGUILayout.Toggle(true);
                                    //levels.allLevels[i].board[x, y] = (BlockColors)EditorGUILayout.EnumPopup(levels.allLevels[i].board[x, y], enumStyle);
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndHorizontal();

                    }

                }


            }

        private void ShowModelOutput()
        {
            IMLConfiguration IMLConfigNode = (IMLConfiguration)target;
            EditorGUILayout.Space();

            showModelOutput = true;
            showModelOutput = EditorGUILayout.Foldout(showModelOutput, "Model Output");
            if (showModelOutput)
            {

                for (int i = 0; i < IMLConfigNode.PredictedOutput.Count; i++)
                {
                    //IMLConfigNode.TransformPredictedOuputToIMLTypes();
                    var outputFeature = IMLConfigNode.PredictedOutput[i];
                    switch (outputFeature.DataType)
                    {
                        case IMLSpecifications.DataTypes.Float:
                            EditorGUILayout.FloatField("Output " + i + ": ", outputFeature.Values[0]);
                            break;
                        case IMLSpecifications.DataTypes.Integer:
                            EditorGUILayout.IntField("Output " + i + ": ", (int) outputFeature.Values[0]);
                            break;
                        case IMLSpecifications.DataTypes.Vector2:
                            Vector2 values = new Vector2(outputFeature.Values[0], outputFeature.Values[1]);
                            EditorGUILayout.Vector2Field("Output " + i + ": ", values);                       
                            break;
                        case IMLSpecifications.DataTypes.Vector3:
                            Vector3 valuesV3 = new Vector3(outputFeature.Values[0], outputFeature.Values[1], outputFeature.Values[2]);
                            EditorGUILayout.Vector3Field("Output " + i + ": ", valuesV3);
                            break;
                        case IMLSpecifications.DataTypes.Vector4:
                            Vector4 valuesV4 = new Vector4(outputFeature.Values[0], outputFeature.Values[1], outputFeature.Values[2], outputFeature.Values[3]);
                            EditorGUILayout.Vector4Field("Output " + i + ": ", valuesV4);
                            break;
                        case IMLSpecifications.DataTypes.SerialVector:
                            EditorGUILayout.LabelField("Output " + i + ": ");
                            EditorGUI.indentLevel++;
                            for (int j = 0; j < outputFeature.Values.Length; j++)
                            {
                                EditorGUILayout.FloatField("", outputFeature.Values[j]);
                            }
                            EditorGUI.indentLevel--;
                            break;
                        default:
                            break;
                    }

                    // DEBUG CODE
                    //if (IMLConfigNode.Running && outputFeature.Values.Length > 0)
                    //{
                    //    for (int j = 0; j < outputFeature.Values.Length; j++)
                    //    {
                    //        Debug.Log(outputFeature.Values[j]);

                    //    }

                    //}

                }
            }
        }

        #endregion

    }

}
