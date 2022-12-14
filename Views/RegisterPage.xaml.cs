
using System.Text;
using FantasyFootballMAUI.Models;

namespace FantasyFootballMAUI;

public partial class RegisterPage : ContentPage
{
    // UI components

    // Data components

    CancellationTokenSource _cancellationTokenSource = new();
    CancellationTokenSource _cancellationTokenSource2ForWindowsColors = new();

    //private readonly ProgressArc _progressArc;

    // Data fields

    private int _passwordLength;
    private int _duration = 6000;
    private int _duration2ForWindowsColors = 18000;

    private string _symbols = "!/#$%&\\()*+,-./:;<=>?@[]_{|}~";

    private StringBuilder _password;

    private DateTime _startTime;

    private bool hasDisplayed = false;

    // Constructor
    public RegisterPage()
    {
        InitializeComponent();
    } // End constructor

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Task scaleTitle = Task.Factory.StartNew(async () => { await TitleLabel.ScaleTo(2, 1000); });
    } // End method

    // Button event handlers
    private async void OnAccountBtnClicked(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(AccountBtn.Text);
        string user_name = UsernameEntry.Text;
        string name = NameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string password_confirm = PasswordConfirmEntry.Text;

        if (PasswordEntry.Text != PasswordConfirmEntry.Text)
        {
            await DisplayAlert("Error", "Passwords do not match, please try again", "Ok");
            return;
        }

        NewUser nu = new NewUser(user_name, name, email, password, password_confirm);
        await RegisterNewUserAsync(nu);
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");

    } // End method

    public async Task RegisterNewUserAsync(NewUser nu)
    {
        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(nu);
        var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var url = "https://buttonhookfantasyfootballapi.azurewebsites.net/api/register"; // access the register endpoint to register new user
        using var client = new HttpClient();

        var response = await client.PostAsync(url, data);

        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Congratulations", $"{nu.email} registered successfully, please sign in", "Ok");
        }
        else
        {
            await DisplayAlert("Not successful or account with email address already exists", "Please try again", "Ok");
        }
    } // End method

    private async void OnPasswordBtnClicked(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(GeneratePasswordBtn.Text);


        //PasswordLengthEditor.IsVisible= true;
        PasswordLengthEditor.Focus();
        EnterPasswordLengthButton.IsVisible = true;
        InvisibleEnterPasswordLengthButton.IsVisible = true;
        PasswordLengthGrid.IsVisible = true;


        Grid.SetRow(alreadyhaveGrid, 9);
        Grid.SetRowSpan(alreadyhaveGrid, 1);
        
        if (!hasDisplayed)
        {
			await DisplayAlert("Hello", "Please enter a valid password length between 8 and 16", "Ok");
            hasDisplayed = true;
		}
		InvisibleEnterPasswordLengthButton.Clicked += OnInvisibleEnterPasswordLengthButtonClickedAsync;

        GeneratePasswordBtn.Clicked -= OnPasswordBtnClicked;

        
    } // End method

    private async void OnSignInButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    } // End method


    private void OnShowPasswordBtnClicked(object sender, EventArgs e)
    {
        if (PasswordEntry.IsPassword && PasswordConfirmEntry.IsPassword)
        {
            PasswordEntry.IsPassword = false;
            PasswordConfirmEntry.IsPassword = false;
        }
        else
        {
            PasswordEntry.IsPassword = true;
            PasswordConfirmEntry.IsPassword = true;
        }

    }


    private async void OnInvisibleEnterPasswordLengthButtonClickedAsync(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(EnterPasswordLengthButton.Text);

        if (PasswordLengthEditor.Text == null || PasswordLengthEditor.Text.Length == 0)
        {
			await DisplayAlert("Invalid entry", "Please enter a length between 8 and 16", "OK");
            return;
		}
        try
        {
			CheckBoxGrid.IsVisible= true;
			_passwordLength = Int32.Parse(PasswordLengthEditor.Text);

            if (_passwordLength < 8 || _passwordLength > 16 || PasswordLengthEditor.Text.Length == 0)
            {
                await DisplayAlert("Invalid entry", "Please enter a length between 8 and 16", "OK");
            }
            else
            {
                InvisibleEnterPasswordLengthButton.Clicked -= OnInvisibleEnterPasswordLengthButtonClickedAsync;


                UpperCaseLabel.IsVisible = true;
                UpperCaseCheckBox.IsVisible = true;
                LowerCaseLabel.IsVisible = true;
                LowerCaseCheckBox.IsVisible = true;
                IncludeNumbersLabel.IsVisible = true;
                IncludeNumbersCheckBox.IsVisible = true;
                IncludeSymbolsLabel.IsVisible = true;
                IncludeSymbolsCheckBox.IsVisible = true;
                GoButton.IsVisible = true;
                //b5.IsVisible = true;

                GoButton.Clicked += OnGoButtonClicked;

                //ProgressView = new GraphicsView()
                //{
                //    WidthRequest = 100,
                //    HeightRequest = 100
                //};
            } // End else
        } // End try
        catch (FormatException fe)
        {
            await DisplayAlert("Invalid entry", "Please enter a valid password length between 8 and 16", "OK");
        }
    } // End method

    private async void OnGoButtonClicked(object sender, EventArgs e)
    {
        try
        {
            _passwordLength = Int32.Parse(PasswordLengthEditor.Text);

            if (_passwordLength < 8 || _passwordLength > 16)
            {
                await DisplayAlert("Invalid entry", "Please enter a length between 8 and 16", "OK");
            }
            else
            {
                PasswordEntry.Text = $"{string.Empty}";
                PasswordConfirmEntry.Text = $"{string.Empty}";
                PasswordEntry.IsPassword = false;
                PasswordConfirmEntry.IsPassword = false;
                Random rand = new Random();

                int alphabetIndex;
                int numIndex;
                int symIndex;
                int numUpper;
                int numLower;
                int numNum;
                int numSym;
                int indexToAdd;

                int[] upperIndexes;
                int[] lowerIndexes;
                int[] numIndexes;
                int[] symIndexes;

                _password = new StringBuilder();

                _passwordLength = Int32.Parse(PasswordLengthEditor.Text);

            Start:
                if (UpperCaseCheckBox.IsChecked == true && LowerCaseCheckBox.IsChecked == false && IncludeNumbersCheckBox.IsChecked == false && IncludeSymbolsCheckBox.IsChecked == false)
                {
                    // Populate all uppercase letters
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        alphabetIndex = rand.Next(0, 26);
                        _password.Append((char)(Char.ToUpper('a') + alphabetIndex));
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == false && LowerCaseCheckBox.IsChecked == true && IncludeNumbersCheckBox.IsChecked == false && IncludeSymbolsCheckBox.IsChecked == false)
                {
                    // Populate all lowercase letters
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        alphabetIndex = rand.Next(0, 26);
                        _password.Append((char)(Char.ToLower('a') + alphabetIndex));
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == false && LowerCaseCheckBox.IsChecked == false && IncludeNumbersCheckBox.IsChecked == true && IncludeSymbolsCheckBox.IsChecked == false)
                {
                    // Populate all numbers
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        numIndex = rand.Next(0, 10);
                        _password.Append(numIndex);
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == false && LowerCaseCheckBox.IsChecked == false && IncludeNumbersCheckBox.IsChecked == false && IncludeSymbolsCheckBox.IsChecked == true)
                {
                    // Populate all symbols
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        symIndex = rand.Next(0, _symbols.Length);
                        _password.Append(_symbols[0 + symIndex]);
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == true && LowerCaseCheckBox.IsChecked == true && IncludeNumbersCheckBox.IsChecked == false && IncludeSymbolsCheckBox.IsChecked == false)
                {
                    numUpper = rand.Next(1, _passwordLength - 1); // Number of upper case characters to be added

                    // Establish indexes uppercase characters will appear at
                    upperIndexes = new int[numUpper];
                    for (int i = 0; i < numUpper; i++)
                    {
                        upperIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        alphabetIndex = rand.Next(0, 26);
                        // If index is index uppercase should appear at, populate uppercase letter
                        if (upperIndexes.Contains(i))
                        {
                            _password.Append((char)(Char.ToUpper('a') + alphabetIndex));
                        }
                        // Populate lowercase letter
                        else
                        {
                            // If index is index lowercase should appear at, populate lowercase letter
                            _password.Append((char)(Char.ToLower('a') + alphabetIndex));
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == true && LowerCaseCheckBox.IsChecked == false && IncludeNumbersCheckBox.IsChecked == true && IncludeSymbolsCheckBox.IsChecked == false)
                {
                    numUpper = rand.Next(1, _passwordLength - 1); // Number of upper case characters to be added

                    // Establish indexes uppercase characters will appear at
                    upperIndexes = new int[numUpper];
                    for (int i = 0; i < numUpper; i++)
                    {
                        upperIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        alphabetIndex = rand.Next(0, 26);
                        // If index is index uppercase should appear at, populate uppercase letter
                        if (upperIndexes.Contains(i))
                        {
                            _password.Append((char)(Char.ToUpper('a') + alphabetIndex));
                        }
                        // Populate number
                        else
                        {
                            numIndex = rand.Next(0, 10);

                            // If index is index number should appear at, populate number
                            _password.Append((numIndex));
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == true && LowerCaseCheckBox.IsChecked == false && IncludeNumbersCheckBox.IsChecked == false && IncludeSymbolsCheckBox.IsChecked == true)
                {
                    numUpper = rand.Next(1, _passwordLength - 1); // Number of upper case characters to be added

                    // Establish indexes uppercase characters will appear at
                    upperIndexes = new int[numUpper];
                    for (int i = 0; i < numUpper; i++)
                    {
                        upperIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        alphabetIndex = rand.Next(0, 26);
                        // If index is index uppercase should appear at, populate uppercase letter
                        if (upperIndexes.Contains(i))
                        {
                            _password.Append((char)(Char.ToUpper('a') + alphabetIndex));
                        }
                        // Populate symbol
                        else
                        {
                            symIndex = rand.Next(0, _symbols.Length);

                            // If index is index symbol should appear at, populate symbol
                            _password.Append(_symbols[0 + symIndex]);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == false && LowerCaseCheckBox.IsChecked == true && IncludeNumbersCheckBox.IsChecked == true && IncludeSymbolsCheckBox.IsChecked == false)
                {
                    numLower = rand.Next(1, _passwordLength - 1); // Number of lowercase characters to be added

                    // Establish indexes lowercase characters will appear at
                    lowerIndexes = new int[numLower];
                    for (int i = 0; i < numLower; i++)
                    {
                        lowerIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        alphabetIndex = rand.Next(0, 26);
                        // If index is index lowercase should appear at, populate lowercase letter
                        if (lowerIndexes.Contains(i))
                        {
                            _password.Append((char)(Char.ToLower('a') + alphabetIndex));
                        }
                        // Populate number
                        else
                        {
                            numIndex = rand.Next(0, 10);

                            // If index is index number should appear at, populate number
                            _password.Append(numIndex);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == false && LowerCaseCheckBox.IsChecked == true && IncludeNumbersCheckBox.IsChecked == false && IncludeSymbolsCheckBox.IsChecked == true)
                {
                    numLower = rand.Next(1, _passwordLength - 1); // Number of lowercase characters to be added

                    // Establish indexes uppercase characters will appear at
                    lowerIndexes = new int[numLower];
                    for (int i = 0; i < numLower; i++)
                    {
                        lowerIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        alphabetIndex = rand.Next(0, 26);
                        // If index is index lowercase should appear at, populate lowercase letter
                        if (lowerIndexes.Contains(i))
                        {
                            _password.Append((char)(Char.ToLower('a') + alphabetIndex));
                        }
                        // Populate number
                        else
                        {
                            symIndex = rand.Next(0, _symbols.Length);

                            // If index is index symbol should appear at, populate symbol
                            _password.Append(_symbols[0 + symIndex]);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == true && LowerCaseCheckBox.IsChecked == true && IncludeNumbersCheckBox.IsChecked == true && IncludeSymbolsCheckBox.IsChecked == false)
                {
                    numUpper = rand.Next(1, _passwordLength - 2); // Number of uppercase characters to be added
                    numLower = rand.Next(1, _passwordLength - numUpper - 1); // Number of lowercase characters to be added
                    numNum = _passwordLength - numLower; // Number of numbers (0-9) to be added

                    // Establish indexes uppercase characters will appear at
                    upperIndexes = new int[numUpper];
                    for (int i = 0; i < numUpper; i++)
                    {
                        upperIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Establish indexes lowercase characters will appear at
                    lowerIndexes = new int[numLower];
                    for (int i = 0; i < numLower; i++)
                    {
                    Restart:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd))
                        {
                            lowerIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart;
                        }
                    }
                    // Establish indexes number characters will appear at
                    numIndexes = new int[numNum];
                    for (int i = 0; i < numNum; i++)
                    {
                    Restart2:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd) && !lowerIndexes.Contains(indexToAdd))
                        {
                            numIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart2;
                        }
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        if (upperIndexes.Contains(i) || lowerIndexes.Contains(i))
                        {
                            alphabetIndex = rand.Next(0, 26);

                            // If index is index uppercase should appear at, populate uppercase letter
                            if (upperIndexes.Contains(i))
                            {
                                _password.Append((char)(Char.ToUpper('a') + alphabetIndex));
                            }
                            // If index is index lowercase should appear at, populate uppercase letter
                            else if (lowerIndexes.Contains(i))
                            {
                                _password.Append((char)(Char.ToLower('a') + alphabetIndex));
                            }
                        }
                        else
                        {
                            numIndex = rand.Next(0, 10);

                            // If index is index number should appear at, populate number
                            _password.Append(numIndex);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == true && LowerCaseCheckBox.IsChecked == true && IncludeNumbersCheckBox.IsChecked == true && IncludeSymbolsCheckBox.IsChecked == true)
                {
                    numUpper = rand.Next(1, _passwordLength - 3); // Number of uppercase characters to be added
                    numLower = rand.Next(1, _passwordLength - numUpper - 2); // Number of lowercase characters to be added
                    numNum = rand.Next(1, _passwordLength - numLower - 1); // Number of numbers (0-9) to be added
                    numSym = _passwordLength - numNum; // Number of symbols to be added

                    // Establish indexes uppercase characters will appear at
                    upperIndexes = new int[numUpper];
                    for (int i = 0; i < numUpper; i++)
                    {
                        upperIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Establish indexes lowercase characters will appear at
                    lowerIndexes = new int[numLower];
                    for (int i = 0; i < numLower; i++)
                    {
                    Restart:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd))
                        {
                            lowerIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart;
                        }
                    }
                    // Establish indexes number characters will appear at
                    numIndexes = new int[numNum];
                    for (int i = 0; i < numNum; i++)
                    {
                    Restart2:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd) && !lowerIndexes.Contains(indexToAdd))
                        {
                            numIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart2;
                        }
                    }
                    // Establish indexes symbols will appear at
                    symIndexes = new int[numSym];
                    for (int i = 0; i < numSym; i++)
                    {
                    Restart3:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd) && !lowerIndexes.Contains(indexToAdd) && !numIndexes.Contains(indexToAdd))
                        {
                            symIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart3;
                        }
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        if (upperIndexes.Contains(i) || lowerIndexes.Contains(i))
                        {
                            alphabetIndex = rand.Next(0, 26);

                            // If index is index uppercase should appear at, populate uppercase letter
                            if (upperIndexes.Contains(i))
                            {
                                _password.Append((char)(Char.ToUpper('a') + alphabetIndex));
                            }
                            // If index is index lowercase should appear at, populate uppercase letter
                            else if (lowerIndexes.Contains(i))
                            {
                                _password.Append((char)(Char.ToLower('a') + alphabetIndex));
                            }
                        }
                        // 
                        else if (numIndexes.Contains(i))
                        {
                            numIndex = rand.Next(0, 10);

                            // If index is index number should appear at, populate number
                            _password.Append(numIndex);
                        }
                        else
                        {
                            symIndex = rand.Next(0, _symbols.Length);

                            // If index is index symbol should appear at, populate number
                            _password.Append(_symbols[0 + symIndex]);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == true && LowerCaseCheckBox.IsChecked == false && IncludeNumbersCheckBox.IsChecked == true && IncludeSymbolsCheckBox.IsChecked == true)
                {
                    numUpper = rand.Next(1, _passwordLength - 2); // Number of uppercase characters to be added
                    numNum = rand.Next(1, _passwordLength - numUpper - 1); // Number of numbers (0-9) to be added
                    numSym = _passwordLength - numNum; // Number of symbols to be added

                    // Establish indexes uppercase characters will appear at
                    upperIndexes = new int[numUpper];
                    for (int i = 0; i < numUpper; i++)
                    {
                        upperIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Establish indexes numbers characters will appear at
                    numIndexes = new int[numNum];
                    for (int i = 0; i < numNum; i++)
                    {
                    Restart:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd))
                        {
                            numIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart;
                        }
                    }
                    // Establish indexes symbol characters will appear at
                    symIndexes = new int[numSym];
                    for (int i = 0; i < numNum; i++)
                    {
                    Restart2:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd) && !numIndexes.Contains(indexToAdd))
                        {
                            symIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart2;
                        }
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        if (upperIndexes.Contains(i))
                        {
                            alphabetIndex = rand.Next(0, 26);

                            // If index is index uppercase should appear at, populate uppercase letter
                            if (upperIndexes.Contains(i))
                            {
                                _password.Append((char)(Char.ToUpper('a') + alphabetIndex));
                            }
                        }
                        else if (numIndexes.Contains(i))
                        {
                            numIndex = rand.Next(0, 10);

                            // If index is index number should appear at, populate number
                            _password.Append(numIndex);
                        }
                        else
                        {
                            symIndex = rand.Next(0, _symbols.Length);

                            // If index is index symbol should appear at, populate symbol
                            _password.Append(_symbols[0 + symIndex]);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == true && LowerCaseCheckBox.IsChecked == true && IncludeNumbersCheckBox.IsChecked == false && IncludeSymbolsCheckBox.IsChecked == true)
                {
                    numUpper = rand.Next(1, _passwordLength - 2); // Number of uppercase characters to be added
                    numLower = rand.Next(1, _passwordLength - numUpper - 1); // Number of lowercase characters to be added
                    numSym = _passwordLength - numLower; // Number of symbols to be added

                    // Establish indexes uppercase characters will appear at
                    upperIndexes = new int[numUpper];
                    for (int i = 0; i < numUpper; i++)
                    {
                        upperIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Establish indexes lowercase characters will appear at
                    lowerIndexes = new int[numLower];
                    for (int i = 0; i < numLower; i++)
                    {
                    Restart:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd))
                        {
                            lowerIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart;
                        }
                    }
                    // Establish indexes symbol characters will appear at
                    symIndexes = new int[numSym];
                    for (int i = 0; i < numSym; i++)
                    {
                    Restart2:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!upperIndexes.Contains(indexToAdd) && !lowerIndexes.Contains(indexToAdd))
                        {
                            symIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart2;
                        }
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        if (upperIndexes.Contains(i) || lowerIndexes.Contains(i))
                        {
                            alphabetIndex = rand.Next(0, 26);

                            // If index is index uppercase should appear at, populate uppercase letter
                            if (upperIndexes.Contains(i))
                            {
                                _password.Append((char)(Char.ToUpper('a') + alphabetIndex));
                            }
                            // If index is index lowercase should apppear at, populate lowercase letter
                            else if (lowerIndexes.Contains(i))
                            {
                                _password.Append((char)(Char.ToLower('a') + alphabetIndex));
                            }
                        }
                        else
                        {
                            symIndex = rand.Next(0, _symbols.Length);

                            // If index is index symbol should appear at, populate symbol
                            _password.Append(_symbols[0 + symIndex]);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == false && LowerCaseCheckBox.IsChecked == true && IncludeNumbersCheckBox.IsChecked == true && IncludeSymbolsCheckBox.IsChecked == true)
                {
                    numLower = rand.Next(1, _passwordLength - 2); // Number of lowercase characters to be added
                    numNum = rand.Next(1, _passwordLength - numLower - 1); // Number of number characters (0-9) to be added
                    numSym = _passwordLength - numLower; // Number of symbols to be added

                    // Establish indexes lowercase characters will appear at
                    lowerIndexes = new int[numLower];
                    for (int i = 0; i < numLower; i++)
                    {
                        lowerIndexes[i] = rand.Next(0, _passwordLength - 1);
                    }
                    // Establish indexes number characters will appear at
                    numIndexes = new int[numNum];
                    for (int i = 0; i < numNum; i++)
                    {
                    Restart:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!lowerIndexes.Contains(indexToAdd))
                        {
                            numIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart;
                        }
                    }
                    // Establish indexes symbol characters will appear at
                    symIndexes = new int[numSym];
                    for (int i = 0; i < numSym; i++)
                    {
                    Restart2:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!lowerIndexes.Contains(indexToAdd) && !numIndexes.Contains(indexToAdd))
                        {
                            symIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart2;
                        }
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        if (lowerIndexes.Contains(i))
                        {
                            alphabetIndex = rand.Next(0, 26);

                            // If index is index lowercase should appear at, populate lowercase letter
                            if (lowerIndexes.Contains(i))
                            {
                                _password.Append((char)(Char.ToLower('a') + alphabetIndex));
                            }
                        }
                        else if (numIndexes.Contains(i))
                        {
                            numIndex = rand.Next(0, 10);

                            // If index in index number should appear at, populate number
                            _password.Append(numIndex);
                        }
                        else
                        {
                            symIndex = rand.Next(0, _symbols.Length);

                            // If index is index symbol should appear at, populate symbol
                            _password.Append(_symbols[0 + symIndex]);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == false && LowerCaseCheckBox.IsChecked == false && IncludeNumbersCheckBox.IsChecked == true && IncludeSymbolsCheckBox.IsChecked == true)
                {
                    numNum = rand.Next(1, _passwordLength - 1); // Number of number characters (0-9) to be added
                    numSym = _passwordLength - numNum; // Number of symbols to be added

                    // Establish indexes number characters will appear at
                    numIndexes = new int[numNum];
                    for (int i = 0; i < numNum; i++)
                    {
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        numIndexes[i] = indexToAdd;
                    }
                    // Establish indexes symbol characters will appear at
                    symIndexes = new int[numSym];
                    for (int i = 0; i < numSym; i++)
                    {
                    Restart:
                        indexToAdd = rand.Next(0, _passwordLength - 1);
                        if (!numIndexes.Contains(indexToAdd))
                        {
                            symIndexes[i] = indexToAdd;
                        }
                        else
                        {
                            goto Restart;
                        }
                    }
                    // Add characters to _password
                    for (int i = 0; i < _passwordLength; i++)
                    {
                        if (numIndexes.Contains(i))
                        {
                            numIndex = rand.Next(0, 10);

                            // If index in index number should appear at, populate number
                            _password.Append(numIndex);
                        }
                        else
                        {
                            symIndex = rand.Next(0, _symbols.Length);

                            // If index is index symbol should appear at, populate symbol
                            _password.Append(_symbols[0 + symIndex]);
                        }
                    }
                }
                else if (UpperCaseCheckBox.IsChecked == false && LowerCaseCheckBox.IsChecked == false && IncludeNumbersCheckBox.IsChecked == false && IncludeSymbolsCheckBox.IsChecked == false)
                {
                    CheckBox[] checkBoxes = new CheckBox[4] { UpperCaseCheckBox, LowerCaseCheckBox, IncludeNumbersCheckBox, IncludeSymbolsCheckBox };
                    for (int i = 0; i < checkBoxes.Length; i++)
                    {
                        int result = rand.Next(0, 2);

                        if (result == 0)
                        {
                            checkBoxes[i].IsChecked = true;
                        }
                        else if (result == 1)
                        {
                            checkBoxes[i].IsChecked = false;
                        }
                    }
                    GeneratingRandomLabel.IsVisible = true;
                    OpeningDashesLabel.IsVisible = true;
                    ClosingDashesLabel.IsVisible= true;
                    goto Start;
                }
                PasswordEntry.Text = $"{_password.ToString()}";
                PasswordConfirmEntry.Text = $"{_password.ToString()}";

                _startTime = DateTime.Now;
                _cancellationTokenSource = new CancellationTokenSource();
				_cancellationTokenSource2ForWindowsColors = new CancellationTokenSource();
				UpdateArc();
                //SpinColors();

            }
        }
        catch (FormatException fe) 
        {
            await DisplayAlert("Invalid entry", "Please enter a valid password length between 8 and 16", "OK");

        }
    } // End method

    private async void UpdateArc()
    {
        GoButton.Clicked -= OnGoButtonClicked;
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            var elapsedTime = (DateTime.Now - _startTime);
            int secondsRemaining = (int)(_duration - elapsedTime.TotalMilliseconds) / 1000;

            GoButton.Text = $"{secondsRemaining}";


            if (secondsRemaining == 0)
            {
                _cancellationTokenSource.Cancel();
            }
            await Task.Delay(500);
        }
        GoButton.Text = "Go";

        UpperCaseCheckBox.IsChecked = false;
        LowerCaseCheckBox.IsChecked = false;
        IncludeNumbersCheckBox.IsChecked = false;
        IncludeSymbolsCheckBox.IsChecked = false;

        GeneratingRandomLabel.IsVisible = false; // Hide generating random label
        OpeningDashesLabel.IsVisible = false;
        ClosingDashesLabel.IsVisible= false;

        PasswordEntry.IsPassword = true; // Conceal password
        PasswordConfirmEntry.IsPassword = true; // Conceal password

        GoButton.Clicked += OnGoButtonClicked;
        return;
    } // End method

 //   private async void SpinColors()
 //   {
 //       while (!_cancellationTokenSource2ForWindowsColors.IsCancellationRequested)
 //       {
 //           var elapsedTime = (DateTime.Now - _startTime);
 //           int secondsRemaining = (int)(_duration2ForWindowsColors - elapsedTime.TotalMilliseconds) / 1000;

	//		M1.RotationX += 3;
	//		M1.RotationY -= 3;
	//		M2.RotationX -= 2;
	//		M2.RotationY += 4;
	//		M3.RotationX += 4;
	//		M3.RotationY += 2;
	//		M4.RotationX -= 5;
	//		M4.RotationY += 1;

	//		if (secondsRemaining == 0)
	//		{
	//			_cancellationTokenSource2ForWindowsColors.Cancel();
	//		}

	//		await Task.Delay(500);
	//	}
	//} // End method

} // End class
