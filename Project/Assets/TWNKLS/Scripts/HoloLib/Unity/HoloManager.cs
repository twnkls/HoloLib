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
 *  com.twnkls.HoloLib.Unity.HoloManager
 *  
 *  Manages the com.twnkls.HoloLib internal components.
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.1
 *  Date   : 11 11 2016 
 * 
 */
namespace com.twnkls.HoloLib.Unity
{
    public class HoloManager : UnityEngine.MonoBehaviour
    {
        //publics.
        [UnityEngine.SerializeField] public UnityEngine.Material WireframeMaterial;
        [UnityEngine.SerializeField] public UnityEngine.Vector3 SpatialMappingSize;
        [UnityEngine.SerializeField] public float SpatialMappingSpeed = 2.5f;
        [UnityEngine.SerializeField] public Managers.HoloSpatialMappingManager.LOD MappingLOD;
        [UnityEngine.SerializeField] public bool VisualizeMap   = false;
        [UnityEngine.SerializeField] public bool MappingEnabled = false;
        [UnityEngine.SerializeField] public float MappingDuration = 1.0f;


        //privates.
        private float _currentMappingTime = 0;
        private Managers.HoloGazeManager _gazeManager;
        private Managers.HoloHandsManager _handsManager;
        private Managers.HoloVoiceManager _voiceManager;
        private Managers.HoloSpatialMappingManager _mappingManager;

        // Use this for initialization
        void Awake()
        {
            //get the gaze manager.
            GazeManager.Update();

            //setup the voice manager.
           // VoiceManager.Init();

            //setup the hands manager.
            HandsManager.Init();

            //Setup spatial mapping.
            MappingManager.Init(this.gameObject);
            MappingManager.DrawMaterial   = WireframeMaterial;
            MappingManager.MappingSize    = SpatialMappingSize;
            MappingManager.MappingSpeed   = SpatialMappingSpeed;
            MappingManager.LevelOfDetail  = MappingLOD;
            MappingManager.Visualize      = VisualizeMap;
            MappingManager.MappingEnabled = MappingEnabled;
        }


        /// <summary>
        /// Gets the gaze manager.
        /// </summary>
        public Managers.HoloGazeManager GazeManager
        {
            get
            {
                if(_gazeManager == null )
                    _gazeManager = Managers.HoloGazeManager.GetInstance();
                return _gazeManager;
            }
        }


        /// <summary>
        /// Gets the hands manager.
        /// </summary>
        public Managers.HoloHandsManager HandsManager
        {
            get
            {
                if (_handsManager == null)
                    _handsManager = Managers.HoloHandsManager.GetInstance();
                return _handsManager;
            }
        }


        /// <summary>
        /// Gets the voice recognition manager
        /// </summary>
        public Managers.HoloVoiceManager VoiceManager
        {
            get
            {
                if (_voiceManager == null)
                    _voiceManager = Managers.HoloVoiceManager.GetInstance();
                return _voiceManager;
            }
        }


        /// <summary>
        /// Gets the Spatial mapping manager.
        /// </summary>
        public Managers.HoloSpatialMappingManager MappingManager
        {
            get
            {
                if(_mappingManager == null )
                    _mappingManager = Managers.HoloSpatialMappingManager.GetInstance();
                return _mappingManager;
            }
        }


        /// <summary>
        /// Updates the gaze manager.
        /// </summary>
        void Update()
        {
            //update the gazemanager.
            GazeManager.Update();

            //check if user enabled mapping.
            if (this.MappingEnabled)
            {
                if ( _currentMappingTime < this.MappingDuration)
                {
                    _currentMappingTime += UnityEngine.Time.deltaTime;
                }
                else if (_currentMappingTime >= this.MappingDuration)
                {
                    this.MappingEnabled           = false;
                    MappingManager.MappingEnabled = false;
                    MappingManager.Visualize      = false;
                    _currentMappingTime           = 0;
                }
            }
        }
    }
}

