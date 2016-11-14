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
 * 
 *  com.twnkls.HoloLib.HoloGazeHit
 *  
 *  Dataholder for raycast hits.
 *  
 *  Author : Robin Kollau
 *  Version: 1.0.0
 *  Date   : 11 11 2016 
 *  
 *  ------------------------------------------
 *  
 *  com.twnkls.HoloLib.HoloGazeManager
 *  
 *  Updates the user head information.
 *  Casts a ray in the head direction.
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.2
 *  Date   : 11 11 2016 
 * 
 */
namespace com.twnkls.HoloLib.Managers
{
    //GAZEHIT
    public class HoloGazeHit
    {
        public UnityEngine.RaycastHit RayHit;
        public bool HasHit = false;
    }


    //HOLOGAZEMANAGER
    public class HoloGazeManager
    {
        //delegates.
        public delegate void OnTappedEvent();
        public OnTappedEvent onTappedDelegate;

        public delegate void OnHoldStartEvent();
        public OnHoldStartEvent onHoldStartDelegate;

        public delegate void OnHoldEndEvent();
        public OnHoldEndEvent onHoldEndDelegate;

        public delegate void OnManipulateStartEvent();
        public OnManipulateStartEvent onManipulationStartDelegate;

        public delegate void OnManipulateEvent();
        public OnManipulateEvent onManipulationDelegate;

        public delegate void OnManipulateEndEvent();
        public OnManipulateEndEvent onManipulationEndDelegate;


        //publics.
        public UnityEngine.GameObject FocusedObject { get; private set; }
        public UnityEngine.GameObject ManipulationObject { get; private set; }
        public Misc.HoloHead Head { private set; get; }
        public HoloGazeHit GazeHit { private set; get; }

        //privates
        private UnityEngine.RaycastHit _rayHit;
        private HoloGazeHit _gazeHit;
        private UnityEngine.VR.WSA.Input.GestureRecognizer _gestureRecognizer;
        //private UnityEngine.VR.WSA.Input.InteractionManager _interactionManager;


        /// <summary>
        /// Singleton constructor.
        /// use: GazeManager.GetInstance()
        /// </summary>
        private static HoloGazeManager _instance = null;
        private HoloGazeManager()
        {
            //head instance.
            Head = new Misc.HoloHead();

            //Gaze hit instance.
            GazeHit = new HoloGazeHit();

            //gesture recognizer.
            _gestureRecognizer = new UnityEngine.VR.WSA.Input.GestureRecognizer();

            //set recognizable gestures.
            _gestureRecognizer.SetRecognizableGestures(UnityEngine.VR.WSA.Input.GestureSettings.Tap   |
                                                        UnityEngine.VR.WSA.Input.GestureSettings.Hold |
                                                        UnityEngine.VR.WSA.Input.GestureSettings.ManipulationTranslate |
                                                        UnityEngine.VR.WSA.Input.GestureSettings.DoubleTap );

            //attach listeners.
            _gestureRecognizer.TappedEvent                += OnTapEvent;
            _gestureRecognizer.HoldStartedEvent           += OnHoldStartedEvent;
            _gestureRecognizer.HoldCompletedEvent         += OnHoldCompletedEvent;
            _gestureRecognizer.ManipulationStartedEvent   += OnManipulationStartedEvent;
            _gestureRecognizer.ManipulationUpdatedEvent   += OnManipulationUpdateEvent;
            _gestureRecognizer.ManipulationCompletedEvent += OnManipulationEndedEvent;

            //start recognizing.
            _gestureRecognizer.StartCapturingGestures();
        }
        public static HoloGazeManager GetInstance()
        {
            if (_instance == null)
                _instance = new HoloGazeManager();
            return _instance;
        }


        /// <summary>
        /// Fired when user tapped an object in the scene.
        /// </summary>
        /// <param name="source">InteractionSourceKind</param>
        /// <param name="tapCount">int</param>
        /// <param name="ray">Ray</param>
        private void OnTapEvent( UnityEngine.VR.WSA.Input.InteractionSourceKind source, int tapCount, UnityEngine.Ray head_ray )
        {
            if (onTappedDelegate != null)
                onTappedDelegate();

           if ( this.FocusedObject != null )
           {
                this.FocusedObject.SendMessageUpwards(Misc.HoloEvents.ON_SELECT);
           }
        }

        
        /// <summary>
        /// Fired when user started holding the focussed object.
        /// </summary>
        /// <param name="source">InteractionSourceKind</param>
        /// <param name="ray">Ray</param>
        private void OnHoldStartedEvent(UnityEngine.VR.WSA.Input.InteractionSourceKind source, UnityEngine.Ray head_ray)
        {
            if (onHoldStartDelegate != null)
                onHoldStartDelegate();

            if (this.FocusedObject != null)
            {
                this.FocusedObject.SendMessageUpwards(Misc.HoloEvents.ON_HOLD_START, head_ray);
            }
        }


