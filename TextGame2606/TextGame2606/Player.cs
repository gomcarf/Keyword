class Player
{
    public string Name { get; set; }
    public float Hp { get; set; }
    public float Mp { get; set; }
    public float Attack { get; set; }
    public float Defence { get; set; }

    public Player(string name, float hp, float mp, float attack, float defence)
    {
        Name = name;
        Hp = hp;
        Mp = mp;
        Attack = attack;
        Defence = defence;
    }

    public void Show()
    {
        Console.WriteLine($"\n============================================\n{Name}의 상태창\n============================================");
        Console.WriteLine($"HP : {Hp}, MP : {Mp}, 공격력 : {Attack}, 방어력 : {Defence}");
        Console.WriteLine("============================================");
    }
}
