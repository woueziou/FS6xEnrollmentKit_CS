// ScanAPIHelper.h

#pragma once

using namespace System;
using namespace System::Text;
using namespace System::Globalization;
using namespace System::Collections;
using namespace System::Drawing;

namespace Futronic {
namespace MathAPIHelper {

    public enum class FTRSCAN_INTERFACE_STATUS
    {
	    FTRSCAN_INTERFACE_STATUS_CONNECTED,
	    FTRSCAN_INTERFACE_STATUS_DISCONNECTED
    };

    public value struct DeviceInfo
    {
        Byte    DeviceCompatibility;
        Size    imageSize;

        static initonly DeviceInfo Empty = { (Size::Empty, 0 ) };
    };

	public value struct ROLL_FRAME_PARAMETERS
	{
		UINT32 dwSize;
		UINT32 dwFlags;
		UINT32 dwStatus;
		UINT32 dwRollingResult;
		UINT32 dwDirection;
		UINT32 dwFrameIndex;
		UINT32 dwFrameDose;
		UINT32 dwFrameContrast;
		UINT32 dwFrameTimeMs;
	};

    public value struct Version
    {
	    Int16       wMajorVersionHi; 
	    Int16       wMajorVersionLo; 
	    Int16       wMinorVersionHi; 
	    Int16       wMinorVersionLo;

        virtual String^ ToString() override
        {
            StringBuilder^ version = gcnew StringBuilder();
            if( wMajorVersionHi != -1 )
            {
                version->Append( wMajorVersionHi.ToString( CultureInfo::InvariantCulture->NumberFormat ) );

                if( wMajorVersionLo != -1 )
                {
                    version->Append( "." );
                    version->Append( wMajorVersionLo.ToString( CultureInfo::InvariantCulture->NumberFormat ) );

                    if( wMinorVersionHi != -1 )
                    {
                        version->Append( "." );
                        version->Append( wMinorVersionHi.ToString( CultureInfo::InvariantCulture->NumberFormat ) );

                        if( wMinorVersionLo != -1 )
                        {
                            version->Append( "." );
                            version->Append( wMinorVersionLo.ToString( CultureInfo::InvariantCulture->NumberFormat ) );
                        }
                    }
                }
            }
            return version->ToString();
        }

        static initonly Version Empty = { (0, 0, 0, 0 ) };
    };

    public value struct VersionInfo
    {
	    Version     APIVersion;
	    Version     HardwareVersion;
	    Version     FirmwareVersion;

        static initonly VersionInfo Empty = { (Version::Empty, Version::Empty, Version::Empty, Version::Empty ) };
    };

    [FlagsAttribute]
    public enum class LogMask : int
    {
        off = FTR_LOG_MASK_OFF,
        to_file = FTR_LOG_MASK_TO_FILE,
        to_aux = FTR_LOG_MASK_TO_AUX,
        timestamp = FTR_LOG_MASK_TIMESTAMP
    };

    public enum class LogLevel : int
    {
        minimum = FTR_LOG_LEVEL_MIN,
        optimal = FTR_LOG_LEVEL_OPTIMAL,
        full = FTR_LOG_LEVEL_FULL
    };

    public enum class DiodesStatus : Byte
    {
        turn_off = 0,
        turn_on_period  = 1,
        turn_on_permanent = 255
    };

    public enum class ErrorCodes
    {
        kNO_ERROR = FTR_ERROR_NO_ERROR,
        kEMPTY_FRAME = FTR_ERROR_EMPTY_FRAME,
        kMOVABLE_FINGER = FTR_ERROR_MOVABLE_FINGER,
        kNO_FRAME = FTR_ERROR_NO_FRAME,
        kUSER_CANCELED = FTR_ERROR_USER_CANCELED,
        kHARDWARE_INCOMPATIBLE = FTR_ERROR_HARDWARE_INCOMPATIBLE,
        kFIRMWARE_INCOMPATIBLE = FTR_ERROR_FIRMWARE_INCOMPATIBLE,
        kINVALID_AUTHORIZATION_CODE = FTR_ERROR_INVALID_AUTHORIZATION_CODE,
        kROLL_NOT_STARTED = FTR_ERROR_ROLL_NOT_STARTED,
        kROLL_PROGRESS_DATA = FTR_ERROR_ROLL_PROGRESS_DATA,
        kROLL_TIMEOUT = FTR_ERROR_ROLL_TIMEOUT,
        kROLL_ABORTED = FTR_ERROR_ROLL_ABORTED,
        kROLL_ALREADY_STARTED = FTR_ERROR_ROLL_ALREADY_STARTED,
		kROLL_PROGRESS_REMOVE_FINGER = FTR_ERROR_ROLL_PROGRESS_REMOVE_FINGER,
		kROLL_PROGRESS_PUT_FINGER = FTR_ERROR_ROLL_PROGRESS_PUT_FINGER,
		kROLL_PROGRESS_POST_PROCESSING = FTR_ERROR_ROLL_PROGRESS_POST_PROCESSING,
        kNO_MORE_ITEMS = FTR_ERROR_NO_MORE_ITEMS,
        kNOT_ENOUGH_MEMORY = FTR_ERROR_NOT_ENOUGH_MEMORY,
        kNO_SYSTEM_RESOURCES = FTR_ERROR_NO_SYSTEM_RESOURCES,
        kTIMEOUT = FTR_ERROR_TIMEOUT,
        kNOT_READY = FTR_ERROR_NOT_READY,
        kBAD_CONFIGURATION = FTR_ERROR_BAD_CONFIGURATION,
        kINVALID_PARAMETER = FTR_ERROR_INVALID_PARAMETER,
        kCALL_NOT_IMPLEMENTED = FTR_ERROR_CALL_NOT_IMPLEMENTED,
        kNOT_SUPPORTED = FTR_ERROR_NOT_SUPPORTED,
        kWRITE_PROTECT = FTR_ERROR_WRITE_PROTECT,
        kMESSAGE_EXCEEDS_MAX_SIZE = FTR_ERROR_MESSAGE_EXCEEDS_MAX_SIZE,
		kMESSAGE_FINGER_IS_PRESENT = FTR_ERROR_FINGER_IS_PRESENT
    };

/// MathAPI

	public value struct SubfPointCoord
    {
		int xs;
		int ys;
		int ws;
		int hs;
		int err;
		int nfiq;
        int qfutr;

	};

	public value struct SegmParameters
	{
		int nParamFing;
		int nParamAngle;
		int nParamNfiq;
		int nParamFixedSize;
		int nWidthSubf;
		int nHeightSubf;
		int nHandType;
		DWORD dwTimeScan;
		double dAngle;
		int nErr;
	};
	
//	struct SegmAdjustment
	
	public enum struct SegmAdjustment : int
    {
		//undeffing = UNDEFFING,
        nfing1 = NFING1,
		nfing2 = NFING2,
        nfing3 = NFING3,
		nfing4 = NFING4,
		noangle = NOANGLE,
		angle = ANGLE,
		nonfiq = NONFIQ,
		nfiq = NFIQ,
        qfutr = QFUTR,
        qfull = QFULL,
		realsize = REALSIZE,
		fixedsize = FIXEDSIZE

    };
	
	public enum struct MathNums : int
    {
		xsize = XSIZE,
		ysize = YSIZE,
		minsubfsize = MINSUBFSIZE,
		maxsubfsize = MAXSUBFSIZE,
		xsizefs50 = XSIZEFS50,
		ysizefs50 = YSIZEFS50,
		xsizefs60 = XSIZEFS60,
		ysizefs60 = YSIZEFS60
	};

    public ref class XYTQ_Struct
    {
		public :
		int nrows;
		array< int > ^xcol;
        array< int > ^ycol;
        array< int > ^thetacol;
        array< int > ^qcol;
		XYTQ_Struct()
		{
			nrows = 0;
			xcol = gcnew array< int >(MAX_FILE_MINUTIAE);
			ycol = gcnew array< int >(MAX_FILE_MINUTIAE);
			thetacol = gcnew array< int >(MAX_FILE_MINUTIAE);
			qcol = gcnew array< int >(MAX_FILE_MINUTIAE);
		}
    };

