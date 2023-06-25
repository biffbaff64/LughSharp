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
using System.Runtime.Serialization;

using LibGDXSharp.Core.Utils.Collections.Extensions;
using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections.Extensions;
using LibGDXSharp.Utils.Json;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A skin stores resources for UI widgets to use (texture regions, ninepatches,
/// fonts, colors, etc). Resources are named and can be looked up by name and type.
/// <para>
/// Resources can be described in JSON.
/// </para>
/// <para>
/// Skin provides useful conversions, such as allowing access to regions in the
/// atlas as ninepatches, sprites, drawables, etc. The get* methods return an
/// instance of the object in the skin.
/// </para>
/// <para>
/// The new* methods return a copy of an instance in the skin.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class Skin : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public struct TintedDrawable
    {
        public string Name  { get; set; }
        public Color  Color { get; set; }
    }

    //@formatter:off
    private readonly static Type[] defaultTagClasses =
    {
        typeof( BitmapFont ),                           typeof( Color ),
        typeof( TintedDrawable ),                       typeof( NinePatchDrawable ),
        typeof( SpriteDrawable ),                       typeof( TextureRegionDrawable ),
        typeof( TiledDrawable ),                        typeof( Button.ButtonStyle ),
        typeof( TextButton.TextButtonStyle ),           typeof( CheckBox.CheckBoxStyle ),
        typeof( Label.LabelStyle ),                     typeof( ProgressBar.ProgressBarStyle ),
        typeof( TextField.TextFieldStyle ),             typeof( ImageButton.ImageButtonStyle ),
        typeof( ImageTextButton.ImageTextButtonStyle ), typeof( List.ListStyle ),                       
        typeof( ScrollPane.ScrollPaneStyle ),           typeof( SelectBox.SelectBoxStyle ),
        typeof( Slider.SliderStyle ),                   typeof( SplitPane.SplitPaneStyle ),
        typeof( TextTooltip.TextTooltipStyle ),         typeof( Touchpad.TouchpadStyle ),
        typeof( Tree.TreeStyle ),                       typeof( Window.WindowStyle )
    };
    //@formatter:on

    public static Dictionary< Type, Dictionary< string, object >? > Resources     { get; set; }
    public static Dictionary< string, Type >                        JsonClassTags { get; set; }

    /// <summary>
    /// Returns the <see cref="TextureAtlas"/> passed to this skin constructor, or null.
    /// </summary>
    public TextureAtlas? Atlas { get; set; }

    /// <summary>
    /// The scale used to size drawables created by this skin.
    /// <para>
    /// This can be useful when scaling an entire UI (eg with a stage's viewport)
    /// then using an atlas with images whose resolution matches the UI scale.
    /// The skin can then be scaled the opposite amount so that the larger or smaller
    /// images are drawn at the original size. For example, if the UI is scaled 2x,
    /// the atlas would have images that are twice the size, then the skin's scale
    /// would be set to 0.5. 
    /// </para>
    /// </summary>
    public float Scale { get; set; }

    static Skin()
    {
        Resources     = new Dictionary< Type, Dictionary< string, object >? >();
        JsonClassTags = new Dictionary< string, Type >( defaultTagClasses.Length );

        foreach ( Type c in defaultTagClasses )
        {
            JsonClassTags.Add( c.Name, c );
        }
    }

    /// <summary>
    /// Creates an empty Skin.
    /// </summary>
    public Skin()
    {
    }

    /// <summary>
    /// Creates a skin containing the resources in the specified skin JSON file.
    /// If a file in the same directory with a ".atlas" extension exists, it is
    /// loaded as a <see cref="TextureAtlas"/> and the texture regions added to
    /// the skin. The atlas is automatically disposed when the skin is disposed. 
    /// </summary>
    public Skin( FileInfo skinFile )
    {
        var name      = Path.GetFileNameWithoutExtension( skinFile.Name );
        var atlasFile = new FileInfo( name + ".atlas" );

        if ( atlasFile.Exists )
        {
            Atlas = new TextureAtlas( atlasFile );
            AddRegions( Atlas );
        }

        Load( skinFile );
    }

    /// <summary>
    /// Creates a skin containing the resources in the specified skin JSON file and
    /// the texture regions from the specified atlas.
    /// <para>
    /// The atlas is automatically disposed when the skin is disposed. 
    /// </para>
    /// </summary>
    public Skin( FileInfo skinFile, TextureAtlas atlas )
    {
        this.Atlas = atlas;
        AddRegions( atlas );

        Load( skinFile );
    }

    /// <summary>
    /// Creates a skin containing the texture regions from the specified atlas.
    /// The atlas is automatically disposed when the skin is disposed. 
    /// </summary>
    public Skin( TextureAtlas atlas )
    {
        this.Atlas = atlas;
        AddRegions( atlas );
    }

    /// <summary>
    /// Adds all resources in the specified skin JSON file.
    /// </summary>
    public void Load( FileInfo skinFile )
    {
        try
        {
            GetJsonLoader( skinFile ).FromJson( typeof( Skin ), skinFile );
        }
        catch ( SerializationException ex )
        {
            throw new SerializationException( "Error reading file: " + skinFile, ex );
        }
    }

    /// <summary>
    /// Adds all named texture regions from the atlas. The atlas will not be
    /// automatically disposed when the skin is disposed.
    /// </summary>
    public void AddRegions( TextureAtlas atlas )
    {
        for ( int i = 0, n = atlas.Regions.Count; i < n; i++ )
        {
            var name = atlas.Regions[ i ]?.Name;

            if ( atlas.Regions[ i ]?.Index != -1 )
            {
                name += "_" + atlas.Regions[ i ]?.Index;
            }

            Add( name, atlas.Regions[ i ], typeof( TextureRegion ) );
        }
    }

    public void Add( string name, object resource )
    {
        Add( name, resource, resource.GetType() );
    }

    public void Add( string? name, object? resource, Type type )
    {
        if ( string.ReferenceEquals( name, null ) )
        {
            throw new System.ArgumentException( "name cannot be null." );
        }

        if ( resource == null )
        {
            throw new System.ArgumentException( "resource cannot be null." );
        }

        Dictionary< string, object >? typeResources = Resources.Get( type );

        if ( typeResources == null )
        {
            typeResources = new Dictionary< string, object >
                (
                 ( type == typeof( TextureRegion ) )
                 || ( type == typeof( IDrawable ) )
                 || ( type == typeof( Sprite ) )
                     ? 256
                     : 64
                );

            Resources.Put( type, typeResources );
        }

        typeResources.Put( name, resource );
    }

    public void Remove( string name, Type type )
    {
        if ( string.ReferenceEquals( name, null ) )
        {
            throw new System.ArgumentException( "name cannot be null." );
        }

        Resources.Get( type )?.Remove( name );
    }

    /// <summary>
    /// Returns a resource named "default" for the specified type.
    /// </summary>
    /// <exception cref="GdxRuntimeException">if the resource was not found.</exception>
    public T Get<T>()
    {
        return ( T )Get< T >( "default" );
    }

    /// <summary>
    /// Returns a named resource of the specified type.
    /// </summary>
    /// <exception cref="GdxRuntimeException">if the resource was not found.</exception>
    public object Get<T>( string name )
    {
        ArgumentNullException.ThrowIfNull( name );

        if ( typeof( T ) == typeof( IDrawable ) ) return GetDrawable( name );

        if ( typeof( T ) == typeof( TextureRegion ) ) return GetRegion( name );

        if ( typeof( T ) == typeof( NinePatch ) ) return GetPatch( name );

        if ( typeof( T ) == typeof( Sprite ) ) return GetSprite( name );

        Dictionary< string, object >? typeResources = Resources[ typeof( T ) ];

        if ( typeResources == null )
        {
            throw new GdxRuntimeException( $"No {typeof( T ).FullName} registered with name: {name}" );
        }

        var resource = typeResources[ name ];

        if ( resource == null )
        {
            throw new GdxRuntimeException( $"No {typeof( T ).FullName} registered with name: {name}" );
        }
        
        return resource;
    }

    /// <summary>
    /// Returns a named resource of the specified type.
    /// </summary>
    /// <returns> null if not found. </returns>
    public T? Optional<T>( string name )
    {
        ArgumentNullException.ThrowIfNull( name );

        if ( typeof( T ) == null )
        {
            throw new System.ArgumentException( "type cannot be null." );
        }

        return ( T? )Resources[ typeof( T ) ]?.Get( name );
    }

    public bool Has( string name, Type type ) => Resources[ type ]!.ContainsKey( name );

    /// <summary>
    /// Returns the name to resource mapping for the specified type, or
    /// null if no resources of that type exist.
    /// </summary>
    public Dictionary< string, object >? GetAll( Type type ) => Resources[ type ];

    public Color GetColor( string name ) => ( Color )Get< Color >( name );

    public BitmapFont GetFont( string name ) => ( BitmapFont )Get< BitmapFont >( name );

    /// <summary>
    /// Returns a registered texture region. If no region is found but a
    /// texture exists with the name, a region is created from the texture
    /// and stored in the skin. 
    /// </summary>
    public TextureRegion GetRegion( string name )
    {
        var region = Optional< TextureRegion? >( name );

        if ( region != null ) return region;

        var texture = Optional< Texture >( name );

        if ( texture == null )
        {
            throw new GdxRuntimeException( "No TextureRegion or Texture registered with name: " + name );
        }

        region = new TextureRegion( texture );

        Add( name, region, typeof( TextureRegion ) );

        return region;
    }

    /// <returns> an array with the <see cref="TextureRegion"/> that have an index != -1, or null if none are found. </returns>
    public List< TextureRegion >? GetRegions( string regionName )
    {
        var i = 0;

        List< TextureRegion >? regions = null;
        var                    region  = Optional< TextureRegion? >( regionName + "_" + ( i++ ) );

        if ( region != null )
        {
            regions = new List< TextureRegion >();

            while ( region != null )
            {
                regions.Add( region );
                region = Optional< TextureRegion? >( regionName + "_" + ( i++ ) );
            }
        }

        return regions;
    }

    /// <summary>
    /// Returns a registered tiled drawable. If no tiled drawable is found but a
    /// region exists with the name, a tiled drawable is created from the region
    /// and stored in the skin. 
    /// </summary>
    public TiledDrawable GetTiledDrawable( string name )
    {
        var tiled = Optional< TiledDrawable? >( name );

        if ( tiled != null )
        {
            return tiled;
        }

        tiled = new TiledDrawable( GetRegion( name ) )
        {
            Name = name
        };

        if ( Scale is not 1.0f )
        {
            tiled = ( TiledDrawable )ScaleDrawable( tiled );

            tiled.Scale = this.Scale;
        }

        Add( name, tiled, typeof( TiledDrawable ) );

        return tiled;
    }

    /// <summary>
    /// Returns a registered ninepatch. If no ninepatch is found but a region exists with
    /// the name, a ninepatch is created from the region and stored in the skin. If the
    /// region is an <see cref="AtlasRegion"/> then its split <see cref="AtlasRegion.values"/>
    /// are used, otherwise the ninepatch will have the region as the center patch. 
    /// </summary>
    public NinePatch GetPatch( string name )
    {
        var patch = Optional< NinePatch? >( name );

        if ( patch != null )
        {
            return patch;
        }

        try
        {
            TextureRegion region = GetRegion( name );

            if ( region is AtlasRegion atlasRegion )
            {
                var splits = atlasRegion.FindValue( "split" );

                if ( splits != null )
                {
                    patch = new NinePatch( atlasRegion, splits[ 0 ], splits[ 1 ], splits[ 2 ], splits[ 3 ] );

                    var pads = atlasRegion.FindValue( "pad" );

                    if ( pads != null )
                    {
                        patch.SetPadding( pads[ 0 ], pads[ 1 ], pads[ 2 ], pads[ 3 ] );
                    }
                }
            }

            patch ??= new NinePatch( region );

            if ( Scale is not 1.0f )
            {
                patch.Scale( Scale, Scale );
            }

            Add( name, patch, typeof( NinePatch ) );

            return patch;
        }
        catch ( GdxRuntimeException )
        {
            throw new GdxRuntimeException( "No NinePatch, TextureRegion, or Texture registered with name: " + name );
        }
    }

    /// <summary>
    /// Returns a registered sprite. If no sprite is found but a region exists
    /// with the name, a sprite is created from the region and stored in the skin.
    /// If the region is an <see cref="AtlasRegion"/> then an <see cref="AtlasSprite"/>
    /// is used if the region has been whitespace stripped or packed rotated 90 degrees. 
    /// </summary>
    public Sprite GetSprite( string name )
    {
        Sprite? sprite;

        if ( ( sprite = Optional< Sprite >( name ) ) != null )
        {
            return sprite;
        }

        try
        {
            TextureRegion textureRegion = GetRegion( name );

            if ( textureRegion is AtlasRegion region )
            {
                if ( region.Rotate
                     || ( region.PackedWidth != region.OriginalWidth )
                     || ( region.PackedHeight != region.OriginalHeight ) )
                {
                    sprite = new AtlasSprite( region );
                }
            }

            sprite ??= new Sprite( textureRegion );

            if ( this.Scale is not 1.0f )
            {
                sprite.SetSize( sprite.Width * Scale, sprite.Height * Scale );
            }

            Add( name, sprite, typeof( Sprite ) );

            return sprite;
        }
        catch ( GdxRuntimeException )
        {
            throw new GdxRuntimeException( "No NinePatch, TextureRegion, or Texture registered with name: " + name );
        }
    }

    /// <summary>
    /// Returns a registered drawable. If no drawable is found but a region, ninepatch,
    /// or sprite exists with the name, then the appropriate drawable is created and
    /// stored in the skin. 
    /// </summary>
    public IDrawable GetDrawable( string name )
    {
        var drawable = Optional< IDrawable >( name );

        if ( drawable != null )
        {
            return drawable;
        }

        // Use texture or texture region. If it has splits, use ninepatch.
        // If it has rotation or whitespace stripping, use sprite.
        try
        {
            TextureRegion textureRegion = GetRegion( name );

            if ( textureRegion is AtlasRegion region )
            {
                if ( region.FindValue( "split" ) != null )
                {
                    drawable = new NinePatchDrawable( GetPatch( name ) );
                }
                else if ( region.Rotate
                          || ( region.PackedWidth != region.OriginalWidth )
                          || ( region.PackedHeight != region.OriginalHeight ) )
                {
                    drawable = new SpriteDrawable( GetSprite( name ) );
                }
            }

            if ( drawable == null )
            {
                drawable = new TextureRegionDrawable( textureRegion );

                if ( Scale is not 1.0f )
                {
                    ScaleDrawable( drawable );
                }
            }
        }
        catch ( GdxRuntimeException )
        {
        }

        // Check for explicit registration of ninepatch, sprite, or tiled drawable.
        if ( drawable == null )
        {
            var patch = Optional< NinePatch >( name );

            if ( patch != null )
            {
                drawable = new NinePatchDrawable( patch );
            }
            else
            {
                var sprite = Optional< Sprite >( name );

                if ( sprite != null )
                {
                    drawable = new SpriteDrawable( sprite );
                }
                else
                {
                    throw new GdxRuntimeException
                        ( "No IDrawable, NinePatch, TextureRegion, Texture, or Sprite registered with name: " + name );
                }
            }
        }

        if ( drawable is BaseDrawable baseDrawable )
        {
            baseDrawable.Name = name;
        }

        Add( name, drawable, typeof( IDrawable ) );

        return drawable;
    }

    /// <summary>
    /// Returns the name of the specified style object, or null if it is not in the skin.
    /// This compares potentially every style object in the skin of the same type as the
    /// specified style, which may be a somewhat expensive operation. 
    /// </summary>
    public string? Find( object resource )
    {
        if ( resource == null )
        {
            throw new System.ArgumentException( "style cannot be null." );
        }

        Dictionary< string, object >? typeResources = Resources[ resource.GetType() ];

        return typeResources?.FindKey( resource );
    }

    /// <summary>
    /// Returns a copy of a drawable found in the skin via <see cref="GetDrawable(String)"/>.
    /// </summary>
    public IDrawable NewDrawable( string name )
    {
        return NewDrawable( GetDrawable( name ) );
    }

    /// <summary>
    /// Returns a tinted copy of a drawable found in the skin via <see cref="GetDrawable(String)"/>.
    /// </summary>
    public IDrawable NewDrawable( string name, float r, float g, float b, float a )
    {
        return NewDrawable( GetDrawable( name ), new Color( r, g, b, a ) );
    }

    /// <summary>
    /// Returns a tinted copy of a drawable found in the skin via <see cref="GetDrawable(String)"/>.
    /// </summary>
    public IDrawable NewDrawable( string name, Color tint )
    {
        return NewDrawable( GetDrawable( name ), tint );
    }

    /// <summary>
    /// Returns a copy of the specified drawable.
    /// </summary>
    public IDrawable NewDrawable( IDrawable drawable )
    {
        if ( drawable is TiledDrawable tiledDrawable )
        {
            return new TiledDrawable( tiledDrawable );
        }

        if ( drawable is TextureRegionDrawable regionDrawable )
        {
            return new TextureRegionDrawable( regionDrawable );
        }

        if ( drawable is NinePatchDrawable patchDrawable )
        {
            return new NinePatchDrawable( patchDrawable );
        }

        if ( drawable is SpriteDrawable spriteDrawable )
        {
            return new SpriteDrawable( spriteDrawable );
        }

        throw new GdxRuntimeException( "Unable to copy, unknown drawable type: " + drawable.GetType() );
    }

    /// <summary>
    /// Returns a tinted copy of a drawable found in the skin via <see cref="GetDrawable(String)"/>.
    /// </summary>
    public IDrawable NewDrawable( IDrawable drawable, float r, float g, float b, float a )
    {
        return NewDrawable( drawable, new Color( r, g, b, a ) );
    }

    /// <summary>
    /// Returns a tinted copy of a drawable found in the skin via <see cref="GetDrawable(String)"/>.
    /// </summary>
    public IDrawable NewDrawable( IDrawable drawable, Color tint )
    {
        IDrawable newDrawable;

        if ( drawable is TextureRegionDrawable )
        {
            newDrawable = ( ( TextureRegionDrawable )drawable ).Tint( tint );
        }
        else if ( drawable is NinePatchDrawable )
        {
            newDrawable = ( ( NinePatchDrawable )drawable ).Tint( tint );
        }
        else if ( drawable is SpriteDrawable )
        {
            newDrawable = ( ( SpriteDrawable )drawable ).Tint( tint );
        }
        else
        {
            throw new GdxRuntimeException( "Unable to copy, unknown drawable type: " + drawable.GetType() );
        }

        if ( newDrawable is BaseDrawable )
        {
            var named = ( BaseDrawable )newDrawable;

            if ( drawable is BaseDrawable )
            {
                named.Name = ( ( ( BaseDrawable )drawable ).Name + " (" + tint + ")" );
            }
            else
            {
                named.Name = ( " (" + tint + ")" );
            }
        }

        return newDrawable;
    }

    /// <summary>
    /// Scales the drawable's :-
    /// <see cref="IDrawable.LeftWidth"/>,
    /// <see cref="IDrawable.RightWidth"/>,
    /// <see cref="IDrawable.BottomHeight"/>,
    /// <see cref="IDrawable.TopHeight"/>,
    /// <see cref="IDrawable.MinWidth"/>,
    /// <see cref="IDrawable.MinHeight"/>. 
    /// </summary>
    public IDrawable ScaleDrawable( IDrawable drawable )
    {
        drawable.LeftWidth    = ( drawable.LeftWidth * Scale );
        drawable.RightWidth   = ( drawable.RightWidth * Scale );
        drawable.BottomHeight = ( drawable.BottomHeight * Scale );
        drawable.TopHeight    = ( drawable.TopHeight * Scale );
        drawable.MinWidth     = ( drawable.MinWidth * Scale );
        drawable.MinHeight    = ( drawable.MinHeight * Scale );

        return drawable;
    }

    /// <summary>
    /// Sets the style on the actor to disabled or enabled. This is done by appending
    /// "-disabled" to the style name when enabled is false, and removing "-disabled"
    /// from the style name when enabled is true. A method named "GetStyle" is called
    /// the actor via reflection and the name of that style is found in the skin. If
    /// the actor doesn't have a "GetStyle" method or the style was not found in the
    /// skin, no exception is thrown and the actor is left unchanged. 
    /// </summary>
    public void SetEnabled( Actor actor, bool enabled )
    {
        // Get current style.
        System.Reflection.MethodInfo? method = actor.GetType().GetMethod( "GetStyle" );

        if ( method == null )
        {
            return;
        }

        object style;

        try
        {
            style = method.Invoke( actor );
        }
        catch ( Exception )
        {
            return;
        }

        // Determine new style.
        var name = Find( style );

        if ( string.ReferenceEquals( name, null ) )
        {
            return;
        }

        name  = name.Replace( "-disabled", "" ) + ( enabled ? "" : "-disabled" );
        style = Get<>( name, style.GetType() );

        // Set new style.
        method = FindMethod( actor.GetType(), "setStyle" );

        if ( method == null )
        {
            return;
        }

        try
        {
            method.Invoke( actor, style );
        }
        catch ( Exception )
        {
        }
    }

    private static System.Reflection.MethodInfo? FindMethod( Type type, string name )
    {
        return type.GetMethod( name );
    }

    /// <summary>
    /// Disposes the <see cref="TextureAtlas"/> and all <see cref="IDisposable"/> resources in the skin.
    /// </summary>
    public void Dispose()
    {
        Atlas?.Dispose();

        foreach ( Dictionary< string, object >? entry in Resources.Values )
        {
            foreach ( var resource in entry!.Values )
            {
                if ( resource is IDisposable disposable )
                {
                    disposable.Dispose();
                }
            }
        }
    }

    public Json GetJsonLoader( in FileInfo skinFile )
    {
        throw new NotImplementedException();
    }
}