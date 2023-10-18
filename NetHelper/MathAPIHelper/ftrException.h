#pragma once
using namespace System;

namespace Futronic {
namespace MathAPIHelper
{
    public ref class FutronicException : public Exception
    {
    public:
        FutronicException( int errorCode )
            : Exception()
            , m_ErrorCode( errorCode )
        {
        }

        FutronicException( int errorCode, String ^ message )
            : Exception( message )
            , m_ErrorCode( errorCode )
        {
        }

        ///<summary>
        /// Gets error code
        ///</summary>
        property int ErrorCode
        {
            int get()
            {
                return m_ErrorCode;
            }
        }
    protected:

        int m_ErrorCode;
    };
}
}