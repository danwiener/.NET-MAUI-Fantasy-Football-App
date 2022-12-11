using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FantasyFootballMAUI.Models
{
	public class LeagueRules
	{
		[JsonPropertyName("LeagueId")]
		public int LeagueId { get; set; }

		[JsonPropertyName("maxteams")]
		public int MaxTeams { get; set; } = 12;

		[JsonPropertyName("qbcount")]
		public int QbCount { get; set; } = 3;

		[JsonPropertyName("rbcount")]
		public int RbCount { get; set; } = 4;

		[JsonPropertyName("wrcount")]
		public int WrCount { get; set; } = 5;

		[JsonPropertyName("tecount")]
		public int TeCount { get; set; } = 3;

		[JsonPropertyName("defensecount")]
		public int DCount { get; set; } = 2;

		[JsonPropertyName("kcount")]
		public int KCount { get; set; } = 1;

		[JsonPropertyName("passingtdpoints")]
		public int PassingTDPoints { get; set; } = 4;

		[JsonPropertyName("ppc")]
		public double PPC { get; set; } = 0.3; // Points per completion

		[JsonPropertyName("ppi")]
		public double PPI { get; set; } = -0.1; // Points per incompletion

		[JsonPropertyName("pptwentyfivepass")]
		public int PPTwentyFiveYdsPass { get; set; } = -3;

		[JsonPropertyName("fortyyardpassbonus")]
		public int FortyYardPassBonus { get; set; } = 2;

		[JsonPropertyName("sixtyyardpassbonus")]
		public int SixtyYardPassBonus { get; set; } = 3;

		[JsonPropertyName("threehundredyardpassbonus")]
		public int ThreeHundredYardPassBonus { get; set; } = 3;

		[JsonPropertyName("fivehundredyardpassbonus")]
		public int FiveHundredYardPassBonus { get; set; } = 5;

		[JsonPropertyName("rushingtdpoints")]
		public int RushingTDPoints { get; set; } = 6;

		[JsonPropertyName("receivingtdpoints")]
		public int ReceivingTDPoints { get; set; } = 6;

		[JsonPropertyName("pptenrush")]
		public int PPTenRush { get; set; } = 1;

		[JsonPropertyName("fortyyardrushreceivingbonus")]
		public int FortyYardRushReceivingBonus { get; set; } = 3;

		[JsonPropertyName("sixtyyardrushreceivingbonus")]
		public int SixtyYardRushReceivingBonus { get; set; } = 4;

		[JsonPropertyName("onehundredyardrushreceivingbonus")]
		public int OneHundredYardRushReceivingBonus { get; set; } = 3;

		[JsonPropertyName("twohundredyardrushreceivingbonus")]
		public int TwoHundredYardRushReceivingBonus { get; set; } = 5;

		[JsonPropertyName("ppr")]
		public double PPR { get; set; } = 0.5; // Points per reception

		[JsonPropertyName("twopointconversion")]
		public int TwoPointConversion { get; set; } = 1;

		[JsonPropertyName("interceptionoffense")]
		public int InterceptionOffense { get; set; } = -1;

		[JsonPropertyName("fumbleoffense")]
		public int FumbleOffense { get; set; } = -1;

		[JsonPropertyName("safetyoffense")]
		public int SafetyOffense { get; set; } = -1;

		[JsonPropertyName("sackdefense")]
		public int SackDefense { get; set; } = 1;

		[JsonPropertyName("tackledefense")]
		public int TackleDefense { get; set; } = 0;

		[JsonPropertyName("fgpuntblock")]
		public int FgPuntBlock { get; set; } = 1;

		[JsonPropertyName("interceptiondefense")]
		public int InterceptionDefense { get; set; } = 2;

		[JsonPropertyName("fumbledefense")]
		public int FumbleDefense { get; set; } = 2;

		[JsonPropertyName("safetydefense")]
		public int SafetyDefense { get; set; } = 2;

		[JsonPropertyName("inttd")]
		public int IntTd { get; set; } = 6;

		[JsonPropertyName("fumbletd")]
		public int FumbleTd { get; set; } = 6;

		[JsonPropertyName("returntd")]
		public int ReturnTd { get; set; } = 6;

		[JsonPropertyName("fgtentotwenty")]
		public int FgTenToTwenty { get; set; } = 3;

		[JsonPropertyName("fgmissedten")]
		public int FgMissedTen { get; set; } = -4;

		[JsonPropertyName("fgtwentytothirty")]
		public int FgTwentyToThirty { get; set; } = 3;

		[JsonPropertyName("fgmissedtwenty")]
		public int FgMissedTwenty { get; set; } = -2;

		[JsonPropertyName("fgthirtytoforty")]
		public int FgThirtyToForty { get; set; } = 3;

		[JsonPropertyName("fgmissedthirty")]
		public int FgMissedThirty { get; set; } = -1;

		[JsonPropertyName("fgfortytofifty")]
		public int FgFortyToFifty { get; set; } = 4;

		[JsonPropertyName("fgmissedforty")]
		public int FgMissedforty { get; set; } = -1;

		[JsonPropertyName("fgfiftytosixty")]
		public int FgFiftyToSixty { get; set; } = 5;

		[JsonPropertyName("fgmissedfifty")]
		public int FgMissedFifty { get; set; } = -1;

		[JsonPropertyName("fgsixtyplus")]
		public int FgSixtyPlus { get; set; } = 6;

		[JsonPropertyName("fgmissedsixty")]
		public int FgMissedSixty { get; set; } = -1;

		[JsonPropertyName("xpmade")]
		public int XpMade { get; set; } = 1;

		[JsonPropertyName("xpmissed")]
		public int XpMissed { get; set; } = -2;

		public LeagueRules(int leagueId)
		{
			LeagueId = leagueId;
		}
	}
}
