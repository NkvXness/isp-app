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
                new Tariff { Name = "Домашний Старт",    Speed = 50,   MonthlyPrice = 390,  TariffType = "Домашний" },
                new Tariff { Name = "Домашний Базовый",  Speed = 100,  MonthlyPrice = 550,  TariffType = "Домашний" },
                new Tariff { Name = "Домашний Макс",     Speed = 300,  MonthlyPrice = 750,  TariffType = "Домашний" },
                new Tariff { Name = "Домашний Ультра",   Speed = 500,  MonthlyPrice = 990,  TariffType = "Домашний" },
                new Tariff { Name = "Офисный Старт",     Speed = 100,  MonthlyPrice = 1200, TariffType = "Офисный"  },
                new Tariff { Name = "Офисный Бизнес",    Speed = 300,  MonthlyPrice = 2500, TariffType = "Офисный"  },
                new Tariff { Name = "Офисный Премиум",   Speed = 1000, MonthlyPrice = 4500, TariffType = "Офисный"  },
            };

            db.Tariffs.AddRange(tariffs);
            db.SaveChanges();

            // абоненты
            var rand = new Random(42);

            string[] firstNames = { "Александр", "Дмитрий", "Сергей", "Андрей", "Максим",
                                    "Иван", "Николай", "Алексей", "Виктор", "Павел",
                                    "Елена", "Ольга", "Наталья", "Татьяна", "Марина" };
            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Козлов", "Новиков",
                                    "Морозов", "Волков", "Соколов", "Попов", "Лебедев",
                                    "Иванова", "Петрова", "Сидорова", "Козлова", "Новикова" };
            string[] midNames = { "Александрович", "Дмитриевич", "Сергеевич", "Андреевич",
                                    "Александровна", "Дмитриевна", "Сергеевна" };
            string[] streets = { "Ленина", "Мира", "Гагарина", "Советская", "Садовая",
                                    "Пушкина", "Кирова", "Победы", "Молодёжная", "Центральная" };

            var subscribers = new List<Subscriber>();
            for (int i = 0; i < 30; i++)
            {
                string last = lastNames[rand.Next(lastNames.Length)];
                string first = firstNames[rand.Next(firstNames.Length)];
                string mid = midNames[rand.Next(midNames.Length)];
                string street = streets[rand.Next(streets.Length)];

                // у каждого 5-го абонента отрицательный баланс
                decimal balance = i % 5 == 0
                    ? -rand.Next(100, 800)
                    : rand.Next(0, 3000);

                subscribers.Add(new Subscriber
                {
                    FullName = $"{last} {first} {mid}",
                    Address = $"ул. {street}, д. {rand.Next(1, 120)}, кв. {rand.Next(1, 200)}",
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
                        Amount = rand.Next(3, 12) * 100m,
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