namespace ConsoleApp1;

public interface ICommunicator
{ 
    public void setTemperature(string hardwareId, int level);
    public void setLight(string hardwareId, int level);

    public Task<double> getTemperature();
    public Task SwitchWindow();
    public Task SwitchDoor();

}