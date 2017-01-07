using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Compatibility;

namespace Dywidendy.Model
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void When_getting_all_money_with_same_rate_differential_is_zero()
        {
            var model = new CurrencyModel(() =>
            new List<IChangeDepositEvent>
            {
                new MoneyChanged(200, 2, DateTime.Now)
            });
            model.Withdrawn(200, 2, DateTime.Now);
            var result = model.Withdrawns.First();
            
            Assert.That(result.ToDoMoney, Is.EqualTo(0));
            Assert.That(result.RateDifferential, Is.EqualTo(0));
        }

        [Test]
        public void When_getting_part_money_with_same_rate_differential_is_zero()
        {
            var model = new CurrencyModel(() =>
                new List<IChangeDepositEvent>
                {
                    new MoneyChanged(200, 2, DateTime.Now)
                });
            model.Withdrawn(150, 2, DateTime.Now);
            var result = model.Withdrawns.First();

            Assert.That(result.ToDoMoney, Is.EqualTo(0));
            Assert.That(result.RateDifferential, Is.EqualTo(0));
        }

        [Test]
        public void When_getting_part_by_part_differential_is_zero()
        {
            var model = new CurrencyModel(() =>
            new List<IChangeDepositEvent>
            {
                new MoneyChanged(200, 2, DateTime.Now)
            });

            model.Withdrawn(150, 2, DateTime.Now);
            var result = model.Withdrawns.First();

            Assert.That(result.ToDoMoney, Is.EqualTo(0));
            Assert.That(result.RateDifferential, Is.EqualTo(0));

            model.Withdrawn(50, 2, DateTime.Now);
            result = model.Withdrawns.First();

            Assert.That(result.ToDoMoney, Is.EqualTo(0));
            Assert.That(result.RateDifferential, Is.EqualTo(0));
        }

        [Test]
        public void Other()
        {
            var model = new CurrencyModel(() =>
                new List<IChangeDepositEvent>
                {
                    new MoneyChanged(100, 1, DateTime.Now),
                    new MoneyChanged(200, 2, DateTime.Now),
                    new MoneyChanged(500, 3, DateTime.Now)
                });

            model.Withdrawn(700, 4, DateTime.Now);
            var result = model.Withdrawns.First();

            Assert.That(result.Deposits[0].Ammount, Is.EqualTo(100));
            Assert.That(result.Deposits[0].Rate, Is.EqualTo(1));

            Assert.That(result.Deposits[1].Ammount, Is.EqualTo(200));
            Assert.That(result.Deposits[1].Rate, Is.EqualTo(2));

            Assert.That(result.Deposits[2].Ammount, Is.EqualTo(400));
            Assert.That(result.Deposits[2].Rate, Is.EqualTo(3));

            Assert.That(result.ToDoMoney, Is.EqualTo(0));
            Assert.That(result.RateDifferential, Is.EqualTo(1100));


            model = new CurrencyModel(() =>
                new List<IChangeDepositEvent>
                {
                    new MoneyChanged(100, 1, DateTime.Now),
                    new MoneyChanged(200, 2, DateTime.Now),
                    new MoneyChanged(-200, 2, DateTime.Now),
                    new MoneyChanged(500, 3, DateTime.Now)
                });
            model.Withdrawn(150, 5, DateTime.Now);
            result = model.Withdrawns[1];

            Assert.That(result.Deposits.First().Ammount, Is.EqualTo(100));
            Assert.That(result.Deposits.First().Rate, Is.EqualTo(2));

            Assert.That(result.Deposits[1].Ammount, Is.EqualTo(50));
            Assert.That(result.Deposits[1].Rate, Is.EqualTo(3));

            model = new CurrencyModel(() =>
               new List<IChangeDepositEvent>
               {
                    new MoneyChanged(100, 1, DateTime.Now),
                    new MoneyChanged(200, 2, DateTime.Now),
                    new MoneyChanged(-200, 2, DateTime.Now),
                    new MoneyChanged(500, 3, DateTime.Now)
               });
            model.Withdrawn(900, 5, DateTime.Now);
            result = model.Withdrawns[1];

            Assert.That(result.Deposits.First().Ammount, Is.EqualTo(100));
            Assert.That(result.Deposits.First().Rate, Is.EqualTo(2));

            Assert.That(result.Deposits[1].Ammount, Is.EqualTo(500));
            Assert.That(result.Deposits[1].Rate, Is.EqualTo(3));

            Assert.That(result.ToDoMoney, Is.EqualTo(300));
            Assert.That(result.RateDifferential, Is.Null);

            model.Deposit(300, 6, DateTime.Now);

            Assert.That(result.ToDoMoney, Is.EqualTo(0));
            Assert.That(result.RateDifferential, Is.EqualTo(1000));
        }

        [Test, Ignore("Just perfomrance test")]
        public void Tests_from_files()
        {
            var sw = new Stopwatch();
            sw.Start();
            var model = new CurrencyModel(()=>Helpers.GetEventsFromFile(@"c:\temp\temp.dat"));

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);

        }

        [Test, Ignore("Just perfomrance test")]
        public void GenerateFile()
        {
            const string path = @"c:\temp\temp2.dat";
            var startDate = DateTime.Now.AddDays(1000);
            var r = new Random();
            var refRate = (r.NextDouble() + 1.0)*10;
            var list = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                var rateDiff = Math.Pow(-1, i)*r.NextDouble();
                var value = r.NextDouble()*100;
                var date = startDate.AddDays(i);
                list.Add(string.Format("{0:0.00};{1:0.00};{2}", refRate + rateDiff, value, date));

            }

            File.WriteAllLines(path, list);

        }

        
    }
}