using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Compass
{
    public class CompassUI : MonoBehaviour
    {
        [Header("Value")]
        public float Bearing = 0;
        
        #region Inspector
        [Header("Assets")]
        public GameObject ElementPrefab;

        [Header("Settings")]
        public int ElementCount = 12;
        public float Width = 1;
        public float Angle = 120;
        public float Snap = 1f;

        [Header("Components")]
        public Text BearingText;
        #endregion

        #region Private
        private CompassUIElement[] Elements; 
        #endregion

        private void Start()
        {
            //Initialize Elements
            Elements = new CompassUIElement[ElementCount];
            for (int i = 0; i < ElementCount; i++)
            {
                Elements[i] = GameObject.Instantiate(ElementPrefab, transform).GetComponent<CompassUIElement>();

                //Reset position
                Elements[i]._rectTransform.anchoredPosition = new Vector2();
            }
        }
        private void Update()
        {
            //Update main text
            UpdateMainText(Bearing);

            //Update every single element
            for (int i = 0; i < Elements.Length; i++)
                Elements[i].OnUpdate();

            //Calculate the transforms
            Calculate(Bearing);
        }


        #region Main Bearing Text
        private float lastSetMainBearing = -1;
        private void UpdateMainText(float newBearing)
        {
            //Don't update if difference isn't much.
            if (Mathf.Abs(lastSetMainBearing - newBearing) < 0.1f)
                return;
            lastSetMainBearing = newBearing;


            newBearing = Normalize(newBearing);
            float anglePerElement = Angle / Elements.Length;

            //This is the range value that we will snap bearing to one of 'North','NorthEast','East'... value.
            float range = anglePerElement* Snap;

            getDelta(newBearing, range, out int value);

            //Check if bearing is close to North,NorthEast,East,SouthEast... If it's set main text as the direction if not as numberic value. 

            //Bearing is close to one of North,NorthEast,East,SouthEast... direction.
            if (value != -1)
            {
                BearingText.fontSize = 32;
                BearingText.text = GetString(value);
            }
            else
            {
                BearingText.fontSize = 18;
                BearingText.text = newBearing.ToString("f0");
            }
        }
        #endregion

        #region Elements Transform
        private float lastSetBearing = -1;
        private void Calculate(float newBearing)
        {
            if (Mathf.Abs(lastSetBearing - newBearing) < 0.1f)
                return;
            lastSetBearing = newBearing;

            newBearing = Normalize(newBearing);

            float halfAngle = Angle / 2f;
            float anglePerElement = Angle / Elements.Length;

            Vector2 position = default(Vector2);

            for (int i = 0; i < Elements.Length; i++)
            {
                //Position
                position.x = newBearing + ((i * anglePerElement));
                position.x %= Angle;
                position.x -= halfAngle;
                Elements[i].bearingValue = -position.x;
                position.x *= -Width;

                //Transform Flag
                bool changedPosition = Mathf.Abs(Elements[i].targetPosition.x - position.x) > 0.01f;
                if (changedPosition)
                    Elements[i].targetPosition = position;

                //Label Flag
                string newString = GetString(Normalize(Mathf.Round(Elements[i].bearingValue + newBearing)));
                bool changedString = Elements[i].targetString != newString;
                if (changedString)
                    Elements[i].targetString = newString;

                float distance = Mathf.Abs(Elements[i].bearingValue);
                Elements[i].targetAlpha = ((halfAngle - distance) / halfAngle);
                float f = Mathf.Clamp((halfAngle - distance) / halfAngle, 0.5f, 1f);
                Elements[i].targetScale = new Vector3(f, f, f);

                if (changedPosition)
                    Elements[i].transformChangedFlag = true;
                if (changedString)
                    Elements[i].stringChangedFlag = true;
            }
        } 
        #endregion

        #region Functions
        private string GetString(float angle)
        {
            if (angle == 0)
                return "N";
            if (angle == 45)
                return "NE";
            if (angle == 90)
                return "E";
            if (angle == 135)
                return "SE";
            if (angle == 180)
                return "S";
            if (angle == 225)
                return "SW";
            if (angle == 270)
                return "W";
            if (angle == 315)
                return "NW";
            return angle.ToString();
        }
        private float FixFloat(float f)
        {
            if (Mathf.Abs(f) < 0.001)
                return 0;
            return f;
        }
        private float Normalize(float f)
        {
            f = f % 360;
            if (f < 0)
                f += 360;
            return f;
        }
        private float getDelta(float f, float range, out int value)
        {
            value = -1;
            float min = float.PositiveInfinity;
            if (Mathf.Abs(f) < range)
            {
                float d = Mathf.Abs(f);
                if (d < min)
                {
                    min = d;
                    value = 0;
                }
            }
            if (Mathf.Abs(45 - f) < range)
            {
                float d = Mathf.Abs(45 - f);
                if (d < min)
                {
                    min = d;
                    value = 45;
                }
            }
            if (Mathf.Abs(90 - f) < range)
            {
                float d = Mathf.Abs(90 - f);
                if (d < min)
                {
                    min = d;
                    value = 90;
                }
            }
            if (Mathf.Abs(135 - f) < range)
            {
                float d = Mathf.Abs(135 - f);
                if (d < min)
                {
                    min = d;
                    value = 135;
                }
            }
            if (Mathf.Abs(180 - f) < range)
            {
                float d = Mathf.Abs(180 - f);
                if (d < min)
                {
                    min = d;
                    value = 180;
                }
            }
            if (Mathf.Abs(225 - f) < range)
            {
                float d = Mathf.Abs(225 - f);
                if (d < min)
                {
                    min = d;
                    value = 225;
                }
            }
            if (Mathf.Abs(270 - f) < range)
            {
                float d = Mathf.Abs(270 - f);
                if (d < min)
                {
                    min = d;
                    value = 270;
                }
            }
            if (Mathf.Abs(315 - f) < range)
            {
                float d = Mathf.Abs(315 - f);
                if (d < min)
                {
                    min = d;
                    value = 315;
                }
            }
            if (Mathf.Abs(360 - f) < range)
            {
                float d = Mathf.Abs(360 - f);
                if (d < min)
                {
                    min = d;
                    value = 0;
                }
            }
            return min;
        } 
        #endregion
    }
}
