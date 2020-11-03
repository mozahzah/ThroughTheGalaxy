using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ThroughtTheGalaxy.UI
{
    //[ExecuteInEditMode]
    public class InGameCanvas : MonoBehaviour
    {
        [SerializeField] Transform MG_Text;
        [SerializeField] Transform MSL_Text;
        [SerializeField] Transform NB_Text;

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
                MG_Text.GetChild(0).GetComponent<Image>().enabled = true;
                MG_Text.gameObject.GetComponent<Text>().color = highlightColor;

                MSL_Text.GetChild(0).GetComponent<Image>().enabled = false;
                MSL_Text.gameObject.GetComponent<Text>().color = nonHighlightColor;

                NB_Text.GetChild(0).GetComponent<Image>().enabled = false;
                NB_Text.gameObject.GetComponent<Text>().color = nonHighlightColor;
            }
            if (selectedWeapon == 1)
            {
                MG_Text.GetChild(0).GetComponent<Image>().enabled = false;
                MG_Text.gameObject.GetComponent<Text>().color = nonHighlightColor;

                MSL_Text.GetChild(0).GetComponent<Image>().enabled = true;
                MSL_Text.gameObject.GetComponent<Text>().color = highlightColor;

                NB_Text.GetChild(0).GetComponent<Image>().enabled = false;
                NB_Text.gameObject.GetComponent<Text>().color = nonHighlightColor;
            }

            if (selectedWeapon == 2)
            {
                MG_Text.GetChild(0).GetComponent<Image>().enabled = false;
                MG_Text.gameObject.GetComponent<Text>().color = nonHighlightColor;

                MSL_Text.GetChild(0).GetComponent<Image>().enabled = false;
                MSL_Text.gameObject.GetComponent<Text>().color = nonHighlightColor;

                NB_Text.GetChild(0).GetComponent<Image>().enabled = true;
                NB_Text.gameObject.GetComponent<Text>().color = highlightColor;
            } 
        }
    }
}

