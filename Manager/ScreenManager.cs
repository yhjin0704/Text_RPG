using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG
{
    internal class ScreenManager
    {
        private static ScreenManager instance = new ScreenManager();

        private ScreenManager() { }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }
                return instance;
            }
        }

        public int DrawBaseScreen(string _MainText, string[] _OptionText, bool _IsMain, Screen _FuncScreen) //mainText의 줄바꿈은 \n으로 사용
        {
            int index;
            while (true)
            {
                if (null != _OptionText)
                {
                    index = _OptionText.Length;
                }
                else if (GameManager.Instance.NowScreen == EScreen.Buy || GameManager.Instance.NowScreen == EScreen.Sell || GameManager.Instance.NowScreen == EScreen.Equip)
                {
                    index = GameManager.Instance.ShopItem.Count;
                }
                else
                {
                    index = 0;
                }

                Console.Clear();

                //메인 텍스트
                Console.WriteLine(_MainText);
                //골드가 필요한 화면의 경우 골드 표시
                if ((GameManager.Instance.NowScreen == EScreen.Shop || GameManager.Instance.NowScreen == EScreen.Buy || GameManager.Instance.NowScreen == EScreen.Sell || GameManager.Instance.NowScreen == EScreen.Rest))
                {
                    Console.WriteLine($"[보유 골드]\n{Player.Instance.Gold} G\n");
                }

                if (null != _FuncScreen)
                {
                    _FuncScreen.DrawFuncScreen();
                }

                if (GameManager.Instance.NowScreen != EScreen.Buy && GameManager.Instance.NowScreen != EScreen.Sell && GameManager.Instance.NowScreen != EScreen.Equip)
                {
                    for (int i = 1; i <= index; i++)
                    {
                        Console.WriteLine($"{i}. {_OptionText[i - 1]}");
                    }
                }

                if (false == _IsMain)
                {
                    Console.WriteLine("\n0. 나가기");
                }

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int result))
                {
                    if (false == _IsMain)
                    {
                        if (int.Parse(input) < 0 || int.Parse(input) > index)
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.ReadKey();

                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        if (int.Parse(input) < 1 || int.Parse(input) > index)
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.ReadKey();

                        }
                        else
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }
        }

        public int DrawBaseScreen(string _MainText, string[] _OptionText, bool _IsMain) //mainText의 줄바꿈은 \n으로 사용
        {
            return DrawBaseScreen(_MainText, _OptionText, _IsMain, null);
        }
    }
}
