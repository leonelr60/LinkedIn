import java.text.NumberFormat;
import java.util.Locale;
import java.util.Scanner;

public class Mortgage {
    final static byte months = 12;
    final static byte percentage = 100;
    final static byte formulaNumber = 1;
    double principalValue = 0;
    float annualInterestRate = 0;
    byte periodYears = 0;
    double mortgage = 0.00;

    public Mortgage(double principalMinValue,double principalMaxValue, float anualMinInterestRate, float anualMaxInterestRate, byte periodMinYears, byte periodMaxYears) {
        Console readConsole = new Console();
        this.principalValue = readConsole.readValue("Principal: ",principalMinValue, principalMaxValue);
        this.annualInterestRate = (float)readConsole.readValue("Annual Interest Rate: ", anualMinInterestRate, anualMaxInterestRate);
        this.periodYears = (byte)readConsole.readValue("Period (Years): ", periodMinYears, periodMaxYears);

    }

    public void calculateMortgage(){
        /*Calculations*/
        annualInterestRate = (annualInterestRate /percentage)/months;
        double dividend = Math.pow(formulaNumber+ annualInterestRate, (periodYears*months));
        double divisor = Math.pow(formulaNumber+ annualInterestRate, (periodYears*months))-formulaNumber;
        mortgage = principalValue * (annualInterestRate * (dividend/divisor));
        printReport();

    }

    private void printReport(){
        Report balanceReport = new Report();
        balanceReport.printBalance(mortgage, principalValue, annualInterestRate, periodYears);
    }

}
