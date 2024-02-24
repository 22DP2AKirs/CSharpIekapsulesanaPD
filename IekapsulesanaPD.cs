namespace Projekts
{
  class IekapsulesanaPD
  {
    public static Random rand = new(); // Izmantoju tikai .Next() metodei, kuras darbība ir iedot rand. skaitli no 0 (ieskaitot) - n (neieskaitot).
    
    static void Main(string[] args) {
      /* Šī ir galvenā metode, kura izpildīs visas programmas darbības.*/
      CleanScreen();
      int userWalletSize;
      
      Console.WriteLine("Enter the amount of money that is in your wallet: "); 
      while (!int.TryParse(Console.ReadLine(), out userWalletSize)) {} // Kamēr nav ievadīts skaitlis, tikmēr prasīs jaunu ievadi.
      CleanScreen();

      // Bankomāta darbību cikls.
      while (true) {
        Console.WriteLine("ATM balance: " + ATM.atm.MachineBalance + ", User account balance: " + ATM.atm.UserAccountBalance + ", User wallet size: " + userWalletSize);

        Console.WriteLine("Available actions:");
        Console.WriteLine("[1] - Take out money, [2] - Put in money, [3] - Find different ATM.");
        if (int.TryParse(Console.ReadLine(), out int lietotajaIzvele)) {
          switch (lietotajaIzvele) {
            case 1:
              ATM.atm.CashDispense();
              break;
            case 2: // Palielina lietotāja bankas konta balansu un bankomāta naudas dadzumu par ievadīto summu, un samazina maka naudas daudzumu.
              int takenMoneyAmount = ATM.atm.CashIntake(); // Iegūst naudas summas skaitli, kuru lietotājs ielika bankomātā.
              ATM.atm.UserAccountBalance += takenMoneyAmount; // Pieliek lietotāja konta balansam ievadīto summu.
              userWalletSize -= takenMoneyAmount; // Noņem no maka bankomātā ielikto summu.
              break;
            case 3:
              ATM.atm.FindDifferentATM();
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
      Console.Write("Enter amount that you want to withdraw: ");
      while (true)
      {
        if (int.TryParse(Console.ReadLine(), out int number)) // TryParse garantē, ka atgriests būs int, tikai tad, kad ievade sastāves tikai no cipariem.
        { 
          if (number % 10 == 5 || number % 10 == 0) // Pārbauda vai ievadītais daudzums var būt izdots ar banknotēm (5 , 10 , 20 , 50 , ..., ).
          { 
            // Pārbauda vai norādīto summu var noņemt no bankomāta un konta, neejot mīnusos.
            if (CheckIfFundsCanBeSubtracted(machineBalance, number) && CheckIfFundsCanBeSubtracted(userAccountBalance, number))
            {
              break;
            }
            else // Brīdina, ka bankomātā nav tik daudz naudas.
            {
              Console.WriteLine("ATM does not have enough funds for that action, please enter smaller amount!");
            }
          }
          else {
            Console.WriteLine("Please enter amount that can be dispensed! Number must end with 0 or 5.");
          }
        }
      }
    }

    public int CashIntake() 
    {
      Console.WriteLine("ATM approves only banknotes with size of: 5 , 10 , 20 , 50 , 100 , 500");
      Console.WriteLine("To end inake press 'ENTER'");

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
            takenMoney += insertedMoney;
          }
          else // Pabrīdina, ka formāts nav pareizs.
          {
            Console.WriteLine("\x1b[1A\x1b[6C" + " <---  Entered cash format is not approved! Cash wasn't taken."); // Dīvainie simboli ir ANSI escape sequences. \x1b ir hexidecimālais formāts (To atbalsta .NET).
          }
        }
        else // Iziet ārā no naudas ievākšanas posma, uzrādot cik daudz nauda tika ievākta.
        {
          Console.WriteLine("Total money inserted: " + takenMoney + " EURO.");
          machineBalance += takenMoney; // TODO: Jāuzliek drošība ar bankomāta naudas limitu.
          return takenMoney;
        }
      }
    }

    private bool CheckIfRequestCanBeCompleated() 
    { // Metode pārbauda vai naudas summa var būt pārskaitīta, un ja nevar, tad pasaka iemeslu.
      if (dailyDepositLimit) {
        
      }
      return false;
    }


    private bool CheckIfFundsCanBeSubtracted(int firstValue, int secondValue)
    {
      if (firstValue - secondValue >= 0) 
      {
        return true;
      }
      return false;
    }
  }
}