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

namespace LibGDXSharp.G2D;

/// <summary>
/// Renders bitmap fonts using distance field textures, see the
/// <a href="https://github.com/libgdx/libgdx/wiki/Distance-field-fonts">
/// Distance Field Fonts wiki article</a> for usage. Initialize
/// the SpriteBatch with the <see cref="CreateDistanceFieldShader()"/> shader.
/// <para>
/// Attention: The batch is flushed before and after each string is rendered.
/// </para>
/// </summary>
public class DistanceFieldFont : BitmapFont
{
    private float _distanceFieldSmoothing;

    public DistanceFieldFont( BitmapFontData data, List< TextureRegion > pageRegions, bool integer )
        : base( data, pageRegions, integer )
    {
    }

    public DistanceFieldFont( BitmapFontData data, TextureRegion region, bool integer )
        : base( data, region, integer )
    {
    }

    public DistanceFieldFont( FileInfo fontFile, bool flip )
        : base( fontFile, flip )
    {
    }

    public DistanceFieldFont( FileInfo fontFile, FileInfo imageFile, bool flip, bool integer )
        : base( fontFile, imageFile, flip, integer )
    {
    }

    public DistanceFieldFont( FileInfo fontFile, FileInfo imageFile, bool flip )
        : base( fontFile, imageFile, flip )
    {
    }

    public DistanceFieldFont( FileInfo fontFile, TextureRegion region, bool flip )
        : base( fontFile, region, flip )
    {
    }

    public DistanceFieldFont( FileInfo fontFile, TextureRegion region )
        : base( fontFile, region )
    {
    }

    public DistanceFieldFont( FileInfo fontFile )
        : base( fontFile )
    {
    }

    protected override void Load( BitmapFontData data )
    {
        base.Load( data );

        // Distance field font rendering requires font texture to be filtered linear.
        List< TextureRegion > regions = GetRegions();

        foreach ( TextureRegion region in regions )
        {
            region.Texture.SetFilter( TextureFilter.Linear, TextureFilter.Linear );
        }
    }

    public new BitmapFontCache NewFontCache()
    {
        return new DistanceFieldFontCache( this, UseIntegerPositions );
    }

    /// <summary>
    /// Returns the distance field smoothing factor for this font.
    /// </summary>
    public float GetDistanceFieldSmoothing()
    {
        return _distanceFieldSmoothing;
    }

    /// <summary>
    /// </summary>
    /// <param name="distanceFieldSmoothing">
    /// Set the distance field smoothing factor for this font. SpriteBatch needs
    /// to have this shader set for rendering distance field fonts.
    /// </param>
    public void SetDistanceFieldSmoothing( float distanceFieldSmoothing )
    {
        this._distanceFieldSmoothing = distanceFieldSmoothing;
    }

    /// <summary>
    /// Returns a new instance of the distance field shader, see
    /// https://github.com/libgdx/libgdx/wiki/Distance-field-fonts if the u_smoothing
    /// uniform > 0.0. Otherwise the same code as the default SpriteBatch shader is used.
    /// </summary>
    public ShaderProgram CreateDistanceFieldShader()
    {
        var vertexShader = "attribute vec4 "
                           + ShaderProgram.POSITION_ATTRIBUTE
                           + ";\n" //
                           + "attribute vec4 "
                           + ShaderProgram.COLOR_ATTRIBUTE
                           + ";\n" //
                           + "attribute vec2 "
                           + ShaderProgram.TEXCOORD_ATTRIBUTE
                           + "0;\n"                        //
                           + "uniform mat4 u_projTrans;\n" //
                           + "varying vec4 v_color;\n"     //
                           + "varying vec2 v_texCoords;\n" //
                           + "\n"                          //
                           + "void main() {\n"             //
                           + "	v_color = "
                           + ShaderProgram.COLOR_ATTRIBUTE
                           + ";\n"                                       //
                           + "	v_color.a = v_color.a * (255.0/254.0);\n" //
                           + "	v_texCoords = "
                           + ShaderProgram.TEXCOORD_ATTRIBUTE
                           + "0;\n" //
                           + "	gl_Position =  u_projTrans * "
                           + ShaderProgram.POSITION_ATTRIBUTE
                           + ";\n" //
                           + "}\n";

        var fragmentShader = "#ifdef GL_ES\n"                                                              //
                             + "	precision mediump float;\n"                                               //
                             + "	precision mediump int;\n"                                                 //
                             + "#endif\n"                                                                  //
                             + "\n"                                                                        //
                             + "uniform sampler2D u_texture;\n"                                            //
                             + "uniform float u_smoothing;\n"                                              //
                             + "varying vec4 v_color;\n"                                                   //
                             + "varying vec2 v_texCoords;\n"                                               //
                             + "\n"                                                                        //
                             + "void main() {\n"                                                           //
                             + "	if (u_smoothing > 0.0) {\n"                                               //
                             + "		float smoothing = 0.25 / u_smoothing;\n"                                 //
                             + "		float distance = texture2D(u_texture, v_texCoords).a;\n"                 //
                             + "		float alpha = smoothstep(0.5 - smoothing, 0.5 + smoothing, distance);\n" //
                             + "		gl_FragColor = vec4(v_color.rgb, alpha * v_color.a);\n"                  //
                             + "	} else {\n"                                                               //
                             + "		gl_FragColor = v_color * texture2D(u_texture, v_texCoords);\n"           //
                             + "	}\n"                                                                      //
                             + "}\n";

        var shader = new ShaderProgram( vertexShader, fragmentShader );

        if ( !shader.IsCompiled )
        {
            throw new ArgumentException( $"Error compiling distance field shader: {shader.Log}" );
        }

        return shader;
    }

    /// <summary>
    /// Provides a font cache that uses distance field shader for rendering fonts.
    /// Attention: breaks batching because uniform is needed for smoothing factor,
    /// so a flush is performed before and after every font rendering.
    /// </summary>
    private sealed class DistanceFieldFontCache : BitmapFontCache
    {
        public DistanceFieldFontCache( DistanceFieldFont font )
            : base( font, font.UseIntegerPositions )
        {
        }

        public DistanceFieldFontCache( DistanceFieldFont font, bool integer )
            : base( font, integer )
        {
        }

        private float GetSmoothingFactor()
        {
            var font = ( DistanceFieldFont )base.Font;

            return font.GetDistanceFieldSmoothing() * font.GetScaleX();
        }

        private void SetSmoothingUniform( IBatch spriteBatch, float smoothing )
        {
            spriteBatch.Flush();
            spriteBatch.Shader?.SetUniformf( "u_smoothing", smoothing );
        }

        public override void Draw( IBatch spriteBatch )
        {
            SetSmoothingUniform( spriteBatch, GetSmoothingFactor() );
            base.Draw( spriteBatch );
            SetSmoothingUniform( spriteBatch, 0 );
        }

        protected override void Draw( IBatch spriteBatch, int start, int end )
        {
            SetSmoothingUniform( spriteBatch, GetSmoothingFactor() );
            base.Draw( spriteBatch, start, end );
            SetSmoothingUniform( spriteBatch, 0 );
        }
    }
}