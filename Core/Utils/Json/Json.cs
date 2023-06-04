using System.Runtime.Serialization;

namespace LibGDXSharp.Utils.Json;

/// <summary>
/// Reads/writes Java objects to/from JSON, automatically.
/// </summary>
public class Json
{
    private static bool debug = false;

    public bool          IgnoreUnknownFields { get; set; }
    public bool          IgnoreDeprecated    { get; set; }
    public bool          ReadDeprecated      { get; set; }
    public OutputType    OutputType          { get; set; }
    public bool          QuoteLongValues     { get; set; }
    public bool          EnumNames           { get; set; } = true;
    public JsonWriter    Writer              { get; set; }
    public ISerializer DefaultSerializer   { get; set; }
    public string        TypeName            { get; set; } = "class";
    public bool          UsePrototypes       { get; set; } = true;
    public bool          SortFields          { get; set; }

    private readonly Dictionary< Type, Dictionary< string, FieldMetadata > > _typeToFields = new();

    private readonly Dictionary< string, Type >        _tagToClass           = new();
    private readonly Dictionary< Type, string >        _classToTag           = new();
    private readonly Dictionary< Type, ISerializer > _classToSerializer    = new();
    private readonly Dictionary< Type, object[] >      _classToDefaultValues = new();
    private readonly object?[]                         _equals1              = { null };
    private readonly object?[]                         _equals2              = { null };

    public Json()
    {
        OutputType = OutputType.Minimal;
    }

    public Json( OutputType outputType )
    {
        this.OutputType = outputType;
    }

    /// <summary>
    /// Sets a tag to use instead of the fully qualifier class
    /// name. This can make the JSON easier to read.
    /// </summary>
    public void AddClassTag( string tag, Type type )
    {
        TagToClass.put( tag, type );
        ClassToTag.put( type, tag );
    }

    /// <summary>
    /// Returns the class for the specified tag, or null.
    /// </summary>
    public Type? GetClass( string tag )
    {
        return TagToClass.get( tag );
    }

    /// <summary>
    /// Returns the tag for the specified class, or null.
    /// </summary>
    public string? GetTag( Type type )
    {
        return ClassToTag.get( type );
    }

    /// <summary>
    /// Registers a serializer to use for the specified type instead of the
    /// default behavior of serializing all of an objects fields.
    /// </summary>
    public void SetSerializer( Type type, ISerializer serializer )
    {
        ClassToSerializer.put( type, serializer );
    }

    public ISerializer GetSerializer( Type type )
    {
        return ClassToSerializer.get( type );
    }

    /// <summary>
    /// Sets the type of elements in a collection. When the element type is
    /// known, the class for each element in the collection does not need to
    /// be written unless different from the element type.
    /// </summary>
    public void SetElementType( Type type, string fieldName, Type elementType )
    {
        FieldMetadata metadata = GetFields( type ).get( fieldName );

        if ( metadata == null )
            throw new SerializationException( "Field not found: " + fieldName + " (" + type.Name + ")" );

        metadata.elementType = elementType;
    }

    /// <summary>
    /// The specified field will be treated as if it has or does not have
    /// the <tt>Obsolete</tt> annotation.
    /// </summary>
    public void SetDeprecated( Type type, string fieldName, bool deprecated )
    {
        FieldMetadata metadata = GetFields( type ).get( fieldName );

        if ( metadata == null )
            throw new SerializationException( "Field not found: " + fieldName + " (" + type.Name + ")" );

        metadata.deprecated = deprecated;
    }

    private Dictionary< string, FieldMetadata > GetFields( Type type )
    {
        Dictionary< string, FieldMetadata > fields = typeToFields.get( type );

        if ( fields != null )
        {
            return fields;
        }

        List< Type > classHierarchy = new();

        Type nextClass = type;

        while ( nextClass != typeof(object) )
        {
            classHierarchy.Add( nextClass );
            nextClass = nextClass.BaseType;
        }

        List< System.Reflection.FieldInfo > allFields = new();

        for ( int i = classHierarchy.size - 1; i >= 0; i-- )
        {
            Collections.addAll( allFields, ClassReflection.getDeclaredFields( classHierarchy.get( i ) ) );
        }

        Dictionary< string, FieldMetadata > nameToField = new(allFields.Count);

        for ( int i = 0, n = allFields.Count; i < n; i++ )
        {
            System.Reflection.FieldInfo field = allFields[ i ];

            if ( field.isTransient() )
            {
                continue;
            }

            if ( field.isStatic() )
            {
                continue;
            }

            if ( field.isSynthetic() )
            {
                continue;
            }

            if ( !field.isAccessible() )
            {
                try
                {
                    field.setAccessible( true );
                }
                catch ( AccessControlException )
                {
                    continue;
                }
            }

            nameToField.put( field.Name, new FieldMetadata( field ) );
        }

        if ( sortFields )
        {
            nameToField.keys.sort();
        }

        typeToFields.put( type, nameToField );

        return nameToField;
    }

    public string ToJson( object? obj )
    {
        return ToJson( obj, obj?.GetType(), null );
    }

    public string ToJson( object? obj, Type? knownType )
    {
        return ToJson( obj, knownType, null );
    }

    /// <param name="obj"></param>
    /// <param name="knownType"> May be null if the type is unknown. </param>
    /// <param name="elementType"> May be null if the type is unknown.  </param>
    public string ToJson( object? obj, Type? knownType, Type? elementType )
    {
        var buffer = new StringWriter();

        ToJson( obj, knownType, elementType, buffer );

        return buffer.ToString();
    }

