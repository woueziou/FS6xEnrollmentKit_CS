using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Futronic.MathAPIHelper;

namespace FS6xEnrollmentKit_CS
{
    class LedControl
    {
        /*************************************************************************
        * Turn on/off the Left 4 Leds
        * bOn : TRUE - On, FALSE - Off
        * bTimed : TRUE - Timed On, using Param2
        *		   FALSE - Timed off, using Param1
        * nRedGreed : Turn on red or green leds
        *			1 - Red
        *			2 - Green
        *			3 - Red + Green
        *************************************************************************/
        public static bool SetLeft4Leds(Device hDevice, bool bOn, bool bTimed, byte nRedGreen, bool bBuzzer)
        {
            if (hDevice == null)
                return false;
            uint uiP1 = 0;
            uint uiP2 = 0;
            if (bOn)
            {
                uint uiParam = 0;
                if (nRedGreen == 1)
                    uiParam = 0x55;
                else if (nRedGreen == 2)
                    uiParam = 0xAA;
                else if (nRedGreen == 3)
                    uiParam = 0xFF;
                if (bBuzzer)
                    uiParam |= 0x100000;
                if (bTimed)
                    uiP2 = uiParam;
                else
                    uiP1 = uiParam;
            }
            try
            {
                hDevice.ControlPin3(ref uiP1, uiP2, 0xA0);
                return true;
            }
            catch (FutronicException)
            {
                return false;
            }
        }

        public static bool SetRight4Leds(Device hDevice, bool bOn, bool bTimed, byte nRedGreen, bool bBuzzer)
        {
            uint uiP1 = 0;
            uint uiP2 = 0;
            if (bOn)
            {
                uint uiParam = 0;
                if (nRedGreen == 1)
                    uiParam = 0x55000;
                else if (nRedGreen == 2)
                    uiParam = 0xAA000;
                else if (nRedGreen == 3)
                    uiParam = 0xFF000;
                if (bBuzzer)
                    uiParam |= 0x100000;
                if (bTimed)
                    uiP2 = uiParam;
                else
                    uiP1 = uiParam;
            }
            try
            {
                hDevice.ControlPin3(ref uiP1, uiP2, 0xA0);
                return true;
            }
            catch (FutronicException)
            {
                return false;
            }

        }

        public static bool SetThumb2Leds(Device hDevice, bool bOn, bool bTimed, byte nRedGreen, bool bBuzzer)
        {
            uint uiP1 = 0;
            uint uiP2 = 0;
            if (bOn)
            {
                uint uiParam = 0;
                if (nRedGreen == 1)
                    uiParam = 0x500;
                else if (nRedGreen == 2)
                    uiParam = 0xA00;
                else if (nRedGreen == 3)
                    uiParam = 0xF00;
                if (bBuzzer)
                    uiParam |= 0x100000;
                if (bTimed)
                    uiP2 = uiParam;
                else
                    uiP1 = uiParam;
            }
            try
            {
                hDevice.ControlPin3(ref uiP1, uiP2, 0xA0);
                return true;
            }
            catch (FutronicException)
            {
                return false;
            }

        }

        /***************************************************************************************
                nLed: single LED index
                        0- Left Little   1- Left Ring   2-Left Middle   3-Left Index   4-Left Thumb
                        5- Right Little  6- Right Ring  7-Right Middle  8-Right Index  9-Right Thumb
        ***************************************************************************************/
        public static bool SetSingleLed(Device hDevice, bool bOn, bool bTimed, byte nLed, byte nRedGreen, bool bBuzzer)
        {
            uint uiP1 = 0;
            uint uiP2 = 0;
            if (bOn)
            {
                uint uiParam = 0;
                uiParam = nRedGreen;	// 1, 2, 3
                uiParam = uiParam << (nLed * 2);
                if (bBuzzer)
                    uiParam |= 0x100000;
                if (bTimed)
                    uiP2 = uiParam;
                else
                    uiP1 = uiParam;
            }
            try
            {
                hDevice.ControlPin3(ref uiP1, uiP2, 0xA0);
                return true;
            }
            catch (FutronicException)
            {
                return false;
            }
        }   
    }
}
