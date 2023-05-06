namespace LibGDXSharp.Graphics;

public interface IDownloadPixmapResponseListener {

	/// <summary>
	/// Called on the render thread when image was downloaded successfully.
	/// </summary>
	void DownloadComplete(Pixmap pixmap);

	/// <summary>
	/// Called when image download failed. This might get called on a background thread.
	/// </summary>
	void DownloadFailed(Exception e);
}