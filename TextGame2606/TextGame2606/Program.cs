namespace TextGame2606
{
    class Program
    {
        private static void Main(string[] args)
        {
            string Name = "";
            float hp, mp;
            float attack, defence;
            int HPotion = 3;
            int MPotion = 3;
            int AttackUp = 2;
            int DefenceUp = 3;
            int Menu;
            bool bGameStart = false;

            Console.WriteLine("============================================\r\n플레이어 데이터 입력\r\n============================================");
            while (true)
            {
                Console.Write("플레이어 이름 : ");
                Name = Console.ReadLine()!;
                if (Name.Length <= 3)
                    Console.WriteLine("플레이어의 이름이 너무 짧아요. 다시 입력해주세요.");
                else
                    break;
            }
            while (true)
            {
                Console.Write("HP, MP : ");
                string str = Console.ReadLine()!;
                str = str.Replace(" ", "");
                string[] arr = str.Split(",");
                float.TryParse(arr[0], out hp);
                float.TryParse(arr[1], out mp);
                if (hp < 60 || mp < 40)
                    Console.WriteLine("최소 HP는 60, 최소 MP는 40입니다. 다시 입력해주세요.");
                else
                    break;
            }
            
            while (true)
            {
                Console.Write("공격력, 방어력 : ");
                string str = Console.ReadLine()!;
                str = str.Replace(" ", "");
                string[] arr = str.Split(",");
                float.TryParse(arr[0], out attack);
                float.TryParse(arr[1], out defence);
                if (attack < 15 || defence < 5)
                    Console.WriteLine("최소 공격력은 15, 최소 방어력은 5입니다. 다시 입력해주세요.");
                else
                    break;
            }

            Player player = new Player(Name, hp, mp, attack, defence);
            player.Show();

            Console.WriteLine($"* HP 포션 {HPotion}개, MP 포션 {MPotion}개를 지급했습니다.\n* 공격력 Up 쿠폰 {AttackUp}개, 방어력 Up 쿠폰 {DefenceUp}개를 지급했습니다.");

            while (!bGameStart)
            {
                PrintMenu(player);
                Console.Write("메뉴를 선택하세요 : ");
                Menu = int.Parse(Console.ReadLine()!);

                switch (Menu)
                {
                    case 1:
                        if (HPotion != 0)
                        {
                            player.Hp += 20;
                            HPotion--;
                            Console.WriteLine($"**HP가 20 증가했습니다(HP 포션 -1 => 남은 포션 {HPotion}개)");
                        }
                        else
                            Console.WriteLine("HP 포션이 모두 소진되어 사용이 불가합니다.");
                        break;
                    case 2:
                        if (MPotion != 0)
                        {
                            player.Mp += 20;
                            MPotion--;
                            Console.WriteLine($"**MP가 20 증가했습니다(HP 포션 -1 => 남은 포션 {MPotion}개)");
                        }
                        else
                            Console.WriteLine("MP 포션이 모두 소진되어 사용이 불가합니다.");
                        break;
                    case 3:
                        if (AttackUp != 0)
                        {
                            player.Attack *= 2;
                            AttackUp--;
                            Console.WriteLine($"**공격력이 2배 증가했습니다(공격력 Up 쿠폰 -1 => 남은 쿠폰 {AttackUp}개)");
                        }
                            
                        else
                            Console.WriteLine("공격력 증가 쿠폰이 모두 소진되어 사용이 불가합니다.");
                        break;
                    case 4:
                        if (DefenceUp != 0)
                        {
                            player.Defence *= 2;
                            DefenceUp--;
                            Console.WriteLine($"**방어력이 2배 증가했습니다(방어력 Up 쿠폰 -1 => 남은 쿠폰 {DefenceUp}개)");
                        }
                        else
                            Console.WriteLine("방어력 증가 쿠폰이 모두 소진되어 사용이 불가합니다.");
                        break;
                    case 5:
                        player.Show();
                        break;
                    case 0:
                        bGameStart = true;
                        break;
                    default:
                        Console.WriteLine("다시 선택해주세요");
                        break;
                }//Switch
                
            }//while
            Console.WriteLine("********************************************\r\nGame Start!!!!\r\n********************************************");
        }//Main

        public static void PrintMenu(Player player)
        {
            Console.WriteLine($"============================================\r\n< {player.Name} 강화 >\r\n1. HP Up\r\n2. MP Up\r\n3. 공격력 2배\r\n4. 방어력 2배\r\n5. 능력치 보기\r\n0. 게임 시작\r\n============================================");
        }
    }
}