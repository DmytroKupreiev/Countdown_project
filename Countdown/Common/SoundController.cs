using CommunityToolkit.Maui.Views;

public class SoundController
{
    private MediaElement _mediaElement;
    
    public SoundController(MediaElement mediaElement)
    {
        _mediaElement = mediaElement;
    }

    public void Start()
    {
        _mediaElement.Play();
    }

    public  void Stop()
    {
        _mediaElement.Stop();
    }
}

