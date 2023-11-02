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

namespace LibGDXSharp.Assets;

[PublicAPI]
public class AssetLoaderParameters
{
    /// <summary>
    /// Callback interface that will be invoked when the
    /// <see cref="AssetManager"/> loaded an asset.
    /// </summary>
    public interface ILoadedCallback
    {
        void FinishedLoading( AssetManager assetManager, string fileName, Type type );
    }
    
    public ILoadedCallback? LoadedCallback { get; set; }
}

[PublicAPI]
public class DefaultLoadedCallback : AssetLoaderParameters.ILoadedCallback
{
    private readonly int _refCount;

    public DefaultLoadedCallback( int refCount )
    {
        this._refCount = refCount;
    }

    public void FinishedLoading( AssetManager assetManager, string fileName, Type type )
    {
        assetManager.SetReferenceCount( fileName, _refCount );
    }
}
