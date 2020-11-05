using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ThroughtTheGalaxy.UI
{
    //[ExecuteInEditMode]
    public class InGameCanvas : MonoBehaviour
    {
        [SerializeField] Transform MG_Icon;
        [SerializeField] Transform MSL_Icon;
        [SerializeField] Transform NB_Icon;

        public int selectedWeapon;
        Color highlightColor;
        Color nonHighlightColor;

        // Start is called before the first frame update
        void Start()
        {
            highlightColor = new Color32(255,255,255,100);
            nonHighlightColor = new Color32(255,255,255,5);
        }

        void Update()
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
    }
}

