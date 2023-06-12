// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maps.Tiled;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class TiledMap : Map, IDisposable
{
    public TiledMapTileSets Tilesets       { get;         set; }
    public List< object >?  OwnedResources { private get; set; }

    public TiledMap()
    {
        Tilesets = new TiledMapTileSets();
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
                foreach( IDisposable disposable in OwnedResources )
                {
                    disposable.Dispose();
                }

                OwnedResources = null;
            }
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// Allows an object to try to free resources and perform other cleanup
    /// operations before it is reclaimed by garbage collection.
    /// </summary>
    ~TiledMap()
    {
        Dispose( false );
    }
}