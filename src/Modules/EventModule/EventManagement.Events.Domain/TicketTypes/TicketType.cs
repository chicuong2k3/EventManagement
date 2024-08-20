namespace EventManagement.Events.Domain.TicketTypes
{
    public sealed class TicketType : Entity
    {
        private TicketType()
        {
        }

        public Guid Id { get; private set; }

        public Guid EventId { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public string Currency { get; private set; }

        public int Quantity { get; private set; }

        public static TicketType Create(
            Guid eventId,
            string name,
            decimal price,
            string currency,
            int quantity)
        {
            var ticket = new TicketType
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                Name = name,
                Price = price,
                Currency = currency,
                Quantity = quantity
            };

            ticket.Raise(new TicketTypeCreatedDomainEvent(ticket.Id));

            return ticket;
        }

        public Result UpdatePrice(decimal price)
        {
            if (Price != price)
            {
                Price = price;

                Raise(new TicketTypePriceChangedDomainEvent(Id, Price));
            }


            return Result.Success();
        }
    }
}
