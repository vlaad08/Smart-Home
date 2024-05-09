namespace ConsoleApp1;

public interface ICommunicator
{ 
    public string getTemperature();
    public void setTemperature(string hardwareId, int level);
    public void setLight(string hardwareId, int level);
}