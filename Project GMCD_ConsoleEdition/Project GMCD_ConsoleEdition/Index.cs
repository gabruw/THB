using System;
using System.Collections.Generic;
using System.Text;

namespace Project_GMCD_ConsoleEdition
{
    class Index
    {
        public static void Main(string[] args)
        {
            var pao = new Index();

            pao.InputSTJ();
            
        }


        public void InputSTJ()
        {
            Console.WriteLine("Data1:");
            //string data_inicial = Console.ReadLine();
            string data_inicial = "11/10/2018";

            Console.WriteLine("Data2:");
            //string data_final = Console.ReadLine();
            string data_final = "15/10/2018";

            var newSTJNavigator = new STJNavigator();
            newSTJNavigator.ConnectionPostData(data_inicial, data_final);
        }
    }
}
