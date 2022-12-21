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

		public string MaxTeamsStr { get; set; }

		[JsonPropertyName("maxplayers")]
		public int MaxPlayers { get; set; } = 16;

		public string MaxPlayersStr { get; set; }

		[JsonPropertyName("qbcount")]
		public int QbCount { get; set; } = 3;

		public string QbCountStr { get; set; }

		[JsonPropertyName("rbcount")]
		public int RbCount { get; set; } = 4;

		public string RbCountStr { get; set; }

		[JsonPropertyName("wrcount")]
		public int WrCount { get; set; } = 5;

		public string WrCountStr { get; set; }

		[JsonPropertyName("tecount")]
		public int TeCount { get; set; } = 3;

		public string TeCountStr { get; set; }

		[JsonPropertyName("defensecount")]
		public int defensecount { get; set; } = 2;

		public string defensecountStr { get; set; }

		[JsonPropertyName("kcount")]
		public int kcount { get; set; } = 1;
		public string kcountStr { get; set; }

		[JsonPropertyName("passingtdpoints")]
		public int PassingTDPoints { get; set; } = 4;

		public string PassingTDPointsStr { get; set; }

		[JsonPropertyName("ppc")]
		public double PPC { get; set; } = 0.3; // Points per completion
		public string PPCStr { get; set; }

		[JsonPropertyName("ppi")]
		public double PPI { get; set; } = -0.1; // Points per incompletion

		public string PPIStr { get; set; }

		[JsonPropertyName("pptwentyfivepass")]
		public int PPTwentyFiveYdsPass { get; set; } = -3;

		public string PPTwentyFiveYdsPassStr { get; set; }

		[JsonPropertyName("fortyyardpassbonus")]
		public int FortyYardPassBonus { get; set; } = 2;
		public string FortyYardPassBonusStr { get; set; }

		[JsonPropertyName("sixtyyardpassbonus")]
		public int SixtyYardPassBonus { get; set; } = 3;

		public string SixtyYardPassBonusStr { get; set; }

		[JsonPropertyName("threehundredyardpassbonus")]
		public int ThreeHundredYardPassBonus { get; set; } = 3;

		public string ThreeHundredYardPassBonusStr { get; set; }

		[JsonPropertyName("fivehundredyardpassbonus")]
		public int FiveHundredYardPassBonus { get; set; } = 5;
		public string FiveHundredYardPassBonusStr { get; set; }

		[JsonPropertyName("rushingtdpoints")]
		public int RushingTDPoints { get; set; } = 6;
		public string RushingTDPointsStr { get; set; }

		[JsonPropertyName("receivingtdpoints")]
		public int ReceivingTDPoints { get; set; } = 6;

		public string ReceivingTDPointsStr { get; set; }

		[JsonPropertyName("pptenrush")]
		public int PPTenRush { get; set; } = 1;

		public string PPTenRushStr { get; set; } 

		[JsonPropertyName("fortyyardrushreceivingbonus")]
		public int FortyYardRushReceivingBonus { get; set; } = 3;

		public string FortyYardRushReceivingBonusStr { get; set; }

		[JsonPropertyName("sixtyyardrushreceivingbonus")]
		public int SixtyYardRushReceivingBonus { get; set; } = 4;

		public string SixtyYardRushReceivingBonusStr { get; set; }

		[JsonPropertyName("onehundredyardrushreceivingbonus")]
		public int OneHundredYardRushReceivingBonus { get; set; } = 3;

		public string OneHundredYardRushReceivingBonusStr { get; set; }

		[JsonPropertyName("twohundredyardrushreceivingbonus")]
		public int TwoHundredYardRushReceivingBonus { get; set; } = 5;

		public string TwoHundredYardRushReceivingBonusStr { get; set; }

		[JsonPropertyName("ppr")]
		public double PPR { get; set; } = 0.5; // Points per reception

		public string PPRStr { get; set; }

		[JsonPropertyName("twopointconversion")]
		public int TwoPointConversion { get; set; } = 1;

		public string TwoPointConversionStr { get; set; }

		[JsonPropertyName("interceptionoffense")]
		public int InterceptionOffense { get; set; } = -1;

		public string InterceptionOffenseStr { get; set; }

		[JsonPropertyName("fumbleoffense")]
		public int FumbleOffense { get; set; } = -1;

		public string FumbleOffenseStr { get; set; }

		[JsonPropertyName("safetyoffense")]
		public int SafetyOffense { get; set; } = -1;

		public string SafetyOffenseStr { get; set; }

		[JsonPropertyName("sackdefense")]
		public int SackDefense { get; set; } = 1;

		public string SackDefenseStr { get; set; }

		[JsonPropertyName("tackledefense")]
		public double TackleDefense { get; set; } = 0;

		public string TackleDefenseStr { get; set; }

		[JsonPropertyName("fgpuntblock")]
		public int FgPuntBlock { get; set; } = 1;

		public string FgPuntBlockStr { get; set; }

		[JsonPropertyName("interceptiondefense")]
		public int InterceptionDefense { get; set; } = 2;

		public string InterceptionDefenseStr { get; set; }

		[JsonPropertyName("fumbledefense")]
		public int FumbleDefense { get; set; } = 2;

		public string FumbleDefenseStr { get; set; }

		[JsonPropertyName("safetydefense")]
		public int SafetyDefense { get; set; } = 2;

		public string SafetyDefenseStr { get; set; }

		[JsonPropertyName("inttd")]
		public int IntTd { get; set; } = 6;
		public string IntTdStr { get; set; }

		[JsonPropertyName("fumbletd")]
		public int FumbleTd { get; set; } = 6;
		public string FumbleTdStr { get; set; }

		[JsonPropertyName("returntd")]
		public int ReturnTd { get; set; } = 6;

		public string ReturnTdStr { get; set; }

		[JsonPropertyName("fgtentotwenty")]
		public int FgTenToTwenty { get; set; } = 3;

		public string FgTenToTwentyStr { get; set; }

		[JsonPropertyName("fgmissedten")]
		public int FgMissedTen { get; set; } = -4;

		public string FgMissedTenStr { get; set; }

		[JsonPropertyName("fgtwentytothirty")]
		public int FgTwentyToThirty { get; set; } = 3;

		public string FgTwentyToThirtyStr { get; set; }

		[JsonPropertyName("fgmissedtwenty")]
		public int FgMissedTwenty { get; set; } = -2;

		public string FgMissedTwentyStr { get; set; }

		[JsonPropertyName("fgthirtytoforty")]
		public int FgThirtyToForty { get; set; } = 3;

		public string FgThirtyToFortyStr { get; set; }

		[JsonPropertyName("fgmissedthirty")]
		public int FgMissedThirty { get; set; } = -1;

		public string FgMissedThirtyStr { get; set; }

		[JsonPropertyName("fgfortytofifty")]
		public int FgFortyToFifty { get; set; } = 4;

		public string FgFortyToFiftyStr { get; set; }

		[JsonPropertyName("fgmissedforty")]
		public int FgMissedforty { get; set; } = -1;

		public string FgMissedfortyStr { get; set; }

		[JsonPropertyName("fgfiftytosixty")]
		public int FgFiftyToSixty { get; set; } = 5;

		public string FgFiftyToSixtyStr { get; set; }

		[JsonPropertyName("fgmissedfifty")]
		public int FgMissedFifty { get; set; } = -1;

		public string FgMissedFiftyStr { get; set; }

		[JsonPropertyName("fgsixtyplus")]
		public int FgSixtyPlus { get; set; } = 6;

		public string FgSixtyPlusStr { get; set; }

		[JsonPropertyName("fgmissedsixty")]
		public int FgMissedSixty { get; set; } = -1;

		public string FgMissedSixtyStr { get; set; }

		[JsonPropertyName("xpmade")]
		public int XpMade { get; set; } = 1;

		public string XpMadeStr { get; set; }

		[JsonPropertyName("xpmissed")]
		public int XpMissed { get; set; } = -2;

		public string XpMissedStr { get; set; }


		public LeagueRules(int leagueId, int maxTeams, int maxplayers, int qbCount, int rbCount, int wrCount, int teCount, int dcount, int kcount, int passingTDPoints, double pPC, double pPI, int pPTwentyFiveYdsPass, int fortyYardPassBonus, int sixtyYardPassBonus, int threeHundredYardPassBonus, int fiveHundredYardPassBonus, int rushingTDPoints, int receivingTDPoints, int pPTenRush, int fortyYardRushReceivingBonus, int sixtyYardRushReceivingBonus, int oneHundredYardRushReceivingBonus, int twoHundredYardRushReceivingBonus, double pPR, int twoPointConversion, int interceptionOffense, int fumbleOffense, int safetyOffense, int sackDefense, double tackleDefense, int fgPuntBlock, int interceptionDefense, int fumbleDefense, int safetyDefense, int intTd, int fumbleTd, int returnTd, int fgTenToTwenty, int fgMissedTen, int fgTwentyToThirty, int fgMissedTwenty, int fgThirtyToForty, int fgMissedThirty, int fgFortyToFifty, int fgMissedforty, int fgFiftyToSixty, int fgMissedFifty, int fgSixtyPlus, int fgMissedSixty, int xpMade, int xpMissed)
		{
			LeagueId = leagueId;
			MaxTeams = maxTeams;
			MaxPlayers = maxplayers;
			QbCount = qbCount;
			RbCount = rbCount;
			WrCount = wrCount;
			TeCount = teCount;
			defensecount = dcount;
			kcount = kcount;
			PassingTDPoints = passingTDPoints;
			PPC = pPC;
			PPI = pPI;
			PPTwentyFiveYdsPass = pPTwentyFiveYdsPass;
			FortyYardPassBonus = fortyYardPassBonus;
			SixtyYardPassBonus = sixtyYardPassBonus;
			ThreeHundredYardPassBonus = threeHundredYardPassBonus;
			FiveHundredYardPassBonus = fiveHundredYardPassBonus;
			RushingTDPoints = rushingTDPoints;
			ReceivingTDPoints = receivingTDPoints;
			PPTenRush = pPTenRush;
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

			MaxTeamsStr = $"Max # Teams ({MaxTeams})";
			MaxPlayersStr = $"Max # Players ({MaxPlayers})";
			QbCountStr = $"QB Limit ({QbCount})";
			RbCountStr = $"RB Limit ({RbCount})";
			WrCountStr = $"WR Limit ({WrCount})";
			TeCountStr = $"TE Limit ({TeCount})";
			defensecountStr = $"DEFENSE Limit ({defensecount})";
			kcountStr = $"K Limit ({kcount})";
			PassingTDPointsStr = $"Points Per Passing TD ({PassingTDPoints})";
			PPCStr = $"Points Per Completion ({PPC})";
			PPIStr = $"Points Per Incompletion ({PPI})";
			PPTwentyFiveYdsPassStr = $"Points Per 25 Yds Passing ({PPTwentyFiveYdsPass})";
			FortyYardPassBonusStr = $"40+ Yard Pass Bonus ({FortyYardPassBonus})";
			SixtyYardPassBonusStr = $"60+ Yard Pass Bonus ({SixtyYardPassBonus})";
			ThreeHundredYardPassBonusStr = $"300+ Yds Passing Bonus ({ThreeHundredYardPassBonus})";
			FiveHundredYardPassBonusStr = $"500+ Yds Passing Bonus ({FiveHundredYardPassBonus})";
			RushingTDPointsStr = $"Points Per Rushing TD ({RushingTDPoints})";
			ReceivingTDPointsStr = $"Points Per Receiving TD ({ReceivingTDPoints})";
			PPTenRushStr = $"Points Per 10 Yds Rush/Receiv. ({PPTenRush})";
			FortyYardRushReceivingBonusStr = $"40+ Yard Rush/Reception Bonus ({FortyYardRushReceivingBonus})";
			SixtyYardRushReceivingBonusStr = $"60+ Yard Rush/Reception Bonus ({SixtyYardRushReceivingBonus})";
			OneHundredYardRushReceivingBonusStr = $"100+ Yds Rushing/Receiving Bonus ({OneHundredYardRushReceivingBonus})";
			TwoHundredYardRushReceivingBonusStr = $"200+ Yds Rushing/Receiving Bonus ({TwoHundredYardRushReceivingBonus})";
			PPRStr = $"Points Per Reception ({PPR})";
			TwoPointConversionStr = $"Two Point Conversion ({TwoPointConversion})";
			InterceptionOffenseStr = $"Interception Offense ({InterceptionOffense})";
			FumbleOffenseStr = $"Fumble Offense ({FumbleOffense})";
			SafetyOffenseStr = $"Safety Offense ({SafetyOffense})";
			SackDefenseStr = $"Sack Defense ({SackDefense})";
			TackleDefenseStr = $"Tackle ({TackleDefense})";
			FgPuntBlockStr = $"FG/Punt Blocked ({FgPuntBlock})";
			InterceptionDefenseStr = $"Interception Defense ({InterceptionDefense})";
			FumbleDefenseStr = $"Fumble Defense ({FumbleDefense})";
			SafetyDefenseStr = $"Safety Defense ({SafetyDefense})";
			IntTdStr = $"Interception TD ({IntTd})";
			FumbleTdStr = $"Fumble TD ({FumbleTd})";
			ReturnTdStr = $"Return TD ({ReturnTd})";
			FgTenToTwentyStr = $"Field Goal 10-20 Yds ({FgTenToTwenty})";
			FgMissedTenStr = $"Field Goal Miss 10-20 Yds ({FgMissedTen})";
			FgTwentyToThirtyStr = $"Field Goal 20-30 Yds ({FgTwentyToThirty})";
			FgMissedTwentyStr = $"Field Goal Miss 20-30 Yds ({FgMissedTwenty})";
			FgThirtyToFortyStr = $"Field Goal 30-40 Yds ({FgThirtyToForty})";
			FgMissedThirtyStr = $"Field Goal Miss 30-40 Yds ({FgMissedThirty})";
			FgFortyToFiftyStr = $"Field Goal 40-50 Yds ({FgFortyToFifty})";
			FgMissedfortyStr = $"Field Goal Miss 40-50 Yds ({FgMissedforty})";
			FgFiftyToSixtyStr = $"Field Goal 50-60 Yds ({FgFiftyToSixty})";
			FgMissedFiftyStr = $"Field Goal Miss 50-60 Yds ({FgMissedFifty})";
			FgSixtyPlusStr = $"Field Goal 60+ Yds ({FgSixtyPlus})";
			FgMissedSixtyStr = $"Field Goal Miss 60+ Yds ({FgMissedSixty})";
			XpMadeStr = $"XP Made ({XpMade})";
			XpMissedStr = $"XP Missed ({XpMissed})";
		}

	}
}
