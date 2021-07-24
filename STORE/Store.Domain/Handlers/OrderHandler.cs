using System.Linq;
using Flunt.Notifications;
using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Entities;
using Store.Domain.Handlers.Interfaces;
using Store.Domain.Repositories.Interfaces;
using Store.Domain.Utils;

namespace Store.Domain.Handlers
{
    public class OrderHandler : Notifiable, IHandler<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFreeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderHandler(
            ICustomerRepository customerRepository,
            IDeliveryFeeRepository deliveryFeeRepository,
            IDiscountRepository discountRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository
        )
        {
            _customerRepository = customerRepository;
            _deliveryFreeRepository = deliveryFeeRepository;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public ICommandResult Handle(CreateOrderCommand command)
        {
            //Fail fast validation
            command.Validate();
            if (command.Valid)
                return new GenericCommandResult(false, "Pedido Invalido", command.Notifications);

            //Recupera o cliente
            var customer = _customerRepository.Get(command.Customer);

            //calcula a taxa
            var deliveryFee = _deliveryFreeRepository.Get(command.ZipCode);

            //obterm cupom de desconto
            var discount = _discountRepository.Get(command.PromoCode);

            //gera o pedido
            var products = _productRepository.Get(ExtractGuids.Extract(command.Items)).ToList();
            var order = new Order(customer, deliveryFee, discount);
            foreach (var item in command.Items)
            {
                var product = products.Where(x => x.Id == item.Product).FirstOrDefault();
                order.AddItem(product, item.Quantity);
            }

            //Agrupa notificações
            AddNotifications(order.Notifications);

            //Verifica se tudo está ok
            if(Invalid)
                return new GenericCommandResult(false, "falha ao gerar pedido", Notifications);

            //Retorna o resultado
            _orderRepository.Save(order);
            return new GenericCommandResult(true, $"Pedido {order.Number} gerado com sucesso", order);
        }
    }
}