    public void ToJson( object? obj, FileInfo? file )
    {
        ToJson( obj, obj?.GetType(), null, file );
    }

    /// <param name="knownType"> May be null if the type is unknown. </param>
    public void ToJson( object? obj, Type? knownType, FileInfo? file )
    {
        ToJson( obj, knownType, null, file );
    }

    /// <param name="obj"></param>
    /// <param name="knownType"> May be null if the type is unknown. </param>
    /// <param name="elementType"> May be null if the type is unknown.  </param>
    /// <param name="file"></param>
    public void ToJson( object? obj, Type? knownType, Type? elementType, FileInfo? file )
    {
        Writer writer = null;

        try
        {
            writer = file.Writer( false, "UTF-8" );

            ToJson( obj, knownType, elementType, writer );
        }
        catch ( Exception ex )
        {
            throw new SerializationException( "Error writing file: " + file, ex );
        }
        finally
        {
            StreamUtils.CloseQuietly( writer );
        }
    }

    public void ToJson( object? obj, Writer? writer )
    {
        ToJson( obj, obj == null ? null : obj.GetType(), null, writer );
    }

    /// <param name="knownType"> May be null if the type is unknown. </param>
    public void ToJson( object obj, Type knownType, Writer writer )
    {
        ToJson( obj, knownType, null, writer );
    }

    /// <param name="knownType"> May be null if the type is unknown. </param>
    /// <param name="elementType"> May be null if the type is unknown.  </param>
    public void ToJson( object obj, Type knownType, Type elementType, Writer writer )
    {
        setWriter( writer );

        try
        {
            writeValue( obj, knownType, elementType );
        }
        finally
        {
            StreamUtils.closeQuietly( this.writer );
            this.writer = null;
        }
    }

    /** Sets the writer where JSON output will be written. This is only necessary when not using the toJson methods. */
    public void setWriter( Writer writer )
    {
        if ( !( writer instanceof JsonWriter)) writer = new JsonWriter( writer );
        this.writer                                   = ( JsonWriter )writer;
        this.writer.setOutputType( outputType );
        this.writer.setQuoteLongValues( quoteLongValues );
    }

    public JsonWriter getWriter()
    {
        return writer;
    }

    /** Writes all fields of the specified object to the current JSON object. */
    public void writeFields( Object object) {
        Class type = object.getClass();

        Object[] defaultValues = getDefaultValues( type );

        OrderedMap< String, FieldMetadata > fields       = getFields( type );
        int                                 defaultIndex = 0;
        Array< String >                     fieldNames   = fields.orderedKeys();

        for ( int i = 0, n = fieldNames.size; i < n; i++ )
        {
            FieldMetadata metadata = fields.get( fieldNames.get( i ) );

            if ( ignoreDeprecated && metadata.deprecated ) continue;
            Field field = metadata.field;

            try
            {
                Object value = field.get( object );

                if ( defaultValues != null )
                {
                    Object defaultValue = defaultValues[ defaultIndex++ ];

                    if ( value == null && defaultValue == null ) continue;

                    if ( value != null && defaultValue != null )
                    {
                        if ( value.equals( defaultValue ) ) continue;

                        if ( value.getClass().isArray() && defaultValue.getClass().isArray() )
                        {
                            equals1[ 0 ] = value;
                            equals2[ 0 ] = defaultValue;

                            if ( Arrays.deepEquals( equals1, equals2 ) ) continue;
                        }
                    }
                }

                if ( debug ) System.out.
                println( "Writing field: " + field.getName() + " (" + type.getName() + ")" );
                writer.name( field.getName() );
                writeValue( value, field.getType(), metadata.elementType );
            }
            catch ( ReflectionException ex )
            {
                throw new SerializationException
                    ( "Error accessing field: " + field.getName() + " (" + type.getName() + ")", ex );
            }
            catch ( SerializationException ex )
            {
                ex.addTrace( field + " (" + type.getName() + ")" );

                throw ex;
            }
            catch ( Exception runtimeEx )
            {
                SerializationException ex = new SerializationException( runtimeEx );
                ex.addTrace( field + " (" + type.getName() + ")" );

                throw ex;
            }
        }
    }

    private Object[] getDefaultValues( Class type )
    {
        if ( !usePrototypes ) return null;
        if ( classToDefaultValues.containsKey( type ) ) return classToDefaultValues.get( type );
        Object object;

        try
        {
            object = newInstance( type );
        }
        catch ( Exception ex )
        {
            classToDefaultValues.put( type, null );

            return null;
        }

        OrderedMap< String, FieldMetadata > fields = getFields( type );
        Object[]                            values = new Object[ fields.size ];
        classToDefaultValues.put( type, values );

        int             defaultIndex = 0;
        Array< String > fieldNames   = fields.orderedKeys();

        for ( int i = 0, n = fieldNames.size; i < n; i++ )
        {
            FieldMetadata metadata = fields.get( fieldNames.get( i ) );

            if ( ignoreDeprecated && metadata.deprecated ) continue;
            Field field = metadata.field;

            try
            {
                values[ defaultIndex++ ] = field.get( object );
            }
            catch ( ReflectionException ex )
            {
                throw new SerializationException
                    ( "Error accessing field: " + field.getName() + " (" + type.getName() + ")", ex );
            }
            catch ( SerializationException ex )
            {
                ex.addTrace( field + " (" + type.getName() + ")" );

                throw ex;
            }
            catch ( RuntimeException runtimeEx )
            {
                SerializationException ex = new SerializationException( runtimeEx );
                ex.addTrace( field + " (" + type.getName() + ")" );

                throw ex;
            }
        }

        return values;
    }

