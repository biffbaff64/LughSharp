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

/// <summary>
/// Convenience implementation of <see cref="IGLWindowListener"/>.
/// Derive from this class and only overwrite the methods you are interested in.
/// </summary>
public class DesktopGLWindowAdapter : IGLWindowListener
{
    public virtual void Created( DesktopGLWindow window )
    {
    }

    public virtual void Iconified( bool isIconified )
    {
    }

    public virtual void Maximized( bool isMaximized )
    {
    }

    public virtual void FocusLost()
    {
    }

    public virtual void FocusGained()
    {
    }

    public virtual bool CloseRequested()
    {
        return false;
    }

    public virtual void FilesDropped( string[] files )
    {
    }

    public virtual void RefreshRequested()
    {
    }
}
