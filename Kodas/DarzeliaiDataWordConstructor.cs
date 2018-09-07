using System.Collections;
using System.Collections.Generic;

namespace Darzeliai
{
	class DarzeliaiDataWordConstructor
	{
		private List<Hashtable> darzeliaiData;

		public DarzeliaiDataWordConstructor(List<Hashtable> darzeliaiData) => this.darzeliaiData = darzeliaiData;


		public List<string> constructWordsListFromDarzeliaiLinesData(List<Hashtable> linesData)
		{
			List<string> wordsList = new List<string>();

			foreach (Hashtable lineData in linesData)
				wordsList.Add(constructWordFromDarzeliaiLineData(lineData));

			return wordsList;
		}

		private string constructWordFromDarzeliaiLineData(Hashtable lineData)
		{

			string schoolNameFragment = "",
					typeLabelFragment = "",
					lanLabelFragment = "";

			foreach (string key in lineData.Keys)
			{
				if (key.Equals("SCHOOL_NAME"))
				{
					schoolNameFragment = lineData["SCHOOL_NAME"].ToString().Replace("\"", "").Substring(0, 3);
				}
				else if (key.Equals("TYPE_LABEL"))
				{
					typeLabelFragment = extractRangeStringFromTypeLabel(lineData["TYPE_LABEL"].ToString());
				}
				else if (key.Equals("LAN_LABEL"))
				{
					lanLabelFragment = lineData["LAN_LABEL"].ToString().Substring(0, length: 4);
				}
			}


			return schoolNameFragment + "_" + typeLabelFragment + "_" + lanLabelFragment;
		}


		private string extractRangeStringFromTypeLabel(string typeLabelValue)
		{
			string[] removableTextFragments = { "\"", "Nuo", "iki ", "metų" };

			foreach (string remTxt in removableTextFragments)
				typeLabelValue = typeLabelValue.Replace(remTxt, "");

			typeLabelValue = typeLabelValue.Trim();
			string[] typeLabelNumbers = typeLabelValue.Split(null);

			typeLabelValue = typeLabelNumbers[0];

			if (typeLabelNumbers.Length == 2)
				typeLabelValue += "-" + typeLabelNumbers[1];

			return typeLabelValue;
		}


	}
}