    /** @see #writeField(Object, String, String, Class) */
    public void writeField( Object object, String name) {
        writeField( object, name, name, null );
    }

    /** @param elementType May be null if the type is unknown.
	 * @see #writeField(Object, String, String, Class) */
    public void writeField( Object object, String name, Class elementType) {
        writeField( object, name, name, elementType );
    }

    /** @see #writeField(Object, String, String, Class) */
    public void writeField( Object object, String fieldName, String jsonName) {
        writeField( object, fieldName, jsonName, null );
    }

    /** Writes the specified field to the current JSON object.
	 * @param elementType May be null if the type is unknown. */
    public void writeField( Object object, String fieldName, String jsonName, Class elementType) {
        Class         type     = object.getClass();
        FieldMetadata metadata = getFields( type ).get( fieldName );

        if ( metadata == null )
            throw new SerializationException( "Field not found: " + fieldName + " (" + type.getName() + ")" );

        Field field                            = metadata.field;
        if ( elementType == null ) elementType = metadata.elementType;

        try
        {
            if ( debug ) System.out.
            println( "Writing field: " + field.getName() + " (" + type.getName() + ")" );
            writer.name( jsonName );
            writeValue( field.get( object ), field.getType(), elementType );
        }
        catch ( ReflectionException ex )
        {
            throw new SerializationException( "Error accessing field: " + field.getName() + " (" + type.getName() + ")", ex );
        }
        catch ( SerializationException ex )
        {
            ex.addTrace( field + " (" + type.getName() + ")" );

            throw ex;
        }
        catch ( Exception runtimeEx )
        {
            SerializationException ex = new SerializationException( runtimeEx );
            ex.addTrace( field + " (" + type.getName() + ")" );

            throw ex;
        }
    }

    /** Writes the value as a field on the current JSON object, without writing the actual class.
	 * @param value May be null.
	 * @see #writeValue(String, Object, Class, Class) */
    public void writeValue( String name, Object value )
    {
        try
        {
            writer.name( name );
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }

        if ( value == null )
            writeValue( value, null, null );
        else
            writeValue( value, value.getClass(), null );
    }

    /** Writes the value as a field on the current JSON object, writing the class of the object if it differs from the specified
	 * known type.
	 * @param value May be null.
	 * @param knownType May be null if the type is unknown.
	 * @see #writeValue(String, Object, Class, Class) */
    public void writeValue( String name, Object value, Class knownType )
    {
        try
        {
            writer.name( name );
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }

        writeValue( value, knownType, null );
    }

    /** Writes the value as a field on the current JSON object, writing the class of the object if it differs from the specified
	 * known type. The specified element type is used as the default type for collections.
	 * @param value May be null.
	 * @param knownType May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown. */
    public void writeValue( String name, Object value, Class knownType, Class elementType )
    {
        try
        {
            writer.name( name );
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }

        writeValue( value, knownType, elementType );
    }

    /** Writes the value, without writing the class of the object.
	 * @param value May be null. */
    public void writeValue( Object value )
    {
        if ( value == null )
            writeValue( value, null, null );
        else
            writeValue( value, value.getClass(), null );
    }

    /** Writes the value, writing the class of the object if it differs from the specified known type.
	 * @param value May be null.
	 * @param knownType May be null if the type is unknown. */
    public void writeValue( Object value, Class knownType )
    {
        writeValue( value, knownType, null );
    }

