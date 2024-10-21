// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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


namespace Corelib.LibCore.Maps.Tiled;

/// <summary>
/// Represents a tiled map, adds the concept of tiles and tilesets.
/// </summary>
[PublicAPI]
public class TiledMap : Map, IDisposable
{
    public TiledMapTileSets Tilesets       { get; set; } = new();
    public List< object >?  OwnedResources { get; set; }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
    }

    /// <summary>
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( OwnedResources != null )
            {
                foreach ( IDisposable disposable in OwnedResources )
                {
                    disposable.Dispose();
                }

                OwnedResources = null;
            }
        }
    }

    /// <summary>
    /// Allows an object to try to free resources and perform other cleanup
    /// operations before it is reclaimed by garbage collection.
    /// </summary>
    ~TiledMap()
    {
        Dispose( true );
    }
}
