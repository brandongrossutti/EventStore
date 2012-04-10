﻿using System;
using GHI.Bus;
using GHI.EventRepository;
using GHI.TestDomain.Messages;
using GHI.TestDomain.Model;

namespace GHI.TestDomain.Handlers
{
    public class ChangeAddressCommandHandler : IMessageHandler<ChangeAddressCommand>
    {
        private readonly IRepository<Guid> _repository;

        public ChangeAddressCommandHandler(IRepository<Guid> repository)
        {
            _repository = repository;
        }

        public void HandleMessage(ChangeAddressCommand message)
        {
            TestAggregateRoot root = _repository.GetAggregateRoot<TestAggregateRoot>(message.Id);
            root.ChangeAddress(message.Value);
        }
    }
}
