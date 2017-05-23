/*
MIT License

Copyright(c) 2016 twnkls | augmented reality

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

/**
 *  com.twnkls.HoloLib.HoloSpatialMappingManager
 *  
 *  Manages the spatial mapping of the environment
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.1
 *  Date   : 12 08 2016 
 * 
 */
namespace com.twnkls.HoloLib.Managers
{
    public class HoloSpatialMappingManager
    {
        //level of detail.
        public enum LOD
        {
            HIGH,
            MEDIUM,
            LOW
        }

        //publics.
        public UnityEngine.Material DrawMaterial { set; get; }
        public bool Initialized { get; private set; }
        public int PhysicsLayer { get; set; }
        public static int PhysicsRaycastMask;
        

        //privates.
        private bool _visualize         = false;
        private bool _mappingEnabled    = true;
        private float _mappingSpeed     = 2.5f;
        private int _removalUpdateCount = 10;
        private LOD _levelOfDetail      = LOD.MEDIUM;
        private UnityEngine.Vector3 _mappingSize;
        private UnityEngine.GameObject _targetObj;
        private UnityEngine.VR.WSA.SpatialMappingRenderer _spatialMappingRenderer;
        private UnityEngine.VR.WSA.SpatialMappingCollider _spatialMappingCollider;
        


        /// <summary>
        /// singleton constructor.
        /// </summary>
        /// <returns></returns>
        private static HoloSpatialMappingManager _instance;
        private HoloSpatialMappingManager() {
            Initialized  = false;
            PhysicsLayer = 31;
            _mappingSize = new UnityEngine.Vector3(3.0f, 3.0f, 3.0f);
        }
        public static HoloSpatialMappingManager GetInstance()
        {
            if (_instance == null)
                _instance = new HoloSpatialMappingManager();
            return _instance;
        }


        /// <summary>
        /// Sets the needed components on specified gameobject.
        /// </summary>
        /// <param name="obj">UnityEngine.GameObject</param>
        public void Init( UnityEngine.GameObject obj )
        {
            //check if we already have a target.
            if (_targetObj != null) Clear();

            //set target object.
            _targetObj = obj;

            //create object components.
            CreateComponents();

            //set raycast mask
            PhysicsRaycastMask = 1 << PhysicsLayer;

            //set initialized.
            Initialized = true;
        }


        /// <summary>
        /// Creates the components need to do spatial mapping.
        /// </summary>
        private void CreateComponents()
        {
            if (_targetObj == null) return;
            //setup components on provided gameobject.
            _spatialMappingRenderer = _targetObj.AddComponent<UnityEngine.VR.WSA.SpatialMappingRenderer>();
            _spatialMappingRenderer.surfaceParent = _targetObj;

            //set material.
            _spatialMappingRenderer.visualMaterial = DrawMaterial;
            _spatialMappingRenderer.renderState    = UnityEngine.VR.WSA.SpatialMappingRenderer.RenderState.None;

            //set collider.
            _spatialMappingCollider = _targetObj.AddComponent<UnityEngine.VR.WSA.SpatialMappingCollider>();
            _spatialMappingCollider.surfaceParent = _targetObj;
            _spatialMappingCollider.layer = PhysicsLayer;

            //SET VALUES.
            SetProperties();
        }


        /// <summary>
        /// Sets the properties of SpatialMappingRenderer / Collider
        /// </summary>
        private void SetProperties()
        {
            if (_spatialMappingRenderer != null )
            {
                _spatialMappingRenderer.halfBoxExtents          = _mappingSize;
                _spatialMappingRenderer.secondsBetweenUpdates   = _mappingSpeed;
                _spatialMappingRenderer.lodType                = (UnityEngine.VR.WSA.SpatialMappingBase.LODType)_levelOfDetail;
                _spatialMappingRenderer.numUpdatesBeforeRemoval = _removalUpdateCount;
                _spatialMappingRenderer.freezeUpdates           = !_mappingEnabled;
            }
            if (_spatialMappingCollider != null)
            {
                _spatialMappingCollider.halfBoxExtents          = _mappingSize;
                _spatialMappingCollider.secondsBetweenUpdates   = _mappingSpeed;
                _spatialMappingCollider.lodType                 = (UnityEngine.VR.WSA.SpatialMappingBase.LODType)_levelOfDetail;
                _spatialMappingCollider.numUpdatesBeforeRemoval = _removalUpdateCount;
                _spatialMappingCollider.freezeUpdates           = !_mappingEnabled;
            }

        }


       


        /// <summary>
        /// Clears the TargetObj from all spatial mapping components.
        /// </summary>
        public void Clear()
        {
            if (_targetObj == null) return;
            UnityEngine.GameObject.Destroy(_spatialMappingRenderer);
            UnityEngine.GameObject.Destroy(_spatialMappingCollider);
            _targetObj = null;
        }


        /// <summary>
        /// Sets / Gets the bounding box of the spatial renderer and the spatial collider.
        /// </summary>
        public UnityEngine.Vector3 MappingSize
        {
            set
            {
                _mappingSize = value;
                SetProperties();
            }
            get
            {
                return _mappingSize;
            }
        }


        /// <summary>
        /// Sets / Gets the mapping speed
        /// </summary>
        public float MappingSpeed
        {
            set
            {
                _mappingSpeed = value;
                SetProperties();
            }
            get
            {
                return _mappingSpeed;
            }
        }


        /// <summary>
        /// Sets / Gets the level of detail.
        /// </summary>
        public LOD LevelOfDetail
        {
            set
            {
                _levelOfDetail = value;
                SetProperties();
            }
            get
            {
                return _levelOfDetail;
            }
        }


        /// <summary>
        /// Sets / Gets the update count for removal of 
        /// spatial mapping outside of the bounding size.
        /// </summary>
        public int RemovalUpdateCount
        {
            set
            {
                _removalUpdateCount = value;
                SetProperties();
            }
            get
            {
                return _removalUpdateCount;
            }
        }


        /// <summary>
        /// Sets / Gets mapping visualisation
        /// </summary>
        public bool Visualize
        {
            set
            {
                _visualize = value;
                if (_visualize )
                {
                    if (!Initialized) return;
                    _spatialMappingRenderer.renderState = UnityEngine.VR.WSA.SpatialMappingRenderer.RenderState.Visualization;
                }
                else
                {
                    if (!Initialized) return;
                    _spatialMappingRenderer.renderState = UnityEngine.VR.WSA.SpatialMappingRenderer.RenderState.None;
                }
            }
            get
            {
                return _visualize;
            }
        }


        /// <summary>
        /// Sets / Gets the mapping and collising rendering.
        /// </summary>
        public bool MappingEnabled
        {
            set
            {
                _mappingEnabled = value;
                SetProperties();
            }
            get
            {
                return _mappingEnabled;
            }
        }
    }
}