    public ref class Device : IDisposable
	{
    public:

        Device()
        {
            m_bDispose = false;
            m_hDevice = NULL;
            m_LastErrorCode = 0;
        }

        ///<summary>
        /// Releases the unmanaged resources used by the Device.
        ///</summary>
        virtual ~Device()
        {
            if( m_bDispose )
                return;

            this->!Device();
            m_bDispose = true;

            GC::SuppressFinalize( this );
        }

        ///<summary>
        /// opens device on the default interface
        ///</summary>
        ///<remarks>
        /// You can change the default interface number by calling <c>SetBaseInterface</c> function.
        ///</remarks>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is already opened.</exception>
        void Open()
        {
            CheckDispose();
            if( m_hDevice != NULL )
            {
                throw gcnew InvalidOperationException();
            }

            m_LastErrorCode = 0;
            m_hDevice = ftrScanOpenDevice();
            if( m_hDevice == NULL )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// opens device on the selected interface.
        ///</summary>
        ///<remarks>
        /// You can find the connected devices by calling the <c>GetInterfaces</c> function.
        ///</remarks>
        ///<param name='interfaceNumber'>Index of the device. The maximum number of devices is 128.
        /// The value must be between 0 and 127</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is already opened.</exception>
        ///<exception cref="ArgumentOutOfRangeException">The <c>interfaceNumber</c> parameter is out of range.</exception>
        void Open( int interfaceNumber )
        {
            CheckDispose();
            if( m_hDevice != NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( interfaceNumber >= FTR_MAX_INTERFACE_NUMBER || interfaceNumber < 0 )
            {
                throw gcnew ArgumentOutOfRangeException( "interfaceNumber" );
            }

            m_LastErrorCode = 0;
            m_hDevice = ftrScanOpenDeviceOnInterface( interfaceNumber );
            if( m_hDevice == NULL )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// Gets the current version number of the API, the device hardware version 
        /// and the device firmware version.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        void Close()
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            ftrScanCloseDevice( m_hDevice );
            m_hDevice = NULL;
        }

        ///<summary>
        /// Gets the current version number of the API, the device hardware version 
        /// and the device firmware version.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property VersionInfo VersionInformation
        {
            VersionInfo get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTRSCAN_VERSION_INFO info;
                memset( &info, 0, sizeof( FTRSCAN_VERSION_INFO ) );
                info.dwVersionInfoSize = sizeof( FTRSCAN_VERSION_INFO );
                if( !ftrScanGetVersion( m_hDevice, &info ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
                VersionInfo vinfo;

                vinfo.APIVersion.wMajorVersionHi = info.APIVersion.wMajorVersionHi;
                vinfo.APIVersion.wMajorVersionLo = info.APIVersion.wMajorVersionLo;
                vinfo.APIVersion.wMinorVersionHi = info.APIVersion.wMinorVersionHi;
                vinfo.APIVersion.wMinorVersionLo = info.APIVersion.wMinorVersionLo;

                vinfo.FirmwareVersion.wMajorVersionHi = info.FirmwareVersion.wMajorVersionHi;
                vinfo.FirmwareVersion.wMajorVersionLo = info.FirmwareVersion.wMajorVersionLo;
                vinfo.FirmwareVersion.wMinorVersionHi = info.FirmwareVersion.wMinorVersionHi;
                vinfo.FirmwareVersion.wMinorVersionLo = info.FirmwareVersion.wMinorVersionLo;

                vinfo.HardwareVersion.wMajorVersionHi = info.HardwareVersion.wMajorVersionHi;
                vinfo.HardwareVersion.wMajorVersionLo = info.HardwareVersion.wMajorVersionLo;
                vinfo.HardwareVersion.wMinorVersionHi = info.HardwareVersion.wMinorVersionHi;
                vinfo.HardwareVersion.wMinorVersionLo = info.HardwareVersion.wMinorVersionLo;

                return vinfo;
            }
        }

        ///<summary>
        /// Activates\Deactivates Live Finger Detection (LFD) feature during the capture process.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property bool DetectFakeFinger
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTR_DWORD dwFlags = 0;
                if( !ftrScanGetOptions( m_hDevice, &dwFlags ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return ((dwFlags & FTR_OPTIONS_DETECT_FAKE_FINGER) != 0);
            }

            void set( bool value )
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_DETECT_FAKE_FINGER, value ? FTR_OPTIONS_DETECT_FAKE_FINGER : 0 ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
            }
        }

        ///<summary>
        /// Activates\Deactivates fast finger detection method
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property bool FastFingerDetectMethod
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTR_DWORD dwFlags = 0;
                if( !ftrScanGetOptions( m_hDevice, &dwFlags ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return ((dwFlags & FTR_OPTIONS_FAST_FINGER_DETECT_METHOD) != 0);
            }

            void set( bool value )
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_FAST_FINGER_DETECT_METHOD, value ? FTR_OPTIONS_FAST_FINGER_DETECT_METHOD : 0 ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
            }
        }

        ///<summary>
        /// Increases\Decreases the image size and pixel size
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property bool ReceiveLongImage
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTR_DWORD dwFlags = 0;
                if( !ftrScanGetOptions( m_hDevice, &dwFlags ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return ((dwFlags & FTR_OPTIONS_RECEIVE_LONG_IMAGE) != 0);
            }

            void set( bool value )
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_RECEIVE_LONG_IMAGE, value ? FTR_OPTIONS_RECEIVE_LONG_IMAGE : 0 ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
            }
        }

        ///<summary>
        /// Activates\Deactivates Live Finger Detection (LFD) feature during the capture process.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property bool InvertImage
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTR_DWORD dwFlags = 0;
                if( !ftrScanGetOptions( m_hDevice, &dwFlags ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return ((dwFlags & FTR_OPTIONS_INVERT_IMAGE) != 0);
            }

            void set( bool value )
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_INVERT_IMAGE, value ? FTR_OPTIONS_INVERT_IMAGE : 0 ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
            }
        }

