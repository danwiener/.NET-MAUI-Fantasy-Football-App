using FantasyFootballMAUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyFootballMAUI.ViewModels
{
	public class HomePageViewModel
	{
		public ObservableCollection<League> LeaguesBelongedTo { get; set; }

		public HomePageViewModel()
		{
			League l = new League(1, "hello", 4, 5);
			LeaguesBelongedTo = new ObservableCollection<League>()
			{
				l
			};
		}


	}
}
