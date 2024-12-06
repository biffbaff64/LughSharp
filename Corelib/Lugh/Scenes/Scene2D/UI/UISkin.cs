// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace Corelib.Lugh.Scenes.Scene2D.UI;

[PublicAPI]
public class UISkin
{
    // ========================================================================

    [PublicAPI]
    public class RootObject
    {
        [JsonPropertyName( "BitmapFont" )]
        public BitmapFont? BitmapFont { get; set; }

        [JsonPropertyName( "Color" )]
        public UIColor? Color { get; set; }

        [JsonPropertyName( "TintedDrawable" )]
        public TintedDrawable? TintedDrawable { get; set; }

        [JsonPropertyName( "" )]
        public ButtonStyle? ButtonStyle { get; set; }

        [JsonPropertyName( "" )]
        public TextButtonStyle? TextButtonStyle { get; set; }

        [JsonPropertyName( "" )]
        public ScrollPaneStyle? ScrollPaneStyle { get; set; }

        [JsonPropertyName( "" )]
        public SelectBoxStyle? SelectBoxStyle { get; set; }

        [JsonPropertyName( "" )]
        public SplitPaneStyle? SplitPaneStyle { get; set; }

        [JsonPropertyName( "" )]
        public WindowStyle? WindowStyle { get; set; }

        [JsonPropertyName( "" )]
        public ProgressBarStyle? ProgressBarStyle { get; set; }

        [JsonPropertyName( "" )]
        public SliderStyle? SliderStyle { get; set; }

        [JsonPropertyName( "" )]
        public LabelStyle? LabelStyle { get; set; }

        [JsonPropertyName( "" )]
        public TextFieldStyle? TextFieldStyle { get; set; }

        [JsonPropertyName( "" )]
        public CheckBoxStyle? CheckBoxStyle { get; set; }

        [JsonPropertyName( "" )]
        public ListStyle? ListStyle { get; set; }

        [JsonPropertyName( "" )]
        public TouchpadStyle? TouchpadStyle { get; set; }

        [JsonPropertyName( "" )]
        public TreeStyle? TreeStyle { get; set; }

        [JsonPropertyName( "" )]
        public TextTooltipStyle? TextTooltipStyle { get; set; }
    }

    [PublicAPI]
    public class BitmapFont
    {
        [JsonPropertyName( "Default-font" )]
        public UISkinFile? DefaultFont { get; set; }
    }

    [PublicAPI]
    public class UISkinFile
    {
        [JsonPropertyName( "file" )]
        public string? File { get; set; }
    }

    [PublicAPI]
    public class UIColor
    {
        [JsonPropertyName( "green" )]
        public UISkinColor? Green { get; set; }

        [JsonPropertyName( "white" )]
        public UISkinColor? White { get; set; }

        [JsonPropertyName( "red" )]
        public UISkinColor? Red { get; set; }

        [JsonPropertyName( "black" )]
        public UISkinColor? Black { get; set; }

        [JsonPropertyName( "gray" )]
        public UISkinColor? Gray { get; set; }
    }

    [PublicAPI]
    public class TintedDrawable
    {
        [JsonPropertyName( "dialogDim" )]
        public DialogDim? DialogDim { get; set; }
    }

    [PublicAPI]
    public class DialogDim
    {
        [JsonPropertyName( "name" )]
        public string? Name { get; set; }

        [JsonPropertyName( "color" )]
        public UISkinColor? Color { get; set; }
    }

    [PublicAPI]
    public class UISkinColor
    {
        [JsonPropertyName( "a" )]
        public float A { get; set; }

        [JsonPropertyName( "b" )]
        public float B { get; set; }

        [JsonPropertyName( "g" )]
        public float G { get; set; }

