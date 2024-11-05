using System.Threading.Channels;

namespace ConvCrmContracts.Crm.Events;

public class ConversationDocument
{
    public Guid id { get; private set; }
    public string? codigo_cliente { get; private set; }
    public string active_channel { get; private set; }
    public string source_id { get; private set; }
    public UserDetails? user_details { get; private set; }
    public List<ConversationMessages> messages { get; set; } = [];

    public ConversationDocument(Guid id, string? codigo_cliente, string active_channel, string source_id, UserDetails? user_details, List<ConversationMessages> messages)
    {
        this.id = id;
        this.codigo_cliente = codigo_cliente;
        this.active_channel = active_channel;
        this.source_id = source_id;
        this.user_details = user_details;
        this.messages = messages;
    }
}