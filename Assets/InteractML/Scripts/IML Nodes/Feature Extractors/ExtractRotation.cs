﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XNode;

namespace InteractML.FeatureExtractors
{
    /// <summary>
    /// Feature extractor for rotations
    /// </summary>
    public class ExtractRotation : Node, IFeatureIML
    {
        /// <summary>
        /// GameObject from which we extract a feature
        /// </summary>
        [Input]
        public GameObject gameObjectIntoNode;

        /// <summary>
        /// Node data sent outside of this node onwards
        /// </summary>
        [Output]
        public Node rotationExtracted;

        /// <summary>
        /// Controls if we output local or world space
        /// </summary>
        public bool useLocalSpace;

        /// <summary>
        /// Feature Values extracted (ready to be read by a different node)
        /// </summary>
        public IMLBaseDataType FeatureValues { get { return m_RotationExtracted; } }

        /// <summary>
        /// The private feature values extracted in a more specific data type
        /// </summary>
        [SerializeField]
        private IMLVector4 m_RotationExtracted;

        [HideInInspector]
        public bool GameObjInputMissing;

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

            if (m_RotationExtracted == null)
            {
                m_RotationExtracted = new IMLVector4();

            }
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return UpdateFeature();
        }

        /// <summary>
        /// Updates Feature values
        /// </summary>
        /// <returns></returns>
        public object UpdateFeature()
        {
            if (m_RotationExtracted == null)
            {
                m_RotationExtracted = new IMLVector4();

            }

            var gameObjRef = GetInputValue<GameObject>("gameObjectIntoNode", this.gameObjectIntoNode);

            if (gameObjRef == null)
            {
                if ((graph as IMLController).IsGraphRunning)
                {
                    // If the gameobject is null, we throw an error on the editor console
                    Debug.LogWarning("GameObject missing in Extract Rotation Node!");
                }
                GameObjInputMissing = true;
            }
            else
            {
                // Set values of our feature extracted
                if (useLocalSpace)
                {
                    m_RotationExtracted.SetValues(gameObjRef.transform.localRotation);
                }
                else
                {
                    m_RotationExtracted.SetValues(gameObjRef.transform.rotation);
                }
                GameObjInputMissing = false;
            }

            return this;

        }
    }
}
