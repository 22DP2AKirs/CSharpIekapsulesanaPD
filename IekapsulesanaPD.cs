using System.Reflection.Metadata.Ecma335;

namespace Projekts
{
  class IekapsulesanaPD
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

      

    //   Console.WriteLine("\n\nProgram end. Press <ENTER>");
    //   Console.ReadLine(); // Neaizver programmu.
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

    public static void CleanScreen() {
      Console.WriteLine("\x1b[2J\x1b[H");
    }
  }

  public class ATM {
    // Public nozīmē, ka mainīgo var izmantot jebkurā klasē.
    // Static nozīmē, ka, visiem šis klases izveidotajiem objektiem, norādītais mainīgais būs vienāds.
    // Private nozīmē, ka mainīgais vai objekts ir pieejams tikai klasē, kur to izveidoja. Tomēr tā vērtību var iegūt un mainīt no šīs klases "instance" ar getteriem un setteriem (Iekapsulēšana).

    /* ? Idejas:
      Valūtas pārveidošana.
      Naudas izņemšana.
      Naudas ielikšana.
      Lietotāja bankas info. parādīšana.
      Bankomāta limiti.
      Dažādas valūtas daudzuma pieņemšanas iespējas (5 eiro, 10 eiro, 20, 50 ... ).
      Saskaitīt cik daudz naudas lietotājs ir ielicis bankomātā.
      No konta ikdienas ieņemšanas un izņemšanas limits.
    */

    public static ATM atm = new(IekapsulesanaPD.rand.Next(10001), IekapsulesanaPD.rand.Next(5001)); // Izveido klases instance šinī klasē, lai to varētu pārveidot (izveidot no jauna) ar šis klases metodēm.

    // Mainīgie:
    public static int ATMcount = 1; // Skaita cik šīs klases objekti tika izveidoti visas programmas laikā. 1, jo sāk no jau atrasta ATM.
    private int machineBalance; // Bankomāta valūtas daudzums. Nosaka cik daudz naudas tas var izdot.
    private int userAccountBalance; // Bankomāta lietotāja naudas daudzums kontā. Nosaka cik daudz naudas lietotājs var izņemt no bankomāta.

    public ATM(int machineBalance, int userAccountBalance) 
    {
      this.machineBalance = machineBalance; // Nepieciešams, lai zinātu cik daudz naudas 'šis' bankomāts var izdot.
      this.userAccountBalance = userAccountBalance; // Nepieciešams, lai saprastu cik daudz naudas ir uz lietotāja konta.

      ATMcount++; // Palielina katru reizi, kad tiek izveidots jauns klases objekts.
    }

    // * Iekapsulēšanas (property).
    public int UserAccountBalance
    {
      get { return userAccountBalance; }
      set { userAccountBalance = value; }
    }

    public int MachineBalance
    {
      get { return machineBalance; }
      set { machineBalance = value; }
    }

    // * Parastās metodes.
    public void FindDifferentATM()
    {
      atm = new(IekapsulesanaPD.rand.Next(10001), userAccountBalance); // Izveido jaunu bankomātu ar citiem datiem, bet ar tiem pašiem lietotāja konta datiem.
    }

    public void CashDispense() 
    {
      Console.WriteLine("Enter amount that you want to withdraw.");
      Console.WriteLine("To go back press 'ENTER'");
      while (true)
      {
        if (int.TryParse(Console.ReadLine(), out int number)) // TryParse garantē, ka atgriests būs int, tikai tad, kad ievade sastāves tikai no cipariem.
        { 
          if (number % 10 == 5 || number % 10 == 0) // Pārbauda vai ievadītais daudzums var būt izdots ar banknotēm (5 , 10 , 20 , 50 , ..., ).
          { 
            if(CheckIfWithdrawRequestCanBeCompleated(number)) {
              WithdrawSum(number);
              break;
            }
            else
            {
              Console.WriteLine("Please enter different amount!");
            }
          }
          else {
            Console.WriteLine("Please enter amount that can be dispensed! Number must end with 0 or 5.");
          }
        }
        else // Iziet ārā no cikla, ja ievade bija tukša ''.
        {
          break;
        }
      }
    }

    public void CashIntake() 
    {
      Console.WriteLine("ATM approves only banknotes with size of: 5 , 10 , 20 , 50 , 100 , 500");
      Console.WriteLine("To end intake press 'ENTER'");

      string userInput; // Izveido mainīgo, kurš uzglabās lietotāja ievadi.
      int takenMoney = 0; // Mainīgais nosaka cik daudz naudas lietotājs ir ielicis bankomātā.

      while (true) 
      {
        userInput = Console.ReadLine(); // Ievāc ievadi.

        // Ja ievadi var pārveidot par int, tad to izdara un skaitli saglabā 'insertedMoney' mainīgajā. Mainīgo 'insertedMoney' izveido metodē (int insertedMoney).
        if (int.TryParse(userInput, out int insertedMoney)) 
        { 
          // Pārbauda vai ir ievadīts pareizais banknotes formāts.
          if (insertedMoney == 5 || insertedMoney == 10 || insertedMoney == 20 || insertedMoney == 50 || insertedMoney == 100 || insertedMoney == 500) 
          {
            // Pievieno banknoti pie ievāktās summas.
            takenMoney += insertedMoney;
          }
          else // Pabrīdina, ka formāts nav pareizs.
          {
            Console.WriteLine("\x1b[1A\x1b[6C" + " <---  Entered cash format is not approved! Cash wasn't taken."); // Dīvainie simboli ir ANSI escape sequences. \x1b ir hexidecimālais formāts (To atbalsta .NET).
          }
        }
        else // Iziet ārā no naudas ievākšanas posma, uzrādot cik daudz nauda tika ievākta.
        {
          if (CheckIfDepositRequestCanBeCompleated(takenMoney))
          {
            Console.WriteLine("Total money inserted: " + takenMoney + " EURO.");
            DepositSum(takenMoney);
            break;
          }
          else // Nodzēš ievākto naudu.
          {
            Console.WriteLine("Your inserted cash was returned!");
            takenMoney = 0;
          }
        }
      }
    }

    private void DepositSum(int value)
    { // * Metode pieskaita ievākto summu norādītajos mainīgajos.
      IekapsulesanaPD.userDailyDepositLimit -= value;
      IekapsulesanaPD.userWalletSize -= value;
      userAccountBalance += value;
      machineBalance += value;
    }

    private void WithdrawSum(int value)
    { // * Metode atņem norādīto summu norādītajos mainīgajos.
      IekapsulesanaPD.userWalletSize += value;
      IekapsulesanaPD.userDailyWithdrawLimit -= value;
      userAccountBalance -= value;
      machineBalance -= value;
    }

    private bool CheckIfWithdrawRequestCanBeCompleated(int value) 
    { // * Metode pārbauda norādīto naudas summu var izņemt no bankomāta.
      if (userAccountBalance - value >= 0) 
      { // Pārbauda vai summu var noņemt no konta.
        if (IekapsulesanaPD.userDailyWithdrawLimit - value >= 0) 
        { // Pārbauda vai lietotājs nepārsniedz savu ikdienas izņemšanas limitu.
          if (machineBalance - value >= 0) 
          { // Pārbauda vai summu var izņemt no bankomāta.
            return true;
          }
          else
          {
            Console.WriteLine("ATM does not have enough funds to complete that action!");
          }
        }
        else
        {
          Console.WriteLine("You are exeeding your daily withdraw limit! Today you can withdraw: " + IekapsulesanaPD.userDailyWithdrawLimit + " Euro.");
        }
      }
      else
      {
        Console.WriteLine("Your account doesn't have those funds!");
      }
      return false;
    }
    private bool CheckIfDepositRequestCanBeCompleated(int value) 
    { // * Metode pārbauda vai naudas summu var būt pārskaitīt uz kontu, un ja nevar, tad pasaka iemeslu.
      if (IekapsulesanaPD.userDailyDepositLimit - value >= 0) 
      { // Pārbauda vai lietotājs nepārsniedz savu ikdienas naudas ieskaitīšanas limitu.
        if (IekapsulesanaPD.userWalletSize - value >= 0) 
        {
          return true;
        }
        else
        {
          Console.WriteLine("Your dont have those funds!");
        }
      }
      else
      {
        Console.WriteLine("You are exeeding your daily deposit limit! You can still deposit: " + IekapsulesanaPD.userDailyDepositLimit + " Euro.");
      }
      return false;
    }
  }
}