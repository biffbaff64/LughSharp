namespace LibGDXSharp.Utils.Json;

public interface IBaseJsonReader
{
    JsonValue Parse( InputStream input );

    JsonValue Parse( FileInfo file );
}
