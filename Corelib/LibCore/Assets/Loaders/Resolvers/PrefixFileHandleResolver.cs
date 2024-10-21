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


namespace Corelib.LibCore.Assets.Loaders.Resolvers;

/// <summary>
/// A <see cref="IFileHandleResolver"/> that adds a prefix to the filename before
/// passing it to the base resolver. Can be used e.g. to use a given subfolder from
/// the base resolver. The prefix is added as is, you have to include any trailing
/// '/' character if needed.
/// </summary>
[PublicAPI]
public class PrefixFileHandleResolver : IFileHandleResolver
{
    public PrefixFileHandleResolver( IFileHandleResolver baseResolver, string prefix )
    {
        BaseResolver = baseResolver;
        Prefix       = prefix;
    }

    public string              Prefix       { get; }
    public IFileHandleResolver BaseResolver { get; }

    /// <inheritdoc />
    public FileInfo Resolve( string fileName )
    {
        return BaseResolver.Resolve( Prefix + fileName );
    }
}
