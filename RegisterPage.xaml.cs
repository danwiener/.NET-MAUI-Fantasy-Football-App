using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace FantasyFootballMAUI;

public partial class RegisterPage : ContentPage
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
    public RegisterPage()
    {
        InitializeComponent();
        //_progressArc = new ProgressArc();
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

        Label PasswordLengthLabel = new Label
        {
            FontFamily = "OpenSansRegular",
            TextColor = Color.FromArgb("#5b2a86"),
            FontSize = 16,
            LineBreakMode = LineBreakMode.WordWrap,
            Text = "How long would you like your password to be (between 8-16 characters)",
            HorizontalOptions= LayoutOptions.Center,
            Margin = new Thickness(5, 0, 0, 0)
        };

        HorizontalStackLayout HorizontalStack = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        Grid PasswordLengthInputGrid = new Grid
        {
            WidthRequest = 50,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(5, 0, 0, 0)
        };


        Frame PasswordLengthFrame = new Frame
        {
            BorderColor = Color.FromArgb("#360568"),
            BackgroundColor = Color.FromArgb("#317773")

        };
        PasswordLengthEntry = new Entry
        {
            BackgroundColor = Color.FromArgb("#9ac6c5"),
            PlaceholderColor = Color.Parse("LightGray"),
            TextColor = Color.FromArgb("#360568"),
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Start,
            MaxLength = 2
        };

        PasswordLengthInputGrid.Add(PasswordLengthFrame);
        PasswordLengthInputGrid.Add(PasswordLengthEntry);

        EnterPasswordLengthBtn = new Button
        {
            Text = "Enter",
            Background = Color.FromArgb("#03254C"),
            TextColor = Color.FromArgb("1167B1"),
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 200
        };
        EnterPasswordLengthBtn.Clicked += OnEnterPasswordLengthBtnClicked;
        SemanticProperties.SetHint(AccountBtn, "Creates an account with provided login credentials if account doesn't already exist");

        HorizontalStack.Add(PasswordLengthInputGrid);
        HorizontalStack.Add(EnterPasswordLengthBtn);

        StackLayoutAdd.Add(PasswordLengthLabel);
        StackLayoutAdd.Add(HorizontalStack);

        GeneratePasswordBtn.Clicked -= OnPasswordBtnClicked;
    } // End method

    private async void OnEnterPasswordLengthBtnClicked(object sender, EventArgs e)
    {
        //SemanticScreenReader.Announce(EnterPasswordLengthBtn.Text);

        try
        {
            _passwordLength = Int32.Parse(PasswordLengthEntry.Text);

            if (_passwordLength < 8 || _passwordLength > 16)
            {
                await DisplayAlert("Invalid entry", "Please enter a length between 8 and 16", "OK");
            }
            else
            {
                EnterPasswordLengthBtn.Clicked -= OnEnterPasswordLengthBtnClicked;
                HorizontalStackLayout PasswordAttributesHorizontalStack = new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center
                };
                HorizontalStackLayout PasswordAttributesHorizontalStack2 = new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center
                };

                Label UpperCaseLabel = new Label
                {
                    FontFamily = "OpenSansRegular",
                    TextColor = Color.FromArgb("#360568"),
                    FontSize = 16,
                    Text = "Include uppercase letters",
                    Margin = new Thickness(5, 0, 0, 0)
                };

                UpperCaseCheckBox = new CheckBox
                {
                    Color = Color.FromArgb("a5e6ba")
                };

                Label LowerCaseLabel = new Label
                {
                    FontFamily = "OpenSansRegular",
                    TextColor = Color.FromArgb("#360568"),
                    FontSize = 16,
                    Text = "Include lowercase letters",
                    Margin = new Thickness(5, 0, 0, 0)
                };

                LowerCaseCheckBox = new CheckBox
                {
                    Color = Color.FromArgb("a5e6ba")

                };

                Label IncludeNumbersLabel = new Label
                {
                    FontFamily = "OpenSansRegular",
                    TextColor = Color.FromArgb("#360568"),
                    FontSize = 16,
                    Text = "Include numbers",
                    Margin = new Thickness(5, 0, 0, 0)
                };

                IncludeNumbersCheckBox = new CheckBox
                {
                    Color = Color.FromArgb("a5e6ba")

                };

                Label IncludeSymbolsLabel = new Label
                {
                    FontFamily = "OpenSansRegular",
                    TextColor = Color.FromArgb("#360568"),
                    FontSize = 16,
                    Text = "Include symbols",
                    Margin = new Thickness(5, 0, 0, 0)
                };

                IncludeSymbolsCheckBox = new CheckBox
                {
                    Color = Color.FromArgb("a5e6ba")

                };

                PasswordAttributesHorizontalStack.Add(UpperCaseLabel);
                PasswordAttributesHorizontalStack.Add(UpperCaseCheckBox);
                PasswordAttributesHorizontalStack.Add(LowerCaseLabel);
                PasswordAttributesHorizontalStack.Add(LowerCaseCheckBox);
                PasswordAttributesHorizontalStack2.Add(IncludeNumbersLabel);
                PasswordAttributesHorizontalStack2.Add(IncludeNumbersCheckBox);
                PasswordAttributesHorizontalStack2.Add(IncludeSymbolsLabel);
                PasswordAttributesHorizontalStack2.Add(IncludeSymbolsCheckBox);

                StackLayoutAdd.Add(PasswordAttributesHorizontalStack);
                StackLayoutAdd.Add(PasswordAttributesHorizontalStack2);

                ButtonAndGenerateRandomHorizontalStack = new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center
                };

                //Grid GoBtnGrid = new Grid();
                //ProgressView = new GraphicsView()
                //{
                //    WidthRequest = 100,
                //    HeightRequest = 100
                //};
                GoBtn = new Button
                {
                    Text = "Go",
                    Background = Color.FromArgb("#a5e6ba"),
                    TextColor = Color.FromArgb("#360568"),
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Start,
                    CornerRadius = 40,
                    Margin = new Thickness(5, 0, 0, 0),
                    HeightRequest = 80,
                    WidthRequest = 80,
                    BackgroundColor = Color.FromArgb("a5e6ba")
                };
                GoBtn.Clicked += OnGoBtnClicked;
                SemanticProperties.SetHint(GoBtn, "Creates an account with provided login credentials if account doesn't already exist");

                GeneratingRandomLabel = new Label
                {
                    FontFamily = "OpenSansRegular",
                    TextColor = Color.FromArgb("#7785ac"),
                    FontSize = 16,
                    Text = "[Generating Random]",
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(5, 0, 0, 0)
                };

                ButtonAndGenerateRandomHorizontalStack.Add(GoBtn);
                ButtonAndGenerateRandomHorizontalStack.Add(GeneratingRandomLabel);
                StackLayoutAdd.Add(ButtonAndGenerateRandomHorizontalStack);
            }
        }
        catch (FormatException fe)
        {
            await DisplayAlert("Invalid entry", "Please enter a length between 8 and 16", "OK");
        }
    } // End method

    private void OnGoBtnClicked(object sender, EventArgs e)
    {
        PasswordEntry.Text = $"{string.Empty}";
        PasswordConfirmEntry.Text = $"{string.Empty}";
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

        _passwordLength = Int32.Parse(PasswordLengthEntry.Text);

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
            GeneratingRandomLabel.TextColor = Color.FromRgb(243, 79, 28);
            goto Start;
        }
        PasswordEntry.Text = $"{_password.ToString()}";
        PasswordConfirmEntry.Text = $"{_password.ToString()}";

        _startTime = DateTime.Now;
        _cancellationTokenSource = new CancellationTokenSource();
        UpdateArc();
    } // End method

    private async void UpdateArc()
    {
        GoBtn.Clicked -= OnGoBtnClicked;
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            var elapsedTime = (DateTime.Now - _startTime);
            int secondsRemaining = (int)(_duration - elapsedTime.TotalMilliseconds) / 1000;

            GoBtn.Text = $"{secondsRemaining}";

            //_progress = Math.Ceiling(elapsedTime.TotalSeconds);
            //_progress %= _duration;
            //_progressArc.Progress = _progress / _duration;
            //ProgressView.Invalidate();

            if (secondsRemaining == 0)
            {
                _cancellationTokenSource.Cancel();
            }
            await Task.Delay(500);
        }
        GoBtn.Text = "Go";

        UpperCaseCheckBox.IsChecked = false;
        LowerCaseCheckBox.IsChecked = false;
        IncludeNumbersCheckBox.IsChecked = false;
        IncludeSymbolsCheckBox.IsChecked = false;

        GeneratingRandomLabel.TextColor = Color.FromArgb("#7785ac");

        GoBtn.Clicked += OnGoBtnClicked;
        return;
    } // End method


} // End class

//public class ProgressArc : IDrawable
//{
//    public double Progress { get; set; } = 100;
//    public void Draw(ICanvas canvas, RectF dirtyRect)
//    {
//        // Angle of the arc in degrees
//        var endAngle = 90 - (int)Math.Round(Progress * 360, MidpointRounding.AwayFromZero);
//        // Drawing code goes here
//        canvas.StrokeColor = Color.FromRgba("F34F1C");
//        canvas.StrokeSize = 4;
//        Debug.WriteLine($"The rect width is {dirtyRect.Width} and height is {dirtyRect.Height}");
//        canvas.DrawArc(5, 5, (dirtyRect.Width - 10), (dirtyRect.Height - 10), 90, endAngle, false, false);
//    }
//}
