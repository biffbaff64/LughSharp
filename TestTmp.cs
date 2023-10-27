
public void remove( N node )
{
}

public void clearChildren()
{
}

public void invalidate()
{
}

private float plusMinusWidth()
{
}

private void computeSize()
{
}

private void computeSize( Array< N > nodes, float indent, float plusMinusWidth )
{
}

public void layout()
{
}

private float layout( Array< N > nodes, float indent, float y, float plusMinusWidth )
{
}

public void draw( Batch batch, float parentAlpha )
{
}

protected void drawBackground( Batch batch, float parentAlpha )
{
}

private void draw( Batch batch, Array< N > nodes, float indent, float plusMinusWidth )
{
}

protected void drawSelection( N node, Drawable selection, Batch batch, float x, float y, float width, float height )
{
}

protected void drawOver( N node, Drawable over, Batch batch, float x, float y, float width, float height )
{
}

protected void drawExpandIcon( N node, Drawable expandIcon, Batch batch, float x, float y )
{
}

protected void drawIcon( N node, Drawable icon, Batch batch, float x, float y )
{
}

protected Drawable getExpandIcon( N node, float iconX )
{
}

public @Null N getNodeAt( float y )
{
}

private float getNodeAt( Array< N > nodes, float y, float rowY )
{
}

void selectNodes( Array< N > nodes, float low, float high )
{
}

public Selection< N > getSelection()
{
}

public @Null N getSelectedNode()
{
}

public @Null V getSelectedValue()
{
}

public Tree< , >.TreeStyle getStyle()
{
    return style;
}

public Array< N > getRootNodes()
{
}

public Array< N > getNodes()
{
}

public void updateRootNodes()
{
}

public @Null N getOverNode()
{
}

/** @return May be null. */
public @Null V getOverValue()
{
}

public void setOverNode( @Null N overNode )
{
}

public void setPadding( float padding )
{
}

public void setPadding( float left, float right )
{
}

public void setIndentSpacing( float indentSpacing )
{
}

public float getIndentSpacing()
{
}

public void setYSpacing( float ySpacing )
{
}

public float getYSpacing()
{
}

public void setIconSpacing( float left, float right )
{
}

public float getPrefWidth()
{
}

public float getPrefHeight()
{
}

public void findExpandedValues( Array< V > values )
{
}

public void restoreExpandedValues( Array< V > values )
{
}

private static boolean findExpandedValues( Array<? extends Tree< , >.Node< ,, > > nodes, Array values)
{
}

public @Null N findNode( V value )
{
}

private static @Null Node findNode( Array < ? extends Node > nodes, Object value )
{
}

public void collapseAll()
{
}

private static void collapseAll( Array<? extends Node > nodes )
{
}

public void expandAll()
{
}

private static void expandAll( Array<? extends Node > nodes)
{
}

public ClickListener getClickListener()
{
}