    /** Writes the value, writing the class of the object if it differs from the specified known type. The specified element type
	 * is used as the default type for collections.
	 * @param value May be null.
	 * @param knownType May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown. */
    public void writeValue( Object value, Class knownType, Class elementType )
    {
        try
        {
            if ( value == null )
            {
                writer.value( null );

                return;
            }

            if ( ( knownType != null && knownType.isPrimitive() ) || knownType == String.class || knownType == Integer.class
            || knownType == Boolean.class || knownType == Float.class || knownType == Long.class || knownType == Double.class
            || knownType == Short.class || knownType == Byte.class || knownType == Character.class) {
                writer.value( value );

                return;
            }

            Class actualType = value.getClass();

            if ( actualType.isPrimitive() || actualType == String.class || actualType == Integer.class
            || actualType == Boolean.class
            || actualType == Float.class || actualType == Long.class || actualType == Double.class || actualType == Short.class
            || actualType == Byte.class || actualType == Character.class) {
                writeObjectStart( actualType, null );
                writeValue( "value", value );
                writeObjectEnd();

                return;
            }

            if ( value instanceof Serializable) {
                writeObjectStart( actualType, knownType );
                ( ( ISerializable )value ).Write( this );
                writeObjectEnd();

                return;
            }

            ISerializer<> serializer = classToSerializer.get( actualType );

            if ( serializer != null )
            {
                serializer.write( this, value, knownType );

                return;
            }

            // JSON array special cases.
            if ( value instanceof Array) {
                if ( knownType != null && actualType != knownType && actualType != Array.class)

                throw new SerializationException
                    (
                    "Serialization of an Array other than the known type is not supported.\n"
                    + "Known type: "
                    + knownType
                    + "\nActual type: "
                    + actualType
                    );

                writeArrayStart();
                Array array = ( Array )value;

                for ( int i = 0, n = array.size; i < n; i++ )
                    writeValue( array.get( i ), elementType, null );

                writeArrayEnd();

                return;
            }

            if ( value instanceof Queue) {
                if ( knownType != null && actualType != knownType && actualType != Queue.class)

                throw new SerializationException
                    (
                    "Serialization of a Queue other than the known type is not supported.\n"
                    + "Known type: "
                    + knownType
                    + "\nActual type: "
                    + actualType
                    );

                writeArrayStart();
                Queue queue = ( Queue )value;

                for ( int i = 0, n = queue.size; i < n; i++ )
                    writeValue( queue.get( i ), elementType, null );

                writeArrayEnd();

                return;
            }

            if ( value instanceof Collection) {
                if ( typeName != null && actualType != ArrayList.class && ( knownType == null || knownType != actualType )) {
                    writeObjectStart( actualType, knownType );
                    writeArrayStart( "items" );
                    for ( Object item :
                    ( Collection )value)
                    writeValue( item, elementType, null );
                    writeArrayEnd();
                    writeObjectEnd();
                } else {
                    writeArrayStart();
                    for ( Object item :
                    ( Collection )value)
                    writeValue( item, elementType, null );
                    writeArrayEnd();
                }

                return;
            }

            if ( actualType.isArray() )
            {
                if ( elementType == null ) elementType = actualType.getComponentType();
                int length                             = ArrayReflection.getLength( value );
                writeArrayStart();

                for ( int i = 0; i < length; i++ )
                    writeValue( ArrayReflection.get( value, i ), elementType, null );

                writeArrayEnd();

                return;
            }

            // JSON object special cases.
            if ( value instanceof ObjectMap) {
                if ( knownType == null ) knownType = ObjectMap.class;
                writeObjectStart( actualType, knownType );
                for ( Entry entry :
                ( ( ObjectMap < ?,  ?>)value).entries()) {
                    writer.name( convertToString( entry.key ) );
                    writeValue( entry.value, elementType, null );
                }

                writeObjectEnd();

                return;
            }

            if ( value instanceof ObjectIntMap) {
                if ( knownType == null ) knownType = ObjectIntMap.class;
                writeObjectStart( actualType, knownType );
                for ( ObjectIntMap.Entry entry :
                ( ( ObjectIntMap < ?>)value).entries()) {
                    writer.name( convertToString( entry.key ) );
                    writeValue( entry.value, Integer.class);
                }

                writeObjectEnd();

                return;
            }

            if ( value instanceof ObjectFloatMap) {
                if ( knownType == null ) knownType = ObjectFloatMap.class;
                writeObjectStart( actualType, knownType );
                for ( ObjectFloatMap.Entry entry :
                ( ( ObjectFloatMap < ?>)value).entries()) {
                    writer.name( convertToString( entry.key ) );
                    writeValue( entry.value, Float.class);
                }

                writeObjectEnd();

                return;
            }

            if ( value instanceof ObjectSet) {
                if ( knownType == null ) knownType = ObjectSet.class;
                writeObjectStart( actualType, knownType );
                writer.name( "values" );
                writeArrayStart();
                for ( Object entry :
                ( ObjectSet )value)
                writeValue( entry, elementType, null );
                writeArrayEnd();
                writeObjectEnd();

                return;
            }

            if ( value instanceof IntMap) {
                if ( knownType == null ) knownType = IntMap.class;
                writeObjectStart( actualType, knownType );
                for ( IntMap.Entry entry :
                ( ( IntMap < ?>)value).entries()) {
                    writer.name( String.valueOf( entry.key ) );
                    writeValue( entry.value, elementType, null );
                }

                writeObjectEnd();

                return;
            }

            if ( value instanceof LongMap) {
                if ( knownType == null ) knownType = LongMap.class;
                writeObjectStart( actualType, knownType );
                for ( LongMap.Entry entry :
                ( ( LongMap < ?>)value).entries()) {
                    writer.name( String.valueOf( entry.key ) );
                    writeValue( entry.value, elementType, null );
                }

                writeObjectEnd();

                return;
            }

            if ( value instanceof IntSet) {
                if ( knownType == null ) knownType = IntSet.class;
                writeObjectStart( actualType, knownType );
                writer.name( "values" );
                writeArrayStart();

                for ( IntSetIterator iter = ( ( IntSet )value ).iterator(); iter.hasNext; )
                    writeValue( iter.next(), Integer.class, null);
                writeArrayEnd();
                writeObjectEnd();

                return;
            }

            if ( value instanceof ArrayMap) {
                if ( knownType == null ) knownType = ArrayMap.class;
                writeObjectStart( actualType, knownType );
                ArrayMap map = ( ArrayMap )value;

                for ( int i = 0, n = map.size; i < n; i++ )
                {
                    writer.name( convertToString( map.keys[ i ] ) );
                    writeValue( map.values[ i ], elementType, null );
                }

                writeObjectEnd();

                return;
            }

            if ( value instanceof Map) {
                if ( knownType == null ) knownType = HashMap.class;
                writeObjectStart( actualType, knownType );
                for ( Map.Entry entry :
                ( ( Map < ?,  ?>)value).entrySet()) {
                    writer.name( convertToString( entry.getKey() ) );
                    writeValue( entry.getValue(), elementType, null );
                }

                writeObjectEnd();

                return;
            }

            // Enum special case.
            if ( ClassReflection.isAssignableFrom( Enum.class, actualType)) {
                if ( typeName != null && ( knownType == null || knownType != actualType ) )
                {
                    // Ensures that enums with specific implementations (abstract logic) serialize correctly.
                    if ( actualType.getEnumConstants() == null ) actualType = actualType.getSuperclass();

                    writeObjectStart( actualType, null );
                    writer.name( "value" );
                    writer.value( convertToString( ( Enum )value ) );
                    writeObjectEnd();
                }
                else
                {
                    writer.value( convertToString( ( Enum )value ) );
                }

                return;
            }

            writeObjectStart( actualType, knownType );
            writeFields( value );
            writeObjectEnd();
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }
    }