        ///<summary>
        /// Gets fingerprint present indication
        ///</summary>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property bool IsFingerPresent
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                FTR_DWORD dwFlags = 0;
                m_LastErrorCode = 0;
                BOOL bResult = ftrScanIsFingerPresent( m_hDevice, NULL );
                if( !bResult )
                {
                    m_LastErrorCode = (int)GetLastError();
                }
                return (bResult != 0);
            }
        }

        ///<summary>
        /// Gets the device specific information
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property DeviceInfo Information
        {
            DeviceInfo get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTRSCAN_DEVICE_INFO n_DeviceInfo;
                memset( &n_DeviceInfo, 0, sizeof( FTRSCAN_DEVICE_INFO ) );
                n_DeviceInfo.dwStructSize = sizeof( FTRSCAN_DEVICE_INFO );
                if( !ftrScanGetDeviceInfo( m_hDevice, &n_DeviceInfo ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
                DeviceInfo DevInfo;
                DevInfo.DeviceCompatibility = n_DeviceInfo.byDeviceCompatibility;
                DevInfo.imageSize.Height = n_DeviceInfo.wPixelSizeX;
                DevInfo.imageSize.Width = n_DeviceInfo.wPixelSizeY;

                return DevInfo;
            }
        }

        ///<summary>
        /// Gets the image size
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property Size ImageSize
        {
            Size get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTRSCAN_IMAGE_SIZE n_ImageSize;
                if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
                Size imageSize;
                imageSize.Height = n_ImageSize.nHeight;
                imageSize.Width = n_ImageSize.nWidth;

                return imageSize;
            }
        }

        ///<summary>
        /// Gets the raw image size
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property Size RawImageSize
        {
            Size get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTRSCAN_IMAGE_SIZE n_ImageSize;
                if( !ftrScanGetRawImageSize( m_hDevice, &n_ImageSize ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
                Size imageSize;
                imageSize.Height = n_ImageSize.nHeight;
                imageSize.Width = n_ImageSize.nWidth;

                return imageSize;
            }
        }

        ///<summary>
        /// Gets status to green gimmick diodes.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property bool GreenDiodesStatus
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                BOOL bIsGreenDiodeOn, bIsRedDiodeOn;
                if( !ftrScanGetDiodesStatus( m_hDevice, &bIsGreenDiodeOn, &bIsRedDiodeOn ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return (bIsGreenDiodeOn != 0);
            }
        }

        ///<summary>
        /// Gets status to red gimmick diodes.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property bool RedDiodesStatus
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                BOOL bIsGreenDiodeOn, bIsRedDiodeOn;
                if( !ftrScanGetDiodesStatus( m_hDevice, &bIsGreenDiodeOn, &bIsRedDiodeOn ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return (bIsRedDiodeOn != 0);
            }
        }

        ///<summary>
        /// Gets last error code
        ///</summary>
        property int LastErrorCode
        {
            int get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                return m_LastErrorCode;
            }
        }

        ///<summary>
        /// Sets new status to green and red gimmick diodes.
        ///</summary>
        ///<param name='green'>New status for the green gimmick diode.</param>
        ///<param name='red'>New status for the red gimmick diode.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        void SetDiodesStatus( DiodesStatus green, DiodesStatus red )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            m_LastErrorCode = 0;
            if( !ftrScanSetDiodesStatus( m_hDevice, (FTR_BYTE)green, (FTR_BYTE)red ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// Gets the EEPROM size in bytes.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property int MemorySize
        {
            int get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                int nSize;
                if( !ftrScanGetExtMemorySize( m_hDevice, &nSize ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return nSize;
            }
        }

        ///<summary>
        /// Gets device serial number
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property array<Byte>^ SerialNumber
        {
            array<Byte>^ get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                array< Byte >^ sn = gcnew array< Byte >(8);
                pin_ptr< Byte > p = &sn[0];
                if( !ftrScanGetSerialNumber( m_hDevice, (PVOID)p ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return sn;
            }
        }

		//void SetImageFormat( int nFormat )
		//{
  //          CheckDispose();
  //          if( m_hDevice == NULL )
  //          {
  //              throw gcnew InvalidOperationException();
  //          }
  //          m_LastErrorCode = 0;
  //          if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_IMAGE_FORMAT_MASK, FTR_OPTIONS_IMAGE_FORMAT_1 * nFormat ) )
  //          {
  //              m_LastErrorCode = (int)GetLastError();
  //              throw gcnew FutronicException( m_LastErrorCode );
  //          }
  //      }

        ///<summary>
        /// Gets a raw image from the device
        ///</summary>
        ///<param name='dose'>Durability of infrared led. The value must be in the 1 ?7 range.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentOutOfRangeException">The <c>dose</c> parameter is out of range.</exception>
        array<Byte>^ GetImage( int dose )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( dose > 7 || dose < 1 )
            {
                throw gcnew ArgumentOutOfRangeException( "dose" );
            }

            m_LastErrorCode = 0;
            FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            array< Byte >^ image = gcnew array< Byte >(n_ImageSize.nImageSize);
            pin_ptr< Byte > p = &image[0];
            if( !ftrScanGetImage2( m_hDevice, dose, (PVOID)p ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            return image;
        }

        ///<summary>
        /// Gets a raw image from the device without any internal illumination.
        ///</summary>
        ///<param name='dose'>Durability of infrared led. The value must be in the 1 ?7 range.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        array<Byte>^ GetDarkImage()
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }

            m_LastErrorCode = 0;
            FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            array< Byte >^ image = gcnew array< Byte >(n_ImageSize.nImageSize);
            pin_ptr< Byte > p = &image[0];
            if( !ftrScanGetDarkImage( m_hDevice, (PVOID)p ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            return image;
        }

        ///<summary>
        /// Gets a frame from the device.
        ///</summary>
        ///<param name='dose'>Durability of infrared led. The value must be in the 1 ?7 range.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        array<Byte>^ GetFrame()
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }

            m_LastErrorCode = 0;
            FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            array< Byte >^ image = gcnew array< Byte >(n_ImageSize.nImageSize);
            pin_ptr< Byte > p = &image[0];
            if( !ftrScanGetFrame( m_hDevice, (PVOID)p, NULL ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            return image;
        }

        ///<summary>
        /// Stores a 7-bytes length buffer on the device.
        ///</summary>
        ///<param name='buffer'>the 7-bytes length buffer.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentNullException">The <c>buffer</c> parameter is a null (Nothing in VB).</exception>
        ///<exception cref="ArgumentException">The length of <c>buffer</c> parameter is out of range.</exception>
        void SaveBytes( array< Byte >^ buffer )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( buffer == nullptr )
            {
                throw gcnew ArgumentNullException( "buffer" );
            }
            if( buffer->Length != 7 )
            {
                throw gcnew ArgumentException( "buffer" );
            }

            m_LastErrorCode = 0;
            pin_ptr< Byte > p = &buffer[0];
            if( !ftrScanSave7Bytes( m_hDevice, (PVOID)p ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// Restores a 7-bytes length buffer on the device.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        array< Byte >^ RestoreBytes()
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            m_LastErrorCode = 0;
            array< Byte >^ buffer = gcnew array< Byte >(7);
            pin_ptr< Byte > p = &buffer[0];
            if( !ftrScanRestore7Bytes( m_hDevice, (PVOID)p ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            return buffer;
        }

        ///<summary>
        /// Stores the authorization code to use with SaveSecretBytes/RestoreSecretBytes functions.
        ///</summary>
        ///<param name='buffer'>the 7-bytes length authorization code.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentNullException">The <c>buffer</c> parameter is a null (Nothing in VB).</exception>
        ///<exception cref="ArgumentException">The length of <c>buffer</c> parameter is out of range.</exception>
        void SetAuthorizationCode( array< Byte >^ buffer )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( buffer == nullptr )
            {
                throw gcnew ArgumentNullException( "buffer" );
            }
            if( buffer->Length != 7 )
            {
                throw gcnew ArgumentException( "buffer" );
            }

            m_LastErrorCode = 0;
            pin_ptr< Byte > p = &buffer[0];
            if( !ftrScanSetNewAuthorizationCode( m_hDevice, (PVOID)p ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// stores a 7-bytes length buffer on the device.
        ///</summary>
        ///<param name='code'>the 7-bytes length authorization code.</param>
        ///<param name='buffer'>the 7-bytes length buffer.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentNullException">The <c>code</c> or <c>buffer</c> parameter is a null (Nothing in VB).</exception>
        ///<exception cref="ArgumentException">The length of <c>code</c> or <c>buffer</c> parameter is out of range.</exception>
        void SaveSecretBytes( array< Byte >^ code, array< Byte >^ buffer )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( code == nullptr )
            {
                throw gcnew ArgumentNullException( "code" );
            }
            if( code->Length != 7 )
            {
                throw gcnew ArgumentException( "code" );
            }
            if( buffer == nullptr )
            {
                throw gcnew ArgumentNullException( "buffer" );
            }
            if( buffer->Length != 7 )
            {
                throw gcnew ArgumentException( "buffer" );
            }

            m_LastErrorCode = 0;
            pin_ptr< Byte > c = &code[0];
            pin_ptr< Byte > p = &buffer[0];
            if( !ftrScanSaveSecret7Bytes( m_hDevice, (FTR_PVOID)c, (FTR_PVOID)p ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// restores a 7-bytes length buffer from the device.
        ///</summary>
        ///<param name='code'>the 7-bytes length authorization code.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentNullException">The <c>code</c> parameter is a null (Nothing in VB).</exception>
        ///<exception cref="ArgumentException">The length of <c>code</c> parameter is out of range.</exception>
        array< Byte >^ RestoreSecretBytes( array< Byte >^ code )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( code == nullptr )
            {
                throw gcnew ArgumentNullException( "buffer" );
            }
            if( code->Length != 7 )
            {
                throw gcnew ArgumentException( "buffer" );
            }

            m_LastErrorCode = 0;
            array< Byte >^ buffer = gcnew array< Byte >(7);
            pin_ptr< Byte > p = &buffer[0];
            pin_ptr< Byte > c = &code[0];
            if( !ftrScanRestoreSecret7Bytes( m_hDevice, (FTR_PVOID)c, (FTR_PVOID)p ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            return buffer;
        }

        ///<summary>
        /// stores data to EEPROM at the specified position.
        ///</summary>
        ///<param name='data'>buffer containing the data to be written to EEPROM.</param>
        ///<param name='offset'>Specifies the number of bytes to offset the EEPROM pointer.</param>
        ///<param name='count'>Number of bytes to be written to EEPROM.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentNullException">The <c>data</c> parameter is a null (Nothing in VB).</exception>
        ///<exception cref="ArgumentException">The <c>offset</c> or <c>count</c> parameter is less than 0.</exception>
        ///<exception cref="ArgumentOutOfRangeException">The <c>count</c> is more than length of buffer data or the 
        /// sum of <c>offset</c> and <c>count</c> is more than memory size.</exception>
        void SaveMemory( array< Byte >^ data, int offset, int count )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( offset < 0 )
            {
                throw gcnew ArgumentException( "offset" );
            }
            if( count < 0 )
            {
                throw gcnew ArgumentException( "count" );
            }
            if( data == nullptr )
            {
                throw gcnew ArgumentNullException( "buffer" );
            }
            if( data->Length < count )
            {
                throw gcnew ArgumentOutOfRangeException( "count" );
            }

            int nMemSize = this->MemorySize;
            if( offset + count > nMemSize )
            {
                throw gcnew ArgumentOutOfRangeException();
            }
            m_LastErrorCode = 0;
            pin_ptr< Byte > p = &data[0];
            if( !ftrScanSaveExtMemory( m_hDevice, (FTR_PVOID)p, offset, count ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// restores data from EEPROM at the specified position.
        ///</summary>
        ///<param name='offset'>Specifies the number of bytes to offset the EEPROM pointer.</param>
        ///<param name='count'>Number of bytes to be read from EEPROM</param>
        ///<returns>the data readed from EEPROM</returns>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentNullException">The <c>data</c> parameter is a null (Nothing in VB).</exception>
        ///<exception cref="ArgumentException">The <c>offset</c> or <c>count</c> parameter is less than 0.</exception>
        ///<exception cref="ArgumentOutOfRangeException">The <c>count</c> is more than length of buffer data or the 
        /// sum of <c>offset</c> and <c>count</c> is more than memory size.</exception>
        array< Byte >^ RestoreMemory( int offset, int count )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( offset < 0 )
            {
                throw gcnew ArgumentException( "offset" );
            }
            if( count < 0 )
            {
                throw gcnew ArgumentException( "count" );
            }
            int nMemSize = this->MemorySize;
            if( offset + count > nMemSize )
            {
                throw gcnew ArgumentOutOfRangeException();
            }

            m_LastErrorCode = 0;
            array< Byte >^ data = gcnew array< Byte >(count);
            pin_ptr< Byte > p = &data[0];
            if( !ftrScanRestoreExtMemory( m_hDevice, (FTR_PVOID)p, offset, count ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            return data;
        }
        ///<summary>
        /// Gets roll supported flag.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        void SetRollNicePreview()
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            m_LastErrorCode = 0;
            if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_ROLL_NICE_PREVIEW, FTR_OPTIONS_ROLL_NICE_PREVIEW ))
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }                
        }

        ///<summary>
        /// Gets roll supported flag.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        property bool IsRollSupported
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                FTR_BOOL featurePresent;
                if( !ftrScanIsScannerFeaturePresent( m_hDevice, FTR_SCANNER_FEATURE_ROLL, &featurePresent ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
                
                return featurePresent ? true : false;
            }
        }

        ///<summary>
        /// Starts roll operation.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        void StartRoll()
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( !IsRollSupported )
            {
                m_LastErrorCode = FTR_ERROR_NOT_SUPPORTED;
                throw gcnew FutronicException( m_LastErrorCode );
            }

            m_LastErrorCode = 0;
            if( !ftrScanRollStart( m_hDevice ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// Starts roll operation with variable dose
        ///</summary>
        ///<param name='variableDose'>..... The value must be in the 0 ?255 range.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentOutOfRangeException">The <c>variableDose</c> parameter is out of range.</exception>
        void StartRollWithVariableDose( int variableDose )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( !IsRollSupported )
            {
                m_LastErrorCode = FTR_ERROR_NOT_SUPPORTED;
                throw gcnew FutronicException( m_LastErrorCode );
            }
            if( variableDose > 255 || variableDose < 0 )
            {
                throw gcnew ArgumentOutOfRangeException( "variableDose" );
            }

            m_LastErrorCode = 0;
            if( !ftrScanRollStarWithVariableDose( m_hDevice, variableDose ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// Starts roll operation.
        ///</summary>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        void StartRollRaw()
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( !IsRollSupported )
            {
                m_LastErrorCode = FTR_ERROR_NOT_SUPPORTED;
                throw gcnew FutronicException( m_LastErrorCode );
            }

            m_LastErrorCode = 0;
            if( !ftrScanRollRawStart( m_hDevice ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// Starts roll operation with variable dose
        ///</summary>
        ///<param name='variableDose'>..... The value must be in the 0 ?255 range.</param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        ///<exception cref="ArgumentOutOfRangeException">The <c>variableDose</c> parameter is out of range.</exception>
        void StartRollRawWithVariableDose( int variableDose )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( !IsRollSupported )
            {
                m_LastErrorCode = FTR_ERROR_NOT_SUPPORTED;
                throw gcnew FutronicException( m_LastErrorCode );
            }
            if( variableDose > 255 || variableDose < 0 )
            {
                throw gcnew ArgumentOutOfRangeException( "variableDose" );
            }

            m_LastErrorCode = 0;
            if( !ftrScanRollRawStarWithVariableDose( m_hDevice, variableDose ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// Aborts roll operation
        ///</summary>
        ///<param name='synchronous'><c>true<\c> to wait for operation aborting otherwise <c>false<\c></param>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        void AbortRoll( bool synchronous )
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            m_LastErrorCode = 0;
            if( !ftrScanRollAbort( m_hDevice, synchronous ? TRUE : FALSE ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
        }

        ///<summary>
        /// Gets current image
        ///</summary>
        ///<param name='imageData'></param>
        ///<param name='millisecondsTimeout'>The number of milliseconds to wait, or Timeout.Infinite (-1) to wait indefinitely.</param>
        ///<returns><c>true</c> if the operation completes, otherwise <c>false</c></returns>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        ///<exception cref="ObjectDisposedException">The class instance is disposed. Any calls are prohibited.</exception>
        ///<exception cref="InvalidOperationException">The device is not opened.</exception>
        bool GetRollImage( array<Byte>^ %imageData, int millisecondsTimeout)
        {
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }

            m_LastErrorCode = 0;
            pin_ptr< Byte > p = &imageData[0];
            if( !ftrScanRollGetImage( m_hDevice, p, millisecondsTimeout ) )
            {
                int lastError = (int)GetLastError();
                if( lastError != FTR_ERROR_ROLL_PROGRESS_DATA )
                {
                    m_LastErrorCode = lastError;
                    throw gcnew FutronicException( m_LastErrorCode );
                }
                operationComplete = false;
            }

            return operationComplete;
        }

        bool GetRollFrameParam( array<Byte>^ %imageData, int millisecondsTimeout, ROLL_FRAME_PARAMETERS% paramsRoll)
        {
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }

            m_LastErrorCode = 0;
            pin_ptr< Byte > p = &imageData[0];
			FTRSCAN_ROLL_FRAME_PARAMETERS params;
	        params.dwSize = sizeof( FTRSCAN_ROLL_FRAME_PARAMETERS );

            if( !ftrScanRollGetFrameParameters( m_hDevice, &params, p, millisecondsTimeout ) )
            {
				m_LastErrorCode = GetLastError();
                operationComplete = false;
				paramsRoll.dwSize = params.dwSize;
				paramsRoll.dwFlags = params.dwFlags;
				paramsRoll.dwStatus = params.dwStatus;
				paramsRoll.dwRollingResult = params.dwRollingResult;
				paramsRoll.dwDirection = params.dwDirection;
				paramsRoll.dwFrameIndex = params.dwFrameIndex;
				paramsRoll.dwFrameDose = params.dwFrameDose;
				paramsRoll.dwFrameContrast = params.dwFrameContrast;
				paramsRoll.dwFrameTimeMs = params.dwFrameTimeMs;
                throw gcnew FutronicException( m_LastErrorCode );
            }
            return operationComplete;
        }

    public:
        ///<summary>
        /// Gets the device status for each interface.
        ///</summary>
        ///<returns>the array of device status</returns>
        ///<exception cref="ScanAPIException">The operation fails. To get extended error information, see error code property</exception>
        static array<FTRSCAN_INTERFACE_STATUS>^ GetInterfaces()
        {
            FTRSCAN_INTERFACES_LIST list;
            if( !ftrScanGetInterfaces( &list ) )
            {
                throw gcnew FutronicException( (int)GetLastError() );
            }
            array<FTRSCAN_INTERFACE_STATUS>^ statusArray = gcnew array<FTRSCAN_INTERFACE_STATUS>( FTR_MAX_INTERFACE_NUMBER );
            for( int index = 0; index < FTR_MAX_INTERFACE_NUMBER; index++ )
            {
                statusArray[ index ] = (FTRSCAN_INTERFACE_STATUS)list.InterfaceStatus[ index ];
            }
            return statusArray;
        }


        ///<summary>
        /// Gets\Sets the default interface number.
        ///</summary>
        ///<exception cref="ArgumentOutOfRangeException">The <c>value</c> parameter is out of range.
        /// The value must be between 0 and 127</exception>
        static property int BaseInterface
        {
            int get()
            {
                return ftrGetBaseInterfaceNumber();
            }

            void set( int value )
            {
                if( value >= FTR_MAX_INTERFACE_NUMBER || value < 0 )
                {
                    throw gcnew ArgumentOutOfRangeException( "value" );
                }
                if( !ftrSetBaseInterface( value ) )
                {
                    throw gcnew FutronicException( (int)GetLastError() );
                }
            }
        }

        static Bitmap^ GetBitmapFromBuffer( array<Byte>^data, int height, int width )
        {
            int length = sizeof( BITMAPFILEHEADER ) + sizeof( BITMAPINFO ) +
                         sizeof(RGBQUAD)*255 + width * height;
            array<byte> ^BmpData = gcnew array<byte>( length );
            pin_ptr<byte> imageSrc = &data[0];
            pin_ptr<byte> pBmpData = &BmpData[0];
            BITMAPFILEHEADER *pFileHeader = (BITMAPFILEHEADER *)pBmpData;
            BITMAPINFO *pBmpInfoHeader = (BITMAPINFO*)(pBmpData + sizeof( BITMAPFILEHEADER ) );
            RGBQUAD *pColorTable = pBmpInfoHeader->bmiColors;

            pFileHeader->bfType = MAKEWORD( 'B', 'M' );
            pFileHeader->bfSize = length;
            pFileHeader->bfOffBits = sizeof( BITMAPFILEHEADER ) + sizeof( BITMAPINFO ) + sizeof(RGBQUAD)*255;

            pBmpInfoHeader->bmiHeader.biSize          = sizeof( BITMAPINFOHEADER );
            pBmpInfoHeader->bmiHeader.biWidth         = width;
            pBmpInfoHeader->bmiHeader.biHeight        = height;
            pBmpInfoHeader->bmiHeader.biPlanes        = 1;
            pBmpInfoHeader->bmiHeader.biBitCount      = 8;
            pBmpInfoHeader->bmiHeader.biCompression   = BI_RGB;

            for( int iCyc = 0; iCyc < 256; iCyc++ )
            {
                pColorTable[iCyc].rgbBlue = pColorTable[iCyc].rgbGreen =
                pColorTable[iCyc].rgbRed = (BYTE)iCyc;
            }

            memcpy( pBmpData+pFileHeader->bfOffBits, imageSrc, width * height );

            System::IO::MemoryStream ^BmpStream = gcnew System::IO::MemoryStream( BmpData );

            return gcnew System::Drawing::Bitmap( BmpStream ); 
        }
		        ///<summary>
        /// Changes the logging facility level.
        ///</summary>
        ///<param name='mask'>Specify where logging output should be directed.</param>
        ///<param name='level'>Specifies the log level.</param>
        ///<param name='fileName'>Specifies the name of the log file.</param>
        ///<returns><c>true</c> if the function succeeds otherwise <c>false</c></returns>
        static bool SetLoggingFacilityLevel( LogMask mask, LogLevel level, String^ fileName )
        {
            IntPtr pointer = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi( fileName );
            FTR_BOOL bResult = ftrSetLoggingFacilityLevel( (FTR_DWORD)mask, (FTR_DWORD)level, (char*)pointer.ToPointer() );
            System::Runtime::InteropServices::Marshal::FreeHGlobal( pointer );

            return bResult ? true : false;
        }

       static Bitmap^ GetBitmapFromBuffer2( array<Byte>^data, int height, int width )
        {
            int length = sizeof( BITMAPFILEHEADER ) + sizeof( BITMAPINFO ) +
                         /*sizeof(RGBQUAD)*255*/ + 3*width * height;
            array<byte> ^BmpData = gcnew array<byte>( length );
            pin_ptr<byte> imageSrc = &data[0];
            pin_ptr<byte> pBmpData = &BmpData[0];

			BYTE *pdatarow;
			pdatarow = new BYTE [3*width];
			if( pdatarow == NULL )
				throw gcnew FutronicException( FTR_ERROR_NOT_ENOUGH_MEMORY );

            BITMAPFILEHEADER *pFileHeader = (BITMAPFILEHEADER *)pBmpData;
            BITMAPINFO *pBmpInfoHeader = (BITMAPINFO*)(pBmpData + sizeof( BITMAPFILEHEADER ) );
            RGBQUAD *pColorTable = pBmpInfoHeader->bmiColors;

            pFileHeader->bfType = MAKEWORD( 'B', 'M' );
            pFileHeader->bfSize = length;
            pFileHeader->bfOffBits = sizeof( BITMAPFILEHEADER ) + sizeof( BITMAPINFO ) /*+ sizeof(RGBQUAD)*255*/;

            pBmpInfoHeader->bmiHeader.biSize          = sizeof( BITMAPINFOHEADER );
            pBmpInfoHeader->bmiHeader.biWidth         = width;
            pBmpInfoHeader->bmiHeader.biHeight        = height;
            pBmpInfoHeader->bmiHeader.biPlanes        = 1;
            pBmpInfoHeader->bmiHeader.biBitCount      = 24;
            pBmpInfoHeader->bmiHeader.biCompression   = BI_RGB;

			int iCyc,i;
			for(iCyc=0;iCyc < height;iCyc++)
			{
				for(i=0;i<width;i++)
				{
					pdatarow[3*i]=imageSrc[(height-iCyc-1)*width+i];
					pdatarow[3*i+1]=imageSrc[(height-iCyc-1)*width+i];
					pdatarow[3*i+2]=imageSrc[(height-iCyc-1)*width+i];
				}
			memcpy(pBmpData+pFileHeader->bfOffBits+iCyc*3*width,pdatarow, width*3);
		
			}

			if( pdatarow != NULL )
                    delete [] pdatarow;
 
            System::IO::MemoryStream ^BmpStream = gcnew System::IO::MemoryStream( BmpData );

            return gcnew System::Drawing::Bitmap( BmpStream ); 
		}

		array<Byte>^ GetImageByVariableDose( int variableDose )
		{
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( variableDose > 255 || variableDose < 0 )
            {
                throw gcnew ArgumentOutOfRangeException( "variableDose" );
            }

            m_LastErrorCode = 0;
            FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            array< Byte >^ image = gcnew array< Byte >(n_ImageSize.nImageSize);
            pin_ptr< Byte > p = &image[0];
            if( !ftrScanGetImageByVariableDose( m_hDevice, variableDose, (PVOID)p ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

            return image;
        }
        property bool EliminateBackground 
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTR_DWORD dwFlags = 0;
                if( !ftrScanGetOptions( m_hDevice, &dwFlags ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return ((dwFlags & FTR_OPTIONS_ELIMINATE_BACKGROUND) != 0);
            }

            void set( bool value )
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_ELIMINATE_BACKGROUND, value ? FTR_OPTIONS_ELIMINATE_BACKGROUND : 0 ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
            }
        }

		property bool PreviewMode 
        {
            bool get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTR_DWORD dwFlags = 0;
                if( !ftrScanGetOptions( m_hDevice, &dwFlags ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

                return ((dwFlags & FTR_OPTIONS_PREVIEW_MODE) != 0);
            }

            void set( bool value )
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_PREVIEW_MODE, value ? FTR_OPTIONS_PREVIEW_MODE : 0 ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
            }
        }

		property int ImageFormat 
        {
            int get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTR_DWORD dwFlags = 0;
				int nImageFormat = 0;
                if( !ftrScanGetOptions( m_hDevice, &dwFlags ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }

				nImageFormat = ( dwFlags & FTR_OPTIONS_IMAGE_FORMAT_MASK ) / FTR_OPTIONS_IMAGE_FORMAT_1;
				return nImageFormat;
            }

            void set( int value )
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }
                m_LastErrorCode = 0;
                if( !ftrScanSetOptions( m_hDevice, FTR_OPTIONS_IMAGE_FORMAT_MASK, value * FTR_OPTIONS_IMAGE_FORMAT_1 ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
            }
        }

		void MainLEDsTimeout ( UInt32 % param1, Byte flag)
		{
			CheckDispose();
			if( m_hDevice == NULL )
            {
                 throw gcnew InvalidOperationException();
            }
			m_LastErrorCode = 0;
			
			FTR_BYTE byFlag;
			FTR_DWORD dwParam1;

			byFlag = flag;
			dwParam1 = param1;
			if( !ftrScanMainLEDsTimeout ( m_hDevice, &dwParam1, byFlag )) 
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
			param1 = dwParam1;

		}

		void ControlPin3( UInt32 % param1, UInt32 param2, UInt32 period )
		{
			CheckDispose();
			if( m_hDevice == NULL )
            {
                 throw gcnew InvalidOperationException();
            }
			m_LastErrorCode = 0;

			FTR_DWORD dwParam1, dwParam2, dwPeriod;
			dwParam1 = param1;
			dwParam2 = param2;
			dwPeriod = period;

			if( !ftrScanControlPin3( m_hDevice, &dwParam1, dwParam2, dwPeriod ))
			{
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }
			param1 = dwParam1;

		}

		void GetButtonState( UInt32 % param1 )
		{
			CheckDispose();
			if( m_hDevice == NULL )
            {
                 throw gcnew InvalidOperationException();
            }
			m_LastErrorCode = 0;

			FTR_DWORD dwParam1;		
			dwParam1 = param1;
			if( !ftrScanGetButtonState( m_hDevice, &dwParam1 ))
			{
				m_LastErrorCode = (int)GetLastError();
				throw gcnew FutronicException( m_LastErrorCode );
			}
			param1 = dwParam1;
		}

        array<Byte>^ GetFrame( int % dosage)
        {
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }

            m_LastErrorCode = 0;
            FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			FTRSCAN_FRAME_PARAMETERS FrameParameters;
            array< Byte >^ image = gcnew array< Byte >(n_ImageSize.nImageSize);
            pin_ptr< Byte > p = &image[0];

			memset( &FrameParameters, 0x00, sizeof(FrameParameters) );
            if( !ftrScanGetFrame( m_hDevice, (PVOID)p, &FrameParameters ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			dosage = FrameParameters.nDose;
            return image;
        }

		///MathAPI
		property Version MathVersion
        {
            Version get()
            {
                CheckDispose();
                if( m_hDevice == NULL )
                {
                    throw gcnew InvalidOperationException();
                }

                m_LastErrorCode = 0;
                FTRSCAN_VERSION info;
                memset( &info, 0, sizeof( FTRSCAN_VERSION ) );
                if( !ftrMathGetVersion( m_hDevice, &info ) )
                {
                    m_LastErrorCode = (int)GetLastError();
                    throw gcnew FutronicException( m_LastErrorCode );
                }
                Version vinfo;

                vinfo.wMajorVersionHi = info.wMajorVersionHi;
                vinfo.wMajorVersionLo = info.wMajorVersionLo;
                vinfo.wMinorVersionHi = info.wMinorVersionHi;
                vinfo.wMinorVersionLo = info.wMinorVersionLo;

                return vinfo;
            }
        }

		bool MathFrameSegment( array<Byte>^ imageData, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg ,  SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm)
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= n_ImageSize.nImageSize ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; 
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			if( !ftrMathScanFrameSegment( m_hDevice, NULL, p,pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 

            return operationComplete;
        }

		bool MathDoseSegment( int variableDose, array<Byte>^ imageData, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg ,  SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm)
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }

			if( variableDose > 255 || variableDose < 0 )
            {
                throw gcnew ArgumentOutOfRangeException( "variableDose" );
            }

            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= n_ImageSize.nImageSize ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; 
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			if( !ftrMathScanDoseSegment( m_hDevice, variableDose, p,pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 

            return operationComplete;
        }

		bool MathFrameSegmentPreview( array<Byte>^ imageData, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg ,  SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm)
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= n_ImageSize.nImageSize ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE / 4 ) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; 
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			if( !ftrMathScanFrameSegmentPreview( m_hDevice, NULL, p,pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 

            return operationComplete;
        }

		bool MathDoseSegmentPreview( int variableDose, array<Byte>^ imageData, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg ,  SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm)
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }

			if( variableDose > 255 || variableDose < 0 )
            {
                throw gcnew ArgumentOutOfRangeException( "variableDose" );
            }

            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= n_ImageSize.nImageSize ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE / 4) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; //FIXEDSIZE;
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			if( !ftrMathScanDoseSegmentPreview( m_hDevice, variableDose, p,pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 

            return operationComplete;
        }

		bool MathScanFrameNfIQ (  array<Byte>^ imageData, int% nfiq, bool% errnfiq ) 
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
			pin_ptr< Byte > p = &imageData[0];
  
			errnfiq = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

			FTR_BOOL bErr;
			int nNfiq;

			if( !ftrMathScanFrameNfIQ ( m_hDevice, p, NULL, &nNfiq, &bErr ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			errnfiq = bErr ? true : false; 
			nfiq = nNfiq;

            return operationComplete;
        }


		bool MathScanDoseNfIQ ( int variableDose, array<Byte>^ imageData, int% nfiq, bool% errnfiq )
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
			if( variableDose > 255 || variableDose < 0 )
            {
                throw gcnew ArgumentOutOfRangeException( "variableDose" );
            }
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
			pin_ptr< Byte > p = &imageData[0];
  
			errnfiq = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

			FTR_BOOL bErr;
			int nNfiq;

			if( !ftrMathScanDoseNfIQ ( m_hDevice, variableDose, p, &nNfiq, &bErr ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			errnfiq = bErr ? true : false; 
			nfiq = nNfiq;

            return operationComplete;
        }

		bool MathFrameSegment(int % dosage, array<Byte>^ imageData, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg ,  SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm)
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= n_ImageSize.nImageSize ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; 
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			FTRSCAN_FRAME_PARAMETERS FrameParameters;
			memset( &FrameParameters, 0x00, sizeof(FrameParameters) );

			if( !ftrMathScanFrameSegment( m_hDevice, &FrameParameters, p,pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			dosage = FrameParameters.nDose;
			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 

            return operationComplete;
        }

		bool MathFrameSegmentPreview( int % dosage, array<Byte>^ imageData, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg ,  SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm)
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= n_ImageSize.nImageSize ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE / 4 ) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; 
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			FTRSCAN_FRAME_PARAMETERS FrameParameters;
			memset( &FrameParameters, 0x00, sizeof(FrameParameters) );

			if( !ftrMathScanFrameSegmentPreview( m_hDevice, &FrameParameters, p,pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }
			
			dosage = FrameParameters.nDose;
			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 

            return operationComplete;
        }

		bool MathScanFrameNfIQ ( int % dosage, array<Byte>^ imageData, int% nfiq, bool% errnfiq ) 
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
			pin_ptr< Byte > p = &imageData[0];
  
			errnfiq = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

			FTR_BOOL bErr;
			int nNfiq;
			FTRSCAN_FRAME_PARAMETERS FrameParameters;
			memset( &FrameParameters, 0x00, sizeof(FrameParameters) );

			if( !ftrMathScanFrameNfIQ ( m_hDevice, p, &FrameParameters, &nNfiq, &bErr ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			dosage = FrameParameters.nDose;
			errnfiq = bErr ? true : false; 
			nfiq = nNfiq;

            return operationComplete;
        }

		bool MathImageNfIQ ( array<Byte>^ imageData,int width, int height, int% nfiq, bool% errnfiq ) 
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
			pin_ptr< Byte > p = &imageData[0];
  
			errnfiq = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < width * height )
            {
                throw gcnew ArgumentException( "imageData" );
			}

			FTR_BOOL bErr;
			int nNfiq;

			if( !ftrMathImageNfIQ ( m_hDevice, p, width, height, &nNfiq, &bErr ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			errnfiq = bErr ? true : false; 
			nfiq = nNfiq;

            return operationComplete;
        }

		bool MathImageSegment(array<Byte>^ imageData,int width, int height, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg, 
			SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm)
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			if( imageData->Length < width * height )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= width * height ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; 
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			if( !ftrMathImageSegment( m_hDevice, p, width, height, pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 

            return operationComplete;
        }

		bool MathFrameSegmentPreviewAuto( int % dosage, array<Byte>^ imageData, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg ,
            SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm, int% nAutoThresh )
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			FTRSCAN_IMAGE_SIZE n_ImageSize;
            if( !ftrScanGetImageSize( m_hDevice, &n_ImageSize ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );
            }

			if( imageData->Length < n_ImageSize.nImageSize )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= n_ImageSize.nImageSize ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE / 4 ) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;
            int AutoThresh;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; 
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			FTRSCAN_FRAME_PARAMETERS FrameParameters;
			memset( &FrameParameters, 0x00, sizeof(FrameParameters) );

			if( !ftrMathScanFrameSegmentPreviewAuto( m_hDevice, &FrameParameters, p,pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm, &AutoThresh ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }
			
			dosage = FrameParameters.nDose;
			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 
            nAutoThresh = AutoThresh;

            return operationComplete;
        }

		bool MathImageSegmentAuto(array<Byte>^ imageData,int width, int height, array<Byte>^ imageDataRes, array<Byte>^ imageDataSeg, 
			SegmParameters% SegmParameters , array< SubfPointCoord >^ SubfPoints , bool% bErrSegm , int% nAutoThresh )
   
		{
            bool operationComplete = true;
            CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
 
            if( imageData == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
  
			bErrSegm = false;
            m_LastErrorCode = 0;

			if( imageData->Length < width * height )
            {
                throw gcnew ArgumentException( "imageData" );
			}

            pin_ptr< Byte > p = &imageData[0];
			pin_ptr< Byte > pres;
			pin_ptr< Byte > presseg;
			
			if(imageDataRes == nullptr) 
			{
				pres = nullptr;
			}
			else
			{
				if( imageDataRes->Length >= width * height ) 
				{
					pres = &imageDataRes[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataRes" );
				}
			}

			if(imageDataSeg == nullptr) 
			{
				presseg = nullptr;
			}
			else
			{
				if( imageDataSeg->Length >= SegmParameters.nParamFing * MAXSUBFSIZE * MAXSUBFSIZE) 
				{
					presseg = &imageDataSeg[0];
				}
				else
				{
					throw gcnew ArgumentException( "imageDataSeg" );
				}
			}

			if( SubfPoints == nullptr)
			{
                throw gcnew ArgumentNullException( "SubfPoints" );
            }
			if( SubfPoints->Length < SegmParameters.nParamFing )
            {
                throw gcnew ArgumentException( "SubfPoints" );
			}

			FTR_BOOL bSegm;
			SegmParam Param;
            int AutoThresh;

			Param.nParamFixedSize = SegmParameters.nParamFixedSize; 
			Param.nParamNfiq = SegmParameters.nParamNfiq;
			Param.nParamAngle = SegmParameters.nParamAngle;
			Param.nWidthSubf = SegmParameters.nWidthSubf;
			Param.nHeightSubf = SegmParameters.nHeightSubf;
			Param.nParamFing = SegmParameters.nParamFing;

			pin_ptr< SubfPointCoord > pSubf;
			pSubf = &SubfPoints[0]; 

			if( !ftrMathImageSegmentAuto( m_hDevice, p, width, height, pres, presseg,&Param,(SubfCoord *)pSubf,&bSegm,&AutoThresh ) )
            {
                m_LastErrorCode = (int)GetLastError();
                throw gcnew FutronicException( m_LastErrorCode );

				operationComplete = false;
            }

			SegmParameters.nParamFixedSize = Param.nParamFixedSize;
			SegmParameters.nParamNfiq = Param.nParamNfiq;
			SegmParameters.nParamAngle =Param.nParamAngle;
			SegmParameters.nWidthSubf = Param.nWidthSubf;
			SegmParameters.nHeightSubf = Param.nHeightSubf;
			SegmParameters.nHandType = Param.nHandType;
			SegmParameters.nParamFing = Param.nParamFing;
			SegmParameters.dwTimeScan = Param.dwTimeScan;
			SegmParameters.dAngle = Param.dAngle;

			bErrSegm = bSegm ? true : false; 
            nAutoThresh = AutoThresh;

            return operationComplete;
        }

		//ftrWSQ.dll
		bool WsqFromRAWImage( array<Byte>^ pRawImage, int nWidth, int nHeight, float fBitrate, int% nWsqSize, array<Byte>^ pWsqImg )
		{
			CheckDispose();
            if( m_hDevice == NULL )
            {
                throw gcnew InvalidOperationException();
            }
            if( pRawImage == nullptr || pWsqImg == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
			FTRIMGPARMS ImgParm;
			ImgParm.Bitrate = fBitrate;	
			ImgParm.DPI = -1;
			ImgParm.Width = nWidth;
			ImgParm.Height = nHeight;
			ImgParm.RAW_size =  nWidth * nHeight;
			pin_ptr< Byte > pRaw = &pRawImage[0];
			pin_ptr< Byte > pWsq = &pWsqImg[0];
			nWsqSize = 0;
			FTR_BOOL bRet = ftrWSQ_FromRAWImage( (void *)m_hDevice, (unsigned char *)pRaw, &ImgParm, (unsigned char *)pWsq );
			if( bRet )
				nWsqSize = ImgParm.WSQ_size;
			return bRet ? true : false;
		}

		//ftrNBIS.dll
		private:
		bool Get_XYTQ( xytq_struct *pDest, XYTQ_Struct^ pSrc )
		{
			pDest->nrows = pSrc->nrows;
			if( pSrc->nrows == 0 )
				return false;
			for( int i=0; i<pSrc->nrows; i++ )
			{
				pDest->xcol[i] = pSrc->xcol[i];
				pDest->ycol[i] = pSrc->ycol[i];
				pDest->thetacol[i] = pSrc->thetacol[i];
				pDest->qcol[i] = pSrc->qcol[i];
			}
			return true;
		}

		bool Get_xytq( XYTQ_Struct^ pDest, xytq_struct *pSrc )
		{
			pDest->nrows = pSrc->nrows;
			if( pSrc->nrows == 0 )
				return false;
			for( int i=0; i<pSrc->nrows; i++ )
			{
				pDest->xcol[i] = pSrc->xcol[i];
				pDest->ycol[i] = pSrc->ycol[i];
				pDest->thetacol[i] = pSrc->thetacol[i];
				pDest->qcol[i] = pSrc->qcol[i];
			}
			return true;
		}

		public:
		bool NbisGetMinutiaeXYTQ( XYTQ_Struct^ pTemplate, array<Byte>^ pImage, int nWidth, int nHeight )
		{
            if( pImage == nullptr )
            {
                throw gcnew ArgumentNullException( "imageData" );
            }
			xytq_struct *pstruct =  (struct xytq_struct *) malloc( sizeof( struct xytq_struct ) );
			if ( pstruct == XYTQ_NULL )
				return false;
			pin_ptr< Byte > pRaw = &pImage[0];

			int ret = ftrGetMinutiaeXYTQ( pstruct, NIST_INTERNAL_XYT_REP, (unsigned char*)pRaw, nWidth, nHeight, 8, 500 );
			if( ret == 0 )
			{
				Get_xytq( pTemplate, pstruct );
			}
			//free memory
			free( pstruct );
			pstruct = XYTQ_NULL;
			return ( ret == 0 ? true : false );
		}

		bool NbisBozorth3SetBaseProbe( int% pHandle, XYTQ_Struct^ pBaseProbe )
		{
			xytq_struct *structP =  (struct xytq_struct *) malloc( sizeof( struct xytq_struct ) );
			if ( structP == XYTQ_NULL )
				return false;
			bool bRet = false;			
			if( Get_XYTQ( structP, pBaseProbe ) )
			{
				void *handle = NULL;
				int ret = ftrBozorth3SetBaseProbe( &handle, structP );
				if( ret == 0 )
				{
					pHandle = (int)handle;
					bRet = true;
				}
			}
			free( structP );
			structP = XYTQ_NULL;
			return bRet;
		}

		bool NbisBozorth3Identify( int pHandle, XYTQ_Struct^ pTemplate, int% score )
		{
			if( pTemplate == nullptr )
				return false;
			xytq_struct *structG =  (struct xytq_struct *) malloc( sizeof( struct xytq_struct ) );
			if ( structG == XYTQ_NULL )
				return false;
			bool bRet = false;
			if( Get_XYTQ( structG, pTemplate ) )
			{
				int ms = 0;
				int ret = ftrBozorth3Identify( (void *)pHandle, structG, &ms );
				if( ret == 0 )
				{
					score = ms;
					bRet = true;
				}
			}
			free( structG );
			structG = XYTQ_NULL;
			return  ( bRet ? true : false );
		}

		void NbisBozorth3ReleaseBaseProbe( int pHandle )
		{
			ftrBozorth3ReleaseBaseProbe( (void*) pHandle );	
		}

		bool NbisBozorth3Verify( XYTQ_Struct^ pstruct, XYTQ_Struct^ gstruct, int% score )
		{
			if( pstruct == nullptr || gstruct == nullptr )
				return false;
			xytq_struct *structP =  (struct xytq_struct *) malloc( sizeof( struct xytq_struct ) );
			if ( structP == XYTQ_NULL )
				return false;
			xytq_struct *structG =  (struct xytq_struct *) malloc( sizeof( struct xytq_struct ) );
			if ( structG == XYTQ_NULL )
			{
				free( structP );
				structP = XYTQ_NULL;
				return false;
			}
			bool bRet = false;
			if( Get_XYTQ( structP, pstruct) && Get_XYTQ( structG, gstruct) )
			{
				int ms = 0;
				int ret = ftrBozorth3Verify( structP, structG, &ms );
				if( ret == 0 )
				{
					score = ms;
					bRet = true;
				}
			}
			free( structP );
			structP = XYTQ_NULL;
			free( structG );
			structG = XYTQ_NULL;
			return ( bRet ? true : false );
		}

		//ftrbiomdi.dll
		public:
		bool BiomdiNewFIR(int% pFir, int nStd, short nDeviceID)
		{
			void * hFir = ftrNewFIR( nStd, nDeviceID);
			if( hFir != NULL )
			{
				pFir = (int) hFir;
				return true;
			}
			return false;
		}

		bool BiomdiFreeFIR(int pFir)
		{
			if( pFir != 0 )
				ftrFreeFIR((void *)pFir, true);
			return true;
		}

		bool BiomdiFIRAddImage(int pFir, array<Byte>^ pImage, int nImageSize, int nWidth, int nHeight, byte nFingerPosition, byte nNFIQ, byte nImpressionType)
		{
			pin_ptr< Byte > pRaw = &pImage[0];
			return ftrFIRAddImage((void *)pFir, (unsigned char *)pRaw, nImageSize, nWidth, nHeight, nFingerPosition, nNFIQ, nImpressionType, true);			
		}

		bool BiomdiGetFIRDataSize(int pFir, int% nSize)
		{
			unsigned long long sizeData = 0;
			bool bRet = ftrGetFIRDataSize((void *)pFir, &sizeData);
			if( bRet )
				nSize = (int)sizeData;
			return bRet;
		}

		bool BiomdiGetFIRData(int pFir, int nSize, array<Byte>^ pData)
		{
			if( nSize <= 0 )
				return false;
            if( pData == nullptr )
            {
                throw gcnew ArgumentNullException( "GetFIRData" );
            }
  
			if( pData->Length < nSize )
            {
                throw gcnew ArgumentException( "GetFIRData" );
			}
			pin_ptr< Byte > p = &pData[0];
			return ftrGetFIRData( (void *)pFir, (unsigned long)nSize, (unsigned char *)p);	
		}

////////////////////////
    protected:
        ///<summary>
        /// The Finalize method.
        ///</summary>
        !Device()
        {
            if( m_bDispose )
                return;

            if( m_hDevice != NULL )
            {
                ftrScanCloseDevice( m_hDevice );
                m_hDevice = NULL;
            }
        }

        ///<summary>
		/// If the class is disposed, this function raises an exception.
        ///</summary>
        ///<remarks>
		/// This function must be called before any operation in all functions.
        ///</remarks>
        void CheckDispose()
        {
            if( m_bDispose )
            {
                throw gcnew ObjectDisposedException( this->GetType()->FullName );
            }
        }

        ///<summary>
        /// If the class is deleted by calling <c>Dispose</c>, m_bDispose is true.
        ///</summary>
        ///<remarks>
        /// After of calling <c>Dispose</c>, the class cannot be used. 
        /// The class raises the <c>ObjectDisposedException</c> exception in
		/// the event of an invalid usage condition. 
        ///</remarks>
        bool m_bDispose;

        ///<summary>
        /// Last error code.
        ///</summary>
        int m_LastErrorCode;

        FTRHANDLE   m_hDevice;
    };
}
}