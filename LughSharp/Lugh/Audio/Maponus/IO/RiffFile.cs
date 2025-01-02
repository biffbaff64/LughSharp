// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using LughSharp.Lugh.Audio.Maponus.Support;
using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Audio.Maponus.IO;

/// <summary>
/// public class to manage RIFF files.
/// </summary>
/// <remarks>
/// This class is marked as  to indicate to ReSharper that
/// methods and members etc may be called externally.
/// </remarks>
[PublicAPI]
public class RiffFile
{
    protected const int DDC_SUCCESS       = 0; // The operation succeded
    protected const int DDC_FAILURE       = 1; // The operation failed for unspecified reasons
    protected const int DDC_OUT_OF_MEMORY = 2; // Operation failed due to running out of memory
    protected const int DDC_FILE_ERROR    = 3; // Operation encountered file I/O error
    protected const int DDC_INVALID_CALL  = 4; // Operation was called with invalid parameters
    protected const int DDC_USER_ABORT    = 5; // Operation was aborted by the user
    protected const int DDC_INVALID_FILE  = 6; // File format does not match
    protected const int RF_UNKNOWN        = 0; // undefined type (can use to mean "N/A" or "not open")
    protected const int RF_WRITE          = 1; // open for write
    protected const int RF_READ           = 2; // open for read

    private readonly RiffChunkHeader _riffHeader; // header for whole file
    private          Stream?         _file;       // I/O stream to use
    private          int             _fmode;      // current file I/O mode

    public RiffFile()
    {
        _file  = null;
        _fmode = RF_UNKNOWN;

        _riffHeader = new RiffChunkHeader( this )
        {
            CkId   = FourCC( "RIFF" ),
            CkSize = 0,
        };
    }

    /// <summary>
    /// Return File Mode.
    /// </summary>
    public int CurrentFileMode()
    {
        return _fmode;
    }

