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

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A table that can be dragged and act as a modal window. The top padding is
/// used as the window's title height.
/// <p>
/// The preferred size of a window is the preferred size of the title text and
/// the children as laid out by the table. After adding children to the window,
/// it can be convenient to call {@link #pack()} to size the window to the size
/// of the children.
/// </p>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class Window : Table
{
    public bool  DrawTitleTable  { get; set; }
    public Label TitleLabel      { get; set; }
    public bool  IsMovable       { get; set; } = true;
    public bool  IsModal         { get; set; }
    public bool  IsResizable     { get; set; }
    public bool  Dragging        { get; set; }
    public int   ResizeBorder    { get; set; } = 8;
    public bool  KeepWithinStage { get; set; } = true;

    private readonly static Vector2 tmpPosition = new();
    private readonly static Vector2 tmpSize     = new();

    private const int Move = 1 << 5;

    private readonly WindowStyle _style;
    private readonly Table       _titleTable;

    protected int edge;

    public Window( String title, Skin skin )
        : this( title, skin.Get< WindowStyle >() )
    {
        Skin = skin;
    }

    public Window( string title, Skin skin, string styleName )
        : this( title, ( WindowStyle )skin.Get< WindowStyle >( styleName ) )
    {
        Skin = skin;
    }

    public Window( string title, WindowStyle style )
    {
        ArgumentNullException.ThrowIfNull( title );
        ArgumentNullException.ThrowIfNull( style );

        Touchable = Touchable.Enabled;
        Clip      = true;

        TitleLabel = new Label( title, new Label.LabelStyle( style.TitleFont!, style.TitleFontColor! ) );
        TitleLabel.SetEllipsis( true );

        _titleTable = new TitleTableClass( this );
        _titleTable.Add( TitleLabel ).SetExpandX().SetFillX().SetMinWidth( 0 );
        AddActor( _titleTable );

        _style = default!;
        Style  = style;
        SetSize( 150, 150 );

        AddCaptureListener( new WindowCaptureListener( this ) );
        AddListener( new WindowInputListener( this ) );

    }

    public WindowStyle Style
    {
        get => _style;
        init
        {
            ArgumentNullException.ThrowIfNull( value );

            this._style = value;

            SetBackground( _style.Background );
            TitleLabel.Style = new Label.LabelStyle( _style.TitleFont!, _style.TitleFontColor! );
            InvalidateHierarchy();
        }
    }

    public void DoKeepWithinStage()
    {
        if ( !KeepWithinStage ) return;

        if ( this.Stage == null ) return;

        if ( this.Stage.Camera is OrthographicCamera orthographicCamera )
        {
            var parentWidth  = this.Stage.StageWidth;
            var parentHeight = this.Stage.StageHeight;

            if ( ( GetX( Align.Right ) - this.Stage.Camera.Position.X )
                 > ( parentWidth / 2 / orthographicCamera.Zoom ) )
            {
                SetPosition
                    (
                     this.Stage.Camera.Position.X + ( parentWidth / 2 / orthographicCamera.Zoom ),
                     GetY( Align.Right ),
                     Align.Right
                    );
            }

            if ( ( GetX( Align.Left ) - this.Stage.Camera.Position.X )
                 < ( -parentWidth / 2 / orthographicCamera.Zoom ) )
            {
                SetPosition
                    (
                     this.Stage.Camera.Position.X - ( parentWidth / 2 / orthographicCamera.Zoom ),
                     GetY( Align.Left ),
                     Align.Left
                    );
            }

            if ( ( GetY( Align.Top ) - this.Stage.Camera.Position.Y ) > ( parentHeight / 2 / orthographicCamera.Zoom ) )
            {
                SetPosition
                    (
                     GetX( Align.Top ),
                     this.Stage.Camera.Position.Y + ( parentHeight / 2 / orthographicCamera.Zoom ),
                     Align.Top
                    );
            }

            if ( ( GetY( Align.Bottom ) - this.Stage.Camera.Position.Y )
                 < ( -parentHeight / 2 / orthographicCamera.Zoom ) )
            {
                SetPosition
                    (
                     GetX( Align.Bottom ),
                     this.Stage.Camera.Position.Y - ( parentHeight / 2 / orthographicCamera.Zoom ),
                     Align.Bottom
                    );
            }
        }
        else if ( Parent == this.Stage.Root )
        {
            var parentWidth  = this.Stage.StageWidth;
            var parentHeight = this.Stage.StageHeight;

            if ( X < 0 ) X                   = 0;
            if ( RightEdge > parentWidth ) X = ( parentWidth - Width );
            if ( Y < 0 ) Y                   = 0;
            if ( TopEdge > parentHeight ) Y  = ( parentHeight - Height );
        }
    }

    public new void Draw( IBatch batch, float parentAlpha )
    {
        if ( this.Stage != null )
        {
            this.Stage.KeyboardFocus ??= this;

            DoKeepWithinStage();

            if ( Style.StageBackground != null )
            {
                StageToLocalCoordinates( tmpPosition.Set( 0, 0 ) );
                StageToLocalCoordinates( tmpSize.Set( this.Stage.StageWidth, this.Stage.StageHeight ) );

                DrawStageBackground
                    (
                     batch,
                     parentAlpha,
                     ( X + tmpPosition.X ),
                     ( Y + tmpPosition.Y ),
                     ( X + tmpSize.X ),
                     ( Y + tmpSize.Y )
                    );
            }
        }

        base.Draw( batch, parentAlpha );
    }

    protected void DrawStageBackground( IBatch batch, float parentAlpha, float x, float y, float width, float height )
    {
        if ( Color != null )
        {
            batch.SetColor( Color.R, Color.G, Color.B, Color.A * parentAlpha );
        }

        Style.StageBackground?.Draw( batch, x, y, width, height );
    }

    protected new void DrawBackground( IBatch batch, float parentAlpha, float x, float y )
    {
        base.DrawBackground( batch, parentAlpha, x, y );

        // Manually draw the title table before clipping is done.

        if ( _titleTable.Color != null )
        {
            _titleTable.Color.A = Color!.A;
        }

        var padTop  = GetPadTop();
        var padLeft = GetPadLeft();

        _titleTable.SetSize( Width - padLeft - GetPadRight(), padTop );
        _titleTable.SetPosition( padLeft, Height - padTop );

        DrawTitleTable = true;

        _titleTable.Draw( batch, parentAlpha );

        // Avoid drawing the title table again in drawChildren.
        DrawTitleTable = false;
    }

    public new Actor? Hit( float x, float y, bool touchable )
    {
        if ( !IsVisible ) return null;

        Actor? hit = base.Hit( x, y, touchable );

        if ( ( hit == null ) && IsModal && ( !touchable || ( Touchable == Touchable.Enabled ) ) )
        {
            return this;
        }

        if ( ( hit == null ) || ( hit == this ) ) return hit;

        if ( ( y <= Height ) && ( y >= ( Height - GetPadTop() ) ) && ( x >= 0 ) && ( x <= Width ) )
        {
            // Hit the title bar, don't use the hit child if it is in the Window's table.
            Actor? current = hit;

            while ( current?.Parent != this )
            {
                current = current?.Parent;
            }

            if ( GetCell( current ) != null ) return this;
        }

        return hit;
    }

    public new float GetPrefWidth()
    {
        return Math.Max( base.GetPrefWidth(), _titleTable.GetPrefWidth() + GetPadLeft() + GetPadRight() );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    internal class TitleTableClass : Table
    {
        private readonly Window _window;

        public TitleTableClass( Window window )
        {
            this._window = window;
        }

        public new void Draw( IBatch batch, float parentAlpha )
        {
            if ( _window.DrawTitleTable ) base.Draw( batch, parentAlpha );
        }
    }

    internal class WindowCaptureListener : InputListener, IEventListener
    {
        private readonly Window _window;

        internal WindowCaptureListener( Window window )
        {
            this._window = window;
        }

        internal new bool TouchDown( InputEvent ev, float x, float y, int pointer, int button )
        {
            _window.ToFront();

            return false;
        }
    }

    internal class WindowInputListener : InputListener
    {
        private float _startX;
        private float _startY;
        private float _lastX;
        private float _lastY;

        private readonly Window _window;

        internal WindowInputListener( Window window )
        {
            this._window = window;
        }

        private void UpdateEdge( float x, float y )
        {
            var border = _window.ResizeBorder / 2f;
            var width  = _window.Width;
            var height = _window.Height;

            var padTop    = _window.GetPadTop();
            var padLeft   = _window.GetPadLeft();
            var padBottom = _window.GetPadBottom();
            var padRight  = _window.GetPadRight();

            var left   = padLeft;
            var right  = width - padRight;
            var bottom = padBottom;

            _window.edge = 0;

            if ( _window.IsResizable
                 && ( x >= ( left - border ) )
                 && ( x <= ( right + border ) )
                 && ( y >= ( bottom - border ) ) )
            {
                if ( x < ( left + border ) ) _window.edge   |= Align.Left;
                if ( x > ( right - border ) ) _window.edge  |= Align.Right;
                if ( y < ( bottom + border ) ) _window.edge |= Align.Bottom;

                if ( _window.edge != 0 ) border += 25;

                if ( x < ( left + border ) ) _window.edge   |= Align.Left;
                if ( x > ( right - border ) ) _window.edge  |= Align.Right;
                if ( y < ( bottom + border ) ) _window.edge |= Align.Bottom;
            }
        }

        public new bool TouchDown( InputEvent ev, float x, float y, int pointer, int button )
        {
            if ( button == 0 )
            {
                UpdateEdge( x, y );

                _window.Dragging = _window.edge != 0;
                _startX          = x;
                _startY          = y;
                _lastX           = x - _window.Width;
                _lastY           = y - _window.Height;
            }

            return ( _window.edge != 0 ) || _window.IsModal;
        }

        public new void TouchUp( InputEvent ev, float x, float y, int pointer, int button )
        {
            _window.Dragging = false;
        }

        public new void TouchDragged( InputEvent ev, float x, float y, int pointer )
        {
            if ( !_window.Dragging ) return;

            var width   = _window.Width;
            var height  = _window.Height;
            var windowX = _window.X;
            var windowY = _window.Y;

            var minWidth  = _window.GetMinWidth();
            var minHeight = _window.GetMinHeight();
//            var maxWidth  = _window.GetMaxWidth();
//            var maxHeight = _window.GetMaxHeight();

            Stage? stage = _window.Stage;

            var clampPosition = _window.KeepWithinStage
                                && ( stage != null )
                                && ( _window.Parent == stage.Root );

            if ( ( _window.edge & Window.Move ) != 0 )
            {
                var amountX = x - _startX;
                var amountY = y - _startY;

                windowX += amountX;
                windowY += amountY;
            }

            if ( ( _window.edge & Align.Left ) != 0 )
            {
                var amountX = x - _startX;

                if ( ( width - amountX ) < minWidth )
                {
                    amountX = -( minWidth - width );
                }
                
                if ( clampPosition && ( ( windowX + amountX ) < 0 ) ) amountX = -windowX;

                width   -= amountX;
                windowX += amountX;
            }

            if ( ( _window.edge & Align.Bottom ) != 0 )
            {
                var amountY = y - _startY;

                if ( ( height - amountY ) < minHeight )
                {
                    amountY = -( minHeight - height );
                }

                if ( clampPosition && ( ( windowY + amountY ) < 0 ) )
                {
                    amountY = -windowY;
                }

                height  -= amountY;
                windowY += amountY;
            }

            if ( ( _window.edge & Align.Right ) != 0 )
            {
                var amountX = x - _lastX - width;

                if ( ( width + amountX ) < minWidth )
                {
                    amountX = minWidth - width;
                }

                if ( clampPosition && ( ( windowX + width + amountX ) > stage?.StageWidth ) )
                {
                    amountX = stage.StageWidth - windowX - width;
                }

                width += amountX;
            }

            if ( ( _window.edge & Align.Top ) != 0 )
            {
                var amountY                                   = y - _lastY - height;

                if ( ( height + amountY ) < minHeight )
                {
                    amountY = minHeight - height;
                }

                if ( clampPosition && ( ( windowY + height + amountY ) > stage?.StageHeight ) )
                {
                    amountY = stage.StageHeight - windowY - height;
                }

                height += amountY;
            }

            _window.SetBounds( ( float )Math.Round( windowX ), ( float )Math.Round( windowY ),
                               ( float )Math.Round( width ), ( float )Math.Round( height ) );
        }

        public new bool MouseMoved( InputEvent ev, float x, float y )
        {
            UpdateEdge( x, y );

            return _window.IsModal;
        }

        public bool Scrolled( InputEvent ev, float x, float y, int amount )
        {
            return _window.IsModal;
        }

        public new bool KeyDown( InputEvent ev, int keycode )
        {
            return _window.IsModal;
        }

        public new bool KeyUp( InputEvent ev, int keycode )
        {
            return _window.IsModal;
        }

        public new bool KeyTyped( InputEvent ev, char character )
        {
            return _window.IsModal;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// The style for a window, see <see cref="Window"/>.
    /// </summary>
    public class WindowStyle
    {
        public IDrawable?  Background      { get; set; }
        public BitmapFont? TitleFont       { get; set; }
        public Color?      TitleFontColor  { get; set; } = new( 1, 1, 1, 1 );
        public IDrawable?  StageBackground { get; set; }

        public WindowStyle()
        {
        }

        public WindowStyle( BitmapFont titleFont, Color titleFontColor, IDrawable? background )
        {
            this.TitleFont  = titleFont;
            this.Background = background;

            this.TitleFontColor?.Set( titleFontColor );
        }

        public WindowStyle( WindowStyle style )
        {
            Background = style.Background;
            TitleFont  = style.TitleFont;

            if ( style.TitleFontColor != null )
            {
                TitleFontColor = new Color( style.TitleFontColor );
            }

            Background = style.Background;
        }
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------