        /// <summary>
        /// Fired when user completed holding the focussed object.
        /// </summary>
        /// <param name="source">InteractionSourceKind</param>
        /// <param name="ray">Ray</param>
        private void OnHoldCompletedEvent(UnityEngine.VR.WSA.Input.InteractionSourceKind source, UnityEngine.Ray head_ray)
        {
            if (onHoldEndDelegate != null)
                onHoldEndDelegate();

            if (this.FocusedObject != null)
            {
                this.FocusedObject.SendMessageUpwards(Misc.HoloEvents.ON_HOLD_END, head_ray);
            }
        }


        /// <summary>
        /// Fired when user started manipulating the focussed object.
        /// </summary>
        /// <param name="source">InteractionSourceKind</param>
        /// <param name="cumulativeData">Vector3</param>
        /// /// <param name="head_ray">Ray</param>
        private void OnManipulationStartedEvent(UnityEngine.VR.WSA.Input.InteractionSourceKind source, UnityEngine.Vector3 cumulativeData, UnityEngine.Ray head_ray)
        {
            if (onManipulationStartDelegate != null)
                onManipulationStartDelegate();

            if (this.FocusedObject != null)
            {
                this.ManipulationObject = this.FocusedObject;
                this.ManipulationObject.SendMessageUpwards(Misc.HoloEvents.ON_MANIPULATE_START, cumulativeData);
            }
        }


        /// <summary>
        /// Fired when user is manipulating the manipulation object.
        /// </summary>
        /// <param name="source">InteractionSourceKind</param>
        /// <param name="cumulativeData">Vector3</param>
        /// /// <param name="head_ray">Ray</param>
        private void OnManipulationUpdateEvent(UnityEngine.VR.WSA.Input.InteractionSourceKind source, UnityEngine.Vector3 cumulativeData, UnityEngine.Ray head_ray)
        {
            if (onManipulationDelegate != null)
                onManipulationDelegate();

            if (this.ManipulationObject != null)
            {
                this.ManipulationObject.SendMessageUpwards(Misc.HoloEvents.ON_MANIPULATE, cumulativeData);
            }
        }


        /// <summary>
        /// Fired when user has completed manipulating the manipulation object.
        /// </summary>
        /// <param name="source">InteractionSourceKind</param>
        /// <param name="cumulativeData">Vector3</param>
        /// /// <param name="head_ray">Ray</param>
        private void OnManipulationEndedEvent(UnityEngine.VR.WSA.Input.InteractionSourceKind source, UnityEngine.Vector3 cumulativeData, UnityEngine.Ray head_ray)
        {
            if (onManipulationEndDelegate != null)
                onManipulationEndDelegate();

            if (this.ManipulationObject != null)
            {
                this.ManipulationObject.SendMessageUpwards(Misc.HoloEvents.ON_MANIPULATE_END, cumulativeData);
                this.ManipulationObject = null;
            }
        }


        /// <summary>
        /// Finds the object that is focussed on.
        /// if none are found focussed object will be NULL
        /// </summary>
        public void FindFocusedObject()
        {
            //create prevFocusedObject
            UnityEngine.GameObject prevFocusedObject = FocusedObject;

            //shoot ray.
            GazeHit.HasHit = UnityEngine.Physics.Raycast(Head.Position, Head.Direction, out GazeHit.RayHit);

            //Shoot ray and store result.
            if (GazeHit.HasHit)
            {
                this.FocusedObject = this.GazeHit.RayHit.collider.gameObject;
                if (this.FocusedObject.GetComponent<Unity.HoloObject>() != null)
                    if (this.FocusedObject.GetComponent<Unity.HoloObject>().HasFocus == false)
                        this.FocusedObject.GetComponent<Unity.HoloObject>().OnFocusIn();
            }
            else
            {
                if (this.FocusedObject != null)
                {
                    if (this.FocusedObject.GetComponent<Unity.HoloObject>() != null)
                        if (this.FocusedObject.GetComponent<Unity.HoloObject>().HasFocus == true)
                            this.FocusedObject.GetComponent<Unity.HoloObject>().OnFocusOut();
                }
                  
                this.FocusedObject = null;
            }


            // check if prev focused object is not focused object
            // reset the gestures and start over.
            if (this.FocusedObject != prevFocusedObject)
            {
                if (prevFocusedObject != null)
                {
                    if (prevFocusedObject.GetComponent<Unity.HoloObject>() != null)
                        if (prevFocusedObject.GetComponent<Unity.HoloObject>().HasFocus == true)
                            prevFocusedObject.GetComponent<Unity.HoloObject>().OnFocusOut();
                }
                   
                _gestureRecognizer.CancelGestures();
                _gestureRecognizer.StartCapturingGestures();
            }
        }


        /// <summary>
        /// Updates the head and shoots a ray in the direction the
        /// head is looking at ( gaze )
        /// </summary>
        public void Update()
        {
            //update the head.
            Head.Update();

            //Find focussed object.
            this.FindFocusedObject();
        }
    }
}

