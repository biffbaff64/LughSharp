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

namespace LibGDXSharp.Backends.Desktop;

public class GLGraphics : AbstractGraphics, IDisposable
{
    public GLWindow? Window { get; set; }

    private IGL20? _gl20;
    private IGL30? _gl30;

    public GLGraphics( GLWindow window )
    {
        this.Window = window;

        if ( window.GetConfig().useGL30 )
        {
            this._gl30 = new GL30();
            this._gl20 = this._gl30;
        }
        else
        {
            this._gl20 = new GL20();
            this._gl30 = null;
        }

        UpdateFramebufferInfo();
        InitiateGL();

        Glfw.GetApi().SetFramebufferSizeCallback( window.getWindowHandle(), resizeCallback );
    }

    public class GLDisplayMode
    {
    }

    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            this._gl20 = null;
            this._gl30 = null;
        }
    }

    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }
}