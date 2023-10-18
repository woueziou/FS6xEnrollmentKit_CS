using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Futronic.MathAPIHelper;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace FS6xEnrollmentKit_CS
{
    public class FPDevice
    { 
        public Device m_hDevice;
	    private Size m_ImageSize;
	    private byte[] m_pBuffer;
        private byte[] m_pBufferResult;
        private byte[] m_pBufferSubf;
	    private ImageFormat m_ImageFormat;
	    private AcceptedImage[] m_aiImage;
	    private FingerType m_ftCurrent;
	    private SubfPointCoord[] m_Subf;
        private SegmParameters m_ParamSeg;
	    private bool m_bSegmentation;
	    private bool m_bAngle;
	    private bool m_bSound;
	    private bool m_bAutoCapture;
	    private byte m_nDeviceCompatibility = 0;
	    private int m_nNfiq;
	    private bool m_bUnavailable;
	    private int m_nUnavailableFingers;
        private bool m_bPreview = false;
	    private FINGER_SEGMENT[] m_FingerSegments;
	    private string m_strErrMsg;
	    private string m_strDiagnostic;
	    private int m_nDiagnosticCode;
	    private short m_nDeviceID;
        private byte m_nACTL;
        private FPVerify m_verifyFP;
        private FPAnsi m_ansiFP;
        private Thread m_WorkerThread = null;
	    public static bool m_bStop;
	    public static bool m_bIsScanning;
	    public static bool m_bIsRoll;
        public static bool m_bIsDeviceOpened = false;
        public bool m_bIsImageSaved = false;

        public delegate void OnActiveHandler(int nAction);
        public event OnActiveHandler OnAction;

        private ShowFPImage m_ShowImage = new ShowFPImage();
        private ShowFPImage m_ShowEnlargedImage = new ShowFPImage();
        private ShowFPImage[] m_ShowAcceptedImage;

        public FPDevice()
        {
            int i;
            m_bIsDeviceOpened = false;
            m_Subf = new SubfPointCoord[4];
            m_ParamSeg = new SegmParameters();
            m_FingerSegments = new FINGER_SEGMENT[10];
            m_ShowAcceptedImage = new ShowFPImage[FPInfo.NUMBER_FINGER_TYPES];
            m_verifyFP = new FPVerify();
            m_pBuffer = null;
            m_pBufferSubf = null;
            m_bAutoCapture = false;
            m_bIsImageSaved = false;
            m_ImageFormat = ImageFormat.FORMAT_UNKNOWN;
            m_aiImage = new AcceptedImage[FPInfo.NUMBER_FINGER_TYPES];
            for (i = 0; i < FPInfo.NUMBER_FINGER_TYPES; i++)
                m_aiImage[i] = new AcceptedImage();
            for (i = 0; i < 4; i++)
                m_Subf[i] = new SubfPointCoord();
            for (i = 0; i < 10; i++)
                m_FingerSegments[i] = new FINGER_SEGMENT();
            for (i = 0; i < FPInfo.NUMBER_FINGER_TYPES; i++)
                m_ShowAcceptedImage[i] = new ShowFPImage();
            m_bSegmentation = m_bAngle = m_bSound = m_bUnavailable = false;
            m_nUnavailableFingers = 0;
            m_nDeviceCompatibility = 127;	//UnKnown device
            m_strErrMsg = null;
            m_nDeviceID = 0;
            m_strDiagnostic = null;
            m_nDiagnosticCode = 0;
            m_nACTL = FPInfo.AUTO_CAPTURE_DEFAULT_LEVEL;
            m_hDevice = new Device();
            m_ansiFP = new FPAnsi(m_aiImage);
        }

        ~FPDevice()
        {
            if (m_hDevice != null)
                m_hDevice.Dispose();
        }

        public void SetShowImageHandler(PictureBox showArea)
        {
            m_ShowImage.SetPictureBox(showArea);
        }

        public void SetErrorMessage(string strErrMsg)
        {
            m_strErrMsg = strErrMsg;
        }

        public string GetErrorMessage()
        {
            return m_strErrMsg;
        }

        public void SetACTL(byte nLevel)
        {
            m_nACTL = nLevel;
        }

        public byte GetACTL()
        {
            return m_nACTL;
        }

        public void SetImageFormat(ImageFormat nImageFormat)
        {
            m_ImageFormat = nImageFormat;
        }
 
        public bool IsScanning()
        {
            return m_bIsScanning;
        }
   
        public bool Open()       
        {
            if (m_bIsDeviceOpened)
                return true;
            if( m_hDevice == null )
                return false;
            try
            {
                m_hDevice.Open();
                m_nDeviceCompatibility = m_hDevice.Information.DeviceCompatibility;
                FPInfo.GetScannerName(m_nDeviceCompatibility, ref m_nDeviceID);
            }
            catch (FutronicException ex)
            {
                bool bCriticalError = false;
                m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                m_bIsDeviceOpened = false;
                m_nDeviceCompatibility = 0;
                return false;
            }
            m_bIsDeviceOpened = true;
            return true;
        }

        public void Close()
        {
            if( m_bIsDeviceOpened && m_hDevice != null)
                m_hDevice.Close();
            m_bIsDeviceOpened = false;
            m_nDeviceCompatibility = 0;
        }

        public void Stop()
        {
            m_bStop = true;
            if( m_bIsScanning )
            {
                try
                {
                    if( m_bIsRoll )
                        m_hDevice.AbortRoll(false); // Abort roll operation asynchronously
                    m_WorkerThread.Join();
                }
                catch (FutronicException)
                {
                }
            }
        }

        public void SetCurrentFinger(FingerType ftFinger)
        {
            m_ftCurrent = ftFinger;
        }

        public void SetPreviewMode(bool bPreview)
        {
            m_bPreview = bPreview;
        }
    
        public void SetSegmentation(bool bSegmentation)
        {
            m_bSegmentation = bSegmentation;
        }

        public void SetAngle(bool bAngle)
        {
            m_bAngle = bAngle;
        }

        public void SetSound(bool bSound)
        {
            m_bSound = bSound;
        }

        public void SetAutoCapture(bool bAuto)
        {
            m_bAutoCapture = bAuto;
        }

        public bool CanSegmentation()
        {
            if (!m_bIsDeviceOpened)
                if (!Open())
                    return false;
            return (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 ||
                        m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64 ||
                        m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_50);
        }

        public bool CanRoll()
        {
            return CanSegmentation();
        }

        public bool CanSlaps()
        {
            if (!m_bIsDeviceOpened)
                if (!Open())
                    return false;
            return (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 ||
                        m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64 );
        }

        public void FreeBuffer()
        {
            m_pBuffer = null;
            m_pBufferSubf = null;
            m_pBufferResult = null;
	        for( int i=0; i<FPInfo.NUMBER_FINGER_TYPES; i++ )
	        {
                m_aiImage[i].pAcceptedImage = null;		
                m_aiImage[i].nImageHeight = m_aiImage[i].nImageWidth = 0;
                m_aiImage[i].nNFIQ = m_aiImage[i].nAnsiFingerPosition = 0;
                m_aiImage[i].nNumberAmp = m_aiImage[i].nNumberSegments = 0;
	        }
            m_bIsImageSaved = false;
	        m_verifyFP.Init();
        }

        public string GetDiagnostic(ref int nDiagnosticCode)
        {
            nDiagnosticCode = m_nDiagnosticCode;
            return m_strDiagnostic;
        }

        public bool IsUnavailable(ref int nUnavailableFingers)
        {
            nUnavailableFingers = m_nUnavailableFingers;
            return m_bUnavailable;
        }

        public void SetFingerAmpCode(FingerType ftCurrent, byte nFingerIndex, byte nAmpCode)
        {
            m_ansiFP.SetFingerAmpCode(ftCurrent, nFingerIndex, nAmpCode);
        }

        public bool ExportToEFTS(string strFileName, byte nRecordType)
        {
            if (nRecordType != FPInfo.ETFS_RECORD_TYPE_ANSI_NIST_ITL_1_2007_4 && nRecordType != FPInfo.ETFS_RECORD_TYPE_ANSI_NIST_ITL_1_2007_14
                && nRecordType != FPInfo.ETFS_RECORD_TYPE_ANSI_381_2004 && nRecordType != FPInfo.ETFS_RECORD_TYPE_ISO_IEC_19794_4)
            {
                MessageBox.Show("Invalid record type!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //open device
            if (!Open())
                return false;
            bool bRet = false;
            if (nRecordType == FPInfo.ETFS_RECORD_TYPE_ANSI_NIST_ITL_1_2007_4)
                bRet = ExportToAnsi2007(strFileName, (byte)4);
            else if (nRecordType == FPInfo.ETFS_RECORD_TYPE_ANSI_NIST_ITL_1_2007_14)
                bRet = ExportToAnsi2007(strFileName, (byte)14);
            else
                bRet = ExportToOtherStd(strFileName, nRecordType);
            Close();
            return bRet;
        }

        public bool ExportToAnsi2007(string strFileName, byte nRecordType)
        {
            AnsiITL2007 ansi = new AnsiITL2007();
            byte[][] pWsqImage = new byte[FPInfo.NUMBER_FINGER_TYPES][];
            int nWsqSize = 0;
            short nDeviceID = 0;
            String strDevice = FPInfo.GetScannerName(m_nDeviceCompatibility, ref nDeviceID);
            ansi.AddRecordType2(strDevice);
            ansi.m_nType = nRecordType;
            int nSegOffset = 0;
            int i;
            byte[] pTempWsq = null;
            for (i = 0; i < FPInfo.NUMBER_FINGER_TYPES; i++)
                pWsqImage[i] = null;
            // Slaps
            for (i = 0; i < 3; i++)
            {
                if (i == 0)
                    nSegOffset = 0;
                else if (i == 1)
                    nSegOffset = 4;
                else
                    nSegOffset = 6;
                if (i == 1 && nRecordType == 4)	// Record Type4 can not save FT_2_THUMBS
                    continue;
                if (m_aiImage[i].pAcceptedImage != null)
                {
                    pTempWsq = new byte[m_aiImage[i].nImageWidth * m_aiImage[i].nImageHeight];
                    if (!ConvertRAWToWSQ(m_aiImage[i].pAcceptedImage, m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight, pTempWsq, ref nWsqSize))
                        return false;
                    pWsqImage[i] = new byte[nWsqSize];
                    Array.Copy(pTempWsq, 0, pWsqImage[i], 0, nWsqSize);
                    if (nRecordType == 14)
                        ansi.AddRecordType14(pWsqImage[i], nWsqSize, m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight, m_aiImage[i].it,
                            "WSQ20", "Demo", "", m_aiImage[i].nAnsiFingerPosition, m_aiImage[i].nNumberSegments, m_FingerSegments, nSegOffset, m_aiImage[i].nNumberAmp, m_aiImage[i].fAmp, m_aiImage[i].nNFIQ);
                    else if (nRecordType == 4)
                        ansi.AddRecordType4(pWsqImage[i], nWsqSize, (short)m_aiImage[i].nImageWidth, (short)m_aiImage[i].nImageHeight, m_aiImage[i].it, (byte)m_aiImage[i].nAnsiFingerPosition);
                }
                else
                {
                    if (m_aiImage[i].nNumberAmp > 0 && nRecordType == 14)
                    {
                        ansi.AddRecordType14(null, 0, 0, 0, m_aiImage[i].it,
                            "NONE", "Demo", "", m_aiImage[i].nAnsiFingerPosition, 0, null, 0, m_aiImage[i].nNumberAmp, m_aiImage[i].fAmp, m_aiImage[i].nNFIQ);
                    }
                }
            }
            if (nRecordType == 4)	//save FT_PLAIN_LEFT_THUMB & FT_PLAIN_RIGHT_THUMB
            {
                for (i = 23; i < 25; i++)
                {
                    if (m_aiImage[i].pAcceptedImage != null)
                    {
                        pTempWsq = new byte[m_aiImage[i].nImageWidth * m_aiImage[i].nImageHeight];
                        if (!ConvertRAWToWSQ(m_aiImage[i].pAcceptedImage, m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight, pTempWsq, ref nWsqSize))
                            return false;
                        pWsqImage[i] = new byte[nWsqSize];
                        Array.Copy(pTempWsq, 0, pWsqImage[i], 0, nWsqSize);
                        ansi.AddRecordType4(pWsqImage[i], nWsqSize, (short)m_aiImage[i].nImageWidth, (short)m_aiImage[i].nImageHeight, m_aiImage[i].it, (byte)m_aiImage[i].nAnsiFingerPosition);
                    }
                }
            }
            // Rolled fingers
            for (i = 13; i < 23; i++)
            {
                if (m_aiImage[i].pAcceptedImage != null)	//FT_ROLLED_LEFT_LITTLE
                {
                    pTempWsq = new byte[m_aiImage[i].nImageWidth * m_aiImage[i].nImageHeight];
                    if (!ConvertRAWToWSQ(m_aiImage[i].pAcceptedImage, m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight, pTempWsq, ref nWsqSize))
                        return false;
                    pWsqImage[i] = new byte[nWsqSize];
                    Array.Copy(pTempWsq, 0, pWsqImage[i], 0, nWsqSize);
                    if (nRecordType == 14)
                        ansi.AddRecordType14(pWsqImage[i], nWsqSize, m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight, m_aiImage[i].it,
                            "WSQ20", "Demo", "", m_aiImage[i].nAnsiFingerPosition, 0, null, 0, 0, null, m_aiImage[i].nNFIQ);
                    else if (nRecordType == 4)
                        ansi.AddRecordType4(pWsqImage[i], nWsqSize, (short)m_aiImage[i].nImageWidth, (short)m_aiImage[i].nImageHeight, m_aiImage[i].it, (byte)m_aiImage[i].nAnsiFingerPosition);
                }
                else
                {
                    if (m_aiImage[i].nNumberAmp > 0 && nRecordType == 14)
                    {
                        m_aiImage[i].fAmp[0].FingerId = (byte)m_aiImage[i].nAnsiFingerPosition;
                        ansi.AddRecordType14(null, 0, 0, 0, m_aiImage[i].it,
                            "NONE", "Demo", "", m_aiImage[i].nAnsiFingerPosition, 0, null, 0, 1, m_aiImage[i].fAmp, m_aiImage[i].nNFIQ);
                    }
                }
            }
            bool bRet = ansi.SaveRecord(strFileName);
            for (i = 0; i < FPInfo.NUMBER_FINGER_TYPES; i++)
            {
                pWsqImage[i] = null;
            }
            return bRet;            
        }

        public bool ExportToOtherStd(string strFileName, byte nRecordType)
        {
            FPDataInterchange diFP = new FPDataInterchange();
            byte nStd;
            if (nRecordType == FPInfo.ETFS_RECORD_TYPE_ANSI_381_2004)
                nStd = FPInfo.FIR_STD_ANSI;
            else
                nStd = FPInfo.FIR_STD_ISO;
            if (!diFP.Initialize(m_hDevice, nStd, m_nDeviceID))
                return false;
            byte[][] pWsqImage = new byte[FPInfo.NUMBER_FINGER_TYPES][];
            int nWsqSize = 0;
            int i;
            bool bRet = true;
            byte nNumFinger = 0;
            byte[] pTempWsq = null;
            for (i = 0; i < FPInfo.NUMBER_FINGER_TYPES; i++)
                pWsqImage[i] = null;

            for (i = 0; i < (FPInfo.NUMBER_FINGER_TYPES - 2); i++)
            {
                if (i >= (int)FingerType.FT_LEFT_LITTLE && i <= (int)FingerType.FT_RIGHT_LITTLE)	//skip
                    continue;
                if (m_aiImage[i].pAcceptedImage != null)
                {
                    pTempWsq = new byte[m_aiImage[i].nImageWidth * m_aiImage[i].nImageHeight];
                    bRet = ConvertRAWToWSQ(m_aiImage[i].pAcceptedImage, m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight, pTempWsq, ref nWsqSize);
                    if (!bRet)
                        break;
                    pWsqImage[i] = new byte[nWsqSize];
                    Array.Copy(pTempWsq, 0, pWsqImage[i], 0, nWsqSize);
                    bRet = diFP.AddImage(pWsqImage[i], nWsqSize, m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight, (byte)m_aiImage[i].nAnsiFingerPosition, (byte)m_aiImage[i].nNFIQ, m_aiImage[i].it);
                    if (!bRet)
                        break;
                    nNumFinger++;
                }
            }
            if (bRet && (nNumFinger > 0))
            {
                bRet = diFP.SaveRecord(strFileName);
            }
            for (i = 0; i < FPInfo.NUMBER_FINGER_TYPES; i++)
            {
                pWsqImage[i] = null;
            }
            diFP.Terminate();
            return (bRet && (nNumFinger > 0));
        }

        public bool ConvertRAWToWSQ(byte[] pRaw, int nRawWidth, int nRawHeight, byte[] pWsq, ref int nWsqSize)
        {
            if (pRaw == null || pWsq == null || nRawWidth <= 0 || nRawHeight <= 0)
            {
                MessageBox.Show("No Image!", "Convert to WSQ ERROR", MessageBoxButtons.OK);
                return false;
            }
            byte[] pWSQImage = null;
            pWSQImage = new byte[nRawWidth * nRawHeight];
            bool bRet = m_hDevice.WsqFromRAWImage(pRaw, nRawWidth, nRawHeight, 2.25f, ref nWsqSize, pWSQImage);
            if (!bRet)	//error occurs
            {
                pWSQImage = null;
                MessageBox.Show("ftrWSQ_FromRAWImage return false!", "Write wsq file", MessageBoxButtons.OK);
                return false;
            }
            Array.Copy(pWSQImage, 0, pWsq, 0, nWsqSize);
            return true;
        }

        public bool SaveAcceptedImage(FingerType ftCurrent)
        {
            if (ftCurrent > FingerType.FT_PLAIN_RIGHT_THUMB)
            {
                SetErrorMessage("Invalid Finger Type!");
                return false;
            }
            int nIndex = (int)ftCurrent;
            //check PLAIN LEFT/RIGHT THUMB first
            if (nIndex == (int)FingerType.FT_PLAIN_LEFT_THUMB || nIndex == (int)FingerType.FT_PLAIN_RIGHT_THUMB)
            {
                if (m_Subf[0].err == 0)	// only one segmented image
                {
                    m_aiImage[nIndex].pAcceptedImage = new byte[m_Subf[0].ws * m_Subf[0].hs];
                    if (m_aiImage[nIndex].pAcceptedImage == null)
                    {
                        SetErrorMessage("Not enough memory!");
                        return false;
                    }
                    m_aiImage[nIndex].nImageWidth = m_Subf[0].ws;
                    m_aiImage[nIndex].nImageHeight = m_Subf[0].hs;
                    Array.Copy(m_pBufferSubf, 0, m_aiImage[nIndex].pAcceptedImage, 0, m_Subf[0].ws * m_Subf[0].hs);
                    m_aiImage[nIndex].nFingerType = (byte)nIndex;
                    m_aiImage[nIndex].nNFIQ = m_Subf[0].nfiq;
                    m_aiImage[nIndex].nNumberSegments = 0;
                    m_ansiFP.SetAnsiFingerPosition(nIndex);
                    m_verifyFP.Enroll(m_hDevice, m_aiImage[nIndex].pAcceptedImage, m_aiImage[nIndex].nImageWidth, m_aiImage[nIndex].nImageHeight, nIndex - 16);
                    m_bIsImageSaved = true;
                    return true;
                }
                SetErrorMessage("Invalid image");
                return false;
            }
            m_aiImage[nIndex].pAcceptedImage = new byte[m_ImageSize.Width * m_ImageSize.Height];
            if (m_aiImage[nIndex].pAcceptedImage == null)
                return false;
            m_aiImage[nIndex].nImageWidth = m_ImageSize.Width;
            m_aiImage[nIndex].nImageHeight = m_ImageSize.Height;
            Array.Copy(m_pBuffer, 0, m_aiImage[nIndex].pAcceptedImage, 0, m_ImageSize.Width * m_ImageSize.Height);
            m_aiImage[nIndex].nFingerType = (byte)ftCurrent;
            m_aiImage[nIndex].nNumberSegments = 0;
            m_aiImage[nIndex].nNFIQ = m_nNfiq;

            m_ansiFP.SetAnsiFingerPosition(nIndex);

            if (nIndex < 3 && m_bSegmentation)	//check segmentation images
            {
                int nSubIndex = 0;
                int nOffset = 0;
                for (int i = 0; i < m_ParamSeg.nParamFing; i++)
                {
                    if (m_Subf[i].err == 0)
                    {
                        if (ftCurrent == FingerType.FT_LEFT_4_FINGERS)
                        {
                            nOffset = 0;
                            if (m_aiImage[nIndex].nNumberAmp > 0)
                            {
                                bool bSet = false;
                                for (int k = 10; k > 6; k--)	//10.9.8.7
                                {
                                    bSet = false;
                                    for (int j = 0; j < m_aiImage[nIndex].nNumberAmp; j++)	//check the AmpCode first
                                    {
                                        if (m_aiImage[nIndex].fAmp[j].FingerId == (byte)k)
                                        {
                                            bSet = true;
                                            break;
                                        }
                                    }
                                    if (!bSet)
                                    {
                                        for (int j = 0; j < i; j++)
                                        {
                                            if (m_FingerSegments[j].FingerId == (byte)k)
                                            {
                                                bSet = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!bSet)
                                    {
                                        m_FingerSegments[i + nOffset].FingerId = (byte)k;
                                        break;
                                    }
                                }
                            }
                            else
                                m_FingerSegments[i + nOffset].FingerId = (byte)(10 - i);	//10. Left little finger
                        }
                        else if (ftCurrent == FingerType.FT_2_THUMBS)
                        {
                            nOffset = 4;
                            if (m_aiImage[nIndex].nNumberAmp > 1)
                                break;
                            else if (m_aiImage[nIndex].nNumberAmp == 1)
                            {
                                if (m_aiImage[nIndex].fAmp[0].FingerId == 6)
                                    m_FingerSegments[i + nOffset].FingerId = 1;	// 1. Right Thumb
                                else
                                    m_FingerSegments[i + nOffset].FingerId = 6;	// 6. Left Thumb
                            }
                            else
                            {
                                if (i == 0)
                                    m_FingerSegments[i + nOffset].FingerId = 6;	// 6. Left Thumb
                                else
                                    m_FingerSegments[i + nOffset].FingerId = 1;	// 1. Right Thumb
                            }
                        }
                        else if (ftCurrent == FingerType.FT_RIGHT_4_FINGERS)
                        {
                            nOffset = 6;
                            if (m_aiImage[nIndex].nNumberAmp > 0)
                            {
                                bool bSet = false;
                                for (int k = 2; k < 6; k++)	//2.3.4.5
                                {
                                    bSet = false;
                                    for (int j = 0; j < m_aiImage[nIndex].nNumberAmp; j++)	//check the AmpCode first
                                    {
                                        if (m_aiImage[nIndex].fAmp[j].FingerId == (byte)k)
                                        {
                                            bSet = true;
                                            break;
                                        }
                                    }
                                    if (!bSet)
                                    {
                                        for (int j = 6; j < i + 6; j++)
                                        {
                                            if (m_FingerSegments[j].FingerId == (byte)k)
                                            {
                                                bSet = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!bSet)
                                    {
                                        m_FingerSegments[i + nOffset].FingerId = (byte)k;
                                        break;
                                    }
                                }
                            }
                            else
                                m_FingerSegments[i + nOffset].FingerId = (byte)(2 + i);	// 2. Right index finger
                        }
                        m_FingerSegments[i + nOffset].NFIQ = (byte)m_Subf[i].nfiq;
                        if (m_bAngle)
                        {
                            m_FingerSegments[i + nOffset].Top = m_FingerSegments[i + nOffset].Bottom
                                = m_FingerSegments[i + nOffset].Left = m_FingerSegments[i + nOffset].Right = 0;	//unknow bounding box, set to 0
                        }
                        else
                        {
                            m_FingerSegments[i + nOffset].Top = (short)(m_Subf[i].ys - m_Subf[i].hs / 2);
                            m_FingerSegments[i + nOffset].Bottom = (short)(m_Subf[i].ys + m_Subf[i].hs / 2);
                            m_FingerSegments[i + nOffset].Left = (short)(m_Subf[i].xs - m_Subf[i].ws / 2);
                            m_FingerSegments[i + nOffset].Right = (short)(m_Subf[i].xs + m_Subf[i].ws / 2);
                        }
                        switch (m_FingerSegments[i + nOffset].FingerId)
                        {
                            case 10:
                                nSubIndex = 3;
                                break;
                            case 9:
                                nSubIndex = 4;
                                break;
                            case 8:
                                nSubIndex = 5;
                                break;
                            case 7:
                                nSubIndex = 6;
                                break;
                            case 6:
                                nSubIndex = 7;
                                break;
                            case 1:
                                nSubIndex = 8;
                                break;
                            case 2:
                                nSubIndex = 9;
                                break;
                            case 3:
                                nSubIndex = 10;
                                break;
                            case 4:
                                nSubIndex = 11;
                                break;
                            case 5:
                                nSubIndex = 12;
                                break;
                        }
                        m_aiImage[nIndex].nNumberSegments++;
                        m_aiImage[nSubIndex].pAcceptedImage = new byte[m_Subf[i].ws * m_Subf[i].hs];
                        if (m_aiImage[nSubIndex].pAcceptedImage == null)
                            return false;
                        m_aiImage[nSubIndex].nImageWidth = m_Subf[i].ws;
                        m_aiImage[nSubIndex].nImageHeight = m_Subf[i].hs;
                        Array.Copy(m_pBufferSubf, i * m_Subf[i].ws * m_Subf[i].hs, m_aiImage[nSubIndex].pAcceptedImage, 0, m_Subf[i].ws * m_Subf[i].hs);
                        m_aiImage[nSubIndex].nFingerType = (byte)nSubIndex;
                        m_aiImage[nSubIndex].nNFIQ = m_Subf[i].nfiq;

                        m_verifyFP.Enroll(m_hDevice, m_aiImage[nSubIndex].pAcceptedImage, m_aiImage[nSubIndex].nImageWidth, m_aiImage[nSubIndex].nImageHeight, nSubIndex);

                        m_ansiFP.SetAnsiFingerPosition(nSubIndex);
                    }
                }
            }
            else
            {
                m_verifyFP.Enroll(m_hDevice, m_aiImage[nIndex].pAcceptedImage, m_aiImage[nIndex].nImageWidth, m_aiImage[nIndex].nImageHeight, nIndex);
            }
            m_bIsImageSaved = true;
            return true;
        }

        public void ShowAcceptedImage(FingerType ftCurrent, PictureBox pbShow, bool bEnlarged = false)
        {
            byte nIndex = (byte)ftCurrent;
            int nWidth = pbShow.Width;
            int nHeight = pbShow.Height;
            ShowFPImage showImage = null;
            if (bEnlarged)
                showImage = m_ShowEnlargedImage;
            else
                showImage = m_ShowAcceptedImage[(byte)ftCurrent];
            showImage.SetPictureBox(pbShow);
            showImage.SetText(null, Brushes.Green);
            if (m_aiImage[nIndex].pAcceptedImage == null)
            {
                showImage.SetImage(null, nWidth, nHeight);
                if (ftCurrent >= FingerType.FT_LEFT_LITTLE)
                {
                    byte nAmpCode = 0;
                    if (m_ansiFP.GetAmpCode(ftCurrent, ref nAmpCode))
                    {
                        String strAmpCode = null;
                        if (nAmpCode == 1)
                            strAmpCode = "Amputated";
                        else if (nAmpCode == 2)
                            strAmpCode = "Bandaged";
                        showImage.SetText(strAmpCode, Brushes.Red);
                    }
                }
            }
            else
            {
                showImage.SetImage(m_aiImage[nIndex].pAcceptedImage, m_aiImage[nIndex].nImageWidth, m_aiImage[nIndex].nImageHeight);
            }
            if (!bEnlarged)
                showImage = null;
        }

        public bool SaveAcceptedImageToFile(string strFullQualifiedFolderName, string strFolderName, bool bBMP, bool bWSQ)
        {
	        String strFileName;
	        String strWsqFileName;
            if( bWSQ )
            {
                if( !Open() )
                    return false;
            }

	        for(int i=0; i< FPInfo.NUMBER_FINGER_TYPES; i++ )
	        {
                if( m_aiImage[i].pAcceptedImage != null )
                {               
                    strFileName = strFolderName + "_" + FPMapping.GetFileName( (FingerType)i );               
                    strWsqFileName = strFileName;                
                    if( bBMP )                    
                    {
                        strFileName += ".bmp";
                        MyBitmapFile fileBMP = new MyBitmapFile(m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight,  m_aiImage[i].pAcceptedImage);
                        using (FileStream fileStream = new FileStream(strFullQualifiedFolderName+strFileName, FileMode.Create))
                        {
                            fileStream.Write(fileBMP.BitmatFileData, 0, fileBMP.BitmatFileData.Length );
                        }
                    }
                    if( bWSQ )
                    {
                        strWsqFileName += (".wsq");
                        int nWsqSize = 0;
                        byte[] pTempWsq = new byte[m_aiImage[i].nImageWidth * m_aiImage[i].nImageHeight];
                        if( !ConvertRAWToWSQ(m_aiImage[i].pAcceptedImage, m_aiImage[i].nImageWidth, m_aiImage[i].nImageHeight, pTempWsq, ref nWsqSize ) )
                            return false;
                        using (FileStream fileStream = new FileStream(strFullQualifiedFolderName+strWsqFileName, FileMode.Create))
                        {
                            fileStream.Write(pTempWsq, 0, nWsqSize);
                        }
                    }
                }
	        }
            if(bWSQ)
                Close();
	        return true;
        }

        /*******************************************************
        * Verify the captured image with enroll fingers
        *******************************************************/
        public bool VerifyImage(FingerType ftCurrent)
        {
	        if( ftCurrent > FingerType.FT_PLAIN_RIGHT_THUMB )
	        {
                SetErrorMessage("Invalid Finger Type!");
                return false;
	        }
            FingerType nIndex = ftCurrent;
	        int nScore = 0;
	        int nMatchIndex  = 0;
	        String strMsg; 
	        bool bWrong = false;
	        //check PLAIN LEFT/RIGHT THUMB first
            if (nIndex == FingerType.FT_PLAIN_LEFT_THUMB || nIndex == FingerType.FT_PLAIN_RIGHT_THUMB)
	        {
                if( m_Subf[0].err != 0 )	// only one segmented image
                {
                    SetErrorMessage("Invalid image");
                    return false;
                }
                if( m_verifyFP.Identify( m_hDevice, m_pBufferSubf, m_Subf[0].ws, m_Subf[0].hs, ref nMatchIndex, ref nScore ) )	//matched
                {
                    if (nIndex == FingerType.FT_PLAIN_LEFT_THUMB)
                    {
                        if ((nMatchIndex != (int)FingerType.FT_LEFT_THUMB) && (nMatchIndex != (int)FingerType.FT_ROLLED_LEFT_THUMB))
                        {
                            strMsg = "Wrong finger!\t Captured finger is " + FPMapping.GetFileName((FingerType)nMatchIndex);
                            SetErrorMessage(strMsg);
                            return false;
                        }
                    }
                    else //FT_PLAIN_RIGHT_THUMB
                    {
                        if ((nMatchIndex != (int)FingerType.FT_RIGHT_THUMB) && (nMatchIndex != (int)FingerType.FT_ROLLED_RIGHT_THUMB))	
                        {
                            strMsg = "Wrong finger!\t Captured finger is " + FPMapping.GetFileName((FingerType)nMatchIndex);
                            SetErrorMessage(strMsg);
                            return false;
                        }
                    }
                }
                else // not matched
                {
                    //check if reference finger exist
                    if( m_aiImage[(int)nIndex-16].pAcceptedImage != null || m_aiImage[(int)nIndex-6].pAcceptedImage != null )
                    {
                        strMsg = "Failed to check the finger sequence!\tMatch score is " + nScore;
                        SetErrorMessage(strMsg);
                        strMsg += "\r\n\r\nDo you want to continue to accept the image?\r\n\r\n";
                        DialogResult nResponse = MessageBox.Show( strMsg, "Attention", MessageBoxButtons.YesNo);
                        if(nResponse == DialogResult.No)
                            return false;
                    }
                }
    	    }
	        else if( nIndex >= FingerType.FT_ROLLED_LEFT_LITTLE )
	        {
                if( m_verifyFP.Identify( m_hDevice, m_pBuffer, m_ImageSize.Width, m_ImageSize.Height, ref nMatchIndex, ref nScore ) )	//matched
                {
                    if( nMatchIndex != ( (int)nIndex - 10 ) && nMatchIndex != (int)nIndex && ( nMatchIndex > (int)FingerType.FT_RIGHT_4_FINGERS ) )
                    {
                        strMsg = "Wrong finger!\t Captured finger is " + FPMapping.GetFileName((FingerType)nMatchIndex);
                        SetErrorMessage(strMsg);
                        return false;
                    }
                }	
                else
                {
                    //check if reference finger exist
                    if( m_aiImage[(int)nIndex-10].pAcceptedImage != null )
                    {
                        strMsg = "Failed to check the finger sequence!\tMatch score is " + nScore;
                        SetErrorMessage(strMsg);
                        strMsg += "\r\n\r\nDo you want to continue to accept the image?\r\n\r\n";
                        DialogResult nResponse = MessageBox.Show(strMsg, "Attention", MessageBoxButtons.YesNo);
                        if (nResponse == DialogResult.No)
                            return false;
                    }
                }
        	}
	        else if( nIndex >= FingerType.FT_LEFT_LITTLE )	//for flat finger
	        {
                if( m_verifyFP.Identify( m_hDevice, m_pBuffer,  m_ImageSize.Width, m_ImageSize.Height, ref nMatchIndex, ref nScore ) )	//matched
                {
                    if( nMatchIndex != (int)nIndex && nMatchIndex > (int)FingerType.FT_RIGHT_4_FINGERS )
                    {
                        strMsg = "Wrong finger!\t Captured finger is " + FPMapping.GetFileName((FingerType)nMatchIndex);
                        SetErrorMessage(strMsg);
                        return false;
                    }
                }
        	}
	        else	// if( nIndex >= FT_2_THUMBS )
	        {
                strMsg = "Wrong finger! Captured finger is\t";
                String strTemp;
                if( m_bSegmentation )
                {
                    byte[] pTemp = null;
                    for( int i=0; i < m_ParamSeg.nParamFing; i++ )
                    {
                        if( m_Subf[i].err == 0 )
                        {
                            pTemp = new byte[m_Subf[i].ws * m_Subf[i].hs];
                            Array.Copy(m_pBufferSubf,i * m_Subf[i].ws * m_Subf[i].hs , pTemp, 0, m_Subf[i].ws * m_Subf[i].hs);
                            if( m_verifyFP.Identify( m_hDevice, pTemp, m_Subf[i].ws, m_Subf[i].hs, ref nMatchIndex, ref nScore ) )	
                            {
                                if( !FPMapping.IsMatchedFingerInSlaps( nIndex, (FingerType)nMatchIndex ) )
                                {
                                    strTemp = String.Format("{0}. {1}. \t", i+1, FPMapping.GetFileName((FingerType)nMatchIndex));
                                    strMsg += strTemp;
                                    bWrong = true;
                                }
                            }
                            pTemp = null;
                        }
                    }
                }
                else
                {
                    if( m_verifyFP.Identify( m_hDevice, m_pBuffer,  m_ImageSize.Width, m_ImageSize.Height, ref nMatchIndex, ref nScore ) )	
                    {
                        if( nMatchIndex != (int)nIndex )
                        {
                            strTemp = FPMapping.GetFileName((FingerType)nMatchIndex) + "\t";
                            strMsg += strTemp;
                            bWrong = true;
                        }
                    }			
                }
                if( bWrong )
                {
                    SetErrorMessage(strMsg);
                    return false;
                }
	        }
    	    return true;
        }

        public void ResetAmpNumber(FingerType ftCurrent)
        {
            m_ansiFP.ResetAmpNumber(ftCurrent);
        }

        public bool IsFingerUnavailable(FingerType ftCurrent)
        {
            if (ftCurrent < FingerType.FT_LEFT_LITTLE)
                return false;
            byte nAmpCode = 0;
            return m_ansiFP.GetAmpCode(ftCurrent, ref nAmpCode);
        }
  
        public bool Scan()
        {
            if( m_ShowImage == null )
            {
                SetErrorMessage("Please SetShowImageHandler first!");
                return false;            
            }
	        if( m_bIsScanning )
	        {
                SetErrorMessage("Another scanning existed already!");
                return false;
	        }
            if (!Open())
                return false;
            m_bIsRoll = false;
            m_WorkerThread = new Thread(new ThreadStart(ScanThread));
            m_WorkerThread.Start();
            return true;
        }

     	public bool RollScan()
        {
            if( m_ShowImage == null )
            {
                SetErrorMessage("Please SetShowImageHandler first!");
                return false;            
            }
	        if( m_bIsScanning )
	        {
                SetErrorMessage("Another scanning existed already!");
                return false;
	        }
            if (!Open())
                return false;
            m_bIsRoll = true;
            m_WorkerThread = new Thread(new ThreadStart(ScanThread));
            m_WorkerThread.Start();
            return true;
        }

        private void ScanThread()
        {
            m_bIsScanning = true;
	        if( m_bIsRoll )
		        DoRoll();
	        else
                DoScan();
            m_bIsScanning = false;
        }
       
        private bool PrepareSegmentationPreview()
        {
            m_ShowImage.Reset();
            if( m_hDevice == null || !m_bIsDeviceOpened || m_ImageFormat == ImageFormat.FORMAT_UNKNOWN )
                return false;
            try
            {
                if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 ||
                     m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64 ||
                     m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_50)
                {
                    if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64)
                    {
                        m_hDevice.PreviewMode = false;               // non-preview mode
                        m_hDevice.ImageFormat = (int)m_ImageFormat;  // ImageFormat.FORMAT_1600_1500;
                    }
                    m_ShowImage.SetText("PLEASE REMOVE FINGER", Brushes.Orange);
                    m_ShowImage.SetImage(null, m_ImageSize.Width, m_ImageSize.Height);
                    if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64)
                    {
                         if (m_ftCurrent == FingerType.FT_LEFT_4_FINGERS)
                             LedControl.SetLeft4Leds(m_hDevice, true, true, (byte)1, m_bSound);
                         else if (m_ftCurrent == FingerType.FT_2_THUMBS)
                             LedControl.SetThumb2Leds(m_hDevice, true, true, (byte)1, m_bSound);
                         else if (m_ftCurrent == FingerType.FT_RIGHT_4_FINGERS)
                             LedControl.SetRight4Leds(m_hDevice, true, true, (byte)1, m_bSound);
                         else if (m_ftCurrent == FingerType.FT_PLAIN_LEFT_THUMB)
                             LedControl.SetSingleLed(m_hDevice, true, true, (byte)4, (byte)1, m_bSound);
                         else if (m_ftCurrent == FingerType.FT_PLAIN_RIGHT_THUMB)
                             LedControl.SetSingleLed(m_hDevice, true, true, (byte)5, (byte)1, m_bSound);
                         else
                             LedControl.SetSingleLed(m_hDevice, true, true, (byte)(m_ftCurrent - 3), (byte)1, m_bSound);
                    }
                    if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_50)
                    {
                        while( true )
                        {
                            try
                            {
                                m_hDevice.EliminateBackground = true;   //Calibrate background for normal mode
                                break;
                            }
                            catch (FutronicException ex)
                            {
                                if (ex.ErrorCode == (int)ErrorCodes.kMESSAGE_FINGER_IS_PRESENT)
                                {
                                    Thread.Sleep(50);
                                    continue;
                                }
                                else
                                    break;
                            }
                        }
                    }
                    else if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64)
                    {
                        Thread.Sleep(300); // a little delay for changing another fingers
                    }
                    if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64)
                    {
                        if (m_ImageFormat == ImageFormat.FORMAT_1600_1500)
                        {
                            m_ShowImage.SetPreviewMode(true);
                            m_hDevice.PreviewMode = true;   //set to Preview mode
                            if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60)
                                m_hDevice.EliminateBackground = true; 	//Calibrate background for preview mode
                            else	//FS64
                                Thread.Sleep(300); // a little delay for changing another fingers
                        }
                        else	// 800 * 750
                        {
                            Thread.Sleep(200); // a little delay for changing another fingers
                        }
                    }
                    if (m_ftCurrent == FingerType.FT_LEFT_4_FINGERS)
                        LedControl.SetLeft4Leds(m_hDevice, true, false, (byte)2, false);
                    else if (m_ftCurrent == FingerType.FT_2_THUMBS)
                        LedControl.SetThumb2Leds(m_hDevice, true, false, (byte)2, false);
                    else if (m_ftCurrent == FingerType.FT_RIGHT_4_FINGERS)
                        LedControl.SetRight4Leds(m_hDevice, true, false, (byte)2, false);
                    else if (m_ftCurrent == FingerType.FT_PLAIN_LEFT_THUMB)
                        LedControl.SetSingleLed(m_hDevice, true, false, (byte)4, (byte)2, false);
                    else if (m_ftCurrent == FingerType.FT_PLAIN_RIGHT_THUMB)
                        LedControl.SetSingleLed(m_hDevice, true, false, (byte)5, (byte)2, false);
                    else
                        LedControl.SetSingleLed(m_hDevice, true, false, (byte)(m_ftCurrent - 3), (byte)2, false);
                }
                m_hDevice.InvertImage = true;
                m_ImageSize = m_hDevice.ImageSize;
                if (m_pBuffer != null)
                    m_pBuffer = null;
                if (m_pBufferResult != null)
                    m_pBufferResult = null;
                if (m_pBufferSubf != null)
                    m_pBufferSubf = null;
                m_pBuffer = new byte[m_ImageSize.Width * m_ImageSize.Height];
                m_pBufferResult = new byte[m_ImageSize.Width * m_ImageSize.Height];
                m_pBufferSubf = new byte[m_ImageSize.Width * m_ImageSize.Height];
                for (int i = 0; i < m_ImageSize.Width * m_ImageSize.Height; i++)
                {
                    m_pBuffer[i] = 0xFF;
                }   
                m_bPreview = true;
                m_ShowImage.SetText(null, Brushes.Green);
                m_ShowImage.SetImage(null, m_ImageSize.Width, m_ImageSize.Height);
            }
            catch(FutronicException ex)
            {
                bool bCriticalError = false;
                m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                return false;
            }
            return true;
        }

        public bool PrepareSegmentationCapture()
        {
            m_ShowImage.SetPreviewMode(false);
            try
            {
                if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 ||
                     m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64 )
                {
                    m_hDevice.PreviewMode = false;
                    m_hDevice.ImageFormat = (int)m_ImageFormat;
                }
                m_ImageSize = m_hDevice.ImageSize;
                if (m_pBuffer != null)
                    m_pBuffer = null;
                if (m_pBufferResult != null)
                    m_pBufferResult = null;
                if (m_pBufferSubf != null)
                    m_pBufferSubf = null;
                m_pBuffer = new byte[m_ImageSize.Width * m_ImageSize.Height];
                m_pBufferResult = new byte[m_ImageSize.Width * m_ImageSize.Height];
                m_pBufferSubf = new byte[m_ParamSeg.nParamFing * (int)MathNums.maxsubfsize * (int)MathNums.maxsubfsize];
                m_bPreview = true;
            }
            catch (FutronicException ex)
            {
                bool bCriticalError = false;
                m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                return false;
            }
            return true;
        }

        private bool DoScan()
        {
            if( ( m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || 
                m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64 || 
                m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_50 ) && m_bSegmentation )
            {
                if(!DoSegmentScan())
                    return false;
            }
            else
            {
                if(!DoFlatScan() )
                    return false;
            }
            return true;
        }
    
        private bool DoFlatScan()
        {
            bool bCriticalError = false;
            bool bErrNfiq = false;
            int nNfiq = 0;
            m_bStop = false;
            int nErrCode = 0;

            m_bPreview = true;
            if( m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || 
                m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64  )
                m_hDevice.ImageFormat = (int)m_ImageFormat;
            m_hDevice.InvertImage = true;
            m_ImageSize = m_hDevice.ImageSize;
            if (m_pBuffer != null)
                m_pBuffer = null;
            m_pBuffer = new byte[m_ImageSize.Width * m_ImageSize.Height];
            for (int i = 0; i < m_ImageSize.Width * m_ImageSize.Height; i++)
                m_pBuffer[i] = 0xff;
            m_ShowImage.Reset();
            m_ShowImage.SetImage(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height);
            do{
                try
                {
                    m_pBuffer = m_hDevice.GetFrame();
                    m_hDevice.MathImageNfIQ(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height, ref nNfiq, ref bErrNfiq);
                    m_ShowImage.SetNFIQ(nNfiq);
                    m_ShowImage.SetImage(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height);
                    Thread.Sleep(20);
                }
                catch (FutronicException ex)
                {
                    m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                    if ((ErrorCodes)ex.ErrorCode == ErrorCodes.kEMPTY_FRAME || (ErrorCodes)ex.ErrorCode == ErrorCodes.kMOVABLE_FINGER)
                        continue;
                    else
                        break;
                }
            }while(m_bPreview && !m_bStop);

            if(!m_bPreview) //capture
            {
                if(nErrCode == 0)
                {
                    m_nDiagnosticCode = 0;
                    OnAction(0);
                }
                else
                {
                    OnAction(4);
                }
            }
            return true;
        }
    
        public bool DoSegmentScan()
        {
            PrepareSegmentationPreview();

            bool bRC = true;
            for( int i=0; i<4; i++ )
            {
                m_Subf[i] = new SubfPointCoord();
                m_Subf[i].err = 1;
            }
            // Set the param for segment
            if( (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || 
                m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64) && 
                m_ImageFormat == ImageFormat.FORMAT_1600_1500 )
                m_ParamSeg.nParamFing = 4;
            else
                m_ParamSeg.nParamFing = 2;
            if( m_bAngle )
                m_ParamSeg.nParamAngle = 1;
            else
                m_ParamSeg.nParamAngle = 0;
            m_ParamSeg.nParamNfiq = 2;  //QFUTR;	//show quality value for preview mode
            if( m_ftCurrent == FingerType.FT_PLAIN_LEFT_THUMB || m_ftCurrent == FingerType.FT_PLAIN_RIGHT_THUMB )
            {
                m_ParamSeg.nWidthSubf = 500;
                m_ParamSeg.nHeightSubf = 600;
                m_ParamSeg.nParamFing = 1;
            }
            else
            {
                m_ParamSeg.nWidthSubf = 320;    //XSIZE;
                m_ParamSeg.nHeightSubf = 480;   //YSIZE;
            }
            m_ParamSeg.nParamFixedSize = 1;     //FIXEDSIZE;
            int nAutoThreshold = 0;
            bool bError = false;
            bool bCriticalError = false;
            int nDosage = 0;
            bool bPrepareCapture = false;
            m_bUnavailable = false;
            m_bStop = false;
            m_ShowImage.SetSubfCoord(null, 0.0f);
            do{
                if( m_bPreview )
                {
                    try
                    {
                        if( (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || 
                            m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64 ) && 
                            m_ImageFormat == ImageFormat.FORMAT_1600_1500 )
                        {
                            bRC = m_hDevice.MathFrameSegmentPreviewAuto(ref nDosage, m_pBuffer, m_pBufferResult, null, ref m_ParamSeg, m_Subf, ref bError, ref nAutoThreshold);
                        }
                        else
                        {
                            m_pBuffer = m_hDevice.GetFrame();
                            bRC = m_hDevice.MathImageSegmentAuto(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height, m_pBufferResult,
                                    null, ref m_ParamSeg, m_Subf, ref bError, ref nAutoThreshold);
                        }
                        m_ShowImage.SetSubfCoord(m_Subf, m_ParamSeg.dAngle);
                        m_ShowImage.SetImage(m_pBufferResult, m_ImageSize.Width, m_ImageSize.Height);
                    }
                    catch (FutronicException ex)
                    {
                        m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                        if ((ErrorCodes)ex.ErrorCode == ErrorCodes.kEMPTY_FRAME || (ErrorCodes)ex.ErrorCode == ErrorCodes.kMOVABLE_FINGER)
                        {
                            m_ShowImage.SetImage(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height); 
                            continue;
                        }
                        else
                            return false;
                    }

                    if( m_bAutoCapture )
                    {
                        bool bDetectedFinger = false;
                        int nSegOK=0;
                        if( m_ftCurrent == FingerType.FT_PLAIN_LEFT_THUMB || m_ftCurrent == FingerType.FT_PLAIN_RIGHT_THUMB )
                        {
                            if( m_Subf[0].err == 0 && m_Subf[0].nfiq < 4 )
                                bDetectedFinger = true;
                        }
                        else
                        {
                            for( int j=0; j<m_ParamSeg.nParamFing; j++ )
                            {
                                if( m_Subf[j].err == 0 && m_Subf[j].qfutr < 4 )		//v2.1
                                    nSegOK++;
                            }
                            if( nSegOK == m_ParamSeg.nParamFing )
                                bDetectedFinger = true;
                        }
                        if( (bError && (nAutoThreshold >= m_nACTL)) || bDetectedFinger )
                        {
                            m_bPreview = false;
                        }
                    }      
                    Thread.Sleep(150);  // add some delay
                }
                else
                {
                    OnAction(2);
                    if(!bPrepareCapture)
                    {
                        m_ShowImage.SetSubfCoord(null, 0.0);
                        m_ShowImage.SetText("DO NOT REMOVE FINGER", Brushes.Green);
                        m_ShowImage.SetImage(null, m_ImageSize.Width, m_ImageSize.Height);
                        if( !PrepareSegmentationCapture() )
                            return false;
                        bPrepareCapture = true;
                    }
                    try
                    {
                        m_pBuffer = m_hDevice.GetFrame();
                        m_ParamSeg.nParamNfiq = 1;  //set to NFIQ in capture mode
                        bRC = m_hDevice.MathImageSegmentAuto(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height, m_pBufferResult,
                                    m_pBufferSubf, ref m_ParamSeg, m_Subf, ref bError, ref nAutoThreshold);
                        m_ShowImage.SetImage(m_pBufferResult, m_ImageSize.Width, m_ImageSize.Height, true);
                        m_ShowImage.SetSubfCoord(m_Subf, m_ParamSeg.dAngle);
                    }
                    catch (FutronicException ex)
                    {
                        m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                        return false;
                    }

	                m_strDiagnostic = "";
	                m_nDiagnosticCode = 0;
	                m_nUnavailableFingers = 0;
	                if( m_ftCurrent == FingerType.FT_PLAIN_LEFT_THUMB || m_ftCurrent == FingerType.FT_PLAIN_RIGHT_THUMB )
	                {
                        if( m_Subf[0].err != 0 )
                        {
                            m_strDiagnostic = "Please evaluate the image - Missing Finger.\r\n";
                            m_nDiagnosticCode = 2;
                        }
                        else
                        {
                            if( m_Subf[0].nfiq > 3 )
                            {
                                m_strDiagnostic = "Please evaluate the image - Image Quality (NFIQ) > 3.\r\n";
                                m_nDiagnosticCode = 1;
                            }
                        }
	                }
	                else
	                {
                        int i;
                        for( i=0; i<m_ParamSeg.nParamFing; i++ )
                        {
                            if( m_Subf[i].err != 0 && (m_ftCurrent < FingerType.FT_PLAIN_LEFT_THUMB) ) 
                            {
                                m_bUnavailable = true;
                                m_nUnavailableFingers ++;
                            }
                        }
                        if( m_bUnavailable )
                        {
                            m_strDiagnostic = "Please evaluate the image - Missing Finger.\r\n";
                            m_nDiagnosticCode = 2;
                        }
                        else
                        {
                            //v2.3 - ftrMathAPI.dll v1.0.0.71, add parameter to detect left/right hand
                            if ((m_ftCurrent == FingerType.FT_LEFT_4_FINGERS && m_ParamSeg.nHandType != 1)
                                || (m_ftCurrent == FingerType.FT_RIGHT_4_FINGERS && m_ParamSeg.nHandType != 2))
                            {
                                m_strDiagnostic = "Please evaluate the image - Wrong hand!\r\n";
                                m_nDiagnosticCode = 10;
                            }
                        }
                        for( i=0; i<m_ParamSeg.nParamFing; i++ )
                        {
                            if( m_Subf[i].err == 0 && m_Subf[i].nfiq > 3 )
                            {
                                m_strDiagnostic += "Please evaluate the image - Image Quality (NFIQ) > 3.\r\n";
                                m_nDiagnosticCode += 1;
                                break;
                            }
                        }
	                }
	                if( m_nDiagnosticCode == 0 )
                        m_strDiagnostic = "OK";                
                
                    m_ShowImage.SetText(null, Brushes.Green);                      
        	        //get the NFIQ
	                m_hDevice.MathImageNfIQ(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height, ref m_nNfiq, ref bError);                
                    OnAction(0);
                    break;
                }        
            } while(bRC && !m_bStop);
            return true;
        }

        public void TurnOffLed()
        {
            if (!m_bIsDeviceOpened)
                if (!Open())
                    return;
            LedControl.SetSingleLed(m_hDevice, false, false, (byte)0, (byte)1, false);
        }
    
        public bool PrepareRolling()
        {
            m_ShowImage.Reset();
            bool bCriticalError = false;
            if (m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64)
            {
                try{
                    m_hDevice.ImageFormat = (int)ImageFormat.FORMAT_800_750;    //800*750 format
                }
                catch( FutronicException ex )
                {
                    m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                    return false;
                }
            }
            if ((m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64) && m_ftCurrent >= FingerType.FT_ROLLED_LEFT_LITTLE)
            {
                LedControl.SetSingleLed(m_hDevice, true, true, (byte)(m_ftCurrent - FingerType.FT_ROLLED_LEFT_LITTLE), (byte)1, m_bSound);
            }
            
            if ((m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_60 || m_nDeviceCompatibility == (byte)DeviceCompatibility.FTR_DEVICE_USB_2_0_TYPE_64) && m_ftCurrent >= FingerType.FT_ROLLED_LEFT_LITTLE)
            {
                LedControl.SetSingleLed(m_hDevice, true, false, (byte)(m_ftCurrent - FingerType.FT_ROLLED_LEFT_LITTLE), (byte)2, false);
            }

            m_hDevice.InvertImage = true;
            m_hDevice.SetRollNicePreview();

            if (m_pBuffer != null)
                m_pBuffer = null;
            m_ImageSize = m_hDevice.ImageSize;
            m_pBuffer = new byte[m_ImageSize.Width * m_ImageSize.Height];
            try
            {
                m_hDevice.StartRoll();
            }
            catch (FutronicException ex)
            {
                m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                return false;
            }
            return true;
        }

        public bool DoRoll()
        {
            bool bRC = false;
            m_bUnavailable = true;
            m_nUnavailableFingers = 1;
            bool bCriticalError = false;
            if( !PrepareRolling() )
                return false;
            m_bStop = false;
            bool bCanStop = false;
            ROLL_FRAME_PARAMETERS FrameParameters = new ROLL_FRAME_PARAMETERS();
            while( !bRC && !bCriticalError )
            {
                if (!bCanStop)
                {
                    bCanStop = true;
                    OnAction(3);	//Enable buttons
                }
                try
                {
                    bRC = m_hDevice.GetRollFrameParam(ref m_pBuffer, Timeout.Infinite, ref FrameParameters);
                }
                catch (FutronicException ex)
                {
                    m_strErrMsg = FPInfo.GetErrorMessage((ErrorCodes)ex.ErrorCode, out bCriticalError);
                    if (!bCriticalError)
                    {
                        int errCode = ex.ErrorCode;
                        if (errCode == (int)ErrorCodes.kROLL_PROGRESS_REMOVE_FINGER || errCode == (int)ErrorCodes.kROLL_PROGRESS_PUT_FINGER ||
                            errCode == (int)ErrorCodes.kROLL_PROGRESS_POST_PROCESSING)
                            m_ShowImage.SetText(m_strErrMsg, Brushes.Green);
                        else
                        {
                            m_ShowImage.SetText(null, Brushes.Green);
                            m_ShowImage.SetRollingParameters(FrameParameters);
                        }
                        m_ShowImage.SetImage(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height);                        
                    }
                }
                if( bRC )
                {
                    // We've got a final image
                    m_bUnavailable = false;
                    m_nUnavailableFingers = 0;
                    //get the NFIQ
                    m_nNfiq = 0;
                    bool bErr = false;
                    m_hDevice.MathImageNfIQ(m_pBuffer, m_ImageSize.Width, m_ImageSize.Height, ref m_nNfiq, ref bErr);
                    m_ShowImage.SetText(null, Brushes.Green);
                    FrameParameters.dwFrameIndex = 0;
                    m_ShowImage.SetRollingParameters(FrameParameters);
                    m_ShowImage.SetNFIQ(m_nNfiq);
                    m_ShowImage.SetImage( m_pBuffer, m_ImageSize.Width, m_ImageSize.Height);
                    if (m_nNfiq > 0 && m_nNfiq < 4)
                    {
                        m_strDiagnostic = "OK";
                        m_nDiagnosticCode = 0;
                    }
                    else
                    {
                        m_strDiagnostic = "Please evaluate the image - Image Quality (NFIQ) > 3.";
                        m_nDiagnosticCode = 1;
                    }
                    OnAction(1);	//Roll Completed
                }
            }
            Close();
            return bRC;
        }
     }
}
