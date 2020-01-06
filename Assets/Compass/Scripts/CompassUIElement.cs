using System;
using UnityEngine;
using UnityEngine.UI;

namespace Compass
{
    public class CompassUIElement : MonoBehaviour
    {
        #region Inspector
        [NonSerialized] public RectTransform _rectTransform;
        [NonSerialized] public CanvasGroup _canvasGroup;
        [SerializeField] private Text _label;
        #endregion

        //Bearing value
        [NonSerialized] public float bearingValue;

        //A flag indicates that transform is changed
        [NonSerialized] public bool transformChangedFlag = false;
        [NonSerialized] public Vector2 targetPosition;
        [NonSerialized] public float targetAlpha;
        [NonSerialized] public Vector3 targetScale;

        //A flag indicates that string value is changed
        [NonSerialized] public bool stringChangedFlag = false;
        [NonSerialized] public string targetString;

        /*
        I designed this system for multithreading in fact.
        So position,scale,string value,alpha values were being calculate in another thread and only results were being applied.
        That's why there are some flag things going there.
        */


        private void Awake()
        {
            this._rectTransform = GetComponent<RectTransform>();
            this._canvasGroup = GetComponent<CanvasGroup>();
        }
        public void OnUpdate()
        {
            //Check if transform flag set
            if (transformChangedFlag)
            {
                transformChangedFlag = false;

                _rectTransform.anchoredPosition = targetPosition;
                _rectTransform.localScale = targetScale;
                _canvasGroup.alpha = targetAlpha;
            }

            //Check if string flag set
            if (stringChangedFlag)
            {
                stringChangedFlag = false;
                _label.text = targetString;
            }
        }
    }
}
