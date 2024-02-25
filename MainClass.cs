using System.Reflection.Metadata.Ecma335;

namespace Projekts
{
  class MainClass
  {
    public static Random rand = new(); // Izmantoju tikai .Next() metodei, kuras darbība ir iedot rand. skaitli no 0 (ieskaitot) - n (neieskaitot).

    public static int userDailyDepositLimit = 10000; // Lietotājs nevarēs kontā ielikt vairāk par norādīto ikdienas limitu.
    public static int userDailyWithdrawLimit = 5000; // Lietotājs nevarēs no konta izņemt vairāk par norādīto ikdienas limitu.
    public static int userWalletSize = ChoseUsersWalletSize();
    
    static void Main(string[] args) {
      /* Šī ir galvenā metode, kura izpildīs visas programmas darbības.*/
      CleanScreen();

      // Bankomāta darbību cikls.
      while (true) {
        ShowUserAndATMInformation();

        Console.WriteLine("Available actions:");
        Console.WriteLine("[1] - Take out money, [2] - Put in money, [3] - Find different ATM.");
        if (int.TryParse(Console.ReadLine(), out int lietotajaIzvele)) {
          switch (lietotajaIzvele) {
            case 1:
              ATM.atm.CashDispense(); // Izdod lietotājam jeb klientam norādīto summu.
              break;
            case 2: 
              ATM.atm.CashIntake(); // Palielina lietotāja bankas konta balansu un bankomāta naudas dadzumu par ievadīto summu, un samazina maka naudas daudzumu un dienas limitu.
              break;
            case 3:
              ATM.atm.FindDifferentATM(); // Izveido jaunu ATM objektu (ar citiem datiem).
              break;
          }
          CleanScreen();
        }
        else {
          CleanScreen();
          Console.WriteLine("Total amount of visited ATM's: {0}",
          ATM.ATMcount);
          break;
        }
      }

      Console.WriteLine("\n\nProgram ended. Press <ENTER>");
      Console.ReadLine(); // Neaizver programmu.
    }

    public static void CleanScreen() {
      Console.WriteLine("\x1b[2J\x1b[H");
    }
    
    private static void ShowUserAndATMInformation()
    {
      // Saliek datus masīvā kā tabulā.
      string[,] information = 
      { // 1. kol. bankomāta un konta informācija.           2. kol. lietotāja limiti.
        {$"ATM balance: {ATM.atm.MachineBalance}",         "\x1b[G\x1b[30C" + $"Users daily deposit amount: {userDailyDepositLimit}"}, 
        {$"Account balance: {ATM.atm.UserAccountBalance}", "\x1b[G\x1b[30C" + $"Users daily withdraw amount: {userDailyWithdrawLimit}"} 
      };

      for (int i = 0; i < 2; i++) 
      {
        for (int j = 0; j < 2; j++)
        {
          Console.Write(information[i,j]);
        }
        Console.WriteLine();
      }
      Console.WriteLine($"Wallet size: {userWalletSize}\n");
    }

    private static int ChoseUsersWalletSize()
    {
      int userWalletSize; // Izveido mainīgo, kuru vēlāk atgriezīs kā vērtību.
      Console.WriteLine("Enter the amount of money that is in your wallet: "); 
      while (!int.TryParse(Console.ReadLine(),  out userWalletSize)) {} // Kamēr nav ievadīts skaitlis, tikmēr prasīs jaunu ievadi.
      CleanScreen();

      return userWalletSize;
    }
  }
}