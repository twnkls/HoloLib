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
 *  com.twnkls.HoloLib.HoloVoiceManager
 *  
 *  Repsonsible for all speech recognition and invokation
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.1
 *  Date   : 10 08 2016 
 * 
*/
using System.Collections.Generic;
using System.Linq;
namespace com.twnkls.HoloLib.Managers
{
    public class HoloVoiceManager
    {
        //publics
        public Dictionary<string, System.Action> KeyWords { get; private set; }
        public bool Initialized { get; private set; }

        //privates
        private UnityEngine.Windows.Speech.KeywordRecognizer _keywordRecognizer;
        

        /// <summary>
        /// Singleton constructor
        /// </summary>
        private static HoloVoiceManager _instance;
        public static HoloVoiceManager GetInstance()
        {
            if (_instance == null)
                _instance = new HoloVoiceManager();
            return _instance;
        }
        private HoloVoiceManager()
        {
            this.KeyWords = new Dictionary<string, System.Action>();
        }


        /// <summary>
        /// Adds a keyword with an action to the list.
        /// </summary>
        /// <param name="word">string</param>
        /// <param name="action">Action</param>
        public void AddKeyWord( string word, System.Action action )
        {
            //add keyword to dictionary.
            this.KeyWords.Add(word, action);

            //check if we are already initialized.
            if (Initialized)
            {
                //reinitialize
                this.Clear();
                this.Init();
            }
        }


        /// <summary>
        /// Initializes the keyword recognizer.
        /// Adds the keywords to the recognizer
        /// Sets the initialized flag to true.
        /// </summary>
        public void Init()
        {
            _keywordRecognizer = new UnityEngine.Windows.Speech.KeywordRecognizer( this.KeyWords.Keys.ToArray() );
            _keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
            _keywordRecognizer.Start();

            Initialized = true;
        }


        /// <summary>
        /// Stops the key word recognizer
        /// Clears the key word recognizer from listeners
        /// Nullifies the keyword recognizer.
        /// Sets the initialized flag to FALSE
        /// </summary>
        public void Clear()
        {
            _keywordRecognizer.Stop();
            _keywordRecognizer.OnPhraseRecognized -= OnPhraseRecognized;
            _keywordRecognizer = null;

            Initialized = false;
        }


        /// <summary>
        /// Fired when a phrase / keyword is recognized.
        /// </summary>
        /// <param name="args"></param>
        private void OnPhraseRecognized( UnityEngine.Windows.Speech.PhraseRecognizedEventArgs args )
        {
            System.Action keywordAction;
            if( this.KeyWords.TryGetValue( args.text, out keywordAction ) )
            {
                keywordAction.Invoke();
            }
        }
    }
}
