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
        public void When_add_money_can_get_all()
        {
            var model = new CurrencyModel(() =>
            new List<IChangeDepositEvent>
            {
                new ChangeDepositEvent(200, 2, DateTime.Now)
            });

            var result = model.Get(200).Result.First();

            Assert.That(result.Money, Is.EqualTo(200));
            Assert.That(result.Rate, Is.EqualTo(2));
        }

        [Test]
        public void When_add_money_can_get_part()
        {
            var model = new CurrencyModel(() =>
                new List<IChangeDepositEvent>
                {
                    new ChangeDepositEvent(200, 2, DateTime.Now)
                });

            var result = model.Get(150).Result.First();

            Assert.That(result.Money, Is.EqualTo(150));
            Assert.That(result.Rate, Is.EqualTo(2));
        }

        [Test]
        public void When_add_money_can_get_all_by_parts()
        {
            var model = new CurrencyModel(() =>
            new List<IChangeDepositEvent>
            {
                new ChangeDepositEvent(200, 2, DateTime.Now)
            });

            var result = model.Get(150).Result.First();

            Assert.That(result.Money, Is.EqualTo(150));
            Assert.That(result.Rate, Is.EqualTo(2));

            result = model.Get(50).Result.First();

            Assert.That(result.Money, Is.EqualTo(50));
            Assert.That(result.Rate, Is.EqualTo(2));
        }

        [Test]
        public void Other()
        {
            var model = new CurrencyModel(() =>
                new List<IChangeDepositEvent>
                {
                    new ChangeDepositEvent(100, 1, DateTime.Now),
                    new ChangeDepositEvent(200, 2, DateTime.Now),
                    new ChangeDepositEvent(500, 3, DateTime.Now)
                });

            var result = model.Get(700).Result;

            Assert.That(result.First().Money, Is.EqualTo(100));
            Assert.That(result.First().Rate, Is.EqualTo(1));

            Assert.That(result[1].Money, Is.EqualTo(200));
            Assert.That(result[1].Rate, Is.EqualTo(2));

            Assert.That(result[2].Money, Is.EqualTo(400));
            Assert.That(result[2].Rate, Is.EqualTo(3));


            model = new CurrencyModel(() =>
                new List<IChangeDepositEvent>
                {
                    new ChangeDepositEvent(100, 1, DateTime.Now),
                    new ChangeDepositEvent(200, 2, DateTime.Now),
                    new ChangeDepositEvent(-200, 2, DateTime.Now),
                    new ChangeDepositEvent(500, 3, DateTime.Now)
                });
            result = model.Get(150).Result;

            Assert.That(result.First().Money, Is.EqualTo(100));
            Assert.That(result.First().Rate, Is.EqualTo(2));

            Assert.That(result[1].Money, Is.EqualTo(50));
            Assert.That(result[1].Rate, Is.EqualTo(3));
        }

        [Test]
        public void Tests_from_files()
        {
            var sw = new Stopwatch();
            sw.Start();
            var model = new CurrencyModel(()=>Helpers.GetEventsFromFile(@"c:\temp\temp.dat"));

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);

        }

        [Test]
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