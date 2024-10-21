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

using Corelib.LibCore.Graphics.G3D.Attributes;
using Corelib.LibCore.Graphics.G3D.Env;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Graphics.G3D;

public class Environment : AttributesGroup
{
    /// <summary>
    /// Shadow map used to render shadows
    /// </summary>
    public IShadowMap? ShadowMap { get; set; }

    /// <summary>
    /// Adds one or more Lights, extended from <see cref="BaseLight"/>, to the
    /// attributes list in <see cref="AttributesGroup"/> .
    /// </summary>
    /// <param name="lights"></param>
    /// <returns></returns>
    public Environment Add( params BaseLight[] lights )
    {
        foreach ( var light in lights )
        {
            Add( light );
        }

        return this;
    }

    public Environment Add( List< BaseLight > lights )
    {
        foreach ( var light in lights )
        {
            Add( light );
        }

        return this;
    }

    public Environment Add( BaseLight light )
    {
        if ( light is DirectionalLight directionalLight )
        {
            Add( directionalLight );
        }
        else if ( light is PointLight pointLight )
        {
            Add( pointLight );
        }
        else if ( light is SpotLight spotLight )
        {
            Add( spotLight );
        }
        else
        {
            throw new GdxRuntimeException( "Unknown light type" );
        }

        return this;
    }

    public Environment Add( DirectionalLight light )
    {
        var dirLights = ( DirectionalLightsAttribute? ) Get( DirectionalLightsAttribute.Type );

        if ( dirLights == null )
        {
            Set( dirLights = new DirectionalLightsAttribute() );
        }

        dirLights.Lights.Add( light );

        return this;
    }

    public Environment Add( PointLight light )
    {
        var pointLights = ( PointLightsAttribute? ) Get( PointLightsAttribute.Type );

        if ( pointLights == null )
        {
            Set( pointLights = new PointLightsAttribute() );
        }

        pointLights.Lights.Add( light );

        return this;
    }

    public Environment Add( SpotLight light )
    {
        var spotLights = ( SpotLightsAttribute? ) Get( SpotLightsAttribute.Type );

        if ( spotLights == null )
        {
            Set( spotLights = new SpotLightsAttribute() );
        }

        spotLights.Lights.Add( light );

        return this;
    }

    public Environment Remove( params BaseLight[] lights )
    {
        foreach ( var light in lights )
        {
            Remove( light );
        }

        return this;
    }

    public Environment Remove( List< BaseLight > lights )
    {
        foreach ( var light in lights )
        {
            Remove( light );
        }

        return this;
    }

    public Environment Remove( BaseLight light )
    {
        if ( light is DirectionalLight directionalLight )
        {
            Remove( directionalLight );
        }
        else if ( light is PointLight pointLight )
        {
            Remove( pointLight );
        }
        else if ( light is SpotLight spotLight )
        {
            Remove( spotLight );
        }
        else
        {
            throw new GdxRuntimeException( "Unknown light type" );
        }

        return this;
    }

    public Environment Remove( DirectionalLight light )
    {
        if ( Has( DirectionalLightsAttribute.Type ) )
        {
            var dirLights = ( DirectionalLightsAttribute? ) Get( DirectionalLightsAttribute.Type );

            dirLights?.Lights.Remove( light );

            if ( dirLights?.Lights.Count == 0 )
            {
                Remove( DirectionalLightsAttribute.Type );
            }
        }

        return this;
    }

    public Environment Remove( PointLight light )
    {
        if ( Has( PointLightsAttribute.Type ) )
        {
            var pointLights = ( PointLightsAttribute? ) Get( PointLightsAttribute.Type );

            pointLights?.Lights.Remove( light );

            if ( pointLights?.Lights.Count == 0 )
            {
                Remove( PointLightsAttribute.Type );
            }
        }

        return this;
    }

    public Environment Remove( SpotLight light )
    {
        if ( Has( SpotLightsAttribute.Type ) )
        {
            var spotLights = ( SpotLightsAttribute? ) Get( SpotLightsAttribute.Type );

            spotLights?.Lights.Remove( light );

            if ( spotLights?.Lights.Count == 0 )
            {
                Remove( SpotLightsAttribute.Type );
            }
        }

        return this;
    }
}
