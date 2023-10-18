using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS6xEnrollmentKit_CS
{
    class FPMapping
    {
        public static String GetFileName(FingerType ftFinger)
        {
            String strFileName;
            switch (ftFinger)
            {
                case FingerType.FT_LEFT_4_FINGERS:
                    strFileName = "Left_Four_Fingers";
                    break;
                case FingerType.FT_2_THUMBS:
                    strFileName = "Two_Thumbs";
                    break;
                case FingerType.FT_RIGHT_4_FINGERS:
                    strFileName = "Right_Four_Fingers";
                    break;
                case FingerType.FT_LEFT_LITTLE:
                    strFileName = "Left_Little";
                    break;
                case FingerType.FT_LEFT_RING:
                    strFileName = "Left_Ring";
                    break;
                case FingerType.FT_LEFT_MIDDLE:
                    strFileName = "Left_Middle";
                    break;
                case FingerType.FT_LEFT_INDEX:
                    strFileName = "Left_Index";
                    break;
                case FingerType.FT_LEFT_THUMB:
                    strFileName = "Left_Thumb";
                    break;
                case FingerType.FT_RIGHT_THUMB:
                    strFileName = "Right_Thumb";
                    break;
                case FingerType.FT_RIGHT_INDEX:
                    strFileName = "Right_Index";
                    break;
                case FingerType.FT_RIGHT_MIDDLE:
                    strFileName = "Right_Middle";
                    break;
                case FingerType.FT_RIGHT_RING:
                    strFileName = "Right_Ring";
                    break;
                case FingerType.FT_RIGHT_LITTLE:
                    strFileName = "Right_Little";
                    break;
                case FingerType.FT_ROLLED_LEFT_LITTLE:
                    strFileName = "Rolled_Left_Little";
                    break;
                case FingerType.FT_ROLLED_LEFT_RING:
                    strFileName = "Rolled_Left_Ring";
                    break;
                case FingerType.FT_ROLLED_LEFT_MIDDLE:
                    strFileName = "Rolled_Left_Middle";
                    break;
                case FingerType.FT_ROLLED_LEFT_INDEX:
                    strFileName = "Rolled_Left_Index";
                    break;
                case FingerType.FT_ROLLED_LEFT_THUMB:
                    strFileName = "Rolled_Left_Thumb";
                    break;
                case FingerType.FT_ROLLED_RIGHT_THUMB:
                    strFileName = "Rolled_Right_Thumb";
                    break;
                case FingerType.FT_ROLLED_RIGHT_INDEX:
                    strFileName = "Rolled_Right_Index";
                    break;
                case FingerType.FT_ROLLED_RIGHT_MIDDLE:
                    strFileName = "Rolled_Right_Middle";
                    break;
                case FingerType.FT_ROLLED_RIGHT_RING:
                    strFileName = "Rolled_Right_Ring";
                    break;
                case FingerType.FT_ROLLED_RIGHT_LITTLE:
                    strFileName = "Rolled_Right_Little";
                    break;
                case FingerType.FT_PLAIN_LEFT_THUMB:
                    strFileName = "Plain_Left_Thumb";
                    break;
                case FingerType.FT_PLAIN_RIGHT_THUMB:
                    strFileName = "Plain_Right_Thumb";
                    break;
                default:
                    strFileName = "Unknown_Finger";
                    break;
            }
            return strFileName;
        }

        public static bool IsMatchedFingerInSlaps(FingerType ftIndex, FingerType ftMatchIndex)
        {
            if (ftIndex > FingerType.FT_RIGHT_4_FINGERS)
                return false;
            if (ftIndex == FingerType.FT_LEFT_4_FINGERS)
            {
                if ((ftMatchIndex >= FingerType.FT_LEFT_LITTLE && ftMatchIndex <= FingerType.FT_LEFT_INDEX) ||
                    (ftMatchIndex >= FingerType.FT_ROLLED_LEFT_LITTLE && ftMatchIndex <= FingerType.FT_ROLLED_LEFT_INDEX))
                    return true;
            }
            else if (ftIndex == FingerType.FT_2_THUMBS)
            {
                if (ftMatchIndex == FingerType.FT_LEFT_THUMB || ftMatchIndex == FingerType.FT_RIGHT_THUMB ||
                    ftMatchIndex == FingerType.FT_ROLLED_LEFT_THUMB || ftMatchIndex == FingerType.FT_ROLLED_RIGHT_THUMB ||
                    ftMatchIndex == FingerType.FT_PLAIN_LEFT_THUMB || ftMatchIndex == FingerType.FT_PLAIN_RIGHT_THUMB)
                    return true;
            }
            else if (ftIndex == FingerType.FT_RIGHT_4_FINGERS)
            {
                if ((ftMatchIndex >= FingerType.FT_RIGHT_INDEX && ftMatchIndex <= FingerType.FT_RIGHT_LITTLE) ||
                    (ftMatchIndex >= FingerType.FT_ROLLED_RIGHT_INDEX && ftMatchIndex <= FingerType.FT_ROLLED_RIGHT_LITTLE))
                    return true;
            }
            return false;
        }    

    }
}
