using IspApp.Models;

namespace IspApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IspContext db)
        {
            db.Database.EnsureCreated();

            // если тарифы уже есть - база заполнена, выходим
            if (db.Tariffs.Any())
                return;

            var tariffs = new List<Tariff>
            {
                new Tariff { Name = "Домашний Старт",    Speed = 50,   MonthlyPrice = 19.90m,  TariffType = "Домашний" },
                new Tariff { Name = "Домашний Базовый",  Speed = 100,  MonthlyPrice = 29.90m,  TariffType = "Домашний" },
                new Tariff { Name = "Домашний Макс",     Speed = 300,  MonthlyPrice = 39.90m,  TariffType = "Домашний" },
                new Tariff { Name = "Домашний Ультра",   Speed = 500,  MonthlyPrice = 54.90m,  TariffType = "Домашний" },
                new Tariff { Name = "Офисный Старт",     Speed = 100,  MonthlyPrice = 69.90m,  TariffType = "Офисный"  },
                new Tariff { Name = "Офисный Бизнес",    Speed = 300,  MonthlyPrice = 119.90m, TariffType = "Офисный"  },
                new Tariff { Name = "Офисный Премиум",   Speed = 1000, MonthlyPrice = 219.90m, TariffType = "Офисный"  },
            };

            db.Tariffs.AddRange(tariffs);
            db.SaveChanges();

            // абоненты
            var rand = new Random(42);

            string[] maleLastNames = { "Иванов", "Петров", "Сидоров", "Козлов", "Новиков",
                                         "Морозов", "Волков", "Соколов", "Попов", "Лебедев" };
            string[] maleFirstNames = { "Александр", "Дмитрий", "Сергей", "Андрей", "Максим",
                                         "Иван", "Николай", "Алексей", "Виктор", "Павел" };
            string[] maleMidNames = { "Александрович", "Дмитриевич", "Сергеевич",
                                         "Андреевич", "Николаевич", "Викторович" };

            string[] femaleLastNames = { "Иванова", "Петрова", "Сидорова", "Козлова", "Новикова",
                                          "Морозова", "Волкова", "Соколова", "Попова", "Лебедева" };
            string[] femaleFirstNames = { "Елена", "Ольга", "Наталья", "Татьяна", "Марина",
                                          "Светлана", "Анна", "Екатерина", "Ирина", "Юлия" };
            string[] femaleMidNames = { "Александровна", "Дмитриевна", "Сергеевна",
                                          "Андреевна", "Николаевна", "Викторовна" };

            string[] streets = { "Ленина", "Мира", "Гагарина", "Советская", "Садовая",
                                  "Пушкина", "Кирова", "Победы", "Молодёжная", "Центральная" };

            var subscribers = new List<Subscriber>();
            for (int i = 0; i < 30; i++)
            {
                bool isMale = rand.Next(2) == 0;
                string fullName;

                if (isMale)
                {
                    string last = maleLastNames[rand.Next(maleLastNames.Length)];
                    string first = maleFirstNames[rand.Next(maleFirstNames.Length)];
                    string mid = maleMidNames[rand.Next(maleMidNames.Length)];
                    fullName = $"{last} {first} {mid}";
                }
                else
                {
                    string last = femaleLastNames[rand.Next(femaleLastNames.Length)];
                    string first = femaleFirstNames[rand.Next(femaleFirstNames.Length)];
                    string mid = femaleMidNames[rand.Next(femaleMidNames.Length)];
                    fullName = $"{last} {first} {mid}";
                }

                // у каждого 5-го абонента отрицательный баланс
                decimal balance = i % 5 == 0
                    ? -(rand.Next(5, 40) + rand.Next(100) / 100m)
                    : rand.Next(0, 150) + rand.Next(100) / 100m;

                subscribers.Add(new Subscriber
                {
                    FullName = fullName,
                    Address = $"ул. {streets[rand.Next(streets.Length)]}, д. {rand.Next(1, 120)}, кв. {rand.Next(1, 200)}",
                    Passport = $"{rand.Next(10, 99)} {rand.Next(10, 99)} {rand.Next(100000, 999999)}",
                    AccountNumber = $"ISP-{10000 + i}",
                    Balance = balance,
                    TariffId = tariffs[rand.Next(tariffs.Count)].Id
                });
            }

            db.Subscribers.AddRange(subscribers);
            db.SaveChanges();

            // оборудование
            string[] routerModels = { "TP-Link Archer C6", "Asus RT-N12", "Keenetic Air",
                                      "D-Link DIR-615", "ZTE H268N", "Mikrotik hAP ac" };
            string[] stbModels = { "MAG-250", "MAG-322", "Eltex NV-102", "Infomir MAG-324" };

            var equipment = new List<Equipment>();
            foreach (var sub in subscribers)
            {
                // каждому абоненту - роутер
                equipment.Add(new Equipment
                {
                    Model = routerModels[rand.Next(routerModels.Length)],
                    SerialNumber = $"RT{rand.Next(100000, 999999)}",
                    EquipmentType = "Роутер",
                    Status = "В аренде",
                    SubscriberId = sub.Id
                });

                // каждому третьему + приставка
                if (sub.Id % 3 == 0)
                {
                    equipment.Add(new Equipment
                    {
                        Model = stbModels[rand.Next(stbModels.Length)],
                        SerialNumber = $"STB{rand.Next(100000, 999999)}",
                        EquipmentType = "Приставка",
                        Status = rand.Next(2) == 0 ? "В аренде" : "Продан",
                        SubscriberId = sub.Id
                    });
                }
            }

            db.Equipment.AddRange(equipment);
            db.SaveChanges();

            // платежи (последние 3 месяца)
            var payments = new List<Payment>();
            foreach (var sub in subscribers)
            {
                int payCount = rand.Next(1, 5);
                for (int p = 0; p < payCount; p++)
                {
                    payments.Add(new Payment
                    {
                        PaymentDate = DateTime.Now.AddDays(-rand.Next(1, 90)),
                        Amount = rand.Next(1, 8) * 10 + rand.Next(100) / 100m,
                        SubscriberId = sub.Id
                    });
                }
            }

            db.Payments.AddRange(payments);
            db.SaveChanges();

            // заявки в техподдержку
            string[] problems = {
                "Нет подключения к интернету",
                "Низкая скорость интернета",
                "Роутер не раздаёт Wi-Fi",
                "Периодические обрывы соединения",
                "Не работает IPTV приставка",
                "Требуется замена оборудования",
                "Подключение нового абонента"
            };
            string[] masters = { "Смирнов А.В.", "Кузнецов П.И.", "Зайцев Д.С.", "Орлов Н.Н." };

            var requests = new List<SupportRequest>();
            for (int i = 0; i < 40; i++)
            {
                bool closed = rand.Next(2) == 0;
                requests.Add(new SupportRequest
                {
                    RequestDate = DateTime.Now.AddDays(-rand.Next(1, 60)),
                    Problem = problems[rand.Next(problems.Length)],
                    Status = closed ? "Закрыта" : "Открыта",
                    Master = closed ? masters[rand.Next(masters.Length)] : null,
                    SubscriberId = subscribers[rand.Next(subscribers.Count)].Id
                });
            }

            db.SupportRequests.AddRange(requests);
            db.SaveChanges();
        }
    }
}