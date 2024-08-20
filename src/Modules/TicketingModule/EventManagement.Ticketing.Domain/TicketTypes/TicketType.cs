namespace EventManagement.Ticketing.Domain.TicketTypes
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

        public int AvailableQuantity { get; private set; }

        public static TicketType Create(
            Guid id,
            Guid eventId,
            string name,
            decimal price,
            string currency,
            int quantity)
        {
            var ticketType = new TicketType
            {
                Id = id,
                EventId = eventId,
                Name = name,
                Price = price,
                Currency = currency,
                Quantity = quantity,
                AvailableQuantity = quantity
            };

            return ticketType;
        }

        public void UpdatePrice(decimal price)
        {
            Price = price;
        }

        public Result UpdateQuantity(int quantity)
        {
            if (AvailableQuantity < quantity)
            {
                return Result.Failure(TicketTypeErrors.NotEnoughQuantity(AvailableQuantity));
            }

            AvailableQuantity -= quantity;

            if (AvailableQuantity == 0)
            {
                Raise(new TicketTypeSoldOutDomainEvent(Id));
            }

            return Result.Success();
        }
    }

}
