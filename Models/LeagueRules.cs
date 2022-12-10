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
		[JsonPropertyName("maxteams")]
		public int MaxTeams { get; set; } = default!;

		[JsonPropertyName("qbcount")]
		public int QbCount { get; set; } = default!;

		[JsonPropertyName("rbcount")]
		public int RbCount { get; set; } = default!;

		[JsonPropertyName("wrcount")]
		public int WrCount { get; set; } = default!;

		[JsonPropertyName("tecount")]
		public int TeCount { get; set; } = default!;

		[JsonPropertyName("defensecount")]
		public int DCount { get; set; } = default!;

		[JsonPropertyName("kcount")]
		public int KCount { get; set; } = default!;

		[JsonPropertyName("passingtdpoints")]
		public int PassingTDPoints { get; set; } = default!;

		[JsonPropertyName("ppc")]
		public double PPC { get; set; } = default!; // Points per completion

		[JsonPropertyName("ppi")]
		public double PPI { get; set; } = default!; // Points per incompletion

		[JsonPropertyName("pptwentyfivepass")]
		public int PPTwentyFiveYdsPass { get; set; } = default!;

		[JsonPropertyName("fortyyardpassbonus")]
		public int FortyYardPassBonus { get; set; } = default!;

		[JsonPropertyName("sixtyyardpassbonus")]
		public int SixtyYardPassBonus { get; set; } = default!;

		[JsonPropertyName("threehundredyardpassbonus")]
		public int ThreeHundredYardPassBonus { get; set; } = default!;

		[JsonPropertyName("fivehundredyardpassbonus")]
		public int FiveHundredYardPassBonus { get; set; } = default!;

		[JsonPropertyName("rushingtdpoints")]
		public int RushingTDPoints { get; set; } = default!;

		[JsonPropertyName("receivingtdpoints")]
		public int ReceivingTDPoints { get; set; } = default!;

		[JsonPropertyName("pptenrush")]
		public int PPTenRush { get; set; } = default!;

		[JsonPropertyName("fortyyardrushreceivingbonus")]
		public int FortyYardRushReceivingBonus { get; set; } = default!;

		[JsonPropertyName("sixtyyardrushreceivingbonus")]
		public int SixtyYardRushReceivingBonus { get; set; } = default!;

		[JsonPropertyName("onehundredyardrushreceivingbonus")]
		public int OneHundredYardRushReceivingBonus { get; set; } = default!;

		[JsonPropertyName("twohundredyardrushreceivingbonus")]
		public int TwoHundredYardRushReceivingBonus { get; set; } = default!;

		[JsonPropertyName("ppr")]
		public double PPR { get; set; } = default!; // Points per reception

		[JsonPropertyName("twopointconversion")]
		public int TwoPointConversion { get; set; } = default!;

		[JsonPropertyName("interceptionoffense")]
		public int InterceptionOffense { get; set; } = default!;

		[JsonPropertyName("fumbleoffense")]
		public int FumbleOffense { get; set; } = default!;

		[JsonPropertyName("safetyoffense")]
		public int SafetyOffense { get; set; } = default!;

		[JsonPropertyName("sackdefense")]
		public int SackDefense { get; set; } = default!;

		[JsonPropertyName("tackledefense")]
		public int TackleDefense { get; set; } = default!;

		[JsonPropertyName("fgpuntblock")]
		public int FgPuntBlock { get; set; } = default!;

		[JsonPropertyName("interceptiondefense")]
		public int InterceptionDefense { get; set; } = default!;

		[JsonPropertyName("fumbledefense")]
		public int FumbleDefense { get; set; } = default!;

		[JsonPropertyName("safetydefense")]
		public int SafetyDefense { get; set; } = default!;

		[JsonPropertyName("inttd")]
		public int IntTd { get; set; } = default!;

		[JsonPropertyName("fumbletd")]
		public int FumbleTd { get; set; } = default!;

		[JsonPropertyName("returntd")]
		public int ReturnTd { get; set; } = default!;

		[JsonPropertyName("fgtentotwenty")]
		public int FgTenToTwenty { get; set; } = default!;

		[JsonPropertyName("fgmissedten")]
		public int FgMissedTen { get; set; } = default!;

		[JsonPropertyName("fgtwentytothirty")]
		public int FgTwentyToThirty { get; set; } = default!;

		[JsonPropertyName("fgmissedtwenty")]
		public int FgMissedTwenty { get; set; } = default!;

		[JsonPropertyName("fgthirtytoforty")]
		public int FgThirtyToForty { get; set; } = default!;

		[JsonPropertyName("fgmissedthirty")]
		public int FgMissedThirty { get; set; } = default!;

		[JsonPropertyName("fgfortytofifty")]
		public int FgFortyToFifty { get; set; } = default!;

		[JsonPropertyName("fgmissedforty")]
		public int FgMissedforty { get; set; } = default!;

		[JsonPropertyName("fgfiftytosixty")]
		public int FgFiftyToSixty { get; set; } = default!;

		[JsonPropertyName("fgmissedfifty")]
		public int FgMissedFifty { get; set; } = default!;

		[JsonPropertyName("fgsixtyplus")]
		public int FgSixtyPlus { get; set; } = default!;

		[JsonPropertyName("fgmissedsixty")]
		public int FgMissedSixty { get; set; } = default!;

		[JsonPropertyName("xpmade")]
		public int XpMade { get; set; } = default!;

		[JsonPropertyName("xpmissed")]
		public int XpMissed { get; set; } = default!;

		public LeagueRules(int maxteams = 12, int qbCount = 3, int rbCount = 4, int wrCount = 5, int teCount = 3, int dCount = 2, int kCount = 2, int passingTDPoints = 4, double pPC = 0.3, double pPI = -0.1, int ptspertwentyfive = 1, int fortyYardPassBonus = 2, int sixtyYardPassBonus = 3, int threeHundredYardPassBonus = 3, int fiveHundredYardPassBonus = 5, int rushingTDPoints = 6, int receivingTDPoints = 6, int pptenrush = 1, int fortyYardRushReceivingBonus = 3, int sixtyYardRushReceivingBonus = 4, int oneHundredYardRushReceivingBonus = 3, int twoHundredYardRushReceivingBonus = 5, double pPR = 0.5, int twoPointConversion = 1, int interceptionOffense = -1, int fumbleOffense = -1, int safetyOffense = -1, int sackDefense = 1, int tackleDefense = 0, int fgPuntBlock = 1, int interceptionDefense = 2, int fumbleDefense = 2, int safetyDefense = 2, int intTd = 6, int fumbleTd = 6, int returnTd = 6, int fgTenToTwenty = 3, int fgMissedTen = -4, int fgTwentyToThirty = 3, int fgMissedTwenty = -3, int fgThirtyToForty = 3, int fgMissedThirty = -2, int fgFortyToFifty = 4, int fgMissedforty = -1, int fgFiftyToSixty = 5, int fgMissedFifty = -1, int fgSixtyPlus = 6, int fgMissedSixty = -1, int xpMade = 1, int xpMissed = -2)
		{
			MaxTeams = maxteams;
			QbCount = qbCount;
			RbCount = rbCount;
			WrCount = wrCount;
			TeCount = teCount;
			DCount = dCount;
			KCount = kCount;
			PassingTDPoints = passingTDPoints;
			PPC = pPC;
			PPI = pPI;
			PPTwentyFiveYdsPass = ptspertwentyfive;
			FortyYardPassBonus = fortyYardPassBonus;
			SixtyYardPassBonus = sixtyYardPassBonus;
			ThreeHundredYardPassBonus = threeHundredYardPassBonus;
			FiveHundredYardPassBonus = fiveHundredYardPassBonus;
			RushingTDPoints = rushingTDPoints;
			ReceivingTDPoints = receivingTDPoints;
			PPTenRush = pptenrush;
			FortyYardRushReceivingBonus = fortyYardRushReceivingBonus;
			SixtyYardRushReceivingBonus = sixtyYardRushReceivingBonus;
			OneHundredYardRushReceivingBonus = oneHundredYardRushReceivingBonus;
			TwoHundredYardRushReceivingBonus = twoHundredYardRushReceivingBonus;
			PPR = pPR;
			TwoPointConversion = twoPointConversion;
			InterceptionOffense = interceptionOffense;
			FumbleOffense = fumbleOffense;
			SafetyOffense = safetyOffense;
			SackDefense = sackDefense;
			TackleDefense = tackleDefense;
			FgPuntBlock = fgPuntBlock;
			InterceptionDefense = interceptionDefense;
			FumbleDefense = fumbleDefense;
			SafetyDefense = safetyDefense;
			IntTd = intTd;
			FumbleTd = fumbleTd;
			ReturnTd = returnTd;
			FgTenToTwenty = fgTenToTwenty;
			FgMissedTen = fgMissedTen;
			FgTwentyToThirty = fgTwentyToThirty;
			FgMissedTwenty = fgMissedTwenty;
			FgThirtyToForty = fgThirtyToForty;
			FgMissedThirty = fgMissedThirty;
			FgFortyToFifty = fgFortyToFifty;
			FgMissedforty = fgMissedforty;
			FgFiftyToSixty = fgFiftyToSixty;
			FgMissedFifty = fgMissedFifty;
			FgSixtyPlus = fgSixtyPlus;
			FgMissedSixty = fgMissedSixty;
			XpMade = xpMade;
			XpMissed = xpMissed;
		}
	}
}
