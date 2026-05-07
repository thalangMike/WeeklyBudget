using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppSandbox.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MauiAppSandbox.PageModels
{
    public partial class MainPageModel : ObservableObject
    {
        private const decimal WeeklyBudgetValue = 5000m;

        [ObservableProperty]
        private ObservableCollection<PurchaseEntry> _purchases = [];

        [ObservableProperty]
        private string _newPurchaseDescription = string.Empty;

        [ObservableProperty]
        private string _newPurchaseAmount = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public decimal WeeklyBudget => WeeklyBudgetValue;

        public decimal TotalSpent => Purchases.Sum(p => p.Amount);

        public decimal RemainingBudget => WeeklyBudget - TotalSpent;

        public bool IsOverBudget => RemainingBudget < 0;

        public MainPageModel()
        {
            Purchases.CollectionChanged += (_, __) => UpdateBudgetState();
            StatusMessage = "Register purchases for this week to track your budget.";
        }

        [RelayCommand]
        private void AddPurchase()
        {
            var description = NewPurchaseDescription?.Trim();

            if (string.IsNullOrWhiteSpace(description))
            {
                StatusMessage = "Please enter what you purchased.";
                return;
            }

            if (!decimal.TryParse(NewPurchaseAmount, NumberStyles.Number, CultureInfo.CurrentCulture, out var amount) || amount <= 0)
            {
                StatusMessage = "Please enter a valid amount greater than 0.";
                return;
            }

            Purchases.Insert(0, new PurchaseEntry
            {
                Description = description,
                Amount = amount,
                Date = DateTime.Now
            });

            NewPurchaseDescription = string.Empty;
            NewPurchaseAmount = string.Empty;

            UpdateBudgetState();
        }

        [RelayCommand]
        private void RemovePurchase(PurchaseEntry purchase)
        {
            if (purchase is null)
            {
                return;
            }

            if (Purchases.Remove(purchase))
            {
                UpdateBudgetState();
            }
        }

        private void UpdateBudgetState()
        {
            OnPropertyChanged(nameof(TotalSpent));
            OnPropertyChanged(nameof(RemainingBudget));
            OnPropertyChanged(nameof(IsOverBudget));

            if (IsOverBudget)
            {
                StatusMessage = $"You are over budget by {Math.Abs(RemainingBudget):C}.";
            }
            else
            {
                StatusMessage = $"You have {RemainingBudget:C} left this week.";
            }
        }
    }
}