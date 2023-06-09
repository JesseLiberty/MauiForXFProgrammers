﻿using ForgetMeNot.Model;
using ForgetMeNot.Services;

namespace ForgetMeNot.ViewModel
{
    [ObservableObject]
    [QueryProperty(nameof(BuddyId), "id")]

    public partial class NewOccasionViewModel
    {
        [ObservableProperty] private string name;
        [ObservableProperty] private DateTime dateSelected = DateTime.Today;
        [ObservableProperty] private int notice;
        [ObservableProperty] private DateTime minDate = DateTime.Today;
        private readonly BuddyService buddyService;


        private string buddyId;

        public string BuddyId
        {
            get => buddyId;
            set => SetProperty(ref buddyId, value);
        }

        public NewOccasionViewModel(BuddyService buddyService)
        {
            this.buddyService = buddyService;
        }


        [RelayCommand]
        private async Task SaveNewOccasion()
        {
            var occasion = new OccasionModel();
            occasion.Name = name;
            occasion.Date = dateSelected;
            occasion.NumDaysToNotify = notice;
            buddyService.SaveOccasion(BuddyId, occasion);
            await Shell.Current.GoToAsync($"..?id={BuddyId}");

        }
    }
}
