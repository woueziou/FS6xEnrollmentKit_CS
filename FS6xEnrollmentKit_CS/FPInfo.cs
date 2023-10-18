using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

using Futronic.MathAPIHelper;
using System.Drawing;

namespace FS6xEnrollmentKit_CS
{
    public enum DeviceCompatibility : byte
    {
        FTR_DEVICE_UNKNOWN_TYPE = 0,
        FTR_DEVICE_USB_2_0_TYPE_1 = 1,
        FTR_DEVICE_USB_2_0_TYPE_2 = 4,
        FTR_DEVICE_USB_2_0_TYPE_3 = 5,
        FTR_DEVICE_USB_2_0_TYPE_50 = 7,
        FTR_DEVICE_USB_2_0_TYPE_60 = 8,
        FTR_DEVICE_USB_2_0_TYPE_25 = 9,
        FTR_DEVICE_USB_2_0_TYPE_10 = 10,
        FTR_DEVICE_USB_2_0_TYPE_80W = 11,
        FTR_DEVICE_USB_2_0_TYPE_80H = 13,
        FTR_DEVICE_USB_2_0_TYPE_88H = 14,
        FTR_DEVICE_USB_2_0_TYPE_64 = 15
    };

    public enum ImageFormat : byte
    {
        FORMAT_1600_1500 = 0,
        FORMAT_800_750 = 1,
        FORMAT_UNKNOWN = 2
    };

    public enum FingerType : byte
    {
        FT_LEFT_4_FINGERS = 0,
        FT_2_THUMBS,
        FT_RIGHT_4_FINGERS,
        FT_LEFT_LITTLE,
        FT_LEFT_RING,
        FT_LEFT_MIDDLE,
        FT_LEFT_INDEX,
        FT_LEFT_THUMB,
        FT_RIGHT_THUMB,
        FT_RIGHT_INDEX,
        FT_RIGHT_MIDDLE,
        FT_RIGHT_RING,
        FT_RIGHT_LITTLE,
        FT_ROLLED_LEFT_LITTLE,
        FT_ROLLED_LEFT_RING,
        FT_ROLLED_LEFT_MIDDLE,
        FT_ROLLED_LEFT_INDEX,
        FT_ROLLED_LEFT_THUMB,
        FT_ROLLED_RIGHT_THUMB,
        FT_ROLLED_RIGHT_INDEX,
        FT_ROLLED_RIGHT_MIDDLE,
        FT_ROLLED_RIGHT_RING,
        FT_ROLLED_RIGHT_LITTLE,
        FT_PLAIN_LEFT_THUMB,			// for Type4 record, 500*600 image
        FT_PLAIN_RIGHT_THUMB
    };

    public struct FINGER_SEGMENT
    {
        public short Left;
        public short Right;
        public short Top;
        public short Bottom;
        public byte FingerId;
        public byte NFIQ;
    };

    public class FPInfo
    {
        public static byte DEVICE_SCAN_TYPE_SLAPS = 0x01;
        public static byte DEVICE_SCAN_TYPE_2THUMBS = 0x02;	//for FS50 
        public static byte DEVICE_SCAN_TYPE_FLAT_FINGER = 0x04;
        public static byte DEVICE_SCAN_TYPE_ROLLED_FINGER = 0x08;
        public static byte AUTO_CAPTURE_DEFAULT_LEVEL = 3;		//0-7
        public static int NUMBER_FINGER_TYPES = 25;
    
        public static byte ETFS_RECORD_TYPE_ANSI_NIST_ITL_1_2007_4 = 1;
        public static byte ETFS_RECORD_TYPE_ANSI_NIST_ITL_1_2007_14 = 2;
        public static byte ETFS_RECORD_TYPE_ANSI_381_2004 = 3;
        public static byte ETFS_RECORD_TYPE_ISO_IEC_19794_4 = 4;
    
        public static byte FIR_STD_ANSI = 1;
        public static byte FIR_STD_ISO	= 2;
    
        public static byte TYPE_ANSI_NIST_ITL_1_2007_4	= 1;
        public static byte TYPE_ANSI_NIST_ITL_1_2007_14 = 2;
        public static byte TYPE_ANSI_381_2004 = 3;
        public static byte TYPE_ISO_IEC_19794_4 = 4;

