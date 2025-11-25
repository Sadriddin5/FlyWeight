
        double x = 100.0;
        double y = 200.0;

        TankFactory tankFactory = new TankFactory();

        // Создаем танки Т-34
        for (int i = 0; i < 5; i++)
        {
            Tank t34 = tankFactory.GetTank("T-34");
            if (t34 != null)
                t34.Display(x, y, 100); // 100 - здоровье
            x += 10.0;
            y += 10.0;
        }

        // Создаем танки Абрамс
        for (int i = 0; i < 5; i++)
        {
            Tank abrams = tankFactory.GetTank("Abrams");
            if (abrams != null)
                abrams.Display(x, y, 150);
            x += 10.0;
            y += 10.0;
        }

        Console.Read();

abstract class Tank
{
    protected string texture; 
    protected string model3D; 
    protected int speed; 
    protected int armor; 

    public abstract void Display(double x, double y, int health);
}

class T34Tank : Tank
{
    public T34Tank()
    {
        texture = "T34_texture.png";
        model3D = "T34_model.obj";
        speed = 50;
        armor = 30;
    }

    public override void Display(double x, double y, int health)
    {
        Console.WriteLine($"Танк Т-34 [Здоровье: {health}%, Координаты: ({x}, {y})]");
        Console.WriteLine($"  Текстура: {texture}, Модель: {model3D}");
        Console.WriteLine($"  Скорость: {speed}, Броня: {armor}");
    }
}

class AbramsTank : Tank
{
    public AbramsTank()
    {
        texture = "Abrams_texture.png";
        model3D = "Abrams_model.obj";
        speed = 70;
        armor = 80;
    }

    public override void Display(double x, double y, int health)
    {
        Console.WriteLine($"Танк Абрамс [Здоровье: {health}%, Координаты: ({x}, {y})]");
        Console.WriteLine($"  Текстура: {texture}, Модель: {model3D}");
        Console.WriteLine($"  Скорость: {speed}, Броня: {armor}");
    }
}

class TankFactory
{
    Dictionary<string, Tank> tanks = new Dictionary<string, Tank>();

    public TankFactory()
    {
        tanks.Add("T-34", new T34Tank());
        tanks.Add("Abrams", new AbramsTank());
    }

    public Tank GetTank(string key)
    {
        if (tanks.ContainsKey(key))
            return tanks[key];
        else
            return null;
    }
}
