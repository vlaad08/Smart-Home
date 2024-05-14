namespace ConsoleApp1;

public interface ICommunicator
{ 
    public void setTemperature(string hardwareId, int level);
    public void setLight(string hardwareId, int level);

    public Task<string> getTemperature();
    public Task SwitchWindow();
    public Task SwitchDoor();

}