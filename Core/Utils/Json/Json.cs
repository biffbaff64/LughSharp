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

//using System.Collections;
//using System.Runtime.Serialization;
//using System.Security;

//using LibGDXSharp.Core.Utils.Collections;
//using LibGDXSharp.GdxCore.Utils.Collections;
//using LibGDXSharp.Maps;
//using LibGDXSharp.Utils.Collections;
//using LibGDXSharp.Utils.Collections.Extensions;
//using LibGDXSharp.Utils.Reflect;

namespace LibGDXSharp.Utils.Json;

/// <summary>
/// Reads/writes Java objects to/from JSON, automatically.
/// </summary>
public class Json
{
//    private readonly static bool debug = false;
//
//    [Obsolete] public bool                  IgnoreUnknownFields { get; set; }
//    [Obsolete] public bool                  IgnoreDeprecated    { get; set; }
//    [Obsolete] public bool                  ReadDeprecated      { get; set; }
//    [Obsolete] public JsonWriter.OutputType OutputType          { get; set; }
//    [Obsolete] public bool                  QuoteLongValues     { get; set; }
//    [Obsolete] public bool                  EnumNames           { get; set; } = true;
//    [Obsolete] public JsonWriter            Writer              { get; set; }
//    [Obsolete] public ISerializer           DefaultSerializer   { get; set; }
//    [Obsolete] public string                TypeName            { get; set; } = "class";
//    [Obsolete] public bool                  UsePrototypes       { get; set; } = true;
//    [Obsolete] public bool                  SortFields          { get; set; }
//
//    private readonly Dictionary< Type, Dictionary< string, FieldMetadata? > > _typeToFields = new();
//
//    private readonly Dictionary< string, Type >      _tagToClass           = new();
//    private readonly Dictionary< Type, string >      _classToTag           = new();
//    private readonly Dictionary< Type, ISerializer > _classToSerializer    = new();
//    private readonly Dictionary< Type, object[] >    _classToDefaultValues = new();
//    private readonly object?[]                       _equals1              = { null };
//    private readonly object?[]                       _equals2              = { null };
//
//    [Obsolete]
//    public Json()
//    {
//        OutputType = JsonWriter.OutputType.Minimal;
//    }
//
//    [Obsolete]
//    public Json( JsonWriter.OutputType outputType )
//    {
//        this.OutputType = outputType;
//    }
//
//    /// <summary>
//    /// Sets a tag to use instead of the fully qualifier class
//    /// name. This can make the JSON easier to read.
//    /// </summary>
//    [Obsolete]
//    public void AddClassTag( string tag, Type type )
//    {
//        _tagToClass.Put( tag, type );
//        _classToTag.Put( type, tag );
//    }
//
//    /// <summary>
//    /// Returns the class for the specified tag, or null.
//    /// </summary>
//    [Obsolete]
//    public Type? GetClass( string tag )
//    {
//        return _tagToClass.Get( tag );
//    }
//
//    /// <summary>
//    /// Returns the tag for the specified class, or null.
//    /// </summary>
//    [Obsolete]
//    public string? GetTag( Type type )
//    {
//        return _classToTag.Get( type );
//    }
//
//    /// <summary>
//    /// Registers a serializer to use for the specified type instead of the
//    /// default behavior of serializing all of an objects fields.
//    /// </summary>
//    [Obsolete]
//    public void SetSerializer( Type type, ISerializer serializer )
//    {
//        _classToSerializer.Put( type, serializer );
//    }
//
//    [Obsolete]
//    public ISerializer GetSerializer( Type type )
//    {
//        return _classToSerializer.Get( type );
//    }
//
//    /// <summary>
//    /// Sets the type of elements in a collection. When the element type is
//    /// known, the class for each element in the collection does not need to
//    /// be written unless different from the element type.
//    /// </summary>
//    [Obsolete]
//    public void SetElementType( Type type, string fieldname, Type element )
//    {
//        FieldMetadata metadata = GetFields( type ).Get( fieldname );
//
//        if ( metadata == null )
//        {
//            throw new SerializationException( "Field not found: " + fieldname + " (" + type.Name + ")" );
//        }
//
//        metadata.ElementType = element;
//    }
//
//    /// <summary>
//    /// The specified field will be treated as if it has or does not have
//    /// the <tt>Obsolete</tt> annotation.
//    /// </summary>
//    [Obsolete]
//    public void SetDeprecated( Type type, string fieldname, bool deprecated )
//    {
//        FieldMetadata metadata = GetFields( type ).Get( fieldname );
//
//        if ( metadata == null )
//        {
//            throw new SerializationException( "Field not found: " + fieldname + " (" + type.Name + ")" );
//        }
//
//        metadata.Deprecated = deprecated;
//    }
//
//    [Obsolete]
//    private Dictionary< string, FieldMetadata >? GetFields( Type type )
//    {
//        Dictionary< string, FieldMetadata? > fields = _typeToFields.Get( type );
//
//        if ( fields != null )
//        {
//            return fields;
//        }
//
//        List< Type > classHierarchy = new();
//
//        Type nextClass = type;
//
//        while ( nextClass != typeof( object ) )
//        {
//            classHierarchy.Add( nextClass );
//            nextClass = nextClass.BaseType;
//        }
//
//        List< System.Reflection.FieldInfo > allFields = new();
//
//        for ( int i = classHierarchy.size - 1; i >= 0; i-- )
//        {
//            Collections.addAll( allFields, ClassReflection.getDeclaredFields( classHierarchy.get( i ) ) );
//        }
//
//        Dictionary< string, FieldMetadata > nameToField = new( allFields.Count );
//
//        for ( int i = 0, n = allFields.Count; i < n; i++ )
//        {
//            System.Reflection.FieldInfo field = allFields[ i ];
//
//            if ( field.isTransient() )
//            {
//                continue;
//            }
//
//            if ( field.isStatic() )
//            {
//                continue;
//            }
//
//            if ( field.isSynthetic() )
//            {
//                continue;
//            }
//
//            if ( !field.isAccessible() )
//            {
//                try
//                {
//                    field.setAccessible( true );
//                }
//                catch ( AccessControlException )
//                {
//                    continue;
//                }
//            }
//
//            nameToField.put( field.Name, new FieldMetadata( field ) );
//        }
//
//        if ( sortFields )
//        {
//            nameToField.keys.sort();
//        }
//
//        typeToFields.put( type, nameToField );
//
//        return nameToField;
//        throw new NotImplementedException();
//    }
//
//    [Obsolete]
//    public string ToJson( object? obj )
//    {
//        throw new NotImplementedException();
//        return ToJson( obj, obj?.GetType(), null );
//    }

//    [Obsolete]
//    public string ToJson( object? obj, Type? knownType )
//    {
//        throw new NotImplementedException();
//        return ToJson( obj, knownType, null );
//    }

//    /// <param name="obj"></param>
//    /// <param name="knownType"> May be null if the type is unknown. </param>
//    /// <param name="elementType"> May be null if the type is unknown.  </param>
//    [Obsolete]
//    public string ToJson( object? obj, Type? knownType, Type? elementType )
//    {
//        var buffer = new StringWriter();

//        ToJson( obj, knownType, elementType, buffer );

//        return buffer.ToString();
//    }

//    [Obsolete]
//    public void ToJson( object? obj, FileInfo? file )
//    {
//        ToJson( obj, obj?.GetType(), null, file );
//    }

//    /// <param name="knownType"> May be null if the type is unknown. </param>
//    [Obsolete]
//    public void ToJson( object? obj, Type? knownType, FileInfo? file )
//    {
//        ToJson( obj, knownType, null, file );
//    }

//    /// <param name="obj"></param>
//    /// <param name="knownType"> May be null if the type is unknown. </param>
//    /// <param name="elementType"> May be null if the type is unknown.  </param>
//    /// <param name="file"></param>
//    [Obsolete]
//    public void ToJson( object? obj, Type? knownType, Type? elementType, FileInfo? file )
//    {
//        Writer writer = null;
//
//        try
//        {
//            writer = file.Writer( false, "UTF-8" );
//
//            ToJson( obj, knownType, elementType, writer );
//        }
//        catch ( Exception ex )
//        {
//            throw new SerializationException( "Error writing file: " + file, ex );
//        }
//        finally
//        {
//            StreamUtils.CloseQuietly( writer );
//        }
//    }

//    [Obsolete]
//    public void ToJson( object? obj, Writer? writer )
//    {
//        ToJson( obj, obj == null ? null : obj.GetType(), null, writer );
//    }

//    [Obsolete]
//    public void ToJson( object obj, Type knownType, Writer writer )
//    {
//        ToJson( obj, knownType, null, writer );
//    }

//    /// <param name="knownType"> May be null if the type is unknown. </param>
//    /// <param name="elementType"> May be null if the type is unknown.  </param>
//    [Obsolete]
//    public void ToJson( object obj, Type knownType, Type elementType, Writer writer )
//    {
//        setWriter( writer );

//        try
//        {
//            writeValue( obj, knownType, elementType );
//        }
//        finally
//        {
//            StreamUtils.closeQuietly( this.writer );
//            this.writer = null;
//        }
//    }

//    /** Sets the writer where JSON output will be written. This is only necessary when not using the toJson methods. */
//    [Obsolete]
//    public void setWriter( Writer writer )
//    {
//        if ( !( writer is JsonWriter ) ) writer = new JsonWriter( writer );
//        this.writer = ( JsonWriter )writer;
//        this.writer.setOutputType( outputType );
//        this.writer.setQuoteLongValues( quoteLongValues );
//    }

//    [Obsolete]
//    public JsonWriter getWriter()
//    {
//        return writer;
//    }

//    /** Writes all fields of the specified object to the current JSON object. */
//    [Obsolete]
//    public void writeFields( object obj )
//    {
//        Type type = object.getClass();

//        Object[] defaultValues = getDefaultValues( type );

//        OrderedMap< string, FieldMetadata > fields       = getFields( type );
//        int                                 defaultIndex = 0;
//        Array< string >                     fieldNames   = fields.orderedKeys();

//        for ( int i = 0, n = fieldNames.size; i < n; i++ )
//        {
//            FieldMetadata metadata = fields.get( fieldNames.get( i ) );

//            if ( ignoreDeprecated && metadata.deprecated ) continue;
//            Field field = metadata.field;

//            try
//            {
//                Object value = field.get( object );

//                if ( defaultValues != null )
//                {
//                    Object defaultValue = defaultValues[ defaultIndex++ ];

//                    if ( value == null && defaultValue == null ) continue;

//                    if ( value != null && defaultValue != null )
//                    {
//                        if ( value.Equals( defaultValue ) ) continue;

//                        if ( value.getClass().isArray() && defaultValue.getClass().isArray() )
//                        {
//                            equals1[ 0 ] = value;
//                            equals2[ 0 ] = defaultValue;

//                            if ( Arrays.deepEquals( equals1, equals2 ) ) continue;
//                        }
//                    }
//                }

//                if ( debug ) System.out.
//                println( "Writing field: " + field.getName() + " (" + type.getName() + ")" );
//                writer.name( field.getName() );
//                writeValue( value, field.getType(), metadata.elementType );
//            }
//            catch ( ReflectionException ex )
//            {
//                throw new SerializationException
//                    ( "Error accessing field: " + field.getName() + " (" + type.getName() + ")", ex );
//            }
//            catch ( SerializationException ex )
//            {
//                ex.addTrace( field + " (" + type.getName() + ")" );

//                throw ex;
//            }
//            catch ( Exception runtimeEx )
//            {
//                SerializationException ex = new SerializationException( runtimeEx );
//                ex.addTrace( field + " (" + type.getName() + ")" );

//                throw ex;
//            }
//        }
//    }

//    [Obsolete]
//    private Object[] getDefaultValues( Type type )
//    {
//        if ( !usePrototypes ) return null;
//        if ( classToDefaultValues.containsKey( type ) ) return classToDefaultValues.get( type );
//        object obj;

//        try
//        {
//            object = newInstance( type );
//        }
//        catch ( Exception ex )
//        {
//            classToDefaultValues.put( type, null );

//            return null;
//        }

//        OrderedMap< string, FieldMetadata > fields = getFields( type );
//        Object[]                            values = new Object[ fields.size ];
//        classToDefaultValues.put( type, values );

//        int             defaultIndex = 0;
//        Array< string > fieldNames   = fields.orderedKeys();

//        for ( int i = 0, n = fieldNames.size; i < n; i++ )
//        {
//            FieldMetadata metadata = fields.get( fieldNames.get( i ) );

//            if ( ignoreDeprecated && metadata.deprecated ) continue;
//            Field field = metadata.field;

//            try
//            {
//                values[ defaultIndex++ ] = field.get( object );
//            }
//            catch ( ReflectionException ex )
//            {
//                throw new SerializationException
//                    ( "Error accessing field: " + field.getName() + " (" + type.getName() + ")", ex );
//            }
//            catch ( SerializationException ex )
//            {
//                ex.addTrace( field + " (" + type.getName() + ")" );

//                throw ex;
//            }
//            catch ( RuntimeException runtimeEx )
//            {
//                SerializationException ex = new SerializationException( runtimeEx );
//                ex.addTrace( field + " (" + type.getName() + ")" );

//                throw ex;
//            }
//        }

//        return values;
//    }

//    /** @see #writeField(Object, string, string, Type) */
//    [Obsolete]
//    public void writeField( object obj, 

//    private string name) {
//        writeField( object, name, name, null );
//    }

//    /** @param elementType May be null if the type is unknown.
//	 * @see #writeField(Object, string, string, Type) */
//    [Obsolete]
//    public void writeField( object obj, 

//    private string name, Type elementType) {
//        writeField( object, name, name, elementType );
//    }

//    /** @see #writeField(Object, string, string, Type) */
//    [Obsolete]
//    public void writeField( object obj, 

//    private string fieldName, string jsonName) {
//        writeField( object, fieldName, jsonName, null );
//    }

//    /** Writes the specified field to the current JSON object.
//	 * @param elementType May be null if the type is unknown. */
//    [Obsolete]
//    public void writeField( object obj, 

//    private string fieldName, string jsonName,
//    private Type   elementType) {
//        Type          type     = object.getClass();
//        FieldMetadata metadata = getFields( type ).get( fieldName );

//        if ( metadata == null )
//            throw new SerializationException( "Field not found: " + fieldName + " (" + type.getName() + ")" );

//        Field field                            = metadata.field;
//        if ( elementType == null ) elementType = metadata.elementType;

//        try
//        {
//            if ( debug ) System.out.
//            println( "Writing field: " + field.getName() + " (" + type.getName() + ")" );
//            writer.name( jsonName );
//            writeValue( field.get( object ), field.getType(), elementType );
//        }
//        catch ( ReflectionException ex )
//        {
//            throw new SerializationException
//                ( "Error accessing field: " + field.getName() + " (" + type.getName() + ")", ex );
//        }
//        catch ( SerializationException ex )
//        {
//            ex.addTrace( field + " (" + type.getName() + ")" );

//            throw ex;
//        }
//        catch ( Exception runtimeEx )
//        {
//            SerializationException ex = new SerializationException( runtimeEx );
//            ex.addTrace( field + " (" + type.getName() + ")" );

//            throw ex;
//        }
//    }

//    /** Writes the value as a field on the current JSON object, without writing the actual class.
//	 * @param value May be null.
//	 * @see #writeValue(string, Object, Type, Type) */
//    [Obsolete]
//    public void writeValue( string name, Object value )
//    {
//        try
//        {
//            writer.name( name );
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }

//        if ( value == null )
//            writeValue( value, null, null );
//        else
//            writeValue( value, value.getClass(), null );
//    }

//    /** Writes the value as a field on the current JSON object, writing the class of the object if it differs from the specified
//	 * known type.
//	 * @param value May be null.
//	 * @param knownType May be null if the type is unknown.
//	 * @see #writeValue(string, Object, Type, Type) */
//    [Obsolete]
//    public void writeValue( string name, Object value, Type knownType )
//    {
//        try
//        {
//            writer.name( name );
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }

//        writeValue( value, knownType, null );
//    }

//    /** Writes the value as a field on the current JSON object, writing the class of the object if it differs from the specified
//	 * known type. The specified element type is used as the default type for collections.
//	 * @param value May be null.
//	 * @param knownType May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown. */
//    [Obsolete]
//    public void writeValue( string name, Object value, Type knownType, Type elementType )
//    {
//        try
//        {
//            writer.name( name );
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }

//        writeValue( value, knownType, elementType );
//    }

//    /** Writes the value, without writing the class of the object.
//	 * @param value May be null. */
//    [Obsolete]
//    public void writeValue( Object value )
//    {
//        if ( value == null )
//            writeValue( value, null, null );
//        else
//            writeValue( value, value.getClass(), null );
//    }