    public void writeObjectStart( String name )
    {
        try
        {
            writer.name( name );
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }

        writeObjectStart();
    }

    /** @param knownType May be null if the type is unknown. */
    public void writeObjectStart( String name, Class actualType, Class knownType )
    {
        try
        {
            writer.name( name );
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }

        writeObjectStart( actualType, knownType );
    }

    public void writeObjectStart()
    {
        try
        {
            writer.object();
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }
    }

    /** Starts writing an object, writing the actualType to a field if needed.
	 * @param knownType May be null if the type is unknown. */
    public void writeObjectStart( Class actualType, Class knownType )
    {
        try
        {
            writer.object();
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }

        if ( knownType == null || knownType != actualType ) writeType( actualType );
    }

    public void writeObjectEnd()
    {
        try
        {
            writer.pop();
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }
    }

    public void writeArrayStart( String name )
    {
        try
        {
            writer.name( name );
            writer.array();
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }
    }

    public void writeArrayStart()
    {
        try
        {
            writer.array();
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }
    }

    public void writeArrayEnd()
    {
        try
        {
            writer.pop();
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }
    }

    public void writeType( Class type )
    {
        if ( typeName == null ) return;
        String className                   = getTag( type );
        if ( className == null ) className = type.getName();

        try
        {
            writer.set( typeName, className );
        }
        catch ( IOException ex )
        {
            throw new SerializationException( ex );
        }

        if ( debug ) System.out.
        println( "Writing type: " + type.getName() );
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, Reader reader )
    {
        return readValue( type, null, new JsonReader().parse( reader ) );
    }

    /** @param type May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, Class elementType, Reader reader )
    {
        return readValue( type, elementType, new JsonReader().parse( reader ) );
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, InputStream input )
    {
        return readValue( type, null, new JsonReader().parse( input ) );
    }

    /** @param type May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, Class elementType, InputStream input )
    {
        return readValue( type, elementType, new JsonReader().parse( input ) );
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, FileHandle file )
    {
        try
        {
            return readValue( type, null, new JsonReader().parse( file ) );
        }
        catch ( Exception ex )
        {
            throw new SerializationException( "Error reading file: " + file, ex );
        }
    }

    /** @param type May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, Class elementType, FileHandle file )
    {
        try
        {
            return readValue( type, elementType, new JsonReader().parse( file ) );
        }
        catch ( Exception ex )
        {
            throw new SerializationException( "Error reading file: " + file, ex );
        }
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, char[] data, int offset, int length )
    {
        return readValue( type, null, new JsonReader().parse( data, offset, length ) );
    }

    /** @param type May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, Class elementType, char[] data, int offset, int length )
    {
        return readValue( type, elementType, new JsonReader().parse( data, offset, length ) );
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, String json )
    {
        return readValue( type, null, new JsonReader().parse( json ) );
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T fromJson( Class< T > type, Class elementType, String json )
    {
        return readValue( type, elementType, new JsonReader().parse( json ) );
    }

    public void readField( Object object, String name, JsonValue jsonData) {
        readField( object, name, name, null, jsonData );
    }

    public void readField( Object object, String name, Class elementType, JsonValue jsonData) {
        readField( object, name, name, elementType, jsonData );
    }

    public void readField( Object object, String fieldName, String jsonName, JsonValue jsonData) {
        readField( object, fieldName, jsonName, null, jsonData );
    }

    /** @param elementType May be null if the type is unknown. */
    public void readField( Object object, String fieldName, String jsonName, Class elementType, JsonValue jsonMap) {
        Class         type     = object.getClass();
        FieldMetadata metadata = getFields( type ).get( fieldName );

        if ( metadata == null )
            throw new SerializationException( "Field not found: " + fieldName + " (" + type.getName() + ")" );

        Field field                            = metadata.field;
        if ( elementType == null ) elementType = metadata.elementType;
        readField( object, field, jsonName, elementType, jsonMap );
    }

    /** @param object May be null if the field is static.
	 * @param elementType May be null if the type is unknown. */
    public void readField( Object object, Field field, String jsonName, Class elementType, JsonValue jsonMap) {
        JsonValue jsonValue = jsonMap.get( jsonName );

        if ( jsonValue == null ) return;

        try
        {
            field.set( object, readValue( field.getType(), elementType, jsonValue ) );
        }
        catch ( ReflectionException ex )
        {
            throw new SerializationException
                (
                "Error accessing field: " + field.getName() + " (" + field.getDeclaringClass().getName() + ")", ex
                );
        }
        catch ( SerializationException ex )
        {
            ex.addTrace( field.getName() + " (" + field.getDeclaringClass().getName() + ")" );

            throw ex;
        }
        catch ( RuntimeException runtimeEx )
        {
            SerializationException ex = new SerializationException( runtimeEx );
            ex.addTrace( jsonValue.trace() );
            ex.addTrace( field.getName() + " (" + field.getDeclaringClass().getName() + ")" );

            throw ex;
        }
    }

