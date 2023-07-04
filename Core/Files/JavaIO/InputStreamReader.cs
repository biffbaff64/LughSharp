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

namespace LibGDXSharp.Files;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class InputStreamReader : InputStream
{
    /// <summary>
    /// Creates an InputStreamReader that uses the named charset.
    /// </summary>
    /// <param name="istream">An InputStream</param>
    /// <param name="charset">The name of a supported Charset</param>
    ///
    /// @exception  UnsupportedEncodingException
    ///             If the named charset is not supported
    public InputStreamReader( InputStream istream, string charset ) : base()
    {
    }
}