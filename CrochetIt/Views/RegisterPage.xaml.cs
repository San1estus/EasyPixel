using CrochetIt.ViewModels;

namespace CrochetIt.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}