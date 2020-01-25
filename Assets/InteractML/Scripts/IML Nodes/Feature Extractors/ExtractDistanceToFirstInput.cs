﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using InteractML;
using System.Linq;

namespace InteractML.FeatureExtractors
{
    /// <summary>
    /// Extracts the distance from one or several features to another one (i.e. fingers to the palm of the hand)
    /// </summary>
    public class ExtractDistanceToFirstInput : Node, IFeatureIML
    {
        /// <summary>
        /// The feature that has been previously extracted and from which we are calculating the distance from (i.e. position, rotation, etc)
        /// </summary>
        [Input]
        public Node FeatureToInput;

        /// <summary>
        /// The features that we will measure the distance to the first feature to Input
        /// </summary>
        [Input]
        public List<Node> FeaturesToMeasureDistanceToFirst;

        /// <summary>
        /// Node data sent outside of this node onwards
        /// </summary>
        [Output]
        public Node DistanceExtracted;

        /// <summary>
        /// Feature Values extracted (ready to be read by a different node)
        /// </summary>
        public IMLBaseDataType FeatureValues { get { return m_DistancesExtracted; } }

        /// <summary>
        /// The private feature values extracted in a more specific data type
        /// </summary>
        [SerializeField]
        private IMLSerialVector m_DistancesExtracted;
        private float[] m_DistancesToFirstInput;

        /// <summary>
        /// Lets external classes known if they should call UpdateFeature
        /// </summary>
        public bool isExternallyUpdatable { get { return isConnectedToSomething; } }

        /// <summary>
        /// Private logic to know when this node should be updatable
        /// </summary>
        private bool isConnectedToSomething { get { return (Outputs.Count() > 0); } }

        /// <summary>
        /// Was the feature already updated?
        /// </summary>
        public bool isUpdated { get; set; }



        // Use this for initialization
        protected override void Init()
        {
            base.Init();

            // This extractor expects any other feature extracted to make calculations
            FeatureToInput = GetInputValue<Node>("FeatureToInput");
            FeaturesToMeasureDistanceToFirst = GetInputValues<Node>("FeaturesToMeasureDistanceToFirst").ToList();
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return UpdateFeature();
        }

        public object UpdateFeature()
        {
            // This extractor expects any other feature extracted to make calculations
            FeatureToInput = GetInputValue<Node>("FeatureToInput");
            FeaturesToMeasureDistanceToFirst = GetInputValues<Node>("FeaturesToMeasureDistanceToFirst").ToList();

            if (!isUpdated)
            {
                // If we managed to get the input
                if (FeatureToInput != null && FeaturesToMeasureDistanceToFirst != null)
                {
                    // We check that it is an IML feature
                    var featureToUseIML = (FeatureToInput as IFeatureIML).FeatureValues;
                    if (featureToUseIML != null)
                    {
                        // Clear distances vector
                        m_DistancesToFirstInput = new float[FeaturesToMeasureDistanceToFirst.Count];

                        // We check that the features to measure the distance to the first are IML features
                        for (int i = 0; i < FeaturesToMeasureDistanceToFirst.Count; i++)
                        {
                            //Debug.Log("Calculating distance iteration: " + i);
                            var feautureToMeasure = FeaturesToMeasureDistanceToFirst[i];
                            var featureMeasureIML = (feautureToMeasure as IFeatureIML).FeatureValues;
                            // We check that the second inputs are also iml features
                            if (featureMeasureIML != null)
                            {
                                // Then we calculate the distance to the first feature

                                // We make sure that the features to calculate are the same
                                if (featureToUseIML.DataType == featureMeasureIML.DataType)
                                {
                                    // Calculate distance between each of the entries in the values vector
                                    float[] distancesEachFeature = new float[featureToUseIML.Values.Length];
                                    for (int j = 0; j < featureToUseIML.Values.Length; j++)
                                    {
                                        distancesEachFeature[j] = (featureMeasureIML.Values[j] - featureToUseIML.Values[j]);
                                    }
                                    // Follow the euclidean formula for distance: square and add together all distances
                                    for (int j = 0; j < featureToUseIML.Values.Length; j++)
                                    {
                                        m_DistancesToFirstInput[i] += (distancesEachFeature[j] * distancesEachFeature[j]);
                                    }

                                    // We make sure that the extracted serial vector is not null
                                    if (m_DistancesExtracted == null)
                                    {
                                        m_DistancesExtracted = new IMLSerialVector(m_DistancesToFirstInput);
                                    }

                                    // Set values for velocity extracted and for last frame feature value
                                    m_DistancesExtracted.SetValues(m_DistancesToFirstInput);

                                    // Make sure to mark the feature as updated to avoid calculating twice
                                    isUpdated = true;

                                }
                                else
                                {
                                    Debug.LogError("Features Types to measure distance are different!");
                                    return null;
                                }

                            }
                            // If we couldn't get an input (in the second input), we return null
                            else
                            {
                                Debug.LogError("Could not get second input " + i +" when measuring distance!");
                                return null;
                            }

                        }

                        return this;
                    }
                    // If we couldn't get an input (in the first input), we return null
                    else
                    {
                        Debug.LogError("Could not get first input when measuring distance!");
                        return null;
                    }
                }
                // If we couldn't get an input (at all), we return null
                else
                {
                    Debug.LogError("Could not get any input when measuring distance!");
                    return null;
                }

            }
            // Avoid calculating again and return the feature
            else
            {
                return this;
            }

        }
    }
}

