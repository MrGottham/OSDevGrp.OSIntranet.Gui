namespace OSDevGrp.OSIntranet.Gui.App;

public partial class MainPage
{
    #region Private variables

    private int _count;

    #endregion

    #region Constructor

    public MainPage()
    {
        InitializeComponent();
    }

    #endregion

    #region Methods

    private void OnCounterClicked(object sender, EventArgs e)
	{
        _count++;

		if (_count == 1)
			CounterBtn.Text = $"Clicked {_count} time";
		else
			CounterBtn.Text = $"Clicked {_count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

    #endregion
}