        const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

        [DllImport("kernel32.dll")]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
                                        uint dwMessageId, uint dwLanguageId,
                                        ref IntPtr lpBuffer,
                                        uint nSize, IntPtr Arguments);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LocalFree(IntPtr hMem);

        public static string GetScannerName(byte byCompatibility, ref short nDeviceID)
        {
            string strName;
            switch (byCompatibility)
            {
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_1:
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_2:
                    strName = "FS80";
                    nDeviceID = 0x80;
                    break;
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_80H:
                    strName = "FS80H";
                    nDeviceID = 0x80;
                    break;
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_3:
                    strName = "FS88";
                    nDeviceID = 0x88;
                    break;
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_88H:
                    strName = "FS88H";
                    nDeviceID = 0x88;
                    break;
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_25:
                    strName = "FS25";
                    nDeviceID = 0x25;
                    break;
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_50:
                    strName = "FS50";
                    nDeviceID = 0x50;
                    break;
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60:
                    strName = "FS60";
                    nDeviceID = 0x60;
                    break;
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64:
                    strName = "FS64";
                    nDeviceID = 0x64;
                    break;
                case (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_10:
                    strName = "FS10";
                    nDeviceID = 0x10;
                    break;
                default:
                    strName = "Unknown";
                    nDeviceID = 0;
                    break;
            }
            return strName;
        }

        public static String GetErrorMessage(ErrorCodes code, out bool criticalError )
        {
            String message;
            criticalError = false;
            switch( code )
            {
                case ErrorCodes.kEMPTY_FRAME:
                    message = "Empty frame";
                    break;

                case ErrorCodes.kMOVABLE_FINGER:
                    message = "Movable finger";
                    break;

                case ErrorCodes.kNO_FRAME:
                    message = "No frame";
                    break;

                case ErrorCodes.kUSER_CANCELED:
                    message = "User canceled";
                    break;

                case ErrorCodes.kHARDWARE_INCOMPATIBLE:
                    message = "Incompatible hardware";
                    break;

                case ErrorCodes.kFIRMWARE_INCOMPATIBLE:
                    message = "Incompatible firmware";
                    break;

                case ErrorCodes.kINVALID_AUTHORIZATION_CODE:
                    message = "Invalid authorization code";
                    break;

                case ErrorCodes.kNOT_SUPPORTED:
                    message = "This feature is not supported";
                    criticalError = true;
                    break;

                case ErrorCodes.kROLL_NOT_STARTED:
                    message = "The roll operation is not started";
                    criticalError = true;
                    break;
                
                case ErrorCodes.kROLL_PROGRESS_REMOVE_FINGER:
                    message = "Please remove your finger";
                    break;

                case ErrorCodes.kROLL_PROGRESS_PUT_FINGER:
                    message = "Please put your finger to roll";
                    break;

                case ErrorCodes.kROLL_PROGRESS_POST_PROCESSING:
                    message = "Post processing...";
                    break;

                case ErrorCodes.kROLL_PROGRESS_DATA:
                    message = "Processing...";
                    break;

                case ErrorCodes.kROLL_TIMEOUT:
                    message = "Operation timeout...";
                    break;

                case ErrorCodes.kROLL_ABORTED:
                    message = "Operation canceled by user";
                    criticalError = true;
                    break;

                case ErrorCodes.kROLL_ALREADY_STARTED:
                    message = "Operation is already started";
                    criticalError = true;
                    break;

                case ErrorCodes.kNO_MORE_ITEMS:
                    message = "No more data is available.";
                    criticalError = true;
                    break;

                case ErrorCodes.kNOT_ENOUGH_MEMORY:
                    message = "Not enough storage is available to process this command.";
                    criticalError = true;
                    break;

                case ErrorCodes.kNO_SYSTEM_RESOURCES:
                    message = "Insufficient system resources exist to complete the requested service.";
                    criticalError = true;
                    break;

                case ErrorCodes.kTIMEOUT:
                    message = "This operation returned because the timeout period expired.";
                    criticalError = true;
                    break;

                case ErrorCodes.kNOT_READY:
                    message = "The device is not ready.";
                    criticalError = true;
                    break;

                case ErrorCodes.kBAD_CONFIGURATION:
                    message = "The configuration data for this product is corrupt. Contact your support personnel.";
                    criticalError = true;
                    break;

                case ErrorCodes.kINVALID_PARAMETER:
                    message = "The parameter is incorrect.";
                    criticalError = true;
                    break;

                case ErrorCodes.kCALL_NOT_IMPLEMENTED:
                    message = "This function is not supported on this system.";
                    criticalError = true;
                    break;

                case ErrorCodes.kWRITE_PROTECT:
                    message = "The device is write protected.";
                    criticalError = true;
                    break;

                case ErrorCodes.kMESSAGE_EXCEEDS_MAX_SIZE:
                    message = "The message provided exceeds the maximum size allowed for this parameter.";
                    criticalError = true;
                    break;

                default:
                    IntPtr lpMsgBuf = IntPtr.Zero;
                    uint chars = FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
                                   IntPtr.Zero,
                                   (uint)code,
                                   0, // Default language
                                   ref lpMsgBuf,
                                   0, IntPtr.Zero);
                    if (chars == 0)
                    {
                        message = String.Format("Error code: {0}", code);
                    }
                    else
                    {
                        message = Marshal.PtrToStringAnsi(lpMsgBuf);
                        lpMsgBuf = LocalFree(lpMsgBuf);
                    }
                    criticalError = true;
                    break;
            }
            return message;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBitmap(IntPtr hdc, [In] ref BITMAPINFOHEADER lpbmih,
                                            uint fdwInit, byte[] lpbInit, byte[] lpbmi,
                                            uint fuUsage);

        /* constants for CreateDIBitmap */
        const int CBM_INIT = 0x04;   /* initialize bitmap */

        /* DIB color table identifiers */

        const int DIB_RGB_COLORS = 0; /* color table in RGBs */
        const int DIB_PAL_COLORS = 1; /* color table in palette indices */

        const int BI_RGB = 0;
        const int BI_RLE8 = 1;
        const int BI_RLE4 = 2;
        const int BI_BITFIELDS = 3;
        const int BI_JPEG = 4;
        const int BI_PNG = 5;

        public static Bitmap CreateBitmap(IntPtr hDC, Size bmpSize, byte[] data)
        {
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem);
            //BITMAPINFO bmpInfo = new BITMAPINFO();
            BITMAPINFOHEADER bmiHeader = new BITMAPINFOHEADER();
            bmiHeader.biSize = 40;
            bmiHeader.biWidth = bmpSize.Width;
            bmiHeader.biHeight = bmpSize.Height;
            bmiHeader.biPlanes = 1;
            bmiHeader.biBitCount = 8;
            bmiHeader.biCompression = BI_RGB;
            bw.Write(bmiHeader.biSize);
            bw.Write(bmiHeader.biWidth);
            bw.Write(bmiHeader.biHeight);
            bw.Write(bmiHeader.biPlanes);
            bw.Write(bmiHeader.biBitCount);
            bw.Write(bmiHeader.biCompression);
            bw.Write(bmiHeader.biSizeImage);
            bw.Write(bmiHeader.biXPelsPerMeter);
            bw.Write(bmiHeader.biYPelsPerMeter);
            bw.Write(bmiHeader.biClrUsed);
            bw.Write(bmiHeader.biClrImportant);

            for (int i = 0; i < 256; i++)
            {
                bw.Write((byte)i);
                bw.Write((byte)i);
                bw.Write((byte)i);
                bw.Write((byte)0);
            }

            IntPtr hBitmap;
            if (data != null)
            {
                hBitmap = CreateDIBitmap(hDC, ref bmiHeader, CBM_INIT, data, mem.ToArray(), DIB_RGB_COLORS);
            }
            else
            {
                hBitmap = CreateDIBitmap(hDC, ref bmiHeader, 0, null, mem.ToArray(), DIB_RGB_COLORS);
            }
            return Bitmap.FromHbitmap(hBitmap);
        }

    }
}
