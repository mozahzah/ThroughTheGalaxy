using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace unciphering.UI
{
    public class DroneCanvas : MonoBehaviour
    {
        [SerializeField] Transform Normal_Icon;
        [SerializeField] Transform NV_Icon;
        [SerializeField] Transform Scan_Icon;
        [SerializeField] Image healthBarFill;

        public int selectedWeapon;
        Color highlightColor;
        Color nonHighlightColor;
            

        public float Normal_reloadTime {get; set;}
        public float NV_reloadTime {get; set;}
        public float Scan_reloadTime {get; set;}
        public bool Normal_isReloading;
        public bool NV_isReloading;
        public bool Scan_isReloading;

        public float cachedTime1;
        public float cachedTime2;
        public float cachedTime3;

            // Start is called before the first frame update
        void Start()
        {
            highlightColor = new Color32(255,255,255,100);
            nonHighlightColor = new Color32(255,255,255,5);
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (selectedWeapon == 2)
                {
                    selectedWeapon = 0;
                }
                else
                {
                    selectedWeapon ++;
                }
            }


            PlayReloadAnimation();
            HighlightSelection();

            if (Time.timeSinceLevelLoad - cachedTime2 > NV_reloadTime)
            {
                ResetAnimation(2);
            }
            if (Time.timeSinceLevelLoad - cachedTime3 > Scan_reloadTime)
            {
                ResetAnimation(3);
            }

        }

        private void PlayReloadAnimation()
        {
            if (Normal_isReloading == true)
            {
                Normal_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                Normal_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount -= 1.0f / Normal_reloadTime * Time.deltaTime;
            }
            if (NV_isReloading == true)
            {
                NV_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                NV_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount -= 1.0f / NV_reloadTime * Time.deltaTime;
            }
            if (Scan_isReloading == true)
            {
                Scan_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                Scan_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount -= 1.0f / Scan_reloadTime * Time.deltaTime;
            }
            }

        private void HighlightSelection()
        {
            if (selectedWeapon == 0)
            {
                Normal_Icon.GetChild(0).GetComponent<Image>().enabled = true;
                Normal_Icon.gameObject.GetComponent<RawImage>().color = highlightColor;

                NV_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                NV_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;

                Scan_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                Scan_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;
            }
            if (selectedWeapon == 1)
            {
                Normal_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                Normal_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;

                NV_Icon.GetChild(0).GetComponent<Image>().enabled = true;
                NV_Icon.gameObject.GetComponent<RawImage>().color = highlightColor;

                Scan_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                Scan_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;
            }

            if (selectedWeapon == 2)
            {
                Normal_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                Normal_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;

                NV_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                NV_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;

                Scan_Icon.GetChild(0).GetComponent<Image>().enabled = true;
                Scan_Icon.gameObject.GetComponent<RawImage>().color = highlightColor;
            }
        }

        public void ResetAnimation(int index)
        {
            if (index == 1)
            {
                Normal_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                Normal_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1;
            }
            if (index == 2)
            {
                NV_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                NV_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1;
            }
            if (index == 3)
            {
                Scan_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                Scan_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1;
            }
            else 
            {
                return;
            }
        }
    }
}

