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

using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A tooltip that shows a label.
/// </summary>
public class TextTooltip : Tooltip< Label >
{
    public TextTooltip( string text, Skin skin )
        : this( text, TooltipManager.Instance, skin.Get< TextTooltipStyle >() )
    {
    }

    public TextTooltip( string text, Skin skin, string styleName )
        : this( text, TooltipManager.Instance, skin.Get< TextTooltipStyle >( styleName ) )
    {
    }

    public TextTooltip( string text, TextTooltipStyle style )
        : this( text, TooltipManager.Instance, style )
    {
    }

    public TextTooltip( string text, TooltipManager manager, Skin skin )
        : this( text, manager, skin.Get< TextTooltipStyle >() )
    {
    }

    public TextTooltip( string text, TooltipManager manager, Skin skin, string styleName )
        : this( text, manager, skin.Get< TextTooltipStyle >( styleName ) )
    {
    }

    public TextTooltip( string text, TooltipManager manager, TextTooltipStyle style )
        : base( contents: null, manager )
    {
        var label = new Label( text, style.Label )
        {
            Wrap = true
        };

        Container?.SetActor( label );
        Container?.SetWidths( Math.Min( manager.MaxWidth, label.GlyphLayout.Width ) );

        SetStyle( style );
    }

    public void SetStyle( TextTooltipStyle style )
    {
        ArgumentNullException.ThrowIfNull( style );

        if ( Container == null )
        {
            throw new NullReferenceException( "Container cannot be null" );
        }

        Container.GetActor()!.Style = style.Label;
        Container.SetBackground( style.Background );
        Container.MaxWidth = style.WrapWidth;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    public class TextTooltipStyle
    {
        public Label.LabelStyle Label      { get; set; } = null!;
        public IDrawable        Background { get; set; } = null!;
        public float            WrapWidth  { get; set; }

        public TextTooltipStyle()
        {
        }

        public TextTooltipStyle( Label.LabelStyle label, IDrawable background )
        {
            this.Label      = label;
            this.Background = background;
        }

        public TextTooltipStyle( TextTooltipStyle style )
        {
            Label      = new Label.LabelStyle( style.Label );
            Background = style.Background;
            WrapWidth  = style.WrapWidth;
        }
    }
}
