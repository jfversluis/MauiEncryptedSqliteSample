namespace MauiEncryptedSqliteSample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        collectionView.ItemsSource = await App.Database.GetPeopleAsync();
    }

    async void OnButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(nameEntry.Text))
        {
            await App.Database.SavePersonAsync(new Person
            {
                Name = nameEntry.Text,
                Subscribed = subscribed.IsChecked
            });

            nameEntry.Text = string.Empty;
            subscribed.IsChecked = false;

            collectionView.ItemsSource = await App.Database.GetPeopleAsync();
        }
    }

    Person lastSelection;
    void collectionView_SelectionChanged(System.Object sender, SelectionChangedEventArgs e)
    {
        lastSelection = e.CurrentSelection[0] as Person;

        nameEntry.Text = lastSelection.Name;
        subscribed.IsChecked = lastSelection.Subscribed;
    }

    // Update
    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        if (lastSelection != null)
        {
            lastSelection.Name = nameEntry.Text;
            lastSelection.Subscribed = subscribed.IsChecked;

            await App.Database.UpdatePersonAsync(lastSelection);

            collectionView.ItemsSource = await App.Database.GetPeopleAsync();
        }
    }

    // Delete
    async void Button_Clicked_1(System.Object sender, System.EventArgs e)
    {
        if (lastSelection != null)
        {
            await App.Database.DeletePersonAsync(lastSelection);

            collectionView.ItemsSource = await App.Database.GetPeopleAsync();

            nameEntry.Text = "";
            subscribed.IsChecked = false;
        }
    }

    // Get subscribed
    async void Button_Clicked_2(System.Object sender, System.EventArgs e)
    {
        collectionView.ItemsSource = await App.Database.QuerySubscribedAsync();
    }

    // Get Not Subscribed
    async void Button_Clicked_3(System.Object sender, System.EventArgs e)
    {
        collectionView.ItemsSource = await App.Database.LinqNotSubscribedAsync();
    }
}