    public void readFields( Object object, JsonValue jsonMap) {
        Class                               type   = object.getClass();
        OrderedMap< String, FieldMetadata > fields = getFields( type );

        for ( JsonValue child = jsonMap.child; child != null; child = child.next )
        {
            FieldMetadata metadata = fields.get( child.name().replace( " ", "_" ) );

            if ( metadata == null )
            {
                if ( child.name.equals( typeName ) ) continue;

                if ( ignoreUnknownFields || ignoreUnknownField( type, child.name ) )
                {
                    if ( debug ) System.out.
                    println( "Ignoring unknown field: " + child.name + " (" + type.getName() + ")" );

                    continue;
                }
                else
                {
                    SerializationException ex = new SerializationException
                        (
                        "Field not found: " + child.name + " (" + type.getName() + ")"
                        );

                    ex.addTrace( child.trace() );

                    throw ex;
                }
            }
            else
            {
                if ( ignoreDeprecated && !readDeprecated && metadata.deprecated ) continue;
            }

            Field field = metadata.field;

            try
            {
                field.set( object, readValue( field.getType(), metadata.elementType, child ) );
            }
            catch ( ReflectionException ex )
            {
                throw new SerializationException
                    ( "Error accessing field: " + field.getName() + " (" + type.getName() + ")", ex );
            }
            catch ( SerializationException ex )
            {
                ex.addTrace( field.getName() + " (" + type.getName() + ")" );

                throw ex;
            }
            catch ( RuntimeException runtimeEx )
            {
                SerializationException ex = new SerializationException( runtimeEx );
                ex.addTrace( child.trace() );
                ex.addTrace( field.getName() + " (" + type.getName() + ")" );

                throw ex;
            }
        }
    }

