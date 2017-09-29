using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace projectQ
{
    public class VoiceSetting : MonoBehaviour
    {

        public UISprite sp1;
        public UISprite sp2;
        public UISprite sp3;
        public UISprite sp4;
        public UISprite sp5;
        private float showPlayTimer;
        public void SetSprite(bool isRecording)
        {
            if (isRecording)
            {
                showPlayTimer += Time.deltaTime;
                if (showPlayTimer > 0 && showPlayTimer <= 0.2)
                {
                    sp1.gameObject.SetActive(true);
                    sp2.gameObject.SetActive(false);
                    sp3.gameObject.SetActive(false);
                    sp4.gameObject.SetActive(false);
                    sp5.gameObject.SetActive(false);
                }
                else if (showPlayTimer > 0.2 && showPlayTimer <= 0.4)
                {
                    sp1.gameObject.SetActive(true);
                    sp2.gameObject.SetActive(true);
                    sp3.gameObject.SetActive(false);
                    sp4.gameObject.SetActive(false);
                    sp5.gameObject.SetActive(false);
                }
                else if (showPlayTimer > 0.4 && showPlayTimer <= 0.6)
                {
                    sp1.gameObject.SetActive(true);
                    sp2.gameObject.SetActive(true);
                    sp3.gameObject.SetActive(true);
                    sp4.gameObject.SetActive(false);
                    sp5.gameObject.SetActive(false);
                }
                else if (showPlayTimer > 0.6 && showPlayTimer <= 0.8)
                {
                    sp1.gameObject.SetActive(true);
                    sp2.gameObject.SetActive(true);
                    sp3.gameObject.SetActive(true);
                    sp4.gameObject.SetActive(true);
                    sp5.gameObject.SetActive(false);
                }
                else if(showPlayTimer > 0.8 && showPlayTimer <= 1)
                {
                    sp1.gameObject.SetActive(true);
                    sp2.gameObject.SetActive(true);
                    sp3.gameObject.SetActive(true);
                    sp4.gameObject.SetActive(true);
                    sp5.gameObject.SetActive(true);
                }
                else if(showPlayTimer > 1 && showPlayTimer < 1.1)
                {
                    sp1.gameObject.SetActive(false);
                    sp2.gameObject.SetActive(false);
                    sp3.gameObject.SetActive(false);
                    sp4.gameObject.SetActive(false);
                    sp5.gameObject.SetActive(false);
                    showPlayTimer = 0f;
                    
                }
            }
            else
            {
                sp1.gameObject.SetActive(false);
                sp2.gameObject.SetActive(false);
                sp3.gameObject.SetActive(false);
                sp4.gameObject.SetActive(false);
                sp5.gameObject.SetActive(false);
                showPlayTimer = 0f;
                return;
            }
            
        }
    }
}
