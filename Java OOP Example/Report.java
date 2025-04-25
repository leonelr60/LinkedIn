import java.text.NumberFormat;
import java.util.Locale;

public class Report {
    double totalMorgage;
    double principalValue;
    float anualInterestRate;
    byte periodYears;
    public void printBalance(double totalMorgage, double principalValue, float anualInterestRate, byte periodYears){
        this.totalMorgage = totalMorgage;
        this.principalValue = principalValue;
        this.anualInterestRate = anualInterestRate;
        this.periodYears = periodYears;
        calculateRemainingBalance();
    }
    private void calculateRemainingBalance(){
        double balanceMorgage = 0.00;
        NumberFormat formatedMortgage = NumberFormat.getCurrencyInstance(Locale.GERMANY);
        System.out.println();
        System.out.println("MORTGAGE");
        System.out.println("--------");
        System.out.println("Monthly Payments: "+formatedMortgage.format(totalMorgage));
        System.out.println();
        System.out.println("PAYMENT SCHEDULE");
        System.out.println("----------------");
        int periodMonths = periodYears*12;
        for(int currentPaymentNumber = 1; currentPaymentNumber <= periodMonths; currentPaymentNumber++){
            balanceMorgage = principalValue * (
                    Math.pow(1+(anualInterestRate),periodMonths) -
                            Math.pow(1+(anualInterestRate),currentPaymentNumber)
            ) / (
                    Math.pow(1+(anualInterestRate),periodMonths) - 1
            );
            System.out.println("Month "+currentPaymentNumber+": "+formatedMortgage.format(balanceMorgage));
        }

    }
}
