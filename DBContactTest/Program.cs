using DBContactLibraryFrameWork;
using System;

namespace DBContactTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLRepository repo = new SQLRepository();

            repo.UpdateContact("UpdateContact",3,"19850810d0172","Arffen","Aren","a@a.se");
            repo.DeleteContact("DeleteContact",3);
            int outputId = repo.CreateContact("19833334333","rnr","Bulfdfft","hefik@hej.com");
            //Console.WriteLine(outputId);

            //var contact = repo.ReadContact(10);
            foreach (var item in repo.ReadAllContacts())
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
