using System;
using System.Collections.Generic;
using System.Linq;

namespace TankGame.FlyweightPattern
{

    public enum TankType
    {
        T34,
        Abrams,
        Leopard2,
        T90
    }

    
    public class TankFlyweight
    {
        private static readonly Dictionary<TankType, TankFlyweight> _flyweights =
            new Dictionary<TankType, TankFlyweight>();

        public TankType Type { get; }
        public string Texture { get; }
        public string Model3D { get; }
        public int Speed { get; }
        public int Armor { get; }
        public int Firepower { get; }

        private TankFlyweight(TankType type)
        {
            Type = type;

            
            switch (type)
            {
                case TankType.T34:
                    Texture = "Textures/t34.png";
                    Model3D = "Models/t34.fbx";
                    Speed = 50;
                    Armor = 30;
                    Firepower = 45;
                    break;
                case TankType.Abrams:
                    Texture = "Textures/abrams.png";
                    Model3D = "Models/abrams.fbx";
                    Speed = 70;
                    Armor = 85;
                    Firepower = 90;
                    break;
                case TankType.Leopard2:
                    Texture = "Textures/leopard2.png";
                    Model3D = "Models/leopard2.fbx";
                    Speed = 65;
                    Armor = 80;
                    Firepower = 85;
                    break;
                case TankType.T90:
                    Texture = "Textures/t90.png";
                    Model3D = "Models/t90.fbx";
                    Speed = 60;
                    Armor = 75;
                    Firepower = 80;
                    break;
            }
        }

        
        public static TankFlyweight GetFlyweight(TankType type)
        {
            if (!_flyweights.ContainsKey(type))
            {
                _flyweights[type] = new TankFlyweight(type);
            }
            return _flyweights[type];
        }

        public static int GetFlyweightCount()
        {
            return _flyweights.Count;
        }

        public static void DisplayAllFlyweights()
        {
            Console.WriteLine("\n=== ВСЕ ЗАРЕГИСТРИРОВАННЫЕ ЛЕГКОВЕСЫ ===");
            foreach (var flyweight in _flyweights.Values)
            {
                flyweight.DisplayInfo();
                Console.WriteLine("---");
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Тип: {Type}");
            Console.WriteLine($"Текстура: {Texture}");
            Console.WriteLine($"3D-модель: {Model3D}");
            Console.WriteLine($"Скорость: {Speed}, Броня: {Armor}, Мощность: {Firepower}");
        }
    }


    public class Tank
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public int Health { get; private set; }
        public TankFlyweight Flyweight { get; }

        public Tank(float x, float y, int health, TankType type)
        {
            X = x;
            Y = y;
            Health = health;
            Flyweight = TankFlyweight.GetFlyweight(type);
        }

        public void Move(float newX, float newY)
        {
            X = newX;
            Y = newY;
            Console.WriteLine($"{Flyweight.Type} переместился в ({newX}, {newY})");
        }

        public void TakeDamage(int damage)
        {
            double armorProtection = Flyweight.Armor / 100.0;
            int actualDamage = Math.Max(1, (int)(damage - armorProtection * damage));
            Health -= actualDamage;

            Console.WriteLine($"{Flyweight.Type} получил урон {actualDamage}. Здоровье: {Health}");
        }

        public void Display()
        {
            Console.WriteLine($"\n=== Танк {Flyweight.Type} ===");
            Console.WriteLine($"Позиция: ({X}, {Y})");
            Console.WriteLine($"Здоровье: {Health}");
            Flyweight.DisplayInfo();
        }

        public bool IsDestroyed => Health <= 0;
    }

    
    public static class TankFactory
    {
        private static readonly Random _random = new Random();

        public static Tank CreateRandomTank()
        {
            var tankTypes = Enum.GetValues(typeof(TankType));
            var randomType = (TankType)tankTypes.GetValue(_random.Next(tankTypes.Length));
            float x = (float)(_random.NextDouble() * 1000);
            float y = (float)(_random.NextDouble() * 1000);
            int health = _random.Next(50, 101);

            return new Tank(x, y, health, randomType);
        }

        public static List<Tank> CreateArmy(TankType type, int count)
        {
            var army = new List<Tank>();
            for (int i = 0; i < count; i++)
            {
                float x = (float)(_random.NextDouble() * 1000);
                float y = (float)(_random.NextDouble() * 1000);
                int health = _random.Next(50, 101);
                army.Add(new Tank(x, y, health, type));
            }
            return army;
        }
    }

    
    public static class MemoryStats
    {
        public static void DisplayStats(List<Tank> tanks)
        {
            var groups = tanks.GroupBy(t => t.Flyweight.Type);

            Console.WriteLine("\n=== СТАТИСТИКА ПАМЯТИ ===");
            Console.WriteLine($"Всего танков: {tanks.Count}");
            Console.WriteLine($"Уникальных легковесов: {TankFlyweight.GetFlyweightCount()}");

            foreach (var group in groups)
            {
                Console.WriteLine($"{group.Key}: {group.Count()} танков используют 1 легковес");
            }

            
            int estimatedSavings = (tanks.Count - TankFlyweight.GetFlyweightCount()) * 100; 
            Console.WriteLine($"Примерная экономия памяти: ~{estimatedSavings} байт");
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ДЕМОНСТРАЦИЯ ПАТТЕРНА ЛЕГКОВЕС ДЛЯ ТАНКОВ \n");

            
            var tank1 = new Tank(100, 200, 100, TankType.T34);
            var tank2 = new Tank(300, 400, 100, TankType.Abrams);
            var tank3 = new Tank(500, 600, 100, TankType.T34); 

            
            tank1.Display();
            tank2.Display();
            tank3.Display();

            Console.WriteLine("\nПРОВЕРКА ЛЕГКОВЕСА ");
            Console.WriteLine($"tank1 и tank3 используют один объект легковеса: {tank1.Flyweight == tank3.Flyweight}");
            Console.WriteLine($"Хэш-код легковеса tank1: {tank1.Flyweight.GetHashCode()}");
            Console.WriteLine($"Хэш-код легковеса tank3: {tank3.Flyweight.GetHashCode()}");

            Console.WriteLine("\nДЕЙСТВИЯ ТАНКОВ ");
            tank1.Move(150, 250);
            tank2.TakeDamage(30);
            tank3.TakeDamage(50);

            Console.WriteLine("\n СОЗДАНИЕ БОЛЬШОЙ АРМИИ");
            var army = TankFactory.CreateArmy(TankType.Leopard2, 1000);
            Console.WriteLine($"Создана армия из {army.Count} танков Leopard2");

            var abramsArmy = TankFactory.CreateArmy(TankType.Abrams, 500);
            var t34Army = TankFactory.CreateArmy(TankType.T34, 300);

            var allTanks = army.Concat(abramsArmy).Concat(t34Army).ToList();

            MemoryStats.DisplayStats(allTanks);

            TankFlyweight.DisplayAllFlyweights();

            Console.WriteLine("\nСЛУЧАЙНЫЕ ТАНКИ");
            for (int i = 0; i < 3; i++)
            {
                var randomTank = TankFactory.CreateRandomTank();
                randomTank.Display();
            }

            Console.WriteLine("\nТЕСТИРОВАНИЕ БОЯ ");
            var testTank = new Tank(0, 0, 50, TankType.T90);
            testTank.Display();

            while (!testTank.IsDestroyed)
            {
                testTank.TakeDamage(20);
            }
            Console.WriteLine($"Танк {testTank.Flyweight.Type} уничтожен!");

           
        }
    }
}