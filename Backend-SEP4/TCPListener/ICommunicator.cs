namespace ConsoleApp1;

public interface ICommunicator
{ 
    public Task<string> getTemperature();
    public Task SwitchWindow();
}