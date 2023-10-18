using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Futronic.MathAPIHelper;
using System.IO;

namespace FS6xEnrollmentKit_CS
{
    class FPDataInterchange
    {
        public static int FIR_STD_ANSI = 1;
        public static int FIR_STD_ISO = 2;

        private int m_pFir = 0 ;
        private int m_nSize = 0;
        private byte[] m_pRecord = null;
        private Device m_hDevice = null;

        public FPDataInterchange()
        {
            m_pFir = 0;
            m_nSize = 0;
            m_pRecord = null;
            m_hDevice = null;
        }
    
        public bool Initialize(Device hDevice, byte nFirStd, short nDeviceID)
        {
            m_nSize=0;
            m_hDevice = hDevice;
	        if( nFirStd != FIR_STD_ANSI && nFirStd != FIR_STD_ISO)
                return false;
	        if( m_pFir != 0 )
	        {
                m_hDevice.BiomdiFreeFIR(m_pFir);
                m_pFir = 0;
	        }
    	    return m_hDevice.BiomdiNewFIR(ref m_pFir, nFirStd, nDeviceID);
        }

        public void Terminate()
        {
	        if( m_pFir != 0)
	        {
                m_hDevice.BiomdiFreeFIR( m_pFir );
                m_pFir = 0;
	        }
    	    m_pRecord = null;
        }

        public bool AddImage(byte[] pImage, int nImageSize, int nWidth, int nHeight, byte nFingerPosition, byte nNFIQ, byte nImpressionType)
        {
    	    if( m_pFir == 0 || pImage == null || nWidth <=0 || nHeight <= 0 )
                return false;
	        bool bRet = m_hDevice.BiomdiFIRAddImage( m_pFir, pImage, nImageSize, nWidth, nHeight, nFingerPosition, nNFIQ, nImpressionType );
                return bRet;
        }

        public bool SaveRecord( String strFileName )
        {
	        if( m_pFir == 0 )
                return false;
            m_nSize = 0;
	        if( !m_hDevice.BiomdiGetFIRDataSize( m_pFir, ref m_nSize ) )
                return false;
            if( m_nSize <= 0 )
                return false; 

	        m_pRecord = new byte[m_nSize];
	        if( m_hDevice.BiomdiGetFIRData(m_pFir, m_nSize, m_pRecord) )
	        {
                //Save to file
                using (FileStream fileStream = new FileStream(strFileName, FileMode.Create))
                {
                    fileStream.Write(m_pRecord, 0, m_nSize);
                }
                return true;
            }
            return false;
        }

    }
}
