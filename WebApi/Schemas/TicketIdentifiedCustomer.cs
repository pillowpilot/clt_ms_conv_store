using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Schemas.Tickets
{
    [BsonDiscriminator(RootClass = true)]
    public class TicketIdentifiedCustomer
    {
        
        public TicketIdentifiedCustomer(TicketEvent ticketEvent)
        {
            Id = Guid.NewGuid().ToString();
            CustomerName = ticketEvent.UserData.CustomerName;
            AgentName = ticketEvent.Comment?.AgentName;
            CustomerType = ticketEvent.UserData.CustomerType;
            CustomerNumber = ticketEvent.UserData.CustomerPhone;
            CustomerId = ticketEvent.UserData.CustormerId;
            LastMessage = new LastMessage
            {
                Body = ticketEvent.LastMessageBody,
                Timestamp = ticketEvent.LastMessageTimestamp
            };
            TicketStatus = ticketEvent.TicketStatus;

            // Inicializar listas y agregar elementos condicionalmente
            AgentComments = new List<AgentComment>();
            if (ticketEvent.Comment != null)
            {
                AgentComments.Add(new AgentComment
                {
                    Timestamp = ticketEvent.Comment.TimeStamp,
                    Body = ticketEvent.Comment.Body,
                    AgentId = ticketEvent.Comment.AgentId,
                    AgentName = ticketEvent.Comment.AgentName
                });
            }

            Messages = new List<Message>();
            if (ticketEvent.Message != null)
            {
                Messages.Add(new Message
                {
                    Timestamp = ticketEvent.Message.timestamp,
                    Body = ticketEvent.Message.Body,
                    Origin = ticketEvent.Message.origin,
                    OriginDetails = new OriginDetails
                    {
                        DisplayName = ticketEvent.OriginDisplayName
                    }
                });
            }

            Events = new List<EventDetails>();
            if (ticketEvent.EventDetails != null)
            {
                Events.Add(new EventDetails
                {
                    TimeStamp = ticketEvent.EventDetails.TimeStamp,
                    Type = ticketEvent.EventDetails.Type,
                    Version = ticketEvent.EventDetails.Version
                });
            }
        }

        public string Id{ get; private set; }

        [BsonElement("customer_name")]
        public string CustomerName { get; private set; }
        [BsonElement("last_message")]
        public LastMessage LastMessage { get; private set; }

        [BsonElement("agent_name")]
        public string AgentName { get; private set; }

        [BsonElement("customer_id")]
        public string CustomerId { get; private set; }

        [BsonElement("customer_type")]
        public string CustomerType { get; private set; }

        [BsonElement("customer_number")]
        public string CustomerNumber { get; private set; }

        [BsonElement("ticket_status")]
        public string TicketStatus { get; private set; }

        [BsonElement("comments")]
        public List<AgentComment> AgentComments { get; private set; } = new List<AgentComment>();

        [BsonElement("messages")]
        public List<Message> Messages { get; private set; } = new List<Message>();

        [BsonElement("events")]
        public List<EventDetails> Events { get; private set; } = new List<EventDetails>();
    }
}
