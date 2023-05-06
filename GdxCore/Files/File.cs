namespace LibGDXSharp.Files;

public class File
{
    public bool Exists      { get; set; }
    public bool IsDirectory { get; set; }
        
    public File( string fileName )
    {
        throw new NotImplementedException();
    }

    public File( string? parent, string child )
    {
        throw new NotImplementedException();
    }

    public string GetPath()
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        throw new NotImplementedException();
    }

}