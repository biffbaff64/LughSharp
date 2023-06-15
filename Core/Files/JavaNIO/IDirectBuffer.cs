namespace LibGDXSharp.Files;

public interface IDirectBuffer
{
    long Address();

    object Attachment();

    Cleaner Cleaner();
}
