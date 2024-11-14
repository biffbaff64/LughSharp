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


namespace Corelib.LibCore.Assets;

// ====================================================================--------
// ====================================================================--------

/// <summary>
/// Interface representing the parameters required for loading assets.
/// </summary>
[PublicAPI]
public interface ILoaderParameters
{
    ILoadedCallback? LoadedCallback { get; set; }
}

// ====================================================================--------
// ====================================================================--------

/// <summary>
/// Callback interface that will be invoked when the
/// <see cref="AssetManager"/> loaded an asset.
/// </summary>
[PublicAPI]
public interface ILoadedCallback
{
    void FinishedLoading( AssetManager assetManager, string fileName, Type type );
}

// ====================================================================--------
// ====================================================================--------

/// <summary>
/// Base class for parameters used by asset loaders to define specific
/// settings or configurations needed for loading an asset.
/// </summary>
[PublicAPI]
public class AssetLoaderParameters : ILoaderParameters
{
    public ILoadedCallback? LoadedCallback { get; set; }
}

// ====================================================================--------
// ====================================================================--------

/// <summary>
/// Default Loaded callback for when there is no other available.
/// </summary>
[PublicAPI]
public class DefaultLoadedCallback( int refCount ) : ILoadedCallback
{
    public void FinishedLoading( AssetManager assetManager, string fileName, Type type )
    {
        assetManager.SetReferenceCount( fileName, refCount );
    }
}