    /** Called for each unknown field name encountered by {@link #readFields(Object, JsonValue)} when {@link #ignoreUnknownFields}
	 * is false to determine whether the unknown field name should be ignored.
	 * @param type The object type being read.
	 * @param fieldName A field name encountered in the JSON for which there is no matching class field.
	 * @return true if the field name should be ignored and an exception won't be thrown by
	 *         {@link #readFields(Object, JsonValue)}. */
    protected boolean ignoreUnknownField( Class type, String fieldName )
    {
        return false;
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T readValue( String name, Class< T > type, JsonValue jsonMap )
    {
        return readValue( type, null, jsonMap.get( name ) );
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T readValue( String name, Class< T > type, T defaultValue, JsonValue jsonMap )
    {
        JsonValue jsonValue = jsonMap.get( name );

        if ( jsonValue == null ) return defaultValue;

        return readValue( type, null, jsonValue );
    }

    /** @param type May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown.
	 * @return May be null. */
    public <T> T readValue( String name, Class< T > type, Class elementType, JsonValue jsonMap )
    {
        return readValue( type, elementType, jsonMap.get( name ) );
    }

    /** @param type May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown.
	 * @return May be null. */
    public <T> T readValue( String name, Class< T > type, Class elementType, T defaultValue, JsonValue jsonMap )
    {
        JsonValue jsonValue = jsonMap.get( name );

        return readValue( type, elementType, defaultValue, jsonValue );
    }

    /** @param type May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown.
	 * @return May be null. */
    public <T> T readValue( Class< T > type, Class elementType, T defaultValue, JsonValue jsonData )
    {
        if ( jsonData == null ) return defaultValue;

        return readValue( type, elementType, jsonData );
    }

    /** @param type May be null if the type is unknown.
	 * @return May be null. */
    public <T> T readValue( Class< T > type, JsonValue jsonData )
    {
        return readValue( type, null, jsonData );
    }

    /** @param type May be null if the type is unknown.
	 * @param elementType May be null if the type is unknown.
	 * @return May be null. */
    public <T> T readValue( Class< T > type, Class elementType, JsonValue jsonData )
    {
        if ( jsonData == null ) return null;

        if ( jsonData.isObject() )
        {
            String className = typeName == null ? null : jsonData.getString( typeName, null );

            if ( className != null )
            {
                type = getClass( className );

                if ( type == null )
                {
                    try
                    {
                        type = ClassReflection.forName( className );
                    }
                    catch ( ReflectionException ex )
                    {
                        throw new SerializationException( ex );
                    }
                }
            }

            if ( type == null )
            {
                if ( defaultSerializer != null ) return ( T )defaultSerializer.read( this, jsonData, type );

                return ( T )jsonData;
            }

            if ( typeName != null && ClassReflection.isAssignableFrom( Collection.class, type)) {
                // JSON object wrapper to specify type.
                jsonData = jsonData.get( "items" );

                if ( jsonData == null )
                    throw new SerializationException
                        (
                        "Unable to convert object to collection: " + jsonData + " (" + type.getName() + ")"
                        );
            } else {
                ISerializer<> serializer = classToSerializer.get( type );

                if ( serializer != null ) return ( T )serializer.read( this, jsonData, type );

                if ( type == String.class || type == Integer.class || type == Boolean.class || type == Float.class
                || type == Long.class || type == Double.class || type == Short.class || type == Byte.class
                || type == Character.class || ClassReflection.isAssignableFrom( Enum.class, type)) {
                    return readValue( "value", type, jsonData );
                }

                Object object = newInstance( type );

                if ( object instanceof Serializable) {
                    ( ( ISerializable )object ).Read( this, jsonData );

                    return ( T )object;
                }

                // JSON object special cases.
                if ( object instanceof ObjectMap) {
                    ObjectMap result = ( ObjectMap )object;

                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
                        result.put( child.name, readValue( elementType, null, child ) );

                    return ( T )result;
                }

                if ( object instanceof ObjectIntMap) {
                    ObjectIntMap result = ( ObjectIntMap )object;

                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
                        result.put( child.name, readValue( Integer.class, null, child));

                    return ( T )result;
                }

                if ( object instanceof ObjectFloatMap) {
                    ObjectFloatMap result = ( ObjectFloatMap )object;

                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
                        result.put( child.name, readValue( Float.class, null, child));

                    return ( T )result;
                }

                if ( object instanceof ObjectSet) {
                    ObjectSet result = ( ObjectSet )object;

                    for ( JsonValue child = jsonData.getChild( "values" ); child != null; child = child.next )
                        result.add( readValue( elementType, null, child ) );

                    return ( T )result;
                }

                if ( object instanceof IntMap) {
                    IntMap result = ( IntMap )object;

                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
                        result.put( Integer.parseInt( child.name ), readValue( elementType, null, child ) );

                    return ( T )result;
                }

                if ( object instanceof LongMap) {
                    LongMap result = ( LongMap )object;

                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
                        result.put( Long.parseLong( child.name ), readValue( elementType, null, child ) );

                    return ( T )result;
                }

                if ( object instanceof IntSet) {
                    IntSet result = ( IntSet )object;

                    for ( JsonValue child = jsonData.getChild( "values" ); child != null; child = child.next )
                        result.add( child.asInt() );

                    return ( T )result;
                }

                if ( object instanceof ArrayMap) {
                    ArrayMap result = ( ArrayMap )object;

                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
                        result.put( child.name, readValue( elementType, null, child ) );

                    return ( T )result;
                }

                if ( object instanceof Map) {
                    Map result = ( Map )object;

                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
                    {
                        if ( child.name.equals( typeName ) ) continue;
                        result.put( child.name, readValue( elementType, null, child ) );
                    }

                    return ( T )result;
                }

                readFields( object, jsonData );

                return ( T )object;
            }
        }

        if ( type != null )
        {
            ISerializer<> serializer = classToSerializer.get( type );

            if ( serializer != null ) return ( T )serializer.read( this, jsonData, type );

            if ( ClassReflection.isAssignableFrom( ISerializable.class, type)) {
                // A Serializable may be read as an array, string, etc, even though it will be written as an object.
                Object object = newInstance( type );
                ( ( ISerializable )object ).Read( this, jsonData );

                return ( T )object;
            }
        }

        if ( jsonData.isArray() )
        {
            // JSON array special cases.
            if ( type == null || type == Object.class) type = ( Class< T > )Array.class;
            if ( ClassReflection.isAssignableFrom( Array.class, type)) {
                Array result = type == Array.class ? new Array() : ( Array )newInstance( type );

                for ( JsonValue child = jsonData.child; child != null; child = child.next )
                    result.add( readValue( elementType, null, child ) );

                return ( T )result;
            }

            if ( ClassReflection.isAssignableFrom( Queue.class, type)) {
                Queue result = type == Queue.class ? new Queue() : ( Queue )newInstance( type );

                for ( JsonValue child = jsonData.child; child != null; child = child.next )
                    result.addLast( readValue( elementType, null, child ) );

                return ( T )result;
            }

            if ( ClassReflection.isAssignableFrom( Collection.class, type)) {
                Collection result = type.isInterface() ? new ArrayList() : ( Collection )newInstance( type );

                for ( JsonValue child = jsonData.child; child != null; child = child.next )
                    result.add( readValue( elementType, null, child ) );

                return ( T )result;
            }

            if ( type.isArray() )
            {
                Class componentType                    = type.getComponentType();
                if ( elementType == null ) elementType = componentType;
                Object result                          = ArrayReflection.newInstance( componentType, jsonData.size );
                int    i                               = 0;

                for ( JsonValue child = jsonData.child; child != null; child = child.next )
                    ArrayReflection.set( result, i++, readValue( elementType, null, child ) );

                return ( T )result;
            }

            throw new SerializationException
                ( "Unable to convert value to required type: " + jsonData + " (" + type.getName() + ")" );
        }

        if ( jsonData.isNumber() )
        {
            try
            {
                if ( type == null || type == float.class || type == Float.class) return ( T )( Float )jsonData.asFloat();
                if ( type == int.class || type == Integer.class) return ( T )( Integer )jsonData.asInt();
                if ( type == long.class || type == Long.class) return ( T )( Long )jsonData.asLong();
                if ( type == double.class || type == Double.class) return ( T )( Double )jsonData.asDouble();
                if ( type == String.class) return ( T )jsonData.asString();
                if ( type == short.class || type == Short.class) return ( T )( Short )jsonData.asShort();
                if ( type == byte.class || type == Byte.class) return ( T )( Byte )jsonData.asByte();
            }
            catch ( NumberFormatException ignored )
            {
            }

            jsonData = new JsonValue( jsonData.asString() );
        }

        if ( jsonData.isBoolean() )
        {
            try
            {
                if ( type == null || type == boolean.class || type == Boolean.class) return
                    ( T )( Boolean )jsonData.asBoolean();
            }
            catch ( NumberFormatException ignored )
            {
            }

            jsonData = new JsonValue( jsonData.asString() );
        }

        if ( jsonData.isString() )
        {
            String string = jsonData.asString();
            if ( type == null || type == String.class) return ( T )string;

            try
            {
                if ( type == int.class || type == Integer.class) return ( T )Integer.valueOf( string );
                if ( type == float.class || type == Float.class) return ( T )Float.valueOf( string );
                if ( type == long.class || type == Long.class) return ( T )Long.valueOf( string );
                if ( type == double.class || type == Double.class) return ( T )Double.valueOf( string );
                if ( type == short.class || type == Short.class) return ( T )Short.valueOf( string );
                if ( type == byte.class || type == Byte.class) return ( T )Byte.valueOf( string );
            }
            catch ( NumberFormatException ignored )
            {
            }

            if ( type == boolean.class || type == Boolean.class) return ( T )Boolean.valueOf( string );
            if ( type == char.class || type == Character.class) return ( T )( Character )string.charAt( 0 );
            if ( ClassReflection.isAssignableFrom( Enum.class, type)) {
                Enum[] constants = ( Enum[] )type.getEnumConstants();

                for ( int i = 0, n = constants.length; i < n; i++ )
                {
                    Enum e = constants[ i ];

                    if ( string.equals( convertToString( e ) ) ) return ( T )e;
                }
            }

            if ( type == CharSequence.class) return ( T )string;

            throw new SerializationException
                ( "Unable to convert value to required type: " + jsonData + " (" + type.getName() + ")" );
        }

        return null;
    }

    /** Each field on the <code>to</code> object is set to the value for the field with the same name on the <code>from</code>
	 * object. The <code>to</code> object must have at least all the fields of the <code>from</code> object with the same name and
	 * type. */
    public void copyFields( Object from, Object to )
    {
        OrderedMap< String, FieldMetadata > toFields = getFields( to.getClass() );
        for ( ObjectMap.Entry < String, FieldMetadata > entry : getFields( from.getClass() ))

        {
            FieldMetadata toField   = toFields.get( entry.key );
            Field         fromField = entry.value.field;

            if ( toField == null ) throw new SerializationException( "To object is missing field: " + entry.key );

            try
            {
                toField.field.set( to, fromField.get( from ) );
            }
            catch ( ReflectionException ex )
            {
                throw new SerializationException( "Error copying field: " + fromField.getName(), ex );
            }
        }
    }

    private String convertToString( Enum e )
    {
        return enumNames ? e.name() : e.toString();
    }

    private String convertToString( Object object) {
        if ( object instanceof Enum) return convertToString( ( Enum )object );
        if ( object instanceof Class) return ( ( Class )object ).getName();
        return String.valueOf( object );
    }

    protected Object newInstance( Class type )
    {
        try
        {
            return ClassReflection.newInstance( type );
        }
        catch ( Exception ex )
        {
            try
            {
                // Try a private constructor.
                Constructor constructor = ClassReflection.getDeclaredConstructor( type );
                constructor.setAccessible( true );

                return constructor.newInstance();
            }
            catch ( SecurityException ignored )
            {
            }
            catch ( ReflectionException ignored )
            {
                if ( ClassReflection.isAssignableFrom( Enum.class, type)) {
                    if ( type.getEnumConstants() == null ) type = type.getSuperclass();

                    return type.getEnumConstants()[ 0 ];
                }

                if ( type.isArray() )
                    throw new SerializationException
                        ( "Encountered JSON object when expected array of type: " + type.getName(), ex );
                else if ( ClassReflection.isMemberClass( type ) && !ClassReflection.isStaticClass( type ) )
                    throw new SerializationException
                        ( "Class cannot be created (non-static member class): " + type.getName(), ex );
                else
                    throw new SerializationException
                        ( "Class cannot be created (missing no-arg constructor): " + type.getName(), ex );
            }
            catch ( Exception privateConstructorException )
            {
                ex = privateConstructorException;
            }

            throw new SerializationException( "Error constructing instance of class: " + type.getName(), ex );
        }
    }

    public String prettyPrint( Object object) {
        return prettyPrint( object, 0 );
    }

    public String prettyPrint( String json )
    {
        return prettyPrint( json, 0 );
    }

    public String prettyPrint( Object object, int singleLineColumns) {
        return prettyPrint( toJson( object ), singleLineColumns );
    }

    public String prettyPrint( String json, int singleLineColumns )
    {
        return new JsonReader().parse( json ).prettyPrint( outputType, singleLineColumns );
    }

    public String prettyPrint( Object object, PrettyPrintSettings settings) {
        return prettyPrint( toJson( object ), settings );
    }

    public String prettyPrint( String json, PrettyPrintSettings settings )
    {
        return new JsonReader().parse( json ).prettyPrint( settings );
    }

    private class FieldMetadata
    {
        Field _field;
        Type  _elementType;
        bool  _deprecated;

        public FieldMetadata( Field field )
        {
            this._field = field;

            int index = ( field.GetType().IsAssignableFrom(  )
                || ClassReflection.isAssignableFrom( Map.class, field.getType())) ? 1 : 0;

            this._elementType = field.getElementType( index );
            this._deprecated = field.isAnnotationPresent( Deprecated.class);
        }
    }

    public interface ISerializer
    {
        public void Write( Json json, object obj, Type knownType );

        public object Read( Json json, JsonValue jsonData, Type type );
    }

    public abstract class ReadOnlySerializer : ISerializer
    {
        public void Write( Json json, object obj, Type knownType )
        {
        }

        public abstract object Read( Json json, JsonValue jsonData, Type type );
    }

    public interface ISerializable
    {
        public void Write( Json json );

        public void Read( Json json, JsonValue jsonData );
    }
}
