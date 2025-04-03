namespace WebHouse_Server.Packets;

public class Packet
{
    public string Data { get; }
    public PacketDataType DataType { get; }
    public string Sender { get; }
    public string[] Receivers { get; }

    public Packet(string data, PacketDataType dataType, string sender, params string[] receivers)
    {
        this.Data = data;
        this.DataType = dataType;
        this.Sender = sender;
        this.Receivers = receivers;
    }
}