//    /** Writes the value, writing the class of the object if it differs from the specified known type.
//	 * @param value May be null.
//	 * @param knownType May be null if the type is unknown. */
//    [Obsolete]
//    public void writeValue( Object value, Type knownType )
//    {
//        writeValue( value, knownType, null );
//    }

//    /** Writes the value, writing the class of the object if it differs from the specified known type. The specified element type
//	 * is used as the default type for collections.
//	 * @param value May be null.
//	 * @param knownType May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown. */
//    [Obsolete]
//    public void writeValue( Object value, Type knownType, Type elementType )
//    {
//        try
//        {
//            if ( value == null )
//            {
//                writer.value( null );
//
//                return;
//            }
//
//            if ( ( knownType != null && knownType.isPrimitive() ) || knownType == string.class
//            || knownType == Integer.class
//            || knownType == Boolean.class || knownType == Float.class || knownType == Long.class
//            || knownType == Double.class
//            || knownType == Short.class || knownType == Byte.class || knownType == Character.class) {
//                writer.value( value );
//
//                return;
//            }
//
//            Type actualType = value.getClass();
//
//            if ( actualType.isPrimitive() || actualType == string.class || actualType == Integer.class
//            || actualType == Boolean.class
//            || actualType == Float.class || actualType == Long.class || actualType == Double.class || actualType
//                == Short.class
//            || actualType == Byte.class || actualType == Character.class) {
//                writeObjectStart( actualType, null );
//                writeValue( "value", value );
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is Serializable )
//            {
//                writeObjectStart( actualType, knownType );
//                ( ( ISerializable )value ).Write( this );
//                writeObjectEnd();
//
//                return;
//            }
//
//            ISerializer<> serializer = classToSerializer.get( actualType );
//
//            if ( serializer != null )
//            {
//                serializer.write( this, value, knownType );
//
//                return;
//            }
//
//            // JSON array special cases.
//            if ( value is Array )
//            {
//                if ( knownType != null && actualType != knownType && actualType != Array.class)
//
//                throw new SerializationException
//                    (
//                    "Serialization of an Array other than the known type is not supported.\n"
//                    + "Known type: "
//                    + knownType
//                    + "\nActual type: "
//                    + actualType
//                    );
//
//                writeArrayStart();
//                Array array = ( Array )value;
//
//                for ( int i = 0, n = array.size; i < n; i++ )
//                    writeValue( array.get( i ), elementType, null );
//
//                writeArrayEnd();
//
//                return;
//            }
//
//            if ( value is Queue )
//            {
//                if ( knownType != null && actualType != knownType && actualType != Queue.class)
//
//                throw new SerializationException
//                    (
//                    "Serialization of a Queue other than the known type is not supported.\n"
//                    + "Known type: "
//                    + knownType
//                    + "\nActual type: "
//                    + actualType
//                    );
//
//                writeArrayStart();
//                Queue queue = ( Queue )value;
//
//                for ( int i = 0, n = queue.size; i < n; i++ )
//                    writeValue( queue.get( i ), elementType, null );
//
//                writeArrayEnd();
//
//                return;
//            }
//
//            if ( value is Collection )
//            {
//                if ( typeName != null && actualType != ArrayList.class
//                && ( knownType == null || knownType != actualType )) {
//                    writeObjectStart( actualType, knownType );
//                    writeArrayStart( "items" );
//                    for ( Object item :
//                    ( Collection )value)
//                    writeValue( item, elementType, null );
//                    writeArrayEnd();
//                    writeObjectEnd();
//                } else {
//                    writeArrayStart();
//                    for ( Object item :
//                    ( Collection )value)
//                    writeValue( item, elementType, null );
//                    writeArrayEnd();
//                }
//
//                return;
//            }
//
//            if ( actualType.isArray() )
//            {
//                if ( elementType == null ) elementType = actualType.getComponentType();
//                int length                             = ArrayReflection.getLength( value );
//                writeArrayStart();
//
//                for ( int i = 0; i < length; i++ )
//                    writeValue( ArrayReflection.get( value, i ), elementType, null );
//
//                writeArrayEnd();
//
//                return;
//            }
//
//            // JSON object special cases.
//            if ( value is ObjectMap )
//            {
//                if ( knownType == null ) knownType = ObjectMap.class;
//                writeObjectStart( actualType, knownType );
//                for ( Entry entry :
//                ( ( ObjectMap < ?,  ?>)value).entries()) {
//                    writer.name( convertToString( entry.key ) );
//                    writeValue( entry.value, elementType, null );
//                }
//
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is ObjectIntMap )
//            {
//                if ( knownType == null ) knownType = ObjectIntMap.class;
//                writeObjectStart( actualType, knownType );
//                for ( ObjectIntMap.Entry entry :
//                ( ( ObjectIntMap < ?>)value).entries()) {
//                    writer.name( convertToString( entry.key ) );
//                    writeValue( entry.value, Integer.class);
//                }
//
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is ObjectFloatMap )
//            {
//                if ( knownType == null ) knownType = ObjectFloatMap.class;
//                writeObjectStart( actualType, knownType );
//                for ( ObjectFloatMap.Entry entry :
//                ( ( ObjectFloatMap < ?>)value).entries()) {
//                    writer.name( convertToString( entry.key ) );
//                    writeValue( entry.value, Float.class);
//                }
//
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is ObjectSet )
//            {
//                if ( knownType == null ) knownType = ObjectSet.class;
//                writeObjectStart( actualType, knownType );
//                writer.name( "values" );
//                writeArrayStart();
//                for ( Object entry :
//                ( ObjectSet<> )value)
//                writeValue( entry, elementType, null );
//                writeArrayEnd();
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is IntMap )
//            {
//                if ( knownType == null ) knownType = IntMap.class;
//                writeObjectStart( actualType, knownType );
//                for ( IntMap.Entry entry :
//                ( ( IntMap < ?>)value).entries()) {
//                    writer.name( string.valueOf( entry.key ) );
//                    writeValue( entry.value, elementType, null );
//                }
//
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is LongMap )
//            {
//                if ( knownType == null ) knownType = LongMap.class;
//                writeObjectStart( actualType, knownType );
//                for ( LongMap.Entry entry :
//                ( ( LongMap < ?>)value).entries()) {
//                    writer.name( string.valueOf( entry.key ) );
//                    writeValue( entry.value, elementType, null );
//                }
//
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is IntSet )
//            {
//                if ( knownType == null ) knownType = IntSet.class;
//                writeObjectStart( actualType, knownType );
//                writer.name( "values" );
//                writeArrayStart();
//
//                for ( IntSetIterator iter = ( ( IntSet )value ).iterator(); iter.hasNext; )
//                    writeValue( iter.next(), Integer.class, null);
//                writeArrayEnd();
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is ArrayMap )
//            {
//                if ( knownType == null ) knownType = ArrayMap.class;
//                writeObjectStart( actualType, knownType );
//                ArrayMap map = ( ArrayMap )value;
//
//                for ( int i = 0, n = map.size; i < n; i++ )
//                {
//                    writer.name( convertToString( map.keys[ i ] ) );
//                    writeValue( map.values[ i ], elementType, null );
//                }
//
//                writeObjectEnd();
//
//                return;
//            }
//
//            if ( value is Map )
//            {
//                if ( knownType == null ) knownType = HashMap.class;
//                writeObjectStart( actualType, knownType );
//                for ( Map.Entry entry :
//                ( ( Map < ?,  ?>)value).entrySet()) {
//                    writer.name( convertToString( entry.getKey() ) );
//                    writeValue( entry.getValue(), elementType, null );
//                }
//
//                writeObjectEnd();
//
//                return;
//            }
//
//            // Enum special case.
//            if ( ClassReflection.isAssignableFrom( Enum.class, actualType)) {
//                if ( typeName != null && ( knownType == null || knownType != actualType ) )
//                {
//                    // Ensures that enums with specific implementations (abstract logic) serialize correctly.
//                    if ( actualType.getEnumConstants() == null ) actualType = actualType.getSuperclass();
//
//                    writeObjectStart( actualType, null );
//                    writer.name( "value" );
//                    writer.value( convertToString( ( Enum )value ) );
//                    writeObjectEnd();
//                }
//                else
//                {
//                    writer.value( convertToString( ( Enum )value ) );
//                }
//
//                return;
//            }
//
//            writeObjectStart( actualType, knownType );
//            writeFields( value );
//            writeObjectEnd();
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//    }
//
//    [Obsolete]
//    public void writeObjectStart( string name )
//    {
//        try
//        {
//            writer.name( name );
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//
//        writeObjectStart();
//    }
//
//    /** @param knownType May be null if the type is unknown. */
//    [Obsolete]
//    public void writeObjectStart( string name, Type actualType, Type knownType )
//    {
//        try
//        {
//            writer.name( name );
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//
//        writeObjectStart( actualType, knownType );
//    }
//
//    [Obsolete]
//    public void writeObjectStart()
//    {
//        try
//        {
//            writer.object();
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//    }
//
//    /** Starts writing an object, writing the actualType to a field if needed.
//	 * @param knownType May be null if the type is unknown. */
//    [Obsolete]
//    public void writeObjectStart( Type actualType, Type knownType )
//    {
//        try
//        {
//            writer.object();
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//
//        if ( knownType == null || knownType != actualType ) writeType( actualType );
//    }
//
//    [Obsolete]
//    public void writeObjectEnd()
//    {
//        try
//        {
//            writer.pop();
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//    }
//
//    [Obsolete]
//    public void writeArrayStart( string name )
//    {
//        try
//        {
//            writer.name( name );
//            writer.array();
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//    }
//
//    [Obsolete]
//    public void writeArrayStart()
//    {
//        try
//        {
//            writer.array();
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//    }
//
//    [Obsolete]
//    public void writeArrayEnd()
//    {
//        try
//        {
//            writer.pop();
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//    }
//
//    [Obsolete]
//    public void writeType( Type type )
//    {
//        if ( typeName == null ) return;
//        string className                   = getTag( type );
//        if ( className == null ) className = type.getName();
//
//        try
//        {
//            writer.set( typeName, className );
//        }
//        catch ( IOException ex )
//        {
//            throw new SerializationException( ex );
//        }
//
//        if ( debug ) System.out.
//        println( "Writing type: " + type.getName() );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, Reader reader )
//    {
//        return readValue( type, null, new JsonReader().parse( reader ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, Type elementType, Reader reader )
//    {
//        return readValue( type, elementType, new JsonReader().parse( reader ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, InputStream input )
//    {
//        return readValue( type, null, new JsonReader().parse( input ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, Type elementType, InputStream input )
//    {
//        return readValue( type, elementType, new JsonReader().parse( input ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, FileHandle file )
//    {
//        try
//        {
//            return readValue( type, null, new JsonReader().parse( file ) );
//        }
//        catch ( Exception ex )
//        {
//            throw new SerializationException( "Error reading file: " + file, ex );
//        }
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, Type elementType, FileHandle file )
//    {
//        try
//        {
//            return readValue( type, elementType, new JsonReader().parse( file ) );
//        }
//        catch ( Exception ex )
//        {
//            throw new SerializationException( "Error reading file: " + file, ex );
//        }
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, char[] data, int offset, int length )
//    {
//        return readValue( type, null, new JsonReader().parse( data, offset, length ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, Type elementType, char[] data, int offset, int length )
//    {
//        return readValue( type, elementType, new JsonReader().parse( data, offset, length ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, string json )
//    {
//        return readValue( type, null, new JsonReader().parse( json ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T fromJson( Type< T > type, Type elementType, string json )
//    {
//        return readValue( type, elementType, new JsonReader().parse( json ) );
//    }
//
//    [Obsolete]
//    public void readField( object obj, 
//
//    private string name, JsonValue jsonData) {
//        readField( object, name, name, null, jsonData );
//    }
//
//    [Obsolete]
//    public void readField( object obj, 
//
//    private string    name, Type elementType,
//    private JsonValue jsonData) {
//        readField( object, name, name, elementType, jsonData );
//    }
//
//    [Obsolete]
//    public void readField( object obj, 
//
//    private string    fieldName, string jsonName,
//    private JsonValue jsonData) {
//        readField( object, fieldName, jsonName, null, jsonData );
//    }
//
//    /** @param elementType May be null if the type is unknown. */
//    [Obsolete]
//    public void readField( object obj, 
//
//    private string fieldName, string jsonName,
//    private Type   elementType,      JsonValue jsonMap) {
//        Type          type     = object.getClass();
//        FieldMetadata metadata = getFields( type ).get( fieldName );
//
//        if ( metadata == null )
//            throw new SerializationException( "Field not found: " + fieldName + " (" + type.getName() + ")" );
//
//        Field field                            = metadata.field;
//        if ( elementType == null ) elementType = metadata.elementType;
//        readField( object, field, jsonName, elementType, jsonMap );
//    }
//
//    /** @param object May be null if the field is static.
//	 * @param elementType May be null if the type is unknown. */
//    [Obsolete]
//    public void readField( object obj, 
//
//    private Field field, string jsonName,
//    private Type  elementType,  JsonValue jsonMap) {
//        JsonValue jsonValue = jsonMap.get( jsonName );
//
//        if ( jsonValue == null ) return;
//
//        try
//        {
//            field.set( object, readValue( field.getType(), elementType, jsonValue ) );
//        }
//        catch ( ReflectionException ex )
//        {
//            throw new SerializationException
//                (
//                "Error accessing field: " + field.getName() + " (" + field.getDeclaringClass().getName() + ")", ex
//                );
//        }
//        catch ( SerializationException ex )
//        {
//            ex.addTrace( field.getName() + " (" + field.getDeclaringClass().getName() + ")" );
//
//            throw ex;
//        }
//        catch ( RuntimeException runtimeEx )
//        {
//            SerializationException ex = new SerializationException( runtimeEx );
//            ex.addTrace( jsonValue.trace() );
//            ex.addTrace( field.getName() + " (" + field.getDeclaringClass().getName() + ")" );
//
//            throw ex;
//        }
//    }
//
//    [Obsolete]
//    public void readFields( object obj, 
//
//    private JsonValue jsonMap) {
//        Type                                type   = object.getClass();
//        OrderedMap< string, FieldMetadata > fields = getFields( type );
//
//        for ( JsonValue child = jsonMap.child; child != null; child = child.next )
//        {
//            FieldMetadata metadata = fields.get( child.name().replace( " ", "_" ) );
//
//            if ( metadata == null )
//            {
//                if ( child.name.Equals( typeName ) ) continue;
//
//                if ( ignoreUnknownFields || ignoreUnknownField( type, child.name ) )
//                {
//                    if ( debug ) System.out.
//                    println( "Ignoring unknown field: " + child.name + " (" + type.getName() + ")" );
//
//                    continue;
//                }
//                else
//                {
//                    SerializationException ex = new SerializationException
//                        (
//                        "Field not found: " + child.name + " (" + type.getName() + ")"
//                        );
//
//                    ex.addTrace( child.trace() );
//
//                    throw ex;
//                }
//            }
//            else
//            {
//                if ( ignoreDeprecated && !readDeprecated && metadata.deprecated ) continue;
//            }
//
//            Field field = metadata.field;
//
//            try
//            {
//                field.set( object, readValue( field.getType(), metadata.elementType, child ) );
//            }
//            catch ( ReflectionException ex )
//            {
//                throw new SerializationException
//                    ( "Error accessing field: " + field.getName() + " (" + type.getName() + ")", ex );
//            }
//            catch ( SerializationException ex )
//            {
//                ex.addTrace( field.getName() + " (" + type.getName() + ")" );
//
//                throw ex;
//            }
//            catch ( RuntimeException runtimeEx )
//            {
//                SerializationException ex = new SerializationException( runtimeEx );
//                ex.addTrace( child.trace() );
//                ex.addTrace( field.getName() + " (" + type.getName() + ")" );
//
//                throw ex;
//            }
//        }
//    }
//
//    /** Called for each unknown field name encountered by {@link #readFields(Object, JsonValue)} when {@link #ignoreUnknownFields}
//	 * is false to determine whether the unknown field name should be ignored.
//	 * @param type The object type being read.
//	 * @param fieldName A field name encountered in the JSON for which there is no matching class field.
//	 * @return true if the field name should be ignored and an exception won't be thrown by
//	 *         {@link #readFields(Object, JsonValue)}. */
//    [Obsolete]
//    protected bool ignoreUnknownField( Type type, string fieldName )
//    {
//        return false;
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T readValue( string name, Type< T > type, JsonValue jsonMap )
//    {
//        return readValue( type, null, jsonMap.get( name ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T readValue( string name, Type< T > type, T defaultValue, JsonValue jsonMap )
//    {
//        JsonValue jsonValue = jsonMap.get( name );
//
//        if ( jsonValue == null ) return defaultValue;
//
//        return readValue( type, null, jsonValue );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T readValue( string name, Type< T > type, Type elementType, JsonValue jsonMap )
//    {
//        return readValue( type, elementType, jsonMap.get( name ) );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T readValue( string name, Type< T > type, Type elementType, T defaultValue, JsonValue jsonMap )
//    {
//        JsonValue jsonValue = jsonMap.get( name );
//
//        return readValue( type, elementType, defaultValue, jsonValue );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T readValue( Type< T > type, Type elementType, T defaultValue, JsonValue jsonData )
//    {
//        if ( jsonData == null ) return defaultValue;
//
//        return readValue( type, elementType, jsonData );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T readValue( Type< T > type, JsonValue jsonData )
//    {
//        return readValue( type, null, jsonData );
//    }
//
//    /** @param type May be null if the type is unknown.
//	 * @param elementType May be null if the type is unknown.
//	 * @return May be null. */
//    [Obsolete]
//    public <T>
//
//    private T readValue( Type< T > type, Type elementType, JsonValue jsonData )
//    {
//        if ( jsonData == null ) return null;
//
//        if ( jsonData.isObject() )
//        {
//            string className = typeName == null ? null : jsonData.getString( typeName, null );
//
//            if ( className != null )
//            {
//                type = getClass( className );
//
//                if ( type == null )
//                {
//                    try
//                    {
//                        type = ClassReflection.forName( className );
//                    }
//                    catch ( ReflectionException ex )
//                    {
//                        throw new SerializationException( ex );
//                    }
//                }
//            }
//
//            if ( type == null )
//            {
//                if ( defaultSerializer != null ) return ( T )defaultSerializer.read( this, jsonData, type );
//
//                return ( T )jsonData;
//            }
//
//            if ( typeName != null && ClassReflection.isAssignableFrom( Collection.class, type)) {
//                // JSON object wrapper to specify type.
//                jsonData = jsonData.get( "items" );
//
//                if ( jsonData == null )
//                    throw new SerializationException
//                        (
//                        "Unable to convert object to collection: " + jsonData + " (" + type.getName() + ")"
//                        );
//            } else {
//                ISerializer<> serializer = classToSerializer.get( type );
//
//                if ( serializer != null ) return ( T )serializer.read( this, jsonData, type );
//
//                if ( type == string.class || type == Integer.class || type == Boolean.class || type == Float.class
//                || type == Long.class || type == Double.class || type == Short.class || type == Byte.class
//                || type == Character.class || ClassReflection.isAssignableFrom( Enum.class, type)) {
//                    return readValue( "value", type, jsonData );
//                }
//
//                object obj = newInstance( type );
//
//                if ( object is Serializable )
//                {
//                    ( ( ISerializable )object ).Read( this, jsonData );
//
//                    return ( T )object;
//                }
//
//                // JSON object special cases.
//                if ( object is ObjectMap )
//                {
//                    ObjectMap < , > result = ( ObjectMap )object;
//
//                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                        result.put( child.name, readValue( elementType, null, child ) );
//
//                    return ( T )result;
//                }
//
//                if ( object is ObjectIntMap )
//                {
//                    ObjectIntMap result = ( ObjectIntMap )object;
//
//                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                        result.put( child.name, readValue( Integer.class, null, child));
//
//                    return ( T )result;
//                }
//
//                if ( object is ObjectFloatMap )
//                {
//                    ObjectFloatMap result = ( ObjectFloatMap )object;
//
//                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                        result.put( child.name, readValue( Float.class, null, child));
//
//                    return ( T )result;
//                }
//
//                if ( object is ObjectSet )
//                {
//                    ObjectSet result = ( ObjectSet )object;
//
//                    for ( JsonValue child = jsonData.getChild( "values" ); child != null; child = child.next )
//                        result.add( readValue( elementType, null, child ) );
//
//                    return ( T )result;
//                }
//
//                if ( object is IntMap )
//                {
//                    IntMap result = ( IntMap )object;
//
//                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                        result.put( Integer.parseInt( child.name ), readValue( elementType, null, child ) );
//
//                    return ( T )result;
//                }
//
//                if ( object is LongMap )
//                {
//                    LongMap result = ( LongMap )object;
//
//                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                        result.put( Long.parseLong( child.name ), readValue( elementType, null, child ) );
//
//                    return ( T )result;
//                }
//
//                if ( object is IntSet )
//                {
//                    IntSet result = ( IntSet )object;
//
//                    for ( JsonValue child = jsonData.getChild( "values" ); child != null; child = child.next )
//                        result.add( child.asInt() );
//
//                    return ( T )result;
//                }
//
//                if ( object is ArrayMap )
//                {
//                    ArrayMap result = ( ArrayMap )object;
//
//                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                        result.put( child.name, readValue( elementType, null, child ) );
//
//                    return ( T )result;
//                }
//
//                if ( object is Map )
//                {
//                    Map result = ( Map )object;
//
//                    for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                    {
//                        if ( child.name.Equals( typeName ) ) continue;
//                        result.put( child.name, readValue( elementType, null, child ) );
//                    }
//
//                    return ( T )result;
//                }
//
//                readFields( object, jsonData );
//
//                return ( T )object;
//            }
//        }
//
//        if ( type != null )
//        {
//            ISerializer<> serializer = classToSerializer.get( type );
//
//            if ( serializer != null ) return ( T )serializer.read( this, jsonData, type );
//
//            if ( ClassReflection.isAssignableFrom( ISerializable.class, type)) {
//                // A Serializable may be read as an array, string, etc, even though it will be written as an object.
//                object obj = newInstance( type );
//                ( ( ISerializable )object ).Read( this, jsonData );
//
//                return ( T )object;
//            }
//        }
//
//        if ( jsonData.isArray() )
//        {
//            // JSON array special cases.
//            if ( type == null || type == Object.class) type = ( Type< T > )Array.class;
//            if ( ClassReflection.isAssignableFrom( Array.class, type)) {
//                Array result = type == Array.class ? new Array() : ( Array )newInstance( type );
//
//                for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                    result.add( readValue( elementType, null, child ) );
//
//                return ( T )result;
//            }
//
//            if ( ClassReflection.isAssignableFrom( Queue.class, type)) {
//                Queue result = type == Queue.class ? new Queue() : ( Queue )newInstance( type );
//
//                for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                    result.addLast( readValue( elementType, null, child ) );
//
//                return ( T )result;
//            }
//
//            if ( ClassReflection.isAssignableFrom( Collection.class, type)) {
//                Collection result = type.isInterface() ? new ArrayList() : ( Collection )newInstance( type );
//
//                for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                    result.add( readValue( elementType, null, child ) );
//
//                return ( T )result;
//            }
//
//            if ( type.isArray() )
//            {
//                Type componentType                     = type.getComponentType();
//                if ( elementType == null ) elementType = componentType;
//                Object result                          = ArrayReflection.newInstance( componentType, jsonData.size );
//                int    i                               = 0;
//
//                for ( JsonValue child = jsonData.child; child != null; child = child.next )
//                    ArrayReflection.set( result, i++, readValue( elementType, null, child ) );
//
//                return ( T )result;
//            }
//
//            throw new SerializationException
//                ( "Unable to convert value to required type: " + jsonData + " (" + type.getName() + ")" );
//        }
//
//        if ( jsonData.isNumber() )
//        {
//            try
//            {
//                if ( type == null || type == float.class || type == Float.class) return
//                    ( T )( Float )jsonData.asFloat();
//
//                if ( type == int.class || type == Integer.class) return ( T )( Integer )jsonData.asInt();
//                if ( type == long.class || type == Long.class) return ( T )( Long )jsonData.asLong();
//                if ( type == double.class || type == Double.class) return ( T )( Double )jsonData.asDouble();
//                if ( type == string.class) return ( T )jsonData.asString();
//                if ( type == short.class || type == Short.class) return ( T )( Short )jsonData.asShort();
//                if ( type == byte.class || type == Byte.class) return ( T )( Byte )jsonData.asByte();
//            }
//            catch ( NumberFormatException ignored )
//            {
//            }
//
//            jsonData = new JsonValue( jsonData.asString() );
//        }
//
//        if ( jsonData.isBoolean() )
//        {
//            try
//            {
//                if ( type == null || type == bool.class || type == Boolean.class) return
//                    ( T )( Boolean )jsonData.asBoolean();
//            }
//            catch ( NumberFormatException ignored )
//            {
//            }
//
//            jsonData = new JsonValue( jsonData.asString() );
//        }
//
//        if ( jsonData.isString() )
//        {
//            string string = jsonData.asString();
//            if ( type == null || type == string.class) return ( T )string;
//
//            try
//            {
//                if ( type == int.class || type == Integer.class) return ( T )Integer.valueOf( string );
//                if ( type == float.class || type == Float.class) return ( T )Float.valueOf( string );
//                if ( type == long.class || type == Long.class) return ( T )Long.valueOf( string );
//                if ( type == double.class || type == Double.class) return ( T )Double.valueOf( string );
//                if ( type == short.class || type == Short.class) return ( T )Short.valueOf( string );
//                if ( type == byte.class || type == Byte.class) return ( T )Byte.valueOf( string );
//            }
//            catch ( NumberFormatException ignored )
//            {
//            }
//
//            if ( type == bool.class || type == Boolean.class) return ( T )Boolean.valueOf( string );
//            if ( type == char.class || type == Character.class) return ( T )( Character )string.charAt( 0 );
//            if ( ClassReflection.isAssignableFrom( Enum.class, type)) {
//                Enum[] constants = ( Enum[] )type.getEnumConstants();
//
//                for ( int i = 0, n = constants.length; i < n; i++ )
//                {
//                    Enum e = constants[ i ];
//
//                    if ( string.Equals( convertToString( e ) ) ) return ( T )e;
//                }
//            }
//
//            if ( type == string.class) return ( T )string;
//
//            throw new SerializationException
//                ( "Unable to convert value to required type: " + jsonData + " (" + type.getName() + ")" );
//        }
//
//        return null;
//    }
//
//    /** Each field on the <code>to</code> object is set to the value for the field with the same name on the <code>from</code>
//	 * object. The <code>to</code> object must have at least all the fields of the <code>from</code> object with the same name and
//	 * type. */
//    [Obsolete]
//    public void copyFields( Object from, Object to )
//    {
//        OrderedMap< string, FieldMetadata > toFields = getFields( to.getClass() );
//        for ( ObjectMap.Entry< string, FieldMetadata > entry :
//        getFields( from.getClass() ))
//
//        {
//            FieldMetadata toField   = toFields.get( entry.key );
//            Field         fromField = entry.value.field;
//
//            if ( toField == null ) throw new SerializationException( "To object is missing field: " + entry.key );
//
//            try
//            {
//                toField.field.set( to, fromField.get( from ) );
//            }
//            catch ( ReflectionException ex )
//            {
//                throw new SerializationException( "Error copying field: " + fromField.getName(), ex );
//            }
//        }
//    }
//
//    [Obsolete]
//    private string convertToString( Enum e )
//    {
//        return enumNames ? e.name() : e.ToString();
//    }
//
//    [Obsolete]
//    private string convertToString( object obj )
//    {
//        if ( obj is Enum ) return convertToString( ( Enum )obj );
//        if ( obj is Type ) return ( ( Type )obj ).getName();
//
//        return string.valueOf( obj );
//    }
//
//    [Obsolete]
//    protected Object newInstance( Type type )
//    {
//        try
//        {
//            return ClassReflection.newInstance( type );
//        }
//        catch ( Exception ex )
//        {
//            try
//            {
//                // Try a private constructor.
//                Constructor constructor = ClassReflection.getDeclaredConstructor( type );
//                constructor.setAccessible( true );
//
//                return constructor.newInstance();
//            }
//            catch ( SecurityException ignored )
//            {
//            }
//            catch ( ReflectionException ignored )
//            {
//                if ( ClassReflection.isAssignableFrom( Enum.class, type)) {
//                    if ( type.getEnumConstants() == null ) type = type.getSuperclass();
//
//                    return type.getEnumConstants()[ 0 ];
//                }
//
//                if ( type.isArray() )
//                    throw new SerializationException
//                        ( "Encountered JSON object when expected array of type: " + type.getName(), ex );
//                else if ( ClassReflection.isMemberClass( type ) && !ClassReflection.isStaticClass( type ) )
//                    throw new SerializationException
//                        ( "Type cannot be created (non-static member class): " + type.getName(), ex );
//                else
//                    throw new SerializationException
//                        ( "Type cannot be created (missing no-arg constructor): " + type.getName(), ex );
//            }
//            catch ( Exception privateConstructorException )
//            {
//                ex = privateConstructorException;
//            }
//
//            throw new SerializationException( "Error constructing instance of class: " + type.getName(), ex );
//        }
//    }
//
//    [Obsolete]
//    public string prettyPrint( object obj )
//    {
//        return prettyPrint( object, 0 );
//    }
//
//    [Obsolete]
//    public string prettyPrint( string json )
//    {
//        return prettyPrint( json, 0 );
//    }
//
//    [Obsolete]
//    public string prettyPrint( object obj, 
//
//    private int singleLineColumns) {
//        return prettyPrint( toJson( object ), singleLineColumns );
//    }
//
//    [Obsolete]
//    public string prettyPrint( string json, int singleLineColumns )
//    {
//        return new JsonReader().parse( json ).prettyPrint( outputType, singleLineColumns );
//    }
//
//    [Obsolete]
//    public string prettyPrint( object obj, 
//
//    private PrettyPrintSettings settings) {
//        return prettyPrint( toJson( object ), settings );
//    }
//
//    [Obsolete]
//    public string prettyPrint( string json, PrettyPrintSettings settings )
//    {
//        return new JsonReader().Parse( json ).prettyPrint( settings );
//    }
//
//    [Obsolete]
//    private sealed class FieldMetadata
//    {
//        public Field Field       { get; set; }
//        public Type  ElementType { get; set; }
//        public bool  Deprecated  { get; set; }
//
//        public FieldMetadata( Field field )
//        {
//            this.Field = field;
//
//            int index = ( field.GetType().IsAssignableFrom()
//                          || ClassReflection.isAssignableFrom( Map.class, field.GetType())) ? 1 : 0;
//
//            this.ElementType = field.getElementType( index );
//            this.Deprecated  = field.isAnnotationPresent( Deprecated.class);
//        }
//    }
//
//    [Obsolete]
//    public interface ISerializer
//    {
//        public void Write( Json json, object obj, Type knownType );
//
//        public object Read( Json json, JsonValue jsonData, Type type );
//    }
//
//    [Obsolete]
//    public abstract class ReadOnlySerializer : ISerializer
//    {
//        public void Write( Json json, object obj, Type knownType )
//        {
//        }
//
//        public abstract object Read( Json json, JsonValue jsonData, Type type );
//    }
//
//    [Obsolete]
//    public interface ISerializable
//    {
//        public void Write( Json json );
//
//        public void Read( Json json, JsonValue jsonData );
//    }
}