using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store.Domain.Commands;
using Store.Domain.Handlers;
using Store.Tests.Repositories;

namespace Store.Tests.Handlers
{
    [TestClass]
    public class OrderHandlerTests
    {
        private readonly FakeCustomerRepository _customerRepository;
        private readonly FakeDeliveryFeeRepository _deliveryFeeRepository;
        private readonly FakeDiscountRepository _discountRepository;
        private readonly FakeOrderRepository _orderRepository;
        private readonly FakeProductRepository _productRepository;
        public OrderHandlerTests()
        {
            _customerRepository = new FakeCustomerRepository();
            _deliveryFeeRepository = new FakeDeliveryFeeRepository();
            _discountRepository = new FakeDiscountRepository();
            _orderRepository = new FakeOrderRepository();
            _productRepository = new FakeProductRepository();
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_cliente_inexistente_o_pedido_nao_deve_ser_gerado()
        {
                Assert.Fail();
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_cep_invalido_o_pedido_deve_ser_gerado_normalmente()
        {
                Assert.Fail();
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_promoCode_inexistente_o_pedido_deve_ser_gerado_normalmente()
        {
                Assert.Fail();
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_pedido_sem_item_o_mesmo_nao_deve_ser_gerado()
        {
                Assert.Fail();
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_comando_invalido_o_pedido_nao_deve_ser_gerado()
        {
            var command = new CreateOrderCommand();
            command.Customer = "";
            command.ZipCode = "13411080";
            command.PromoCode = "12345678";
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Validate();

            Assert.AreEqual(command.Valid, false);
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_comando_valido_o_pedido_deve_ser_gerado()
        {
            var createOrderCommand = new CreateOrderCommand();
            createOrderCommand.Customer = "12345678911";
            createOrderCommand.ZipCode = "13411080";
            createOrderCommand.PromoCode = "12345678";
            createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            var handler = new OrderHandler(
                _customerRepository,
                _deliveryFeeRepository,
                _discountRepository,
                _productRepository,
                _orderRepository
            );

            handler.Handle(createOrderCommand);
            Assert.AreEqual(handler.Valid, true);
        }

    }
}