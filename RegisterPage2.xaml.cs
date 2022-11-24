using System.Text;

namespace FantasyFootballMAUI;

public partial class RegisterPage2 : ContentPage
{
    // UI components

    Entry PasswordLengthEntry;

    Button EnterPasswordLengthBtn;
    Button GoBtn;

    CheckBox UpperCaseCheckBox;
    CheckBox LowerCaseCheckBox;
    CheckBox IncludeNumbersCheckBox;
    CheckBox IncludeSymbolsCheckBox;

    //GraphicsView ProgressView;

    HorizontalStackLayout ButtonAndGenerateRandomHorizontalStack;

    Label GeneratingRandomLabel;

    // Data components

    CancellationTokenSource _cancellationTokenSource = new();

    //private readonly ProgressArc _progressArc;

    // Data fields

    private int _passwordLength;
    private int _duration = 6000;

    private string _symbols = "!/#$%&\\()*+,-./:;<=>?@[]_{|}~";

    private StringBuilder _password;

    private DateTime _startTime;

    // Constructor
    public RegisterPage2()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //ProgressView.Drawable = _progressArc;
    }

    // Button event handlers
    private async void OnAccountBtnClicked(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(AccountBtn.Text);
        string user_name = UsernameEntry.Text;
        string name = NameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string password_confirm = PasswordConfirmEntry.Text;

        Navigation.PushAsync(new LoginPage2(user_name, name, email, password, password_confirm));
    } // End method

    private void OnPasswordBtnClicked(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(GeneratePasswordBtn.Text);

        //Label PasswordLengthLabel = new Label
        //{
        //    FontFamily = "OpenSansRegular",
        //    TextColor = Color.FromArgb("#438696"),
        //    FontSize = 16,
        //    LineBreakMode = LineBreakMode.WordWrap,
        //    Text = "How long would you like your password",
        //    HorizontalOptions = LayoutOptions.Center,
        //    Margin = new Thickness(5, 0, 0, 0)
        //};
        //Label PasswordLengthLabel2 = new Label
        //{
        //    FontFamily = "OpenSansRegular",
        //    TextColor = Color.FromArgb("#438696"),
        //    FontSize = 16,
        //    LineBreakMode = LineBreakMode.WordWrap,
        //    Text = "to be (between 8-16 characters)",
        //    HorizontalOptions = LayoutOptions.Center,
        //    Margin = new Thickness(5, 0, 0, 0)
        //};

        //HorizontalStackLayout HorizontalStack = new HorizontalStackLayout
        //{
        //    HorizontalOptions = LayoutOptions.Center
        //};

        //Grid PasswordLengthInputGrid = new Grid
        //{
        //    WidthRequest = 50,
        //    HorizontalOptions = LayoutOptions.Center,
        //    Margin = new Thickness(5, 0, 0, 0)
        //};


        //Frame PasswordLengthFrame = new Frame
        //{
        //    BorderColor = Color.FromArgb("#978d83"),
        //    BackgroundColor = Color.FromArgb("#317773")

        //};
        //PasswordLengthEntry = new Entry
        //{
        //    BackgroundColor = Color.FromArgb("#eeeeee"),
        //    TextColor = Color.FromArgb("#438696"),
        //    HorizontalTextAlignment = TextAlignment.Start,
        //    VerticalTextAlignment = TextAlignment.Center,
        //    MaxLength = 2
        //};

        //PasswordLengthInputGrid.Add(PasswordLengthFrame);
        //PasswordLengthInputGrid.Add(PasswordLengthEntry);

        //EnterPasswordLengthBtn = new Button
        //{
        //    Text = "Enter",
        //    Background = Color.FromArgb("#438696"),
        //    TextColor = Color.FromArgb("#eeeeee"),
        //    FontAttributes = FontAttributes.Bold,
        //    HorizontalOptions = LayoutOptions.Center,
        //    WidthRequest = 200
        //};
        //EnterPasswordLengthBtn.Clicked += OnEnterPasswordLengthBtnClicked;
        //SemanticProperties.SetHint(AccountBtn, "Creates an account with provided login credentials if account doesn't already exist");

        //HorizontalStack.Add(PasswordLengthInputGrid);
        //HorizontalStack.Add(EnterPasswordLengthBtn);

        //StackLayoutAdd.Add(PasswordLengthLabel);
        //StackLayoutAdd.Add(PasswordLengthLabel2);
        //StackLayoutAdd.Add(HorizontalStack);

        //GeneratePasswordBtn.Clicked -= OnPasswordBtnClicked;
    } // End method
}