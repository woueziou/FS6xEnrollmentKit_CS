using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Futronic.MathAPIHelper;

namespace FS6xEnrollmentKit_CS
{
    class FPVerify
    {
        public static int FINGER_TYPE_NUMBER = 23;
        public static int m_nThreshold = 25;
        private XYTQ_Struct[] m_pTemplates = null;

        private class VERIFIED_RECORD
        {
            public int nIndex;
            public int nScore;
            public VERIFIED_RECORD()
            {
                nIndex = nScore = 0;
            }
        };

        public FPVerify()
        {
            m_nThreshold = 25;
            m_pTemplates = new XYTQ_Struct[FINGER_TYPE_NUMBER];
            Init();
        }

        public void Init()
        {
            for (int i = 0; i < FINGER_TYPE_NUMBER; i++)
            {
                m_pTemplates[i] = null;
            }
        }

        public bool Enroll(Device hDevice, byte[] pImage, int nWidth, int nHeight, int nIndex)
        {
            if (nIndex < 0 || nIndex > (FINGER_TYPE_NUMBER - 1))
                return false;
            m_pTemplates[nIndex] = new XYTQ_Struct();
            return hDevice.NbisGetMinutiaeXYTQ(m_pTemplates[nIndex], pImage, nWidth, nHeight);
        }


        public bool Identify(Device hDevice, byte[] pImage2, int nWidth2, int nHeight2, ref int nIndex, ref int nScore)
        {
	        XYTQ_Struct pstruct = new XYTQ_Struct();
	        bool bRet = hDevice.NbisGetMinutiaeXYTQ( pstruct, pImage2, nWidth2, nHeight2 );
	        if( !bRet )
            {
                pstruct = null;
                return false;
            }        
	        int pHandle = 0;
	        bRet =  hDevice.NbisBozorth3SetBaseProbe( ref pHandle, pstruct );
	        if( !bRet )
            {
                pstruct = null;
                return false;
            }
            VERIFIED_RECORD[] recVerify = new VERIFIED_RECORD[FINGER_TYPE_NUMBER];
	        int i;
	        int ms;
	        int nCount = 0;
	        int nMaxScore = 0;

	        for( i=0; i<FINGER_TYPE_NUMBER; i++ )
                    recVerify[i] = new VERIFIED_RECORD();
	
	        for( i=0; i<FINGER_TYPE_NUMBER; i++ )
	        {
                if( m_pTemplates[i] == null )
                    continue;
                ms = 0;
                bRet = hDevice.NbisBozorth3Identify( pHandle, m_pTemplates[i], ref ms );
                if( !bRet )
                {
                    //MessageBox(NULL, _T("Failed to call ftrBozorth3Identify!"), _T("Error"), MB_OK|MB_ICONSTOP);
                    hDevice.NbisBozorth3ReleaseBaseProbe( pHandle );	
                    pstruct = null;
                    return false;
                }
                if( nMaxScore < ms )
                    nMaxScore = ms;
                if( ms > m_nThreshold )
                {
                    recVerify[nCount].nIndex = i;
                    recVerify[nCount].nScore = ms;
                    nCount ++;
                }
	        }	
	        hDevice.NbisBozorth3ReleaseBaseProbe( pHandle );	
	        pstruct = null;

	        if( nCount == 0 )
	        {
                    nScore = nMaxScore;
                    return false;
	        }

	        nScore = recVerify[0].nScore;
	        nIndex = recVerify[0].nIndex;
	        for( i=1; i<nCount; i++ )
	        {
                if( recVerify[i].nScore > recVerify[i-1].nScore )
                {
                    nScore = recVerify[i].nScore;
                    nIndex = recVerify[i].nIndex;
                }
    	    }
	        return true;
        }

        public bool Verify(Device hDevice, byte[] pImg1, int nW1, int nH1, byte[] pImg2, int nW2, int nH2, ref int nScore)
        {
            XYTQ_Struct pstruct = new XYTQ_Struct();
            bool bRet = hDevice.NbisGetMinutiaeXYTQ(pstruct, pImg1, nW1, nH1);
            if (!bRet)
            {
                pstruct = null;
                return false;
            }
            XYTQ_Struct gstruct = new XYTQ_Struct();
            bRet = hDevice.NbisGetMinutiaeXYTQ(gstruct, pImg2, nW2, nH2);
            if (!bRet)
            {
                pstruct = null;
                gstruct = null;
                return false;
            }
            int ms = 0;
            bRet = hDevice.NbisBozorth3Verify(pstruct, gstruct, ref ms);
            if (bRet)
                if (ms < m_nThreshold)
                    bRet = false;	//Verify failed
            gstruct = null;
            pstruct = null;
            nScore = ms;
            return bRet;
        }
    }
}
