using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS6xEnrollmentKit_CS
{
    class FPAnsi
    {
        private AcceptedImage[] m_aiImage = null;

        public FPAnsi(AcceptedImage[] aiImage)
        {
            m_aiImage = aiImage;
        }

        public void SetFingerAmpCode(FingerType ftCurrent, byte nFingerIndex, byte nAmpCode)
        {
            if (m_aiImage == null)
                return;

            int nIndex = (int)ftCurrent;
            int i = m_aiImage[nIndex].nNumberAmp;
            byte ftSegmentFinger = 0;
            byte ftRollFinger = 0;

            if (ftCurrent == FingerType.FT_LEFT_4_FINGERS)
            {
                m_aiImage[nIndex].fAmp[i].FingerId = (byte)(10 - nFingerIndex);
                ftSegmentFinger = (byte)(nFingerIndex + FingerType.FT_LEFT_LITTLE);
            }
            else if (ftCurrent == FingerType.FT_2_THUMBS)
            {
                if (nFingerIndex == 0)
                {
                    m_aiImage[nIndex].fAmp[i].FingerId = 6;	// 6. Left Thumb
                    ftSegmentFinger = (byte)FingerType.FT_LEFT_THUMB;
                }
                else
                {
                    m_aiImage[nIndex].fAmp[i].FingerId = 1;	// 1. Right Thumb
                    ftSegmentFinger = (byte)FingerType.FT_RIGHT_THUMB;
                }
            }
            else if (ftCurrent == FingerType.FT_RIGHT_4_FINGERS)
            {
                m_aiImage[nIndex].fAmp[i].FingerId = (byte)(2 + nFingerIndex);	// 2. Right index finger
                ftSegmentFinger = (byte)(nFingerIndex + FingerType.FT_RIGHT_INDEX);
            }
            SetAnsiFingerPosition(nIndex);
            m_aiImage[nIndex].fAmp[i].AMPCode = nAmpCode;
            m_aiImage[nIndex].nNumberAmp++;
            // Set the associated segment/roll/plain finger AMP code
            if (ftCurrent <= FingerType.FT_RIGHT_4_FINGERS)
            {
                //flat fingers
                m_aiImage[ftSegmentFinger].nNumberAmp = 1;
                m_aiImage[ftSegmentFinger].fAmp[0].AMPCode = nAmpCode;
                m_aiImage[ftSegmentFinger].fAmp[0].FingerId = m_aiImage[nIndex].fAmp[i].FingerId;
                m_aiImage[ftSegmentFinger].pAcceptedImage = null;
                //rolled fingers
                ftRollFinger = (byte)(ftSegmentFinger + 10);
                SetAnsiFingerPosition(ftRollFinger);
                m_aiImage[ftRollFinger].nNumberAmp = 1;
                m_aiImage[ftRollFinger].fAmp[0].AMPCode = nAmpCode;
                m_aiImage[ftRollFinger].fAmp[0].FingerId = m_aiImage[nIndex].fAmp[i].FingerId;
                m_aiImage[ftRollFinger].pAcceptedImage = null;
                //thumbs
                if (ftSegmentFinger == (byte)FingerType.FT_LEFT_THUMB)
                {
                    SetAnsiFingerPosition((int)FingerType.FT_PLAIN_LEFT_THUMB);
                    m_aiImage[(byte)FingerType.FT_PLAIN_LEFT_THUMB].nNumberAmp = 1;
                    m_aiImage[(byte)FingerType.FT_PLAIN_LEFT_THUMB].fAmp[0].AMPCode = nAmpCode;
                    m_aiImage[(byte)FingerType.FT_PLAIN_LEFT_THUMB].fAmp[0].FingerId = m_aiImage[nIndex].fAmp[i].FingerId;
                    m_aiImage[(byte)FingerType.FT_PLAIN_LEFT_THUMB].pAcceptedImage = null;
                }
                else if (ftSegmentFinger == (byte)FingerType.FT_RIGHT_THUMB)
                {
                    SetAnsiFingerPosition((int)FingerType.FT_PLAIN_RIGHT_THUMB);
                    m_aiImage[(byte)FingerType.FT_PLAIN_RIGHT_THUMB].nNumberAmp = 1;
                    m_aiImage[(byte)FingerType.FT_PLAIN_RIGHT_THUMB].fAmp[0].AMPCode = nAmpCode;
                    m_aiImage[(byte)FingerType.FT_PLAIN_RIGHT_THUMB].fAmp[0].FingerId = m_aiImage[nIndex].fAmp[i].FingerId;
                    m_aiImage[(byte)FingerType.FT_PLAIN_RIGHT_THUMB].pAcceptedImage = null;
                }
            }
        }

        public void SetAnsiFingerPosition(int nIndex)
        {
            if (m_aiImage == null)
                return;

            switch (nIndex)
            {
                case 0:
                    m_aiImage[nIndex].nAnsiFingerPosition = 14;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_PLAIN;
                    break;
                case 1:
                    m_aiImage[nIndex].nAnsiFingerPosition = 15;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_PLAIN;
                    break;
                case 2:
                    m_aiImage[nIndex].nAnsiFingerPosition = 13;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_PLAIN;
                    break;
                case 13:
                    m_aiImage[nIndex].nAnsiFingerPosition = 10;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 14:
                    m_aiImage[nIndex].nAnsiFingerPosition = 9;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 15:
                    m_aiImage[nIndex].nAnsiFingerPosition = 8;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 16:
                    m_aiImage[nIndex].nAnsiFingerPosition = 7;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 17:
                    m_aiImage[nIndex].nAnsiFingerPosition = 6;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 18:
                    m_aiImage[nIndex].nAnsiFingerPosition = 1;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 19:
                    m_aiImage[nIndex].nAnsiFingerPosition = 2;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 20:
                    m_aiImage[nIndex].nAnsiFingerPosition = 3;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 21:
                    m_aiImage[nIndex].nAnsiFingerPosition = 4;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 22:
                    m_aiImage[nIndex].nAnsiFingerPosition = 5;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_ROLLED;
                    break;
                case 23:	//FT_PLAIN_LEFT_THUMB
                    m_aiImage[nIndex].nAnsiFingerPosition = 12;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_PLAIN;
                    break;
                case 24:	//FT_PLAIN_RIGHT_THUMB
                    m_aiImage[nIndex].nAnsiFingerPosition = 11;
                    m_aiImage[nIndex].it = AcceptedImage.IMPRESSION_TYPE_PLAIN;
                    break;
            }
        }

        public bool GetAmpCode(FingerType ftFinger, ref byte nAmpCode)
        {
            if (m_aiImage == null)
                return false;
            if (ftFinger < FingerType.FT_LEFT_LITTLE)
                return false;
            bool bRet = false;
            if (m_aiImage[(byte)ftFinger].nNumberAmp > 0)
            {
                if (m_aiImage[(byte)ftFinger].fAmp[0].AMPCode > 0)
                {
                    nAmpCode = m_aiImage[(byte)ftFinger].fAmp[0].AMPCode;
                    bRet = true;
                }
            }
            return bRet;
        }

        public void ResetAmpNumber(FingerType ftCurrent)
        {
            if (m_aiImage == null)
                return;
            m_aiImage[(int)ftCurrent].nNumberAmp = 0;

            FingerType i;
            if (ftCurrent == FingerType.FT_LEFT_4_FINGERS)
            {
                for (i = FingerType.FT_LEFT_LITTLE; i <= FingerType.FT_LEFT_INDEX; i++)
                    m_aiImage[(int)i].nNumberAmp = 0;
                for (i = FingerType.FT_ROLLED_LEFT_LITTLE; i <= FingerType.FT_ROLLED_LEFT_INDEX; i++)
                    m_aiImage[(int)i].nNumberAmp = 0;
            }
            else if (ftCurrent == FingerType.FT_2_THUMBS)
            {
                m_aiImage[(int)FingerType.FT_LEFT_THUMB].nNumberAmp = 0;
                m_aiImage[(int)FingerType.FT_RIGHT_THUMB].nNumberAmp = 0;
                m_aiImage[(int)FingerType.FT_ROLLED_LEFT_THUMB].nNumberAmp = 0;
                m_aiImage[(int)FingerType.FT_ROLLED_RIGHT_THUMB].nNumberAmp = 0;
                m_aiImage[(int)FingerType.FT_PLAIN_LEFT_THUMB].nNumberAmp = 0;
                m_aiImage[(int)FingerType.FT_PLAIN_RIGHT_THUMB].nNumberAmp = 0;
            }
            else if (ftCurrent == FingerType.FT_RIGHT_4_FINGERS)
            {
                for (i = FingerType.FT_RIGHT_INDEX; i <= FingerType.FT_RIGHT_LITTLE; i++)
                    m_aiImage[(int)i].nNumberAmp = 0;
                for (i = FingerType.FT_ROLLED_RIGHT_INDEX; i <= FingerType.FT_ROLLED_RIGHT_LITTLE; i++)
                    m_aiImage[(int)i].nNumberAmp = 0;
            }
        }

    }
}
