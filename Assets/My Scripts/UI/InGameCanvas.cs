using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace unciphering.UI
{
    //[ExecuteInEditMode]
    public class InGameCanvas : MonoBehaviour
    {
        [SerializeField] Transform MG_Icon;
        [SerializeField] Transform MSL_Icon;
        [SerializeField] Transform NB_Icon;
        [SerializeField] Image healthBarFill;

        public int selectedWeapon;
        Color highlightColor;
        Color nonHighlightColor;
        

        public float MG_reloadTime {get; set;}
        public float MSL_reloadTime {get; set;}
        public float NB_reloadTime {get; set;}
        public bool MG_isReloading;
        public bool MSL_isReloading;
        public bool NB_isReloading;

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
            PlayReloadAnimation();
            HighlightSelection();

            if (Time.timeSinceLevelLoad - cachedTime2 > MSL_reloadTime)
            {
                ResetAnimation(2);
            }
            if (Time.timeSinceLevelLoad - cachedTime3 > NB_reloadTime)
            {
                ResetAnimation(3);
            }

        }

        private void PlayReloadAnimation()
        {
            if (MG_isReloading == true)
            {
                MG_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                MG_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount -= 1.0f / MG_reloadTime * Time.deltaTime;
            }
            if (MSL_isReloading == true)
            {
                MSL_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                MSL_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount -= 1.0f / MSL_reloadTime * Time.deltaTime;
            }
            if (NB_isReloading == true)
            {
                NB_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                NB_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount -= 1.0f / NB_reloadTime * Time.deltaTime;
            }
        }

        private void HighlightSelection()
        {
            if (selectedWeapon == 0)
            {
                MG_Icon.GetChild(0).GetComponent<Image>().enabled = true;
                MG_Icon.gameObject.GetComponent<RawImage>().color = highlightColor;

                MSL_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                MSL_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;

                NB_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                NB_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;
            }
            if (selectedWeapon == 1)
            {
                MG_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                MG_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;

                MSL_Icon.GetChild(0).GetComponent<Image>().enabled = true;
                MSL_Icon.gameObject.GetComponent<RawImage>().color = highlightColor;

                NB_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                NB_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;
            }

            if (selectedWeapon == 2)
            {
                MG_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                MG_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;

                MSL_Icon.GetChild(0).GetComponent<Image>().enabled = false;
                MSL_Icon.gameObject.GetComponent<RawImage>().color = nonHighlightColor;

                NB_Icon.GetChild(0).GetComponent<Image>().enabled = true;
                NB_Icon.gameObject.GetComponent<RawImage>().color = highlightColor;
            }
        }

        public void ResetAnimation(int index)
        {
            if (index == 1)
            {
                MG_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                MG_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1;
            }
            if (index == 2)
            {
                MSL_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                MSL_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1;
            }
            if (index == 3)
            {
                NB_Icon.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                NB_Icon.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1;
            }
            else 
            {
                return;
            }
        }
    }
}

