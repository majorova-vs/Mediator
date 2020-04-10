using System;

namespace Lab5Mediator
{

    interface IMediator
    {
        public abstract void Send(string msg, Colleague colleague);
    }
    abstract class Colleague
    {
        protected IMediator mediator;

        public Colleague(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public virtual void Send(string message)
        {
            mediator.Send(message, this);
        }
        public abstract void Notify(string message);
    }
    // класс заказчика
    class CustomerColleague : Colleague
    {
        public CustomerColleague(IMediator mediator)
            : base(mediator)
        { }

        public override void Notify(string message)
        {
            Console.WriteLine("Сообщение для заказчика: " + message);
        }
    }
    // класс программиста
    class ProgrammerColleague : Colleague
    {
        public ProgrammerColleague(IMediator mediator)
            : base(mediator)
        { }

        public override void Notify(string message)
        {
            Console.WriteLine("Сообщение для отдела разработки: " + message);
        }
    }
    // класс тестера
    class TesterColleague : Colleague
    {
        public TesterColleague(IMediator mediator)
            : base(mediator)
        { }

        public override void Notify(string message)
        {
            Console.WriteLine("Сообщение для отдела тестирования: " + message);
        }
    }

    class ManagerMediator : IMediator
    {
        public Colleague Customer { get; set; }
        public Colleague Programmer { get; set; }
        public Colleague Tester { get; set; }
        public void Send(string msg, Colleague colleague)
        {
            // если отправитель - заказчик, значит есть новый заказ
            // отправляем сообщение программисту - выполнить заказ
            if (Customer == colleague)
                Programmer.Notify(msg);
            // если отправитель - программист, то можно приступать к тестированию
            // отправляем сообщение тестеру
            else if (Programmer == colleague)
                Tester.Notify(msg);
            // если отправитель - тест, значит продукт готов
            // отправляем сообщение заказчику
            else if (Tester == colleague)
                Customer.Notify(msg);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ManagerMediator mediator = new ManagerMediator();
            Colleague customer = new CustomerColleague(mediator);
            Colleague programmer = new ProgrammerColleague(mediator);
            Colleague tester = new TesterColleague(mediator);
            mediator.Customer = customer;
            mediator.Programmer = programmer;
            mediator.Tester = tester;
            customer.Send("Нужно написать программу.");
            programmer.Send("Нужно протестировать программу.");
            tester.Send("Программа протестирована, баги не найдены. Готова к продаже.");

            Console.Read();
        }
    }
}
