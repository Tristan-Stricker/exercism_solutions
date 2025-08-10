using System;

public class BankAccount
{
    private decimal _balance = 0;

    private Object balanceMutex = new object();

    private bool isOpen = false;

    public void Open()
    {
        isOpen = true;
    }

    public void Close()
    {
        isOpen = false;
    }

    public decimal Balance
    {
        get
        {
            return isOpen ? _balance : throw new InvalidOperationException();
        }
    }

    public void UpdateBalance(decimal change)
    {
        lock (balanceMutex)
        {
            _balance += change;
        }
    }
}