        [JsonPropertyName( "r" )]
        public float R { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class ButtonStyle
    {
        [JsonPropertyName( "Default" )]
        public ButtonStyleDefault? Default { get; set; }

        [JsonPropertyName( "toggle" )]
        public Toggle? Toggle { get; set; }
    }

    [PublicAPI]
    public class ButtonStyleDefault
    {
        [JsonPropertyName( "down" )]
        public string? Down { get; set; }

        [JsonPropertyName( "up" )]
        public string? Up { get; set; }

        [JsonPropertyName( "disabled" )]
        public string? Disabled { get; set; }
    }

    [PublicAPI]
    public class Toggle
    {
        [JsonPropertyName( "parent" )]
        public string? Parent { get; set; }

        [JsonPropertyName( "checked" )]
        public string? Checked { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class TextButtonStyle
    {
        [JsonPropertyName( "" )]
        public TextButtonStyleDefault? Default { get; set; }

        [JsonPropertyName( "" )]
        public Toggle1? Toggle1 { get; set; }
    }

    [PublicAPI]
    public class TextButtonStyleDefault
    {
        [JsonPropertyName( "" )]
        public string? Parent { get; set; }

        [JsonPropertyName( "" )]
        public string? Font { get; set; }

        [JsonPropertyName( "" )]
        public string? FontColor { get; set; }

        [JsonPropertyName( "" )]
        public string? DisabledFontColor { get; set; }
    }

    [PublicAPI]
    public class Toggle1
    {
        [JsonPropertyName( "parent" )]
        public string? Parent { get; set; }

        [JsonPropertyName( "checked" )]
        public string? Checked { get; set; }

        [JsonPropertyName( "downFontColor" )]
        public string? DownFontColor { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class ScrollPaneStyle
    {
        [JsonPropertyName( "" )]
        public Default2? Default2 { get; set; }
    }

    [PublicAPI]
    public class Default2
    {
        [JsonPropertyName( "" )]
        public string? VScroll { get; set; }

        [JsonPropertyName( "" )]
        public string? HScrollKnob { get; set; }

        [JsonPropertyName( "" )]
        public string? Background { get; set; }

        [JsonPropertyName( "" )]
        public string? HScroll { get; set; }

        [JsonPropertyName( "" )]
        public string? VScrollKnob { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class SelectBoxStyle
    {
        [JsonPropertyName( "" )]
        public Default3? Default3 { get; set; }
    }

    [PublicAPI]
    public class Default3
    {
        [JsonPropertyName( "" )]
        public string? Font { get; set; }

        [JsonPropertyName( "" )]
        public string? FontColor { get; set; }

        [JsonPropertyName( "" )]
        public string? Background { get; set; }

        [JsonPropertyName( "" )]
        public string? ScrollStyle { get; set; }

        [JsonPropertyName( "" )]
        public ListStyle1? ListStyle1 { get; set; }
    }

    [PublicAPI]
    public class ListStyle1
    {
        [JsonPropertyName( "" )]
        public string? Font { get; set; }

        [JsonPropertyName( "" )]
        public string? Selection { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class SplitPaneStyle
    {
        [JsonPropertyName( "" )]
        public DefaultVertical? Default_Vertical { get; set; }

        [JsonPropertyName( "" )]
        public DefaultHorizontal? Default_Horizontal { get; set; }
    }

    [PublicAPI]
    public class DefaultVertical
    {
        [JsonPropertyName( "" )]
        public string? Handle { get; set; }
    }

    [PublicAPI]
    public class DefaultHorizontal
    {
        [JsonPropertyName( "" )]
        public string? Handle { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class WindowStyle
    {
        [JsonPropertyName( "" )]
        public Default4? Default4 { get; set; }

        [JsonPropertyName( "" )]
        public Dialog? Dialog { get; set; }
    }

    [PublicAPI]
    public class Default4
    {
        [JsonPropertyName( "" )]
        public string? TitleFont { get; set; }

        [JsonPropertyName( "" )]
        public string? Background { get; set; }

        [JsonPropertyName( "" )]
        public string? TitleFontColor { get; set; }
    }

    [PublicAPI]
    public class Dialog
    {
        [JsonPropertyName( "" )]
        public string? Parent { get; set; }

        [JsonPropertyName( "" )]
        public string? StageBackground { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class ProgressBarStyle
    {
        [JsonPropertyName( "" )]
        public DefaultHorizontal1? Default_Horizontal { get; set; }

        [JsonPropertyName( "" )]
        public DefaultVertical1? Default_Vertical { get; set; }
    }

    [PublicAPI]
    public class DefaultHorizontal1
    {
        [JsonPropertyName( "" )]
        public string? Background { get; set; }

        [JsonPropertyName( "" )]
        public string? Knob { get; set; }
    }

    [PublicAPI]
    public class DefaultVertical1
    {
        [JsonPropertyName( "" )]
        public string? Background { get; set; }

        [JsonPropertyName( "" )]
        public string? Knob { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class SliderStyle
    {
        [JsonPropertyName( "" )]
        public DefaultHorizontal2? Default_Horizontal { get; set; }

        [JsonPropertyName( "" )]
        public DefaultVertical2? Default_Vertical { get; set; }
    }

    [PublicAPI]
    public class DefaultHorizontal2
    {
        [JsonPropertyName( "" )]
        public string? Parent { get; set; }
    }

    [PublicAPI]
    public class DefaultVertical2
    {
        [JsonPropertyName( "" )]
        public string? Parent { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class LabelStyle
    {
        [JsonPropertyName( "" )]
        public Default5? Default5 { get; set; }
    }

    [PublicAPI]
    public class Default5
    {
        [JsonPropertyName( "" )]
        public string? Font { get; set; }

        [JsonPropertyName( "" )]
        public string? FontColor { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class TextFieldStyle
    {
        [JsonPropertyName( "" )]
        public Default6? Default6 { get; set; }
    }

    [PublicAPI]
    public class Default6
    {
        [JsonPropertyName( "" )]
        public string? Selection { get; set; }

        [JsonPropertyName( "" )]
        public string? Background { get; set; }

        [JsonPropertyName( "" )]
        public string? Font { get; set; }

        [JsonPropertyName( "" )]
        public string? FontColor { get; set; }

        [JsonPropertyName( "" )]
        public string? Cursor { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class CheckBoxStyle
    {
        [JsonPropertyName( "" )]
        public Default7? Default7 { get; set; }
    }

    [PublicAPI]
    public class Default7
    {
        [JsonPropertyName( "" )]
        public string? CheckboxOn { get; set; }

        [JsonPropertyName( "" )]
        public string? CheckboxOff { get; set; }

        [JsonPropertyName( "" )]
        public string? Font { get; set; }

        [JsonPropertyName( "" )]
        public string? FontColor { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class ListStyle
    {
        [JsonPropertyName( "" )]
        public ListStyleDefault? Default { get; set; }
    }

    [PublicAPI]
    public class ListStyleDefault
    {
        [JsonPropertyName( "" )]
        public string? FontColorUnselected { get; set; }

        [JsonPropertyName( "" )]
        public string? Selection { get; set; }

        [JsonPropertyName( "" )]
        public string? FontColorSelected { get; set; }

        [JsonPropertyName( "" )]
        public string? Font { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class TouchpadStyle
    {
        [JsonPropertyName( "Default" )]
        public TouchpadStyleDefault? Default { get; set; }
    }

    [PublicAPI]
    public class TouchpadStyleDefault
    {
        [JsonPropertyName( "background" )]
        public string? Background { get; set; }

        [JsonPropertyName( "knob" )]
        public string? Knob { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class TreeStyle
    {
        [JsonPropertyName( "Default" )]
        public TreeStyleDefault? Default { get; set; }
    }

    [PublicAPI]
    public class TreeStyleDefault
    {
        [JsonPropertyName( "minus" )]
        public string? Minus { get; set; }

        [JsonPropertyName( "plus" )]
        public string? Plus { get; set; }

        [JsonPropertyName( "selection" )]
        public string? Selection { get; set; }
    }

    // ========================================================================

    [PublicAPI]
    public class TextTooltipStyle
    {
        [JsonPropertyName( "Default" )]
        public TtsDefault? Default { get; set; }
    }

    [PublicAPI]
    public class TtsDefault
    {
        [JsonPropertyName( "label" )]
        public Label? Label { get; set; }

        [JsonPropertyName( "background" )]
        public string? Background { get; set; }

        [JsonPropertyName( "wrapWidth" )]
        public int WrapWidth { get; set; }
    }

    [PublicAPI]
    public class Label
    {
        [JsonPropertyName( "font" )]
        public string? Font { get; set; }

        [JsonPropertyName( "fontColor" )]
        public string? FontColor { get; set; }
    }
}