    /// <summary>
    /// Open a RIFF file.
    /// </summary>
    public virtual int Open( string filename, int newMode )
    {
        var retcode = DDC_SUCCESS;

        if ( _fmode != RF_UNKNOWN )
        {
            retcode = Close();
        }

        if ( retcode == DDC_SUCCESS )
        {
            switch ( newMode )
            {
                case RF_WRITE:
                    try
                    {
                        _file = RandomAccessFileStream.CreateRandomAccessFile( filename, "rw" );

                        try
                        {
                            // Write the RIFF header...
                            // We will have to come back later and patch it!
                            var br = new sbyte[ 8 ];

                            br[ 0 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 24 ) & 0x000000FF );
                            br[ 1 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 16 ) & 0x000000FF );
                            br[ 2 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 8 ) & 0x000000FF );
                            br[ 3 ] = ( sbyte ) ( _riffHeader.CkId & 0x000000FF );

                            var br4 = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 24 ) & 0x000000FF );
                            var br5 = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 16 ) & 0x000000FF );
                            var br6 = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 8 ) & 0x000000FF );
                            var br7 = ( sbyte ) ( _riffHeader.CkSize & 0x000000FF );

                            br[ 4 ] = br7;
                            br[ 5 ] = br6;
                            br[ 6 ] = br5;
                            br[ 7 ] = br4;

                            _file.Write( SupportClass.ToByteArray( br ), 0, 8 );
                            _fmode = RF_WRITE;
                        }
                        catch
                        {
                            _file.Close();
                            _fmode = RF_UNKNOWN;
                        }
                    }
                    catch
                    {
                        _fmode  = RF_UNKNOWN;
                        retcode = DDC_FILE_ERROR;
                    }

                    break;

                case RF_READ:
                    try
                    {
                        _file = RandomAccessFileStream.CreateRandomAccessFile( filename, "r" );

                        try
                        {
                            // Try to read the RIFF header...
                            var br = new sbyte[ 8 ];
                            SupportClass.ReadInput( _file, ref br, 0, 8 );
                            _fmode = RF_READ;

                            _riffHeader.CkId = ( ( br[ 0 ] << 24 ) & ( int ) SupportClass.Identity( 0xFF000000 ) )
                                             | ( ( br[ 1 ] << 16 ) & 0x00FF0000 )
                                             | ( ( br[ 2 ] << 8 ) & 0x0000FF00 )
                                             | ( br[ 3 ] & 0x000000FF );

                            _riffHeader.CkSize = ( ( br[ 4 ] << 24 ) & ( int ) SupportClass.Identity( 0xFF000000 ) )
                                               | ( ( br[ 5 ] << 16 ) & 0x00FF0000 )
                                               | ( ( br[ 6 ] << 8 ) & 0x0000FF00 )
                                               | ( br[ 7 ] & 0x000000FF );
                        }
                        catch
                        {
                            _file.Close();
                            _fmode = RF_UNKNOWN;
                        }
                    }
                    catch
                    {
                        _fmode  = RF_UNKNOWN;
                        retcode = DDC_FILE_ERROR;
                    }

                    break;

                default:
                    retcode = DDC_INVALID_CALL;

                    break;
            }
        }

        return retcode;
    }

    /// <summary>
    /// Open a RIFF STREAM.
    /// </summary>
    public virtual int Open( Stream stream, int newMode )
    {
        var retcode = DDC_SUCCESS;

        if ( _fmode != RF_UNKNOWN )
        {
            retcode = Close();
        }

        if ( retcode == DDC_SUCCESS )
        {
            switch ( newMode )
            {
                case RF_WRITE:
                    try
                    {
                        //file = SupportClass.RandomAccessFileSupport.CreateRandomAccessFile(Filename, "rw");
                        _file = stream;

                        try
                        {
                            // Write the RIFF header...
                            // We will have to come back later and patch it!
                            var br = new sbyte[ 8 ];

                            br[ 0 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 24 ) & 0x000000FF );
                            br[ 1 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 16 ) & 0x000000FF );
                            br[ 2 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 8 ) & 0x000000FF );
                            br[ 3 ] = ( sbyte ) ( _riffHeader.CkId & 0x000000FF );

                            var br4 = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 24 ) & 0x000000FF );
                            var br5 = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 16 ) & 0x000000FF );
                            var br6 = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 8 ) & 0x000000FF );
                            var br7 = ( sbyte ) ( _riffHeader.CkSize & 0x000000FF );

                            br[ 4 ] = br7;
                            br[ 5 ] = br6;
                            br[ 6 ] = br5;
                            br[ 7 ] = br4;

                            _file.Write( SupportClass.ToByteArray( br ), 0, 8 );
                            _fmode = RF_WRITE;
                        }
                        catch
                        {
                            _file.Close();
                            _fmode = RF_UNKNOWN;
                        }
                    }
                    catch
                    {
                        _fmode  = RF_UNKNOWN;
                        retcode = DDC_FILE_ERROR;
                    }

                    break;

                case RF_READ:
                    try
                    {
                        _file = stream;

                        //file = SupportClass.RandomAccessFileSupport.CreateRandomAccessFile(Filename, "r");
                        try
                        {
                            // Try to read the RIFF header... 
                            var br = new sbyte[ 8 ];

                            SupportClass.ReadInput( _file, ref br, 0, 8 );

                            _fmode = RF_READ;

                            _riffHeader.CkId = ( ( br[ 0 ] << 24 ) & ( int ) SupportClass.Identity( 0xFF000000 ) )
                                             | ( ( br[ 1 ] << 16 ) & 0x00FF0000 )
                                             | ( ( br[ 2 ] << 8 ) & 0x0000FF00 )
                                             | ( br[ 3 ] & 0x000000FF );

                            _riffHeader.CkSize = ( ( br[ 4 ] << 24 ) & ( int ) SupportClass.Identity( 0xFF000000 ) )
                                               | ( ( br[ 5 ] << 16 ) & 0x00FF0000 )
                                               | ( ( br[ 6 ] << 8 ) & 0x0000FF00 )
                                               | ( br[ 7 ] & 0x000000FF );
                        }
                        catch
                        {
                            _file.Close();
                            _fmode = RF_UNKNOWN;
                        }
                    }
                    catch
                    {
                        _fmode  = RF_UNKNOWN;
                        retcode = DDC_FILE_ERROR;
                    }

                    break;

                default:
                    retcode = DDC_INVALID_CALL;

                    break;
            }
        }

        return retcode;
    }

    /// <summary>
    /// Write NumBytes data.
    /// </summary>
    public virtual int Write( sbyte[] data, int numBytes )
    {
        if ( _fmode != RF_WRITE )
        {
            return DDC_INVALID_CALL;
        }

        try
        {
            _file?.Write( SupportClass.ToByteArray( data ), 0, numBytes );
            _fmode = RF_WRITE;
        }
        catch
        {
            return DDC_FILE_ERROR;
        }

        _riffHeader.CkSize += numBytes;

        return DDC_SUCCESS;
    }

    /// <summary>
    /// Write NumBytes data.
    /// </summary>
    public virtual int Write( short[] data, int numBytes )
    {
        var theData = new sbyte[ numBytes ];
        var yc      = 0;

        for ( var y = 0; y < numBytes; y += 2 )
        {
            theData[ y ]     = ( sbyte ) ( data[ yc ] & 0x00FF );
            theData[ y + 1 ] = ( sbyte ) ( SupportClass.URShift( data[ yc++ ], 8 ) & 0x00FF );
        }

        if ( _fmode != RF_WRITE )
        {
            return DDC_INVALID_CALL;
        }

        try
        {
            _file?.Write( SupportClass.ToByteArray( theData ), 0, numBytes );
            _fmode = RF_WRITE;
        }
        catch
        {
            return DDC_FILE_ERROR;
        }

        _riffHeader.CkSize += numBytes;

        return DDC_SUCCESS;
    }

    /// <summary>
    /// Write NumBytes data.
    /// </summary>
    public virtual int Write( RiffChunkHeader riffHeader, int numBytes )
    {
        var br = new sbyte[ 8 ];

        br[ 0 ] = ( sbyte ) ( SupportClass.URShift( riffHeader.CkId, 24 ) & 0x000000FF );
        br[ 1 ] = ( sbyte ) ( SupportClass.URShift( riffHeader.CkId, 16 ) & 0x000000FF );
        br[ 2 ] = ( sbyte ) ( SupportClass.URShift( riffHeader.CkId, 8 ) & 0x000000FF );
        br[ 3 ] = ( sbyte ) ( riffHeader.CkId & 0x000000FF );

        var br4 = ( sbyte ) ( SupportClass.URShift( riffHeader.CkSize, 24 ) & 0x000000FF );
        var br5 = ( sbyte ) ( SupportClass.URShift( riffHeader.CkSize, 16 ) & 0x000000FF );
        var br6 = ( sbyte ) ( SupportClass.URShift( riffHeader.CkSize, 8 ) & 0x000000FF );
        var br7 = ( sbyte ) ( riffHeader.CkSize & 0x000000FF );

        br[ 4 ] = br7;
        br[ 5 ] = br6;
        br[ 6 ] = br5;
        br[ 7 ] = br4;

        if ( _fmode != RF_WRITE )
        {
            return DDC_INVALID_CALL;
        }

        try
        {
            _file?.Write( SupportClass.ToByteArray( br ), 0, numBytes );
            _fmode = RF_WRITE;
        }
        catch
        {
            return DDC_FILE_ERROR;
        }

        _riffHeader.CkSize += numBytes;

        return DDC_SUCCESS;
    }

    /// <summary>
    /// Write NumBytes data.
    /// </summary>
    public virtual int Write( short data, int numBytes )
    {
        if ( _fmode != RF_WRITE )
        {
            return DDC_INVALID_CALL;
        }

        try
        {
            var tempBinaryWriter = new BinaryWriter( _file! );
            tempBinaryWriter.Write( data );
            _fmode = RF_WRITE;
        }
        catch
        {
            return DDC_FILE_ERROR;
        }

        _riffHeader.CkSize += numBytes;

        return DDC_SUCCESS;
    }

    /// <summary>
    /// Write NumBytes data.
    /// </summary>
    public virtual int Write( int data, int numBytes )
    {
        if ( _fmode != RF_WRITE )
        {
            return DDC_INVALID_CALL;
        }

        try
        {
            var tempBinaryWriter = new BinaryWriter( _file! );

            tempBinaryWriter.Write( data );
            _fmode = RF_WRITE;
        }
        catch
        {
            return DDC_FILE_ERROR;
        }

        _riffHeader.CkSize += numBytes;

        return DDC_SUCCESS;
    }

    /// <summary>
    /// Read NumBytes data.
    /// </summary>
    public virtual int Read( sbyte[] data, int numBytes )
    {
        var retcode = DDC_SUCCESS;

        try
        {
            SupportClass.ReadInput( _file!, ref data, 0, numBytes );
        }
        catch
        {
            retcode = DDC_FILE_ERROR;
        }

        return retcode;
    }

    /// <summary>
    /// Expect NumBytes data.
    /// </summary>
    public virtual int Expect( string data, int numBytes )
    {
        var cnt = 0;

        try
        {
            while ( numBytes-- != 0 )
            {
                var target = ( sbyte? ) _file?.ReadByte();

                if ( target != data[ cnt++ ] )
                {
                    return DDC_FILE_ERROR;
                }
            }
        }
        catch
        {
            return DDC_FILE_ERROR;
        }

        return DDC_SUCCESS;
    }

    /// <summary>
    /// Close Riff File.
    /// Length is written too.
    /// </summary>
    public virtual int Close()
    {
        var retcode = DDC_SUCCESS;

        switch ( _fmode )
        {
            case RF_WRITE:
                try
                {
                    _file?.Seek( 0, SeekOrigin.Begin );

                    try
                    {
                        var br = new sbyte[ 8 ];

                        br[ 0 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 24 ) & 0x000000FF );
                        br[ 1 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 16 ) & 0x000000FF );
                        br[ 2 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkId, 8 ) & 0x000000FF );
                        br[ 3 ] = ( sbyte ) ( _riffHeader.CkId & 0x000000FF );

                        br[ 7 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 24 ) & 0x000000FF );
                        br[ 6 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 16 ) & 0x000000FF );
                        br[ 5 ] = ( sbyte ) ( SupportClass.URShift( _riffHeader.CkSize, 8 ) & 0x000000FF );
                        br[ 4 ] = ( sbyte ) ( _riffHeader.CkSize & 0x000000FF );

                        _file?.Write( SupportClass.ToByteArray( br ), 0, 8 );
                        _file?.Close();
                    }
                    catch
                    {
                        retcode = DDC_FILE_ERROR;
                    }
                }
                catch
                {
                    retcode = DDC_FILE_ERROR;
                }

                break;

            case RF_READ:
                try
                {
                    _file?.Close();
                }
                catch
                {
                    retcode = DDC_FILE_ERROR;
                }

                break;
        }

        _file  = null;
        _fmode = RF_UNKNOWN;

        return retcode;
    }

    /// <summary>
    /// Return File Position.
    /// </summary>
    public virtual long CurrentFilePosition()
    {
        GdxRuntimeException.ThrowIfNull( _file );

        long position;

        try
        {
            position = _file.Position;
        }
        catch
        {
            position = -1;
        }

        return position;
    }

    /// <summary>
    /// Write Data to specified offset.
    /// </summary>
    public virtual int Backpatch( long fileOffset, RiffChunkHeader data, int numBytes )
    {
        if ( _file == null )
        {
            return DDC_INVALID_CALL;
        }

        try
        {
            _file.Seek( fileOffset, SeekOrigin.Begin );
        }
        catch
        {
            return DDC_FILE_ERROR;
        }

        return Write( data, numBytes );
    }

    public virtual int Backpatch( long fileOffset, sbyte[] data, int numBytes )
    {
        if ( _file == null )
        {
            return DDC_INVALID_CALL;
        }

        try
        {
            _file.Seek( fileOffset, SeekOrigin.Begin );
        }
        catch
        {
            return DDC_FILE_ERROR;
        }

        return Write( data, numBytes );
    }

    /// <summary>
    /// Seek in the File.
    /// </summary>
    protected virtual int Seek( long offset )
    {
        int rc;

        try
        {
            _file?.Seek( offset, SeekOrigin.Begin );
            rc = DDC_SUCCESS;
        }
        catch
        {
            rc = DDC_FILE_ERROR;
        }

        return rc;
    }

    /// <summary>
    /// Fill the header.
    /// </summary>
    public static int FourCC( string chunkName )
    {
        sbyte[] p = [ 0x20, 0x20, 0x20, 0x20 ];

        SupportClass.GetSBytesFromString( chunkName, 0, 4, ref p, 0 );

        var ret = ( ( p[ 0 ] << 24 ) & ( int ) SupportClass.Identity( 0xFF000000 ) )
                | ( ( p[ 1 ] << 16 ) & 0x00FF0000 )
                | ( ( p[ 2 ] << 8 ) & 0x0000FF00 )
                | ( p[ 3 ] & 0x000000FF );

        return ret;
    }


    public class RiffChunkHeader
    {
        // Length of data in chunk
        public RiffChunkHeader( RiffFile enclosingInstance )
        {
            InitBlock( enclosingInstance );
        }

        public int CkId   { get; set; } // Four-character chunk ID
        public int CkSize { get; set; }

        public RiffFile EnclosingInstance { get; private set; } = null!;

        private void InitBlock( RiffFile instance )
        {
            EnclosingInstance = instance;
        }
    }
}
