namespace tuotanto1
{

    
    public partial class MainPage : ContentPage
    {
        
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
<<<<<<< HEAD
            /// Muutos
            /// toinen
        }


=======
        }

>>>>>>> b5b354666e3996511565ccddb650af3ac1cf34d7
        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
