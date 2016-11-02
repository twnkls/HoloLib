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
 *  com.twnkls.HoloLib.HoloGazeManager
 *  
 *  Updates the user head information.
 *  Casts a ray in the head direction.
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.1
 *  Date   : 10 08 2016 
 * 
 */
namespace com.twnkls.HoloLib
{
    public class HoloGazeManager
    {
        //delegates.
        public delegate void OnTappedEvent();
        public OnTappedEvent onTappedDelegate;

        public delegate void OnHoldStartEvent();
        public OnHoldStartEvent onHoldStartDelegate;

        public delegate void OnHoldEndEvent();
        public OnHoldEndEvent onHoldEndDelegate;

        //publics.
        public UnityEngine.GameObject FocusedObject { get; private set; }
        public HoloHead Head { private set; get; }

        //privates
        private UnityEngine.RaycastHit _rayHit;
        private UnityEngine.VR.WSA.Input.GestureRecognizer _gestureRecognizer;


        /// <summary>
        /// Singleton constructor.
        /// use: GazeManager.GetInstance()
        /// </summary>
        private static HoloGazeManager _instance = null;
        private HoloGazeManager()
        {
            //head instance.
            Head = new HoloHead();

            //gesture recognizer.
            _gestureRecognizer = new UnityEngine.VR.WSA.Input.GestureRecognizer();
            _gestureRecognizer.TappedEvent                += OnTapEvent;
            _gestureRecognizer.HoldStartedEvent           += OnHoldStartedEvent;
            _gestureRecognizer.HoldCompletedEvent         += OnHoldCompletedEvent;          
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
        /// Gets the ray hit.
        /// </summary>
        public UnityEngine.RaycastHit RayHit
        {
            get
            {
                return _rayHit;
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

            //Shoot ray and store result.
            if (UnityEngine.Physics.Raycast(Head.Position, Head.Direction, out _rayHit))
            {
                this.FocusedObject = this.RayHit.collider.gameObject;
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

