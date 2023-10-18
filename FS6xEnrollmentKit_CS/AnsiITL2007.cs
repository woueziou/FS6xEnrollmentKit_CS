using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FS6xEnrollmentKit_CS
{    
    public class RECORD_TYPE_4_14
    {
	    public byte[] pInfo;
	    public byte[] pImage;
	    public int nInfoLength;
	    public int nImageLength;
        public RECORD_TYPE_4_14()
        {
            nInfoLength = nImageLength = 0;
            pInfo = pImage = null;
        }
    }

    public class AnsiITL2007
    {
        public const byte FS = 0x1C;
        public const byte GS	= 0x1D;
        public const byte RS = 0x1E;
        public const byte US = 0x1F;

        public byte m_nType;
        private int m_nRecord2Length;
        private byte[] m_pRecordType2;
        private RECORD_TYPE_4_14[] m_pRecordType14;	//maximum 14 type14 records
        private RECORD_TYPE_4_14[] m_pRecordType4;
        int m_nRecords;
        private String m_strTransactionType;
        private String m_strDestinationAgency;
        private String m_strOriginatingAgency;
        private String m_strTransactionControl;

        public AnsiITL2007()
        {
	        m_nRecords = 0;
	        m_pRecordType2 = null;
	        m_nRecord2Length = 0;
                m_pRecordType14 = new RECORD_TYPE_4_14[14];
                m_pRecordType4 = new RECORD_TYPE_4_14[14];
	        for( int i=0; i<14; i++ )
	        {
                m_pRecordType4[i] = new RECORD_TYPE_4_14();
                m_pRecordType14[i] = new RECORD_TYPE_4_14();
	        }
	        m_strTransactionType = "Demo";
	        m_strDestinationAgency = "Destination";
	        m_strOriginatingAgency = "Futronic";
	        m_strTransactionControl = "123456";
	        m_nType = 0;
        }

        public AnsiITL2007(String strTransactionType, String strDestinationAgency, String strOriginatingAgency, String strTransactionControl)
        {
	        m_nRecords = 0;
	        m_pRecordType2 = null;
	        m_nRecord2Length = 0;
	        for( int i=0; i<14; i++ )
	        {
                m_pRecordType14[i].pImage = null;
                m_pRecordType14[i].pInfo = null;
                m_pRecordType14[i].nImageLength = m_pRecordType14[i].nInfoLength = 0;
	        }
    	    m_strTransactionType = strTransactionType;
	        m_strDestinationAgency = strDestinationAgency;
	        m_strOriginatingAgency = strOriginatingAgency;
	        m_strTransactionControl = strTransactionControl;
        }

        private byte[] GetBytes(string str)
        {
            char[] chars = str.ToArray();
            int nLength = chars.Length;
            byte[] bytes = new byte[nLength];
            for (int i = 0; i < nLength; i++)
                bytes[i] = (byte)chars[i];
            return bytes;
        }

        public void AddRecordType2( String strDevice )
        {
	        //Field 2.001: Logical record length (LEN)
	        //Field 2.002: Image designation character (IDC)
	        //Field 2.003 and above: User-defined fields
	        byte[] pData = new byte[100];
	        int nIndex = 0;
	        FillFieldDataValue(pData, 0, 2, 2, 0, true, ref nIndex);
	        //2.003 Date
	        String strData = DateTime.Now.ToString("yyyyMMdd"); 
	        FillFieldDataString(pData, nIndex, 2, 3, strData, ref nIndex);
	        //2.004 Device name
	        strData = "2.004:Futronic";
	        int nStrLen = strData.Length;
            Array.Copy(GetBytes(strData), 0, pData, nIndex, nStrLen);
	        nIndex += nStrLen;
	        if( !String.IsNullOrEmpty(strDevice) )
	        {
                pData[nIndex] = US;
                nIndex++;
                Array.Copy(GetBytes(strDevice), 0, pData, nIndex, strDevice.Length);
                nIndex += strDevice.Length;
	        }
	        pData[nIndex] = FS;
	        nIndex++;
	        //2.001 calculate the length
	        int nRecLength = nIndex + 7;	//2.001:  + GS (or FS) (6bytes + 1byte) 
	        strData = nRecLength.ToString();
	        nStrLen = strData.Length;
	        nRecLength += nStrLen;
	        strData = nRecLength.ToString();
	        nRecLength = nRecLength + strData.Length - nStrLen;
	        strData = String.Format("2.001:{0}", nRecLength);
	        nStrLen = strData.Length;
	        m_pRecordType2 = new byte[nRecLength];
            Array.Copy( GetBytes(strData), 0, m_pRecordType2, 0, nStrLen);
	        m_pRecordType2[nStrLen] = GS;
            Array.Copy( pData, 0, m_pRecordType2, nStrLen+1, nIndex);
	        m_nRecord2Length = nRecLength;
        }

        public void AddRecordType14( byte[] pImage, int nImageSize, int nWidth, int nHeight, byte itType,
		    String strImageCompression, String strSource, String strComment, int nFingerPosition, 
		    int nNumberOfSegment, FINGER_SEGMENT[] fsSegment, int nSegOffset, int nNumberOfAmp, FS6xEnrollmentKit_CS.AcceptedImage.FINGER_AMP[] faAmp, int nNFIQ)
        {
	        if( m_nType != 14 )
                return;
	        int i=0;
	        byte[] pData = new byte[1000];
	        int nIndex = 0;
	        //LEN M 14.001 LOGICAL RECORD LENGTH
	        // - calculate the len in the end.
	        //IDC M 14.002 IMAGE DESIGNATION CHAR
	        // - IDC 00 is used for Record Type 2
	        //Format("14.002:%02d"), m_nRecords)
	        m_nRecords ++;
	        FillFieldDataValue(pData, 0, 14, 2, m_nRecords, true, ref nIndex);
	        //IMP M 14.003 IMPRESSION TYPE
	        //"14.003:");	//Live-scan Plain or Rolled - 00 or 01
            FillFieldDataValue(pData, nIndex, 14, 3, (int)itType, true, ref nIndex);
	        //SRC M 14.004 SOURCE AGENCY / ORI 
            FillFieldDataString(pData, nIndex, 14, 4, strSource, ref nIndex);
	        //FCD M 14.005 FINGERPRINT CAPTURE DATE
	        String strData = DateTime.Now.ToString("yyyyMMdd");
            FillFieldDataString(pData, nIndex, 14, 5, strData, ref nIndex);       
	        //HLL M 14.006 HORIZONTAL LINE LENGTH
            FillFieldDataValue(pData, nIndex, 14, 6, nWidth, false, ref nIndex);
	        //VLL M 14.007 VERTICAL LINE LENGTH
            FillFieldDataValue(pData, nIndex, 14, 7, nHeight, false, ref nIndex);
	        //SLC M 14.008 SCALE UNITS
	        //"14.008:1");	// 1- per inch, 2 -per cm
            FillFieldDataValue(pData, nIndex, 14, 8, 1, false, ref nIndex);
	        //HPS M 14.009 HORIZONTAL PIXEL SCALE	
	        //"14.009:500");	//PPI
            FillFieldDataValue(pData, nIndex, 14, 9, 500, false, ref nIndex);
	        //VPS M 14.010 VERTICAL PIXEL SCALE
	        //"14.010:500");	//PPI
            FillFieldDataValue(pData, nIndex, 14, 10, 500, false, ref nIndex);	
	        //CGA M 14.011 COMPRESSION ALGORITHM
            FillFieldDataString(pData, nIndex, 14, 11, strImageCompression, ref nIndex);
	        //BPX M 14.012 BITS PER PIXEL
	        // "14.012:8");
            FillFieldDataValue(pData, nIndex, 14, 12, 8, false, ref nIndex);	
	        //FGP M 14.013 FINGER POSITION
            FillFieldDataValue(pData, nIndex, 14, 13, nFingerPosition, false, ref nIndex);	
	        //AMP O 14.018 AMPUTATED OR BANDAGED
	        strData = "14.018:";
	        int nStrLen = strData.Length;
            Array.Copy(GetBytes(strData), 0, pData, nIndex, nStrLen);
	        nIndex += nStrLen;
	        for( i=0; i<nNumberOfAmp; i++ )
	        {
                FillSubFieldData(pData, nIndex, faAmp[i].FingerId, false, US, ref nIndex);
                if( faAmp[i].AMPCode == 2 )
                    strData = "UP";		// Bandaged (unable to print) AMPCD = "UP"
                else
                    strData = "XX";		// Amputation, AMPCD = "XX"
                Array.Copy(GetBytes(strData), 0, pData, nIndex, strData.Length);
                nIndex += strData.Length;
                if( nNumberOfAmp > (i+1) )
                {
                    pData[nIndex] = RS;
                    nIndex ++;
                }			
	        }
	        pData[nIndex] = GS;
	        nIndex ++;
	        //COM O 14.020 COMMENT
	        FillFieldDataString(pData, nIndex, 14, 20, strComment, ref nIndex);
	        //SEG O 14.021 FINGERPRINT SEGMENTATION POSITION(S)
	        strData = "14.021:";
	        nStrLen = strData.Length;
            Array.Copy(GetBytes(strData),0, pData, nIndex, nStrLen);
	        nIndex += nStrLen;
	        for( i=0; i<nNumberOfSegment; i++ )
	        {
                FillSubFieldData(pData, nIndex, fsSegment[i+nSegOffset].FingerId, false, US, ref nIndex);
                FillSubFieldData(pData, nIndex, fsSegment[i + nSegOffset].Left, false, US, ref nIndex);
                FillSubFieldData(pData, nIndex, fsSegment[i + nSegOffset].Right, false, US, ref nIndex);
                FillSubFieldData(pData, nIndex, fsSegment[i + nSegOffset].Top, false, US, ref nIndex);
                if( nNumberOfSegment > (i+1) )
                    FillSubFieldData(pData, nIndex, fsSegment[i + nSegOffset].Bottom, false, RS, ref nIndex);
                else
                    FillSubFieldData(pData, nIndex, fsSegment[i + nSegOffset].Bottom, false, (byte)0, ref nIndex);
	        }
	        pData[nIndex] = GS;
	        nIndex ++;
	        //NQM O 14.022 NIST QUALITY METRIC
	        strData = "14.022:";
	        nStrLen = strData.Length;
            Array.Copy(GetBytes(strData), 0, pData, nIndex, nStrLen);
	        nIndex += nStrLen;
	        if( nNumberOfSegment > 0 ) 
	        {
                for( i=0; i<nNumberOfSegment; i++ )
                {
                    FillSubFieldData(pData, nIndex, fsSegment[i + nSegOffset].FingerId, false, US, ref nIndex);
                    if( nNumberOfSegment > (i+1) )
                        FillSubFieldData(pData, nIndex, fsSegment[i + nSegOffset].NFIQ, false, RS, ref nIndex);
                    else
                        FillSubFieldData(pData, nIndex, fsSegment[i + nSegOffset].NFIQ, false, (byte)0, ref nIndex);
                }
    	    }
	        else
	        {
                if( nNFIQ>0 )
                {
                    FillSubFieldData(pData, nIndex, nFingerPosition, false, US, ref nIndex);
                    FillSubFieldData(pData, nIndex, nNFIQ, false, RS, ref nIndex);
                }
	        }
	        pData[nIndex] = GS;
	        nIndex ++;
	        //DATA M 14.999 IMAGE DATA
	        strData = "14.999:";
	        nStrLen = strData.Length;
            Array.Copy(GetBytes(strData), 0, pData, nIndex, nStrLen);
	        nIndex += nStrLen;
	        // calculate the record length
	        int nRecLength = nImageSize + nIndex + 9;	//14.001:  + GS (or FS) (7bytes + 1byte) + last byte FS
	        strData = nRecLength.ToString();
	        nStrLen = strData.Length;
	        nRecLength += nStrLen;
	        strData = nRecLength.ToString();
	        nRecLength = nRecLength + strData.Length - nStrLen;
	        strData = String.Format("14.001:{0}", nRecLength);
	        nStrLen = strData.Length;
	        m_pRecordType14[m_nRecords-1].pImage = null;
	        m_pRecordType14[m_nRecords-1].nInfoLength = nRecLength-nImageSize-1;	// last byte FS
	        m_pRecordType14[m_nRecords-1].pInfo = new byte[nRecLength-nImageSize-1];
            Array.Copy(GetBytes(strData), 0, m_pRecordType14[m_nRecords-1].pInfo, 0, nStrLen);
	        m_pRecordType14[m_nRecords-1].pInfo[nStrLen] = GS;
            Array.Copy(pData, 0, m_pRecordType14[m_nRecords-1].pInfo, nStrLen+1, nIndex);
	        m_pRecordType14[m_nRecords-1].nImageLength = nImageSize;
	        m_pRecordType14[m_nRecords-1].pImage = pImage;
        }

        public void AddRecordType4( byte[] pImage, int nImageSize, short nWidth, short nHeight, byte itType, byte nFingerPosition)
        {
	        if( m_nType != 4 )
                return;
	        m_nRecords ++;
	        //LEN  LOGICAL RECORD LENGTH - 4bytes 0-3
	        int nLength = 18 + nImageSize;
	        m_pRecordType4[m_nRecords-1].pImage = null;
	        m_pRecordType4[m_nRecords-1].nInfoLength = 18;		//fixed 18 byte
	        m_pRecordType4[m_nRecords-1].pInfo = new byte[18];
	        //fill in the first 18 bytes data
	        m_pRecordType4[m_nRecords-1].pInfo[0] = (byte)( nLength >> 24 );
	        m_pRecordType4[m_nRecords-1].pInfo[1] = (byte)( nLength >> 16 );
	        m_pRecordType4[m_nRecords-1].pInfo[2] = (byte)( nLength >> 8 );
	        m_pRecordType4[m_nRecords-1].pInfo[3] = (byte)nLength;
	        //IDC IMAGE DESIGNATION CHAR
	        m_pRecordType4[m_nRecords-1].pInfo[4] = (byte)m_nRecords;
	        //IMP IMPRESSION TYPE
	        m_pRecordType4[m_nRecords-1].pInfo[5] = (byte)itType;
	        //FGP FINGER POSITION - 6 bytes
	        m_pRecordType4[m_nRecords-1].pInfo[6] = nFingerPosition;
	        m_pRecordType4[m_nRecords-1].pInfo[7] = m_pRecordType4[m_nRecords-1].pInfo[8] = m_pRecordType4[m_nRecords-1].pInfo[9] = 
                    m_pRecordType4[m_nRecords-1].pInfo[10] = m_pRecordType4[m_nRecords-1].pInfo[11] = (byte)0xFF;
	        //ISR Image scanning resolution
	        m_pRecordType4[m_nRecords-1].pInfo[12] = 1;
	        //HLL HORIZONTAL LINE LENGTH - 2bytes
	        m_pRecordType4[m_nRecords-1].pInfo[13] = (byte)(nWidth >> 8);
	        m_pRecordType4[m_nRecords-1].pInfo[14] = (byte)nWidth;
	        //VLL VERTICAL LINE LENGTH
	        m_pRecordType4[m_nRecords-1].pInfo[15] = (byte)(nHeight >> 8);
	        m_pRecordType4[m_nRecords-1].pInfo[16] = (byte)nHeight;
	        //GCA/BCA COMPRESSION ALGORITHM
	        m_pRecordType4[m_nRecords-1].pInfo[17] = 1; //WSQ
	        //DATA Image data
	        m_pRecordType4[m_nRecords-1].nImageLength = nImageSize;
	        m_pRecordType4[m_nRecords-1].pImage = pImage;
        }

        //****************************************************************
        // SaveRecord to file
        //		- add Type1 and Type2 record and then save all the Type14 records
        //****************************************************************
        public bool SaveRecord( String strFileName )
        {
	        byte[] pData = new byte[1000];
	        int nStrLen = 0;
	        int nIndex = 0;
	        int i=0;
	        String strVersionNumber = "0400";
	        String strResolution = "19.69";
	        //LEN M 1.001 LOGICAL RECORD LENGTH 
	        // - calculate in the end
	        //VER M 1.002 VERSION NUMBER
	        // "1.002:0400");
            FillFieldDataString(pData, 0, 1, 2, strVersionNumber, ref nIndex);
	        //CNT M 1.003 FILE CONTENT
	        String strData = "1.003:1";
	        nStrLen = strData.Length;
            Array.Copy(GetBytes(strData), 0, pData, nIndex, nStrLen);        
	        nIndex += nStrLen;
	        pData[nIndex] = US;
	        nIndex ++;
            FillSubFieldData(pData, nIndex, m_nRecords + 1, false, (byte)0, ref nIndex);
	        pData[nIndex] = RS;
	        nIndex ++;
	        //CNT 2:00
	        //- FillSubFieldData(pData, nIndex, 2, false, TRUE, nIndex); -
	        //v1.3 fixed a bug to pass TRUE instead of US
            FillSubFieldData(pData, nIndex, 2, false, US, ref nIndex);
            FillSubFieldData(pData, nIndex, 0, true, (byte)0, ref nIndex);
	        if( m_nRecords > 0 )
                    pData[nIndex] = RS;
	        else
                    pData[nIndex] = GS;
	        nIndex ++;
	        for( i=0; i<m_nRecords; i++ )
	        {
                //FillSubFieldData(pData, nIndex, 14, false, US, nIndex);
                FillSubFieldData(pData, nIndex, m_nType, false, US, ref nIndex);
                if( m_nRecords > (i+1) )
                    FillSubFieldData(pData, nIndex, i + 1, true, RS, ref nIndex);
                else
                    FillSubFieldData(pData, nIndex, i + 1, true, GS, ref nIndex);
	        }
	        //TOT M 1.004 TYPE OF TRANSACTION 
            FillFieldDataString(pData, nIndex, 1, 4, m_strTransactionType, ref nIndex);
	        //DAT M 1.005 DATE
	        //COleDateTime dtNow = COleDateTime::GetCurrentTime();
	        //CString strDate = dtNow.Format("%Y%m%d"));
            String strDate = DateTime.Now.ToString("yyyyMMdd");
	        FillFieldDataString(pData, nIndex, 1, 5, strDate, ref nIndex);
	        //PRY O 1.006 PRIORITY	x
	        //DAI M 1.007 DESTINATION AGENCY IDENTIFIER
            FillFieldDataString(pData, nIndex, 1, 7, m_strDestinationAgency, ref nIndex);	
	        //ORI M 1.008 ORIGINATING AGENCY IDENTIFIER
            FillFieldDataString(pData, nIndex, 1, 8, m_strOriginatingAgency, ref nIndex);	
	        //TCN M 1.009 TRANSACTION CONTROL NUMBER
            FillFieldDataString(pData, nIndex, 1, 9, m_strTransactionControl, ref nIndex);		
	        //TCR O 1.010 TRANSACTION CONTROL REFERENCE NUMBER	x
	        //NSR M 1.011 NATIVE SCANNING RESOLUTION
            FillFieldDataString(pData, nIndex, 1, 11, strResolution, ref nIndex);		
	        //NTR M 1.012 NOMINAL TRANSMITTING RESOLUTION
            FillFieldDataString(pData, nIndex, 1, 12, strResolution, ref nIndex);		
	        //DOM O 1.013 DOMAIN NAME		x
	        //GMT O 1.014 GREENWICH MEAN TIME		x
	        //DCS O 1.015 DIRECTORY OF CHARACTER SETS		x
	        // calculate the record length
	        int nRecLength = nIndex + 7;	//1.001:  + GS (or FS) (6bytes + 1byte) 
	        strData = nRecLength.ToString();
	        nStrLen = strData.Length;
	        nRecLength += nStrLen;
	        strData = nRecLength.ToString();
	        nRecLength = nRecLength + strData.Length - nStrLen;
	        strData = String.Format("1.001:{0}", nRecLength);
	        nStrLen = strData.Length;
	        byte[] pRecord1 = new byte[nRecLength];
            Array.Copy(GetBytes(strData), 0, pRecord1, 0, nStrLen);
	        pRecord1[nStrLen] = GS;
            Array.Copy(pData, 0, pRecord1, nStrLen+1, nIndex);
	        pRecord1[nRecLength-1] = FS;
	        //Save to file
            using (FileStream fileStream = new FileStream(strFileName, FileMode.Create))
            {                
                //Type1            
                fileStream.Write(pRecord1, 0, nRecLength);
                //Type2
                if( m_pRecordType2 == null )
                {
                    AddRecordType2("");
                }
                fileStream.Write( m_pRecordType2, 0, m_nRecord2Length );
                //Type14 or Type4
                for (i = 0; i < m_nRecords; i++)
                {
                    if (m_nType == 14)
                    {
                        fileStream.Write(m_pRecordType14[i].pInfo, 0, m_pRecordType14[i].nInfoLength);
                        if (m_pRecordType14[i].nImageLength > 0 && m_pRecordType14[i].pImage != null)
                            fileStream.Write(m_pRecordType14[i].pImage, 0, m_pRecordType14[i].nImageLength);
                        fileStream.Write(pRecord1, nRecLength - 1, 1);	//FS				
                    }
                    else if (m_nType == 4)
                    {
                        fileStream.Write(m_pRecordType4[i].pInfo, 0, m_pRecordType4[i].nInfoLength);
                        if (m_pRecordType4[i].nImageLength > 0 && m_pRecordType4[i].pImage != null)
                            fileStream.Write(m_pRecordType4[i].pImage, 0, m_pRecordType4[i].nImageLength);
                    }
                }
            }
	        return true;
        }

        private void FillFieldDataString(byte[] pData, int nIndex, int nRecordType, int nRecordIndex, String strData, ref int nDataIndex )
        {
	        String strTemp;
	        strTemp = String.Format("{0}.{1:D3}:{2}", nRecordType, nRecordIndex, strData);
	        int nStrLen = strTemp.Length;
            Array.Copy(GetBytes(strTemp), 0, pData, nIndex, nStrLen);
	        pData[nIndex+nStrLen] = GS;
	        nDataIndex = nDataIndex + nStrLen +1;
        }

        private void FillFieldDataValue(byte[] pData, int nIndex, int nRecordType, int nRecordIndex, int nData, bool bFix2ByteData, ref int nDataIndex )
        {
	        String strTemp;
	        if( bFix2ByteData )
                strTemp = String.Format("{0}.{1:D3}:{2:D2}", nRecordType, nRecordIndex, nData);
	        else
                strTemp = String.Format("{0}.{1:D3}:{2}", nRecordType, nRecordIndex, nData);
	        int nStrLen = strTemp.Length;
            Array.Copy(GetBytes(strTemp), 0, pData, nIndex, nStrLen);
	        pData[nIndex+nStrLen] = GS;
	        nDataIndex = nDataIndex + nStrLen +1;
        }

        //nSeparator == 0 -> Not separator
        private void FillSubFieldData(byte[] pData, int nIndex, int nData, bool bFix2ByteData, byte nSeparator, ref int nDataIndex )
        {
	        String strTemp;
	        if( bFix2ByteData )
                strTemp = String.Format("{0:D2}", nData);
	        else
                strTemp = nData.ToString();
	        int nStrLen = strTemp.Length;
            Array.Copy(GetBytes(strTemp), 0, pData, nIndex, nStrLen);
	        //nSeparator == 0 -> Not separator
	        if( nSeparator != 0 )
	        {
                pData[nIndex+nStrLen] = nSeparator;
                nDataIndex ++;
	        }
	        nDataIndex += nStrLen;
        }
    